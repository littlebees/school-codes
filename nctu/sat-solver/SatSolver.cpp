
#include "Val.h"
#include "parser.h"

#include <cstdlib>
#include <cstring>
#include <iostream>
#include <vector>
#include <memory>
#include <tuple>
#include <set>
#include <unordered_set>
#include <exception>
#include <algorithm>
#include <iterator>
#include <fstream>

using namespace std;

bool USE_DLIS = 0, USE_RESTART = 0;

double ACC_VAL = 4, DECAY_VAL = 2;
struct GlobalStatus {
	class OnClauseI;
	class Literal;
	class Clause;
	class ClauseLike;
    typedef double VSIDS_VAL;
	
	typedef unique_ptr<Literal> LiteralObjBase;
	typedef unique_ptr<Clause> ClauseObjBase;
	typedef unique_ptr<ClauseLike> ClauseLikeObjBase;
	typedef unique_ptr<OnClauseI> OnClauseIObjBase;

	typedef vector<ClauseObjBase> ClauseObjMem;
	typedef vector<LiteralObjBase> LiteralObjMem;

	typedef Literal* LiteralObj;
	typedef vector<LiteralObj> SetOfLiteral;
	typedef Clause* ClauseObj;
	typedef vector<ClauseObj> SetOfClause;
	typedef ClauseLike* ClauseLikeObj;
	typedef OnClauseI* OnClauseIObj;
	typedef vector<OnClauseIObj> SetOfOnClauseI;
	struct SAT : public exception { LiteralObjMem& lits; SAT(LiteralObjMem& l) : lits(l) {} };

	ClauseLikeObjBase CLAUSELIKE_BUF = make_unique<ClauseLike>();
	class OnClauseI {
	public:
		virtual BOOL_VAL getVal() = 0;
		virtual bool IsNoted() = 0;
		virtual void Inspect() = 0;
		virtual void Subscribe(ClauseObj cPtr) = 0;
		LEVEL_VAL getLevel() { return that->getLevel(); }
		bool IsFrom(LiteralObj lPtr) { return that == lPtr; }
		LEVEL_VAL getName() { return that->getName(); }
		LiteralObj getParent() { return that; }
		void SetLevel(LEVEL_VAL l) { that->setLevel(l);}
		void Succ(COUNT_VAL len) {acc+=(ACC_VAL/len);}
		void Decay() {acc/=DECAY_VAL;}
		VSIDS_VAL getVSIDS() {return acc;}
		OnClauseI(LiteralObj l) : that(l) {}
		virtual ~OnClauseI() = default;
	protected:
		LiteralObj that;
		VSIDS_VAL acc = 0;
	};
	class Literal {
	private:
		class Noted : public OnClauseI {
		public:
			BOOL_VAL getVal() { return NOT(that->val); }
			bool IsNoted() { return 1; }
			void Inspect() { cout << '-'; that->Inspect(); }
			void Subscribe(ClauseObj cPtr) { that->Subscribe(cPtr,0); }
			Noted(LiteralObj t) : OnClauseI(t) { }
		};
		class Usual : public OnClauseI {
		public:
			int getID() { return getName(); }
			BOOL_VAL getVal() { return that->val; }
			bool IsNoted() { return 0; }
			void Inspect() { that->Inspect(); }
			void Subscribe(ClauseObj cPtr) { that->Subscribe(cPtr,1); }
			Usual(LiteralObj t) : OnClauseI(t) { }
		};
		BOOL_VAL val;
		VAR_INDEX name;

		LEVEL_VAL level;
		OnClauseIObjBase noted, usual;
		SetOfClause watchMeList;
		vector<bool> watchMeAs;

		void Subscribe(ClauseObj cPtr, bool b) {
			watchMeList.push_back(cPtr);
			watchMeAs.push_back(b);
		}
	public:
		Literal(VAR_INDEX v) : val(N), name(v), level(-2), noted(make_unique<Noted>(this)), usual(make_unique<Usual>(this)) {}
		void getCount(COUNT_VAL& u, COUNT_VAL& n) {
			for(unsigned int i=0;i<watchMeList.size();i++)
				if(watchMeList[i]->Eval() == N)
					watchMeAs[i] ? u++ : n++;
		}
		OnClauseIObj getNotedIns() { return noted.get(); }
		OnClauseIObj getUsualIns() { return usual.get(); }
		void setLevel(LEVEL_VAL l) {level = l;} 
		void Decay() {noted->Decay(); usual->Decay();}
		VSIDS_VAL getVSIDS() {return noted->getVSIDS()+usual->getVSIDS();}
		bool IsNotAssigend() { return val == N; }
		LEVEL_VAL getLevel() { return level; }
		BOOL_VAL getVal() { return val; }
		VAR_INDEX getName() { return name; }

		void Inspect() {
			cout << name+1 << '=' << val << ':' << level << '\n';
		}
		int getOutput() { return (getName()+1)*(getVal() == T ? 1 : -1); }

		// change and call back
		void AssignVal(BOOL_VAL v, LEVEL_VAL lev) {
			val = v, level = lev;
			for (auto cPtr : watchMeList)
				cPtr->CallBackWhenLiteralValChange(this);
		}
	};
	class ClauseLike {
	protected:
		SetOfOnClauseI literals;
	public:
		ClauseLike() {}
		ClauseLike(SetOfOnClauseI& v) {
			sort(v.begin(),v.end(),[](OnClauseIObj a, OnClauseIObj b){
				return a->getName() < b->getName();
			});
			literals.assign(v.begin(),v.end());
		}
		void SetLiterals(SetOfOnClauseI& v) {
			sort(v.begin(),v.end(),[](OnClauseIObj a, OnClauseIObj b){
				return a->getName() < b->getName();
			});
			literals.assign(v.begin(),v.end());
		}
		SetOfOnClauseI& getLiterals() { return literals; }
		bool IsHasThisLiteral(OnClauseIObj lPtr) {
			return find(literals.begin(), literals.end(), lPtr) != literals.end();
		}
		void Inspect() {
			cout << "ClasueLike:\n";
			for(auto ins : literals)
				ins->Inspect();
		}
		// ========================
		COUNT_VAL CountVarInTheLevel(LEVEL_VAL lev) noexcept {
			COUNT_VAL ans = 0;
			for (auto l : literals)
				if (l->getLevel() == lev)
					ans++;
			return ans;
		}
		COUNT_VAL CountVarsInClause() { return literals.size(); }

		LiteralObj SecondLargeLevelLiteral(LEVEL_VAL largeL) {
			unsigned int gap = 99999999;
			OnClauseIObj ans = NULL;
			for(auto lit : literals) {
				unsigned int tmp = largeL - lit->getLevel();
				if(tmp > 0 && gap > tmp)
					ans = lit, gap = tmp;
			}
			return ans != NULL ? ans->getParent() : NULL;
		}
		SetOfLiteral FindALiteralByLevel(LEVEL_VAL lev) {
			SetOfLiteral ans;
			for (auto l : literals)
				if (l->getLevel() == lev)
					ans.push_back(l->getParent());
			return ans;
		}
	};

	void Res(SetOfOnClauseI& cPtrA,SetOfOnClauseI& cPtrB, LiteralObj lPtr, SetOfOnClauseI& ret) { // throw CantLearnAnyClause
			SetOfOnClauseI ans;
			bool findUsual = 0, findNoted = 0;
			auto a = cPtrA.begin(), b = cPtrB.begin();
			auto x = cPtrA.end(), y = cPtrB.end();
			while(a != x || b != y) {
				OnClauseIObj small;
				
				if(a != x && b != y) {
					VAR_INDEX l = (*a)->getParent()->getName(), r = (*b)->getParent()->getName();
					if(r > l)
						small = *a, a++;
					else if(r == l)
						if(((*a)->IsNoted() && !(*b)->IsNoted()) || (!(*a)->IsNoted() && (*b)->IsNoted())) {
							if((*a)->IsFrom(lPtr) && (*b)->IsFrom(lPtr))
								findNoted = 1, findUsual = 1;
							a++, b++;
							continue;
						}
						else
							small = *a, a++, b++;
					else
						small = *b, b++;
				} else if(a != x)
					small = *a, a++;
				else
					small = *b, b++;

				if(small->getParent() != lPtr)
					ans.push_back(small);
				else
					if(small->IsNoted())
						findNoted = 1;
					else
						findUsual = 1;
			}
			if(findUsual && findNoted)
				ret.assign(ans.begin(),ans.end());
			else
				throw BT;
		}

	class Clause : public ClauseLike {
	private:
		OnClauseIObj watchedVar = NULL; // constraint: when var' val is N, this clasue is unit
		COUNT_VAL getCanceledVars() {
			COUNT_VAL ans = 0;
			for(auto l : literals)
				if(l->getVal() == F)
					ans++;
			return ans;
		}
	public:
		struct Conflict : public exception {
			ClauseObj clausePtr;
			LEVEL_VAL level;
			Conflict(ClauseObj c, LEVEL_VAL v) : clausePtr(c), level(v) {}
		};
		OnClauseIObj getWatched() { return watchedVar; }
		BOOL_VAL Eval() { return watchedVar->getVal(); }
		bool IsUnit() { return watchedVar->getVal() == N && literals.size()-1 == getCanceledVars(); }
		Clause(SetOfOnClauseI& v) : ClauseLike(v) {
			watchedVar = v[0];
			for (auto lit : v)
				if(lit->getVal() != F) {
					watchedVar = lit;
					break;
				}
		}
		void Inspect() {
			cout << "Clasue:\n";
			cout << "=> watch: \n";
			watchedVar->Inspect();
			ClauseLike::Inspect();
		}

		void throwconflictIns(LEVEL_VAL curLevel) { // throws Conflict
			watchedVar->SetLevel(curLevel);
			Conflict conflict(this, curLevel);
			throw conflict;	
		}

		void CallBackWhenLiteralValChange(LiteralObj lPtr) {
			if (watchedVar->IsFrom(lPtr)) {
				if(watchedVar->getVal() == F) {
					OnClauseIObj tmp = NULL;
					for(auto lit : literals) {
						if(lit != watchedVar)
							if(lit->getVal() == T) {
								watchedVar = lit;
								return;
							} else if(tmp == NULL && lit->getVal() == N)
								tmp = lit;
							else;
						else;
					}
					if(tmp != NULL)
						watchedVar = tmp;
				}
			} else {
				if(watchedVar->getVal() == F)
					for(auto l : literals)
						if(l->IsFrom(lPtr) && l->getVal() != F) {
							watchedVar = l;
							break;
						}
			}
		}
	};
	
	struct Node {
		LiteralObj var;
		LEVEL_VAL level;
		bool BackTracked = 0;
		vector<tuple<LiteralObj, ClauseObj>> implys;
		void Inspect() {
			cout << "Node:\n";
			var->Inspect();
			cout << "Level: " << level << '\n';
			cout << "BackTracked: " << BackTracked << '\n';
			cout << "-----------\n";
			cout << "Implys:\n";
			for(auto& v : implys)
				get<0>(v)->Inspect();
			cout << "===========\n";
		}
		void UndoImplys() {
			for(auto v = implys.rbegin();v!=implys.rend();v++)
				get<0>(*v)->AssignVal(N, -1);
			implys.clear();
		}
		Node(LiteralObj lPtr, LEVEL_VAL lev) : var(lPtr), level(lev) {}
	};

	// TimeLine part
	vector<unique_ptr<Node>> timeline;
	ClauseObjMem clauses;
	LiteralObjMem literals;
	int BOUNCING_LEVEL = -1;
	COUNT_VAL cla_len;

	GlobalStatus(int s, vector<vector<int>> v) {
		for(int i=0;i<s;i++) {
			auto lit = make_unique<Literal>(i);
			literals.push_back(move(lit));
		}
		int tmp = 0;
		for(auto& vv : v) {
			SetOfOnClauseI cla;
			for(auto var : vv) {
				VAR_INDEX n = abs(var)-1;
				auto ins = var <0 ? literals[n]->getNotedIns() : literals[n]->getUsualIns();
				cla.push_back(ins);
			}
			tmp += vv.size();
			AddClause(cla,1);
		}
		tmp /= v.size();
		cla_len = tmp;
	}
	void throwSAT() { // throw SAT
		SAT sat(literals);
		throw sat;
	}
	void Inspect() {
		cout << "GlobalStatus:\n";
		cout << "**TimeLine**\n";
		for(auto& n : timeline)
			n->Inspect();
		cout << "**TimeLine END**\n\n";
	}
	// TimeLine part
	LEVEL_VAL getCurLevel() { return timeline.size(); }

	void AddClause(SetOfOnClauseI& c2, bool init=0) noexcept {
		for (auto lit : c2)
            lit->Succ(c2.size());
		if(!init)
			if(c2.size()> cla_len) {
				return;
			}
		
		for(auto& cla : clauses) {
			auto& c1 = cla->getLiterals();
			if(c1.size() == c2.size()) { 
				bool notSame = 0;
				for(unsigned int i=0;i<c1.size();i++) {
					auto v1 = c1[i], v2 = c2[i];
					if(v1 != v2) {
						notSame = 1;
						break;
					}
				}
				if(!notSame) {
					return;
				}
			}
		}
		auto added = make_unique<Clause>(c2);
		clauses.push_back(move(added));
		if(!init)
			DecayAll();
		for (auto lit : c2)
			lit->Subscribe(clauses.back().get()); 
	}

	void MakeDecision(LiteralObj lPtr, BOOL_VAL val) { 
		LEVEL_VAL curLevel = getCurLevel();
		timeline.push_back(make_unique<Node>(lPtr, curLevel));
		lPtr->AssignVal(val, curLevel); // throws Conflict
	}
	void MakeImply(LiteralObj lPtr, BOOL_VAL val, ClauseObj cPtr, LEVEL_VAL curLevel) { //throws Conflict
		timeline.back()->implys.push_back(make_tuple(lPtr, cPtr));
		lPtr->AssignVal(val, curLevel);
	}
	void BackTo() { // throws RealUNSAT
		if(timeline.empty()) {
			throw GG;
		}

		auto& node = timeline.back();
		auto var = node->var;
		node->UndoImplys();
		
		if(!node->BackTracked) {
			node->BackTracked = 1;
			var->AssignVal(NOT(var->getVal()), var->getLevel());
		} else {
			var->AssignVal(N, -1);
			timeline.pop_back();
			BackTo();
		}
	}

	void Restart() {
		while (!timeline.empty()) {
			auto& node = timeline.back();
			node->UndoImplys();
			node->var->AssignVal(N, -1); // throws conflict
			timeline.pop_back();
		}
	}

	// Pool part
	SetOfClause FindAllUnitClause() { // throws Conflict | SAT
		SetOfClause ans;
		COUNT_VAL cntSAT = 0;
		for (auto& c : clauses) {
			if (c->IsUnit()) {
				ans.push_back(c.get());
			}
			else {
				BOOL_VAL result = c->Eval();
				if(result == F) {
					c->throwconflictIns(getCurLevel());
				}
				else if(result == T)
					cntSAT++;
			}
		}
		
		sort(ans.begin(),ans.end(),[](auto a,auto b){return a->getWatched()->getParent() < b->getWatched()->getParent();});
		
		for(COUNT_VAL i=0;i+1<ans.size();i++)
			if(ans[i]->getWatched()->getParent() == ans[i+1]->getWatched()->getParent())
				if(ans[i+1]->getWatched() != ans[i]->getWatched())
					ans[i+1]->throwconflictIns(getCurLevel());
		
		if(cntSAT == clauses.size()){
			throwSAT(); // throw SAT
		}
		return ans;
	}

	OnClauseIObj VSIDS() {
		VSIDS_VAL large = -1;
		LiteralObj best = NULL, backUp;
		for (auto& l : literals) {
			if(l->IsNotAssigend()) {
				backUp = l.get();
				if(l->getVSIDS() > large)
					large=l->getVSIDS(), best = l.get();
				else if(l->getVSIDS() == large)
                    best = l.get();
				else;
			}
			l->Decay();
		}
        if(best == NULL)
            best = backUp;
		//return make_tuple(best, best->getUsualIns()->getVSIDS() > best->getNotedIns()->getVSIDS() ? T : F);
		return best->getUsualIns()->getVSIDS() > best->getNotedIns()->getVSIDS() ? best->getUsualIns() : best->getNotedIns();
	}

	OnClauseIObj DLIS() {
		COUNT_VAL usuC = 0, notC = 0, tmpU=0, tmpN=0;
		LiteralObj ul = NULL, nl = NULL;
		for (auto& l : literals)
			if (l->IsNotAssigend()) {
				//auto tup = l->getCount(tmpU, tmpN);
				l->getCount(tmpU, tmpN);
				if(tmpU>usuC) ul = l.get(), usuC = tmpU;
				if(tmpN>notC) nl = l.get(), notC = tmpN;
			}
		if(ul == NULL && nl == NULL)
			throw BT;
		else if(usuC == notC)
			//return rand() % 2 ? make_tuple(ul, T) : make_tuple(nl, F);
			return rand() % 2 ? ul->getUsualIns() : nl->getNotedIns();
		else if(usuC > notC)
			//return make_tuple(ul, T);
			return ul->getUsualIns();
		else
			//return make_tuple(nl, F);
			return nl->getNotedIns();
	}

	OnClauseIObj GuessALiteral() {
        DecayAll();
		return USE_DLIS ? DLIS() : VSIDS();
	}

	void Decision() {
		if(USE_RESTART && RestartProc()) {
			if(BOUNCING_LEVEL <= literals.size())
				if(BOUNCING_LEVEL*2 >= literals.size())
					BOUNCING_LEVEL += (literals.size()-BOUNCING_LEVEL)/2;
				else
					BOUNCING_LEVEL = BOUNCING_LEVEL*2;
			else;
			DecayAll();
			Restart();
		}
		auto tup = GuessALiteral();
		MakeDecision(tup->getParent(),tup->IsNoted() ? F : T);
	}

	void DecayAll() {
		for(auto& l : literals)
			l->Decay();
	}
	void UpdateBouncingLevel(LEVEL_VAL l = -1) {
		if(l == -1)
			l = getCurLevel();
		if(BOUNCING_LEVEL == -1) {
			BOUNCING_LEVEL = l*2;
		}			
	}
	bool RestartProc() {
		return BOUNCING_LEVEL != -1 && BOUNCING_LEVEL < getCurLevel();
	}
	// ===================
	void BackTo(LiteralObj lit, LEVEL_VAL level) { // throws RealUNSAT | conflict
		while (!timeline.empty() && timeline.back()->level > level) {
			auto& node = timeline.back();
			node->UndoImplys();
			node->var->AssignVal(N, -1); // throws conflict
			timeline.pop_back();
		}
		BackTo(); //throw RealUNSAT
	}
	ClauseObj FindAssignedClauseByLiteral(LiteralObj lPtr) { // throws JustBackTrack
		auto& implys = timeline.back()->implys;
		for (auto cur = implys.rbegin(); cur != implys.rend(); cur++)
			if (get<0>(*cur) == lPtr)
				return get<1>(*cur);
		throw BT;
	}
	LiteralObj Learn(ClauseLikeObj cPtrPara, LEVEL_VAL lev, SetOfOnClauseI& ret) { // throws JustBackTrack
		CLAUSELIKE_BUF->SetLiterals(cPtrPara->getLiterals());
		ClauseLikeObj cPtr = CLAUSELIKE_BUF.get();
		COUNT_VAL count = cPtr->CountVarInTheLevel(lev);
		for (; count > 1; count = cPtr->CountVarInTheLevel(lev)) {
			ClauseLikeObj cPtr2 = NULL;
			LiteralObj lPtr = NULL;
			auto lits = cPtr->FindALiteralByLevel(lev);
			if(lits.empty())
				throw BT; // throws JustBackTrack
			else
				for(auto lit : lits)
					if(lit->getName() != timeline.back()->var->getName()) {
						cPtr2 = FindAssignedClauseByLiteral(lit), lPtr = lit; // throws JustBackTrack
						break;
					}
						
			if(cPtr2 == NULL)
				throw BT;
			Res(cPtr->getLiterals(), cPtr2->getLiterals(), lPtr,cPtr->getLiterals());
		}
		if (count == 1) {
			auto lPtr = cPtr->SecondLargeLevelLiteral(lev);
			ret.assign(cPtr->getLiterals().begin(),cPtr->getLiterals().end());
			return lPtr;
		}
		else
			throw BT;
	}
};

struct SatSolver {
	SatSolver(int n, vector<vector<int>>& v) : _ST_(n, v) { // throws RealUNSAT
	}
	GlobalStatus _ST_;
	
	void BCP() { // throws SAT, conflict
		while(1) {
			auto clas = _ST_.FindAllUnitClause(); // throws conflict | SAT
			
			if(clas.empty())
				return;
			else
				for (auto cPtr : clas) {
					_ST_.MakeImply(cPtr->getWatched()->getParent(), cPtr->getWatched()->IsNoted() ? F : T, cPtr, _ST_.getCurLevel()-1);
				}
			
		}
	}
	void Solve() { // throws SAT, RealUNSAT
		_ST_.Decision();
		while (1) {
			try {
				try {
					BCP(); // throws conflict, SAT
					_ST_.Decision(); // throws JustBackTrack
				} catch (GlobalStatus::Clause::Conflict& conflict) {
					//if(conflict.level < 0)
					//	throw BT;
					_ST_.UpdateBouncingLevel(conflict.level);
					GlobalStatus::SetOfOnClauseI cla;
					auto tup = _ST_.Learn(conflict.clausePtr, conflict.level, cla);
					_ST_.AddClause(cla);
					if(tup == NULL)
						throw BT;
					else {
						_ST_.BackTo(tup, tup->getLevel());
					}
						
				}
			} catch (JustBackTrack) {
				_ST_.UpdateBouncingLevel();
				_ST_.BackTo(); // throws RealUNSAT
			}
		}
	}

};



void SatfileName_bang(char* s, string& ss) {
	ss.assign(s);
	for(int i=0;i<3;i++) // pop .cnf
		ss.pop_back();
	ss.append("sat");
}
void setArgs(char* s) {
    for(int i=0,len=strlen(s);i<len;i++)
        if(s[i] == 'D')
            USE_DLIS = 1;
        else if(s[i] == 'R')
            USE_RESTART = 1;
        else;
}
int main(int argc, char* args[]) {
	vector<vector<int>> raw_clas;
	int cnt_clas;
	string base;
	parse_DIMACS_CNF(raw_clas, cnt_clas, args[1]);
	SatfileName_bang(args[1],base);
	if(argc == 3 && args[2][0] == '-') {
        setArgs(args[2]+1);
	}
	SatSolver sater(cnt_clas, raw_clas);
	try {
		
		sater.Solve(); // throws SAT RealUNSAT
	} catch (GlobalStatus::SAT& sat) {
		ofstream fout(base);
		fout << "s SATISFIABLE\nv";
		cout << "s SATISFIABLE\nv";
		for(auto& v : sat.lits) {
			fout << ' ' << v->getOutput();
			cout << ' ' << v->getOutput();
		}
		fout << " 0\n";
		cout << " 0\n";
	} catch (RealUNSAT) {
		ofstream fout(base);
		fout << "s UNSATISFIABLE\n";
		cout << "s UNSATISFIABLE\n";
	}
	return 0;
}
