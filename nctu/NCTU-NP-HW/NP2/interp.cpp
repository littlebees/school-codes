#include "MyInclude.h"
#include <sstream>
#include <memory>
#include <mutex>
#include <thread>
#include <netinet/tcp.h>
#define STR string
#define PASS_STR unique_ptr<string>
#define MAKE_PASS_STR(x) make_unique<string>(x)
#define PASS_REQ unique_ptr<Request>
const int MAX_USERS = 30;
const char* wel = "****************************************\n** Welcome to the information server. **\n****************************************\n";
const char* prompt = "% ";
PASS_STR WELCOME = MAKE_PASS_STR(wel);
PASS_STR PROMPT = MAKE_PASS_STR(prompt);
const STR ROOT("/ras");
enum RequestType { EMPTY,WHO,TELL,YELL,NAME,SEND,RECV,LOGIN,LOGOUT };
PASS_STR ERR = MAKE_PASS_STR();
class ENDSERVER : public exception {};
class NOCOMMAND : public exception
{
  public:
      char var[250];
      int len;
      NOCOMMAND(OLD_STR s)
      {
        memset(var,0,250);
        strncpy(var,s,strlen(s));
        len=strlen(var);
      }
      void Setvar(PASS_STR& s)
      {
        strncpy(var,s->c_str(),s->size());
      }
  };

class FDHandler
{
    private:
        int fd;
        char t[10000];
    public:
        void SetFD(int f) {fd=f;}
        PASS_STR Read()
        {
            PASS_STR s = MAKE_PASS_STR();
            s->clear();
            memset(t,0,10000);
            for(int l=0;(l=read(fd,t,500))>0;)
            {
              if(l>=0)
              {
                if(t[l-1]==10)
                {
                    s->append(t,l-1);
                  break;
                }
                else
                    s->append(t,l);
                memset(t,0,l);
              }
            }
            cout << "Read: \n" << *s<< '\n';
            return s;
        }
        void Write(PASS_STR& str)
        {
          if(str==NULL)
            return;
            cout << "Try Write: \n"<<*str<<'\n';
          auto tt = (char*) str->c_str();
          int l=0;
          for(int remain=str->size();remain>0;)
          {
            if((l=write(fd,tt,remain > 500 ? 500 : remain))<=remain)
            {
                if(l<0)
                {
                  cerr << "write error: "<< strerror(errno) << '\n';
                  //throw exception();
                }
                remain-=l;
                tt+=l;
            }
          }
        }
};

class Interp
{
    class WaitingLine
    {
        class Waiting
        {
            public:
                short times;
                PASS_STR data;
                bool clear;
                Waiting(short t,STR& s)
                {
                    times=t,clear=false;
                    data = MAKE_PASS_STR(s);
                }
        };
        private:
            list<shared_ptr<Waiting>> waitingLine;
        public:
            void RescueWaitingLine()
            {
                for(auto w = waitingLine.begin();w!=waitingLine.end();w++)
                {
                    if((*w)->clear)
                    {
                        (*w)->clear=false,(*w)->times=1;
                    }
                }
            }
            bool HasZeroOne()
            {
                for(auto w = waitingLine.begin();w!=waitingLine.end();w++)
                    if((*w)->times==0)
                        return true;
                return false;
            }
            void MinusWaitingTime()
            {
                for(auto w = waitingLine.begin();w!=waitingLine.end();w++)
                    (*w)->times--;
            }
            void WaitOutput(int t,STR& s)
            {
                waitingLine.push_back(make_shared<Waiting>(t,s));
            }
            void DeleteFromWaitingLine()
            {
                waitingLine.remove_if([](shared_ptr<Waiting>& w){return w->clear;});
            }
            PASS_STR LoadWaitedData()
            {
                auto ans = MAKE_PASS_STR();
                for(auto w = waitingLine.begin();w!=waitingLine.end();w++)
                    if((*w)->times==0)
                    {
                        ans->append(*((*w)->data));
                        (*w)->clear=true;
                    }
                return move(ans);
            }
            void ClearWaitingLine()
            {
                waitingLine.clear();
            }
    };
    class Env
    {
        private:
            char myPath[150];
            char myRoot[100];
            char myPATH_MEM[300];
            map<string,string> env;
            void SetPath(OLD_STR val)
            {
                char tmp[150];
                    memset(tmp,0,sizeof tmp);
                    strcpy(tmp,val);
                    char* toks[150];
                    int l=0;
                    for(toks[l]=strtok(tmp,":");(toks[++l]=strtok(NULL,":"))!=NULL;);
                    for(int i=0;i<l;i++)
                    {
                        if(toks[i][0]=='/')
                        {
                            //memset(output,0,sizeof output);
                            //sprintf(output,"ILLEGAL PATH:[%s].\n",toks[i]);
                            return ;
                        }

                    }

                    memset(myPath,0,sizeof myPath);
                    strcpy(myPath,val);
            }
            string getPATHforPrint()
            {
                string s(myPath);
                return s+"\n";
            }
        public:
            Env(const STR& root)
            {
                strcpy(myPath,getenv("PATH"));
                strcpy(myPath,"bin:.");
                strcpy(myRoot,getenv("HOME"));
                strcat(myRoot,root.c_str());
                chdir(myRoot);
            }
            OLD_STR getPATHforEXEC()
            {
                char tmp[150];
                    memset(tmp,0,sizeof tmp);
                    strcpy(tmp,myPath);
                    memset(myPATH_MEM,0,sizeof myPATH_MEM);
                    //parse
                    char* toks[150];
                    int l=0;
                    for(toks[l]=strtok(tmp,":");(toks[++l]=strtok(NULL,":"))!=NULL;);
                    //reconstruct
                    for(int i=0;i<l-1;i++)
                    {
                        strcat(myPATH_MEM,myRoot);
                        strcat(myPATH_MEM,"/");
                        strcat(myPATH_MEM,toks[i]);
                        strcat(myPATH_MEM,":");
                    }
                    strcat(myPATH_MEM,myRoot);
                    strcat(myPATH_MEM,"/");
                    strcat(myPATH_MEM,toks[l-1]);
                    return myPATH_MEM;
            }
            PASS_STR FindVar(OLD_STR s)
            {
                string ss(s);
                auto ans =MAKE_PASS_STR();
                if(!strcmp(s,"PATH"))
                {
                    string tmp="PATH=";
                    return move(MAKE_PASS_STR(tmp+getPATHforPrint()));
                }
                else
                {
                    string ans = ss+"="+env[ss]+"\n";
                    return MAKE_PASS_STR(ans);
                }
            }
            void SetVar(OLD_STR var, OLD_STR val)
            {
                if(!strcmp(var,"PATH"))
                    SetPath(val);
                else
                {
                    string a(var),b(val);
                    env[a]=b;
                }
            }
    };
    private:
        WaitingLine line;
        Env env;
        char* t = new char[10000];
        void ErrorMsg(OLD_STR s,PASS_STR& ret)
        {
          char* strs = new char[100];
          sprintf(strs, "Unknown command: [%s].\n",s);
          ret->assign(strs);
          delete [] strs;
        }
        void ReadFromFD(int fd,PASS_STR& s)
        {
          memset(t,0,10000);
          for(int l=0;(l=read(fd,t,10000))>0;)
          {
            if(l>=0)
            {
              if(t[l]==10||t[l-1]==10)
              {
                  s->append(t,l-1);
                break;
              }
              else
                  s->append(t,l);
              memset(t,0,l);
            }
          }
        }
        void WriteToFD(int fd,PASS_STR& str)
        {
          if(str==NULL)
            return;
          auto t = (char*) str->c_str();
          for(int remain=str->size(),l=0;remain>0;)
          {
            if((l=write(fd,t,remain > 500 ? 500 : remain))<=remain)
            {
                if(l<0)
                {
                  cerr << "write error\n";
                  throw exception();
                }
                remain-=l;
                t+=l;
            }
          }
        }
        int subProcess(char** comd,int len,OLD_STR path,int in,int out,int err)
        {
            pid_t pid;
            int status;
            char *old_path = getenv("PATH");

            if ((pid = fork ()) == 0)
            {
                  dup2 (err, 2);
                  close (err);
                if (in != 0)
                {
                  dup2 (in, 0);
                  close (in);
                }

                if (out != 1)
                {
                  dup2 (out, 1);
                  close (out);
                }

                setenv("PATH", path, 1);
                execvp ((OLD_STR)comd[0], (COMD)comd);
                _exit(314);
            }
            wait(&status);
            setenv("PATH",old_path,1);
            if(WIFEXITED(status) && WEXITSTATUS(status))
              throw NOCOMMAND((OLD_STR)comd[0]);
            return pid;
        }
        PASS_STR EXEC(char** prog,int progL,OLD_STR path,PASS_STR& extra)
        {
            if(progL <= 0)
                return MAKE_PASS_STR();
            int pp[2],pp2[2],pp3[2];
            int a = pipe(pp);
            int b = pipe(pp2);
            int c = pipe(pp3);
            auto output = MAKE_PASS_STR();
            if(extra!=NULL)
                WriteToFD(pp2[1],extra);
            line.MinusWaitingTime();
            if(line.HasZeroOne())
            {
                auto data = line.LoadWaitedData();
                WriteToFD(pp2[1],data);
            }

            if(a==-1||b==-1||c==-1)
            {
                cout << "Pipe die\n";
                cout << strerror(errno) << '\n';
            }
            close(pp2[1]);
            try{
              subProcess(prog,progL,path,pp2[0],pp[1],pp3[1]);
            } catch(NOCOMMAND &e) {
              close(pp[1]);
              close(pp[0]);
              close(pp2[0]);
              close(pp3[0]);
              close(pp3[1]);
              throw e;
            }
            line.DeleteFromWaitingLine();
            close(pp[1]);
            close(pp3[1]);
            ReadFromFD(pp[0],output);
            ReadFromFD(pp3[0],ERR);
            close(pp[0]);
            close(pp2[0]);
            close(pp3[0]);
                int ll = output->size();
                if(ll>0&&(*output)[ll-1]!='\n')
                    output->push_back('\n');
                return output;
        }
    public:
        Interp(const STR& root):env(root) {}
        ~Interp() {delete [] t;}
        void DelayInput(int t,shared_ptr<STR> s)
        {
            line.WaitOutput(t,*s);
        }
        PASS_STR bash(STR& s,PASS_STR& input)
        {
            STR t;
            istringstream iss(s);
            vector<STR> tmp;
            while(iss >> t)
                tmp.push_back(t);
            char* comd[tmp.size()+1];
            int i=0;
            for(auto s = tmp.begin();s!=tmp.end();s++,i++)
            {
                int len = s->size();
                comd[i] = new char[len+1];
                memset(comd[i],0,len+1);
                strncpy(comd[i],s->c_str(),len);
                comd[i][len]='\0';
            }
            comd[i]=NULL;
            try{
                auto ans = move(EXEC(comd,i,env.getPATHforEXEC(),input));
                i=0;
                for(auto s = tmp.begin();s!=tmp.end();s++,i++)
                  delete [] comd[i];
                return ans;
            } catch(NOCOMMAND& e){
                i=0;
                for(auto s = tmp.begin();s!=tmp.end();s++,i++)
                  delete [] comd[i];
                auto output = MAKE_PASS_STR();
                ErrorMsg(e.var,output);
                e.Setvar(output);
                throw e;
            }
        }
        void SetVar(OLD_STR var,OLD_STR val)
        {
            env.SetVar(var,val);
        }
        PASS_STR FindVar(OLD_STR var)
        {
            return move(env.FindVar(var));
        }
        void RescueWaitingLine()
        {
            line.RescueWaitingLine();
        }
};

class HW1
{
    private:
        Interp interp;
        void WriteTo(STR& name,PASS_STR& msg)
        {
            ofstream myfile;
            myfile.open(name);
            myfile << *msg;
            myfile.close();
        }
    public:
        HW1(const STR& s):interp(s) {}
        PASS_STR shell(STR& s,PASS_STR& extra)
        {
            bool inPipe = false;
            istringstream token(s);
            STR tok,prog;
            PASS_STR tmp = MAKE_PASS_STR(*extra);
            for(int i=0;token>>tok;i++)
            {
                if(tok[0]=='|')
                {
                    inPipe = true;
                    int pipeL = tok.size(),waitTime;
                    try{
                        if(pipeL>1)
                        {
                            waitTime = atoi(tok.c_str()+1);
                        }
                        else
                        {
                            waitTime = 1;
                        }
                        interp.DelayInput(waitTime,interp.bash(prog,(tmp)));

                    } catch(NOCOMMAND &e) {
                        interp.RescueWaitingLine();
                        throw;
                    }
                    prog.clear();
                }
                else if(tok[0]=='>')
                {
                    token>>tok;
                    auto tmpa = MAKE_PASS_STR("");
                    WriteTo(tok,tmpa);
                    auto tmpb = interp.bash(prog,tmp);
                    WriteTo(tok,tmpb);
                    prog.clear();
                }
                else if(i==0&&!tok.compare("printenv"))
                {
                    token>>tok;
                    return move(interp.FindVar(tok.c_str()));
                }
                else if(i==0&&!tok.compare("setenv"))
                {
                    string tok2;
                    token>>tok;token>>tok2;
                    interp.SetVar(tok.c_str(),tok2.c_str());
                    return move(MAKE_PASS_STR());
                }
                else if(i==0&&!tok.compare("exit"))
                {
                    prog.clear();
                    throw ENDSERVER();
                }
                else
                {
                    prog.append(tok);
                    prog.push_back(' ');
                }
            }
            try{
                return move(interp.bash(prog,(tmp)));
            } catch(NOCOMMAND &e) {
                if(inPipe)
                  interp.RescueWaitingLine();
                throw;
            }
        }
};

class MySocket
{
    private:
        int port,fd;
        STR ip,desc;
        FDHandler gate;
        mutex m;
        PASS_STR buf;
    public:
        MySocket(int f,struct sockaddr_in& addr)
        {
            fd = f;
            //ip.assign(inet_ntoa(addr.sin_addr));
            //port = (int) ntohs(addr.sin_port);
            ip.assign("CGILAB");
            port = 511;
            desc.append(ip);
            desc.push_back('/');
            desc.append(to_string(port));
            gate.SetFD(fd);
            buf = MAKE_PASS_STR();
            int flag = 1;
            setsockopt(fd, IPPROTO_TCP, TCP_NODELAY, (char *) &flag, sizeof(int));
        }
        STR& GetDesc() {return desc;}
        void Send(PASS_STR& s) {
            lock_guard<mutex> myLock(m);
            //buf->append(*s);}
            gate.Write(s);
        }
        PASS_STR Recv() {return move(gate.Read());}
        void Close() { shutdown(fd,2); ::close(fd);} 
        int GetFD() {return fd;}
        /*
        void BatchSend()
        {
            gate.Write(buf);
            buf->clear();
        }
        bool HasMsg()
        {
            return !buf->empty();
        }*/
};

class Server
{
    class User
    {
        class MailBox
        {
            private:
                PASS_STR tell,broad,mails[MAX_USERS+1];
                mutex t,b;
                User* user;
            public:
                MailBox(User* s):user(s)
                {
                    for(int i=0;i<MAX_USERS;i++)
                        mails[i]=MAKE_PASS_STR();
                }
                bool SetTell(int from,PASS_STR& s)
                {
                    lock_guard<mutex> mLock(t);
                    if(s!=NULL)
                        user->SendTOClient(s);
                    return 1;
                }
                bool Setbroad(PASS_STR& s)
                {
                    lock_guard<mutex> mLock(t);
                    lock_guard<mutex> mLock2(b);
                    if(s!=NULL)
                        user->SendTOClient(s);
                    return 1;
                }
                void SetMail(int id,PASS_STR& s)
                {
                    mails[id]->assign(*s);
                }
                PASS_STR GetMail(int id)
                {
                    auto ans = MAKE_PASS_STR(*mails[id]);
                    mails[id]->clear();
                    return move(ans);
                }
                PASS_STR GetTell() {return move(tell);}
                PASS_STR GetBroad() {return move(broad);}
                bool HasMailFrom(int id)
                {
                    if(id >= MAX_USERS+1)
                        return 0;
                    return !mails[id]->empty();
                }
        };
        private:
            MailBox msgs;
            MySocket socket;
            STR name;
            Server* server;
            int id;
            HW1 hw1Shell;
        public:
            class Request
            {
                public:
                    RequestType type;
                    int toID;
                    bool suss;
                    PASS_STR msg,newName,comd;
                    User* user;
                    Request(User* self,RequestType t)
                    {
                        type = t;
                        user = self;
                    }
            };
            User(int fd,int i,Server* s,const STR& root,struct sockaddr_in& addr):socket(fd,addr),name("(no name)"),id(i),server(s),hw1Shell(root),msgs(this)
            {
                socket.Send(WELCOME);
            }
            STR& GetName() {return name;}
            int GetID() {return id;}
            int GetFD() {return socket.GetFD();}
            void SetName(PASS_STR& s) {name.assign(*s);}
            STR& GetIPDesc() {return socket.GetDesc();}
            void SetTell(int from,PASS_STR& s) {msgs.SetTell(from,s);}
            void SetBroad(PASS_STR& s) {msgs.Setbroad(s);}
            void SetMail(int id,PASS_STR& s) {msgs.SetMail(id,s);}
            PASS_REQ makeReq(RequestType t) { return move(make_unique<Request>(this,t));}
            void SendTOClient(PASS_STR& s) { socket.Send(s); }
            void SendReq(PASS_REQ& req)
            {
                //server->AddRequest(req);
                //thread m(&Server::ExecReq,server,req);
                //m.join();
                server->ExecReq(req);
            }
            PASS_STR Shell(STR& comd)
            {
                istringstream toks(comd);
                STR prog;
                int i=0;
                int sendTO=-1,recvFrom=-1;
                for(STR tok;toks>>tok;i++)
                {
                    if(i==0&&!tok.compare("who"))
                    {
                        auto req = makeReq(WHO);
                        SendReq(req);
                        break;
                    }
                    else if(i==0&&!tok.compare("name"))
                    {
                        auto req = makeReq(NAME);
                        toks>>tok;
                        req->newName = MAKE_PASS_STR(tok);
                        SendReq(req);
                        break;
                    }
                    else if(i==0&&!tok.compare("yell"))
                    {
                        auto req = makeReq(YELL);
                        string s;
                        toks>>tok;
                        int pos = comd.find(tok);
                        s = comd.substr(pos);
                        req->msg = MAKE_PASS_STR(s);
                        SendReq(req);
                        break;

                    }
                    else if(i==0&&!tok.compare("tell"))
                    {
                        auto req = makeReq(TELL);
                        toks>>tok;
                        req->toID = atoi(tok.c_str());
                        string s;
                        toks>>tok;
                        int pos = comd.find(tok);
                        s = comd.substr(pos);
                        req->msg = MAKE_PASS_STR(s);
                        SendReq(req);
                        break;

                    }
                    else if(tok[0]=='>'&&tok.size()>1)
                    {
                        sendTO = atoi(tok.c_str()+1);
                    }
                    else if(tok[0]=='<')
                    {
                        recvFrom = atoi(tok.c_str()+1);
                    }
                    else
                    {
                        prog.append(tok);
                        prog.push_back(' ');
                    }
                }
                PASS_STR output = MAKE_PASS_STR();
                if(recvFrom!=-1)
                {
                    auto req = makeReq(RECV);
                    req->toID = recvFrom;
                    req->comd = MAKE_PASS_STR(comd);
                    bool re = msgs.HasMailFrom(recvFrom);
                    if(re)
                        output = move(msgs.GetMail(recvFrom));
                    req->suss = re;
                    SendReq(req);
                }
                if(!prog.empty())
                {
                    output = hw1Shell.shell(prog,output);
                }

                if(sendTO!=-1)
                {
                    auto req = makeReq(SEND);
                    req->toID = sendTO;
                    req->comd = MAKE_PASS_STR(comd);
                    req->msg = move(output);
                    SendReq(req);
                    return move(MAKE_PASS_STR()); 
                }
                return move(output);
            }
            PASS_STR who()
            {
                PASS_STR ans = MAKE_PASS_STR();
                ans->append(to_string(GetID()));
                ans->push_back('\t');
                ans->append(name);
                ans->push_back('\t');
                ans->append(socket.GetDesc());
                return move(ans);
            }
            void Main()
            {
                try{
                    auto input = socket.Recv();
                    PASS_STR output;
                    try{
                        output = move(Shell(*input));
                    } catch(NOCOMMAND& e) {
                        output = MAKE_PASS_STR(e.var);
                    }
                    if(!ERR->empty())
                    {
                        ERR->push_back('\n');
                        ERR->append(*output);
                        socket.Send(ERR);
                        ERR->clear();
                    }
                    else
                        socket.Send(output);
                } catch(ENDSERVER& e) {
                    auto req = makeReq(LOGOUT);
                    SendReq(req);
                    socket.Close();
                    throw e;
                }
            }
            bool AlreadyGotMailFrom(int id)
            {
                return msgs.HasMailFrom(id);
            }
            void PromptClient()
            {
                socket.Send(PROMPT);
            }
    };
    class UserPool
    {
        private:
            unique_ptr<User> List[MAX_USERS+2];
            int userL = 1;
            const STR root;
            mutex mut;
        public:
            UserPool(const STR& s):root(s) {}
            void AddUser(int fd,Server* s,struct sockaddr_in& addr)
            {
                lock_guard<mutex> mLock(mut);
                int i=1;
                for(;i<userL;i++)
                    if(List[i]==NULL)
                        break;
                List[i] = move(make_unique<User>(fd,i,s,root,addr));
                userL++;
                auto req = List[i]->makeReq(LOGIN);
                List[i]->SendReq(req);
            }
            bool SetName(int i,PASS_STR& name)
            {
                for(int i=1,c=userL;i<MAX_USERS+1&&c!=1;i++)
                    if(List[i]!=NULL)
                    {
                        if(!List[i]->GetName().compare(*name))
                            return 0;
                        c--;
                    }
                List[i]->SetName(name);
                return 1;
            }
            PASS_STR who(int id)
            {
                auto ans = MAKE_PASS_STR("<ID>\t<nickname>\t<IP/port>\t<indicate me>\n");
                for(int i=1,c=userL;i<MAX_USERS+1&&c!=1;i++)
                    if(List[i]!=NULL)
                    {
                        ans->append(*(List[i]->who()));
                        if(i==id)
                        {
                            ans->push_back('\t');
                            ans->append("<-me");
                        }
                        ans->append("\n");
                        c--;
                    }
                return move(ans);
            }
            User* At(int i)
            {
                if(i<MAX_USERS+1)
                    return (List[i].get());
                else
                    return NULL;
            }
            void Tell(int from,int id,PASS_STR& s)
            {
                List[id]->SetTell(from,s);
            }
            void SendMail(int from,int to,PASS_STR& s)
            {
                List[to]->SetMail(from,s);
            }
            void Broadcast(PASS_STR& s)
            {
                for(int i=1,c=userL;i<MAX_USERS+1&&c!=1;i++)
                {
                    if(List[i]!=NULL)
                    {
                        List[i]->SetBroad(s);
                        c--;
                    }
                }
            }
            User* GetByFD(int fd)
            {
                for(int i=1,c=userL;i<MAX_USERS+1&&c!=1;i++)
                    if(List[i]!=NULL&&List[i]->GetFD()==fd)
                    {
                        return (List[i].get());
                        c--;
                    }
                return NULL;
            }
            void Del(User* u)
            {
                lock_guard<mutex> mLock(mut);
                for(int i=1,c=userL;i<MAX_USERS+1&&c!=1;i++)
                    if(List[i]!=NULL&&List[i].get()==u)
                    {
                        List[i] = NULL;
                        c--;
                    }
                userL--;
            }
    };
    private:
        UserPool users;
        int serverFD;
        struct sockaddr_in server_addr;
        socklen_t size;
        void boradcast(PASS_STR& s)
        {
            users.Broadcast(s);
        }
        void tell(int from,int to,PASS_STR& msg)
        {
            users.Tell(from,to,msg);
        }
        void mail(int from,int to,PASS_STR& msg)
        {
            users.SendMail(from,to,msg);
        }
        void sendBack(User* user,PASS_STR& msg)
        {
            int id = user->GetID();
            users.Tell(id,id,msg);
        }
    public:
        Server(int portNum,const STR& s):users(s)
        {
            serverFD = socket(AF_INET, SOCK_STREAM, 0);
            if (serverFD < 0) 
            {
                cout << "\nError establishing socket..." << endl;
                exit(1);
            }
            server_addr.sin_family = AF_INET;
            server_addr.sin_addr.s_addr = htons(INADDR_ANY);
            server_addr.sin_port = htons(portNum);
            if ((::bind(serverFD, (struct sockaddr*)&server_addr,sizeof(server_addr))) < 0) 
            {
                cout << "=> Error binding connection, the socket has already been established..." << endl;
            }
            size = sizeof(server_addr);
            listen(serverFD, MAX_USERS);
        }
        int GetFD() {return serverFD;}
        void ExecReq(unique_ptr<User::Request>& req)
        {
            PASS_STR buf=MAKE_PASS_STR("");
            RequestType t = req->type;
            if(t == LOGIN)
            {
                const char* temp = "*** User '%s' entered from %s. ***\n";
                char plugin[200];
                sprintf(plugin,temp,req->user->GetName().c_str(),req->user->GetIPDesc().c_str());
                buf->assign(plugin);
                boradcast(buf);
            }
            else if(t == LOGOUT)
            {
                const char* temp = "*** User '%s' left. ***\n";
                char plugin[200];
                sprintf(plugin,temp,req->user->GetName().c_str());
                buf->assign(plugin);
                boradcast(buf);
            }
            else if(t == WHO)
            {
                buf->assign(*(users.who(req->user->GetID())));
                sendBack(req->user,buf);
            }
            else if(t == TELL)
            {
                auto user2 = users.At(req->toID);
                if(user2!=NULL)
                {
                    const char* temp = "*** %s told you ***: ";
                    char plugin[200];
                    sprintf(plugin,temp,req->user->GetName().c_str());
                    buf->assign(plugin);
                    buf->append(*(req->msg));
                    buf->push_back('\n');
                    tell(req->user->GetID(),user2->GetID(),buf);
                }
                else
                {
                    const char* temp = "*** Error: user #%d does not exist yet. ***\n";
                    char plugin[200];
                    sprintf(plugin,temp,req->toID);
                    buf->assign(plugin);
                    sendBack(req->user,buf);
                }
            }
            else if(t == YELL)
            {
                const char* temp = "*** %s yelled ***: ";
                char plugin[200];
                sprintf(plugin,temp,req->user->GetName().c_str());
                buf->assign(plugin);
                buf->append(*(req->msg));
                buf->push_back('\n');
                boradcast(buf);
            }
            else if(t == NAME)
            {
                if(users.SetName(req->user->GetID(),(req->newName)))
                {
                    const char* temp = "*** User from %s is named '%s'. ***\n";
                    char plugin[200];
                    sprintf(plugin,temp,req->user->GetIPDesc().c_str(),req->newName->c_str());
                    buf->assign(plugin);
                    boradcast(buf);
                }
                else
                {
                    const char* temp = "*** User '%s' already exists. ***\n";
                    char plugin[200];
                    sprintf(plugin,temp,req->newName->c_str());
                    buf->assign(plugin);
                    sendBack(req->user,buf);
                }
            }
            else if(t == SEND)
            {
                auto user2 = users.At(req->toID);
                if(user2&&!user2->AlreadyGotMailFrom(req->user->GetID()))
                {
                    const char* temp = "*** %s (#%d) just piped '%s' to %s (#%d) ***\n";
                    char plugin[200];
                    sprintf(plugin,temp,req->user->GetName().c_str(),req->user->GetID(),req->comd->c_str(),user2->GetName().c_str(),user2->GetID());
                    buf->assign(plugin);
                    boradcast(buf);
                    mail(req->user->GetID(),user2->GetID(),req->msg);
                }
                else if(!user2)
                {
                    const char* temp = "*** Error: user #%d does not exist yet. ***\n";
                    char plugin[200];
                    sprintf(plugin,temp,req->toID);
                    buf->assign(plugin);
                    sendBack(req->user,buf);
                }
                else
                {
                    const char* temp = "*** Error: the pipe #%d->#%d already exists. ***\n";
                    char plugin[200];
                    sprintf(plugin,temp,req->user->GetID(),req->toID);
                    buf->assign(plugin);
                    sendBack(req->user,buf);
                }
            }
            else if(t == RECV)
            {
                auto user2 = users.At(req->toID);
                if(req->suss)
                {
                    const char* temp = "*** %s (#%d) just received from %s (#%d) by '%s' ***\n";
                    char plugin[200];
                    sprintf(plugin,temp,req->user->GetName().c_str(),req->user->GetID(),user2->GetName().c_str(),user2->GetID(),req->comd->c_str());
                    buf->assign(plugin);
                    boradcast(buf);
    //                mail(req->user->GetID(),user2->GetID(),req->msg);
                }
                else
                {
                    const char* temp = "*** Error: the pipe #%d->#%d does not exist yet. ***\n";
                    char plugin[200];
                    sprintf(plugin,temp,req->toID,req->user->GetID());
                    buf->assign(plugin);
                    sendBack(req->user,buf);
                }

            }
            else 
            {
                cout << "EMPTY";
            }
        }
        int Accept()
        {
            int clientFD = accept(serverFD,(struct sockaddr *)&server_addr,&size);
            users.AddUser(clientFD,this,server_addr);
            return clientFD;
        }
        User* GetUserByFD(int fd)
        {
            return users.GetByFD(fd);
        }
        void DelUser(User* u)
        {
            users.Del(u);
        }
        void PromptUser(int fd)
        {
            auto u = GetUserByFD(fd);
            u->PromptClient();
        }
};

class ThreadBasedServer
{
    private:
        Server s;
    public:
        ThreadBasedServer(int port):s(port,ROOT) {}
        void StartServer()
        {
            while(1)
            {
                int clientFD = s.Accept();
                if(clientFD>0)
                {
                    auto f = [&](const auto& user) {
                            try {
                                while(1)
                                {
                                    user->PromptClient();
                                    user->Main();
                                }
                            } catch(ENDSERVER& e) {
                                s.DelUser(user); 
                            }
                    };
                    thread m(f,s.GetUserByFD(clientFD));
                    m.detach();
                }
            }
        }
};
class SingleProcServer
{
    private:
        Server s;
        fd_set master;
        fd_set read_fds;
        int fdmax,serverFD;
    public:
        SingleProcServer(int port):s(port,ROOT)
        {
            serverFD = s.GetFD();
            FD_ZERO(&master);
            FD_ZERO(&read_fds);
            FD_SET(serverFD, &master);
            fdmax = serverFD;
        }
        void StartServer()
        {
            int clientFD;
            auto data = MAKE_PASS_STR();
            while(1)
            {
               read_fds = master;
               if (select(fdmax+1, &read_fds, NULL, NULL, NULL) == -1)
                   cerr << "select error\n";
               for(int i = 0; i <= fdmax; i++)
               {
                 if (FD_ISSET(i, &read_fds))
                 {
                   if (i == serverFD)
                   {
                       clientFD = s.Accept();
                       if (clientFD == -1)
                       {
                           cerr << "accept error\n";
                       }
                       else
                       {
                           FD_SET(clientFD, &master);
                           if (clientFD > fdmax)
                           {
                               fdmax = clientFD;
                           }
                           s.PromptUser(clientFD);
                       }
                   }
                   else
                   {
                       auto u = s.GetUserByFD(i);
                       try {
                           u->Main();
                           u->PromptClient();
                       } catch(ENDSERVER& e) {
                           s.DelUser(u);
                           FD_CLR(i, &master);
                       }
                   } // END handle data from client
                 } // END got new incoming connection
               } // END looping through file descriptors
            }
        }
};

int main()
{
    STR s;
    int p;
    cout << "Server Type(S/T): ";
    cin >> s;
    cout << "Port: ";
    cin >> p;
    if(!s.compare("T"))
    {
        ThreadBasedServer s(p);
        s.StartServer();
    }
    else
    {
        SingleProcServer s(p);
        s.StartServer();
    }
}
