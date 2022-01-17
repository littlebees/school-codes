#include <iostream>
#include <fstream>
#include <vector>
#include <cstdio>
#include <sstream>
#include <memory>
#include <stdexcept>
#include <string>
#include <array>
#include <tuple>
#include <cmath>
std::string exec(const char* cmd) {
    std::array<char, 128> buffer;
    std::string result;
    std::shared_ptr<FILE> pipe(popen(cmd, "r"), pclose);
    if (!pipe) throw std::runtime_error("popen() failed!");
    while (!feof(pipe.get())) {
        if (fgets(buffer.data(), 128, pipe.get()) != nullptr)
            result += buffer.data();
    }
    return result;
}
using namespace std;

typedef const char* FilePath;
typedef tuple<short,short,short> VarIndex;
VarIndex Make_VarIndex(short&& a,short&& b,short&& c){
        return make_tuple(a,b,c);
}
void Inspect_VarIndex(const VarIndex& i) {
        cout << '(' << get<0>(i) << ',' << get<1>(i) << ',' << get<2>(i) << ')' << '\n';
}
typedef tuple<VarIndex,short> Var;
Var Make_Var(VarIndex& a,short&& b) {
        return make_tuple(a,b);
}
VarIndex Get_VarIndex_from_Var(Var& v) {
        return get<0>(v);
}
bool Get_BoolAssign_from_Var(Var& v) {
        return get<1>(v);
}
typedef short ID;
typedef tuple<int,int> Mapper;
ID MapToID(Mapper& mapper,VarIndex&& i) {
        int nn=get<1>(mapper), n=get<0>(mapper);
        return nn*get<0>(i)+n*get<1>(i)+get<2>(i)+1;
}
VarIndex MapToVarIndex(Mapper& mapper, ID& id) {
        id--;
        int nn=get<1>(mapper), n=get<0>(mapper);
        int k = id%n;
        int tmp = id/n;
        int c = tmp%n;
        int r = tmp/n;
        return Make_VarIndex(r,c,k);
}

void Initiate_Mapper(Mapper& mapper,int n) {
        mapper = make_tuple(n,n*n);
}
int Get_How_Many_Literals_from_Mapper(Mapper& mapper) {
        return get<0>(mapper)*get<1>(mapper);
}

void solveCNF(FilePath solver, FilePath cnf, FilePath ans)
{
        string cmd(solver);
        cmd.push_back(' ');
        cmd.append(cnf);
        cmd.push_back(' ');
        cmd.append(ans);
        exec(cmd.c_str());
}

struct Clause {
    vector<Var> literals;

    void addLiteral(VarIndex& v, bool&& val) {
        literals.push_back(Make_Var(v,val));
    }
    void addLiteral(VarIndex&& v, bool&& val) {
        literals.push_back(Make_Var(v,val));
    }
    void popLiteral() {literals.pop_back();}
    string toString_CNF(Mapper& mapper) {
        string str;
        for(auto& v : literals) {
                auto id = MapToID(mapper, Get_VarIndex_from_Var(v));
                id = Get_BoolAssign_from_Var(v) ? id : -id;
                str.append(to_string(id));
                str.push_back(' ');
        }
        str.push_back('0');
        str.push_back('\n');
        return str;
    }
};

struct Sudoku{
    int N,RC;
    bool canbesolved = 0;
    vector<int> table;
    Mapper mapper;
    Sudoku(FilePath quz) {
            ifstream of;
            of.open(quz);
        for(int c=0;of>>c;)
            table.push_back(c);
        of.close();
        N = sqrt(table.size());
        RC = sqrt(N);
        Initiate_Mapper(mapper,N);
    }

    int at(int i,int j) { return table[atI(i,j)]; }
    int atI(int i,int j) { return i*N+j; }
    void inspect()
    {
        for(int i=0;i<N;i++) {
            for(int j=0;j<N;j++)
                cout << at(i,j) << ' ';
            cout << '\n';
        }
    }
    void WriteAnsToFile(FilePath ans) {
            ofstream file;
            file.open(ans);
            if(canbesolved) {
                    for(int i=0,j=0,goal=0;i<N;i++) {
                            string str;
                            for(j=0,goal=N-1;j<goal;j++){
                                    str.append(to_string(at(i,j)));
                                    str.push_back(' ');
                            }
                            str.append(to_string(at(i,j)));
                            str.push_back('\n');
                            file << str;
                    }
            }
            else {
                    file << "NO\n";
            }
            file.close();
    }
    void AnswerIt(FilePath ans,FilePath solver) {
            FilePath encodedCNF = "dimacs.cnf",resultCNF="result";
            toDIMACS(encodedCNF);
            solveCNF(solver,encodedCNF,resultCNF);

            ifstream file;
            file.open(resultCNF);
            if (file.is_open()) {
                string line;
                getline(file, line);
                if(line == "SAT")
                {
                    canbesolved = 1;
                    getline(file, line);
                    istringstream ss(line);
                    for(ID i=0;ss>>i&&i!=0;) {
                        if(i>0) {
                                auto&& var =  MapToVarIndex(mapper,i);
                                setDigit(get<0>(var),get<1>(var),get<2>(var)+1);
                        }
                    }
                }
                else
                {
                    canbesolved = 0;
                }
            }
            file.close();

            WriteAnsToFile(ans);
            string cmd = "rm ",cmd1="rm ";
            cmd.append(encodedCNF);
            cmd1.append(resultCNF);
            exec(cmd.c_str()); exec(cmd1.c_str());
    }
    void toDIMACS(FilePath path) {
            string ans;
            ans.append("p cnf ");
            ans.append(to_string(Get_How_Many_Literals_from_Mapper(mapper)));
            ans.push_back(' ');
            auto cnf = encode();
            ans.append(to_string(cnf.size()));
            ans.push_back('\n');
            for(auto& cla : cnf) {
                    ans.append(cla.toString_CNF(mapper));
            }
            ofstream cnfFile;
            cnfFile.open(path);
            cnfFile << ans;
            cnfFile.close();
    }
    private:
    void setDigit(int i,int j,int k) {
            table[atI(i,j)] = k;
    }
    vector<Clause> encode() {
        vector<Clause> cnf;
        Prefill(cnf);
        CellExactlyOneDigit(cnf);
        RowExactlyOneDigit(cnf);
        ColExactlyOneDigit(cnf);
        BlkExactlyOneDigit(cnf);
        return cnf;
    }
        void Prefill(vector<Clause>& cnf) {
            for(int i=0;i<N;i++)
                for(int j=0;j<N;j++)
                    if(at(i,j) != 0) {
                        Clause cla;
                        cla.addLiteral(Make_VarIndex(i,j,at(i,j)-1),1);
                        cnf.push_back(cla);
                    }
        }

        void CellExactlyOneDigit(vector<Clause>& cnf) {
            vector<VarIndex> varSET;
            for(int i=0;i<N;i++)
                for(int j=0;j<N;j++) {
                    for(int k=0;k<N;k++) {
                        varSET.push_back(Make_VarIndex(i,j,k));
                    }
                    ExactlyOneExpansion(cnf,varSET);
                    varSET.clear();
                }
        }
        void RowExactlyOneDigit(vector<Clause>& cnf) {
            vector<VarIndex> varSET;
            for(int i=0;i<N;i++)
                for(int j=0;j<N;j++) {
                    for(int k=0;k<N;k++) {
                        varSET.push_back(Make_VarIndex(i,k,j));
                    }
                    ExactlyOneExpansion(cnf,varSET);
                    varSET.clear();
                }
        }
        void ColExactlyOneDigit(vector<Clause>& cnf) {
            vector<VarIndex> varSET;
            for(int i=0;i<N;i++)
                for(int j=0;j<N;j++) {
                    for(int k=0;k<N;k++) {
                        varSET.push_back(Make_VarIndex(k,i,j));
                    }
                    ExactlyOneExpansion(cnf,varSET);
                    varSET.clear();
                }
        }
        void BlkExactlyOneDigit(vector<Clause>& cnf) {
            vector<VarIndex> varSET;
            for(int i=0;i<N;i+=RC)
                for(int j=0;j<N;j+=RC) {
                    for(int k=0;k<N;k++) {
                            for(int a=i,x=RC+i;a<x;a++)
                                for(int b=j,y=RC+j;b<y;b++){
                                        varSET.push_back(Make_VarIndex(a,b,k));
                                    }
                            ExactlyOneExpansion(cnf,varSET);
                            varSET.clear();
                        }
                }
        }
        void ExactlyOneExpansion(vector<Clause>& cnf, vector<VarIndex>& varSET) {
            AtLeastOneExpansion(cnf,varSET);
            AtMostOneExpansion(cnf,varSET);
        }
        void AtLeastOneExpansion(vector<Clause>& cnf, vector<VarIndex>& varSET) {
            Clause cla;
            for(auto& v : varSET)
                cla.addLiteral(v,1);
            cnf.push_back(cla);
        }
        Clause cot;
        void AtMostOneExpansion(vector<Clause>& cnf, vector<VarIndex>& varSET,int i=0,int rest=2) {
            if(rest == 0) {
                cnf.push_back(cot);
                return;
            }
            else if(rest > 0 && i >= varSET.size())
                return;

            for(;i<varSET.size();i++) {
                cot.addLiteral(varSET[i],0);
                AtMostOneExpansion(cnf,varSET,i+1,rest-1);
                cot.popLiteral();
            }
        }
};

int main(int argv,char* args[]) {
        ios_base::sync_with_stdio(false);
        cin.tie(NULL);
        if(argv == 4) {
                Sudoku sdk(args[1]);
                sdk.AnswerIt(args[2],args[3]);
        }
        else
                cout << "./solver [Input Puzzle] [Output Puzzle] [MiniSatExe]\n";

}