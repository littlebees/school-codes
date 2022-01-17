#include <memory>
#include <map>
#include <string>
#include <vector>
#include <iterator>
#include <iostream>
#include <sstream>
#include <fstream>
#include <functional>
#include <cstring>
#include <sys/socket.h>
#include <sys/types.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <fcntl.h>
#include <ctime>
using namespace std;
#define LAMBDA_u [&](const auto& u)

#define MAKE_REF(name,init) auto* name = &(init)
#define CHANGE_REF(name,val) name = &(val)

#define STR string
#define MY_KEY string
#define MY_VAL string
#define GETS_ARGS map<string,string>

#define PASS(x) unique_ptr<x>
#define DEPASS(x) (*x)
#define MAKE_PASS(x,y) make_unique<x>(y)
#define MAKE_PASS_EMPTY(x) make_unique<x>()

PASS(STR) HEADER = MAKE_PASS(STR,"Content-Type: text/html\n\n<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=big5\" /><title>Network Programming Homework 3</title></head>");
PASS(STR) BEGIN_BODY = MAKE_PASS(STR,"<body bgcolor=#336699><font face=\"Courier New\" size=2 color=#FFFF99>");
PASS(STR) END_BODY = MAKE_PASS(STR,"</font></body></html>");
PASS(STR) BEGIN_TABLE = MAKE_PASS(STR,"<table width=\"800\" border=\"1\"><tr>");
PASS(STR) END_TABLE = MAKE_PASS(STR,"</tr></table><span id=\"log\"style=\"color: azure;\"></span>");
const short MAX_USERS = 5;
const map<STR,STR> ESCAPE_TABLE = {{"&","&amp;"},{"\"","&quot;"},{"'","&apos;"},{" ","&nbsp;"},{"<","&lt;"},{">","&gt;"},{"\n","<br>"},{"\r",""}};
const vector<STR> ESCAPE_ORDER = {"&","\"","'"," ","<",">","\n","\r"};
enum UserState {F_CONNECTING, F_READING, F_WRITING, F_DONE, EXIT};
class FDHandler
{
    private:
        int fd;
        char t[50000];
    public:
        void SetFD(int f) {fd=f;}
        PASS(STR) Read()
        {
            PASS(STR) s = MAKE_PASS_EMPTY(STR);
            s->clear();
            memset(t,0,50000);
            int l = read(fd,t,50000);
            s->append(t,l);
            return s;
        }
        void Write(PASS(STR)& str)
        {
          if(str==NULL)
            return;
          auto tt = (char*) str->c_str();
          int l=0;
          for(int remain=str->size();remain>0;)
          {
            if((l=write(fd,tt,remain))<=remain)
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
PASS(STR) EscapeString(PASS(STR)& s)
{
	int pos;
	//auto ans = MAKE_PASS(STR,*s);
  STR tmp,it(*s);
  for(const auto& key : ESCAPE_ORDER)
  {
    auto& val = ESCAPE_TABLE.at(key);
		//for(int pos = ans->find(key);pos!=string::npos;pos=ans->find(key,pos+1))
		//	ans->replace(pos,1,val);
    for(const auto& c : it)
    {
      if(c == key[0])
        tmp.append(val);
      else
        tmp.push_back(c);
    }
    it.assign(tmp);
    tmp.clear();
  }
  auto ans = MAKE_PASS(STR,it);
	return move(ans);
}
PASS(STR) Bold(PASS(STR)& s)
{
	STR ans("<b>");
	ans.append(*s);
	ans.append("</b>");
	return move(MAKE_PASS(STR,ans));
}
void _GoToWeb(PASS(STR)& s)
{
	cout << *s << std::flush;
}
// Destructor
class UserPool
{
	class User
	{
		private:
			STR ip,port,domID;
			UserState state = F_CONNECTING;
			FDHandler gate;
			int sock;
			bool is_done = false;
			STR BEGIN_MSG;
			ifstream file;
		public:
			User(STR& i,STR& p,STR& f,STR& d)
			{
				ip.assign(i);
				port.assign(p);
				domID.assign(d);
				BEGIN_MSG.append("<script>document.all['");
				BEGIN_MSG.append(domID);
				BEGIN_MSG.append("'].innerHTML += \"");
				file = ifstream(f, ifstream::in);
			}
			~User() {
                if(file.is_open())
                    file.close();
                close(sock);}
			STR GetIP() {return ip;}
			STR GetDOMid() {return domID;};
			UserState GetState() {return state;}
			void SetSock(int s)
			{
				sock=s;
				gate.SetFD(s);
			}
			void TransferState(bool is_prompt=false)
			{
				if(state == F_CONNECTING)
					state = F_READING;
				else if(state == F_READING)
					if(is_prompt)
						state = F_WRITING;
					else if(is_done)
						state = F_DONE;
                    else
                        ;
				else if(state == F_WRITING)
					state = F_READING;
				else
					;
			}
			PASS(STR) Read() {return move(gate.Read());}
			void Write(PASS(STR)& s) {gate.Write(s);}
			PASS(STR) WrapFrame(PASS(STR)& s)
			{
				auto ans = MAKE_PASS(STR,BEGIN_MSG);
				ans->append(DEPASS(s));
				ans->append("\";</script>\n");
				return move(ans);
			}
			bool connect()
			{
				int status;
			    struct addrinfo hints, *res;
			    memset(&hints, 0, sizeof(hints));
			    hints.ai_family = AF_INET;
			    hints.ai_socktype = SOCK_STREAM;
			    hints.ai_flags = AI_PASSIVE;
			    hints.ai_protocol = IPPROTO_TCP;
			    
			    if ((status = getaddrinfo(ip.c_str(), port.c_str(), &hints, &res)) != 0) {
			        cerr<<"getaddrinfo: " + string(gai_strerror(status))<<'\n';
			        return false;
			    }

			    int sockfd = socket(AF_INET, SOCK_STREAM, 0);
			    if (sockfd == -1) {
			        cerr<<"Socket error: " + string(strerror(errno))<<'\n';
			        return false;
			    }
			    // set to nonblocking
			    //int flags = fcntl(sockfd, F_GETFL, 0);
			    fcntl(sockfd, F_SETFL, O_NONBLOCK);
			    SetSock(sockfd);
			    
			    if (::connect(sockfd, res->ai_addr, res->ai_addrlen) == -1 && errno != EINPROGRESS) {
			        cerr << "Connect error: " + string(strerror(errno)) << '\n';
			        return false;
			    }
			    
			    freeaddrinfo(res);
			    return true;
			}
			int getSockFd() {return sock;}
			bool hasSentExit() {return is_done;}
			void Done() {is_done=true;}
			PASS(STR) GetLineFromFile()
			{
				STR tmp;
				getline(file,tmp);
				tmp.push_back('\n');
				return move(MAKE_PASS(STR,tmp));
			}
			void GoToWeb(PASS(STR)& s,bool bold=false)
			{
				auto forFuckingCompiler = EscapeString(s);
				auto forFuckingCompiler2 = bold ? Bold(forFuckingCompiler) : move(forFuckingCompiler);
				auto ans = WrapFrame(forFuckingCompiler2);
				_GoToWeb(ans);
			}
	};
	private:
		User* pool[MAX_USERS];
		short userL=0;

		template<typename F>
		void ForAllUserDo(F& func)
		{
			for(int i=0;i<userL;i++)
				if(pool[i]!=NULL)
					func(pool[i]);
		}
	public:
        ~UserPool()
        {
            auto ClearUsers = LAMBDA_u{
                delete u;
            };
            ForAllUserDo(ClearUsers);
        }
		PASS(STR) GetTableInHTML()
		{
			auto ans = MAKE_PASS(STR,*BEGIN_TABLE);
			STR headers,datas("</tr><tr>");
			auto func = LAMBDA_u{
				headers.append("<td>");
				headers.append(u->GetIP());
				headers.append("</td>");
				datas.append("<td valign=\"top\" id=\"");
				datas.append(u->GetDOMid());
				datas.append("\"></td>");
			};
			ForAllUserDo(func);
			ans->append(headers);
			ans->append(datas);
			ans->append(*END_TABLE);
			return move(ans);
		}
		void AddUser(STR& ip,STR& port,STR& file)
		{
			STR dom("m");
			dom.append(to_string(userL));
			pool[userL] = new User(ip,port,file,dom);
			userL++;
		}
		void HandleAllReqFromUsers()
		{
			int connections = userL;
    		int nfds = FD_SETSIZE;
    		fd_set rfds,wfds,rs,ws;

		    FD_ZERO(&rfds);
    		FD_ZERO(&wfds);
   			FD_ZERO(&rs);
    		FD_ZERO(&ws);
    		auto SetFDs = LAMBDA_u{
    			auto fd = u->getSockFd();
    			FD_SET(fd, &rs);
        		FD_SET(fd, &ws);
    		};
    		ForAllUserDo(SetFDs);

    		auto Handler = LAMBDA_u{
    			auto sockFD = u->getSockFd();
    			switch(u->GetState())
    			{
    				case F_CONNECTING:
    				{
    					if(FD_ISSET(sockFD, &rfds) || FD_ISSET(sockFD, &wfds))
    					{
    						// error handle??
    						u->TransferState();
	                		FD_CLR(sockFD, &ws);
    					}
    				}
    				break;
    				case F_READING:
    				{
    					if(FD_ISSET(sockFD, &rfds))
    					{
    						auto strRead = u->Read();
	                
	                		// write to webpage
	                		u->GoToWeb(strRead);
	                
	                		if (strRead->find("% ") != string::npos) {
	                	    	u->TransferState(true);
	                	    	FD_CLR(sockFD, &rs);
	                    		FD_SET(sockFD, &ws);
	                		} else if (u->hasSentExit()) {
	                    		u->TransferState();
	                    		FD_CLR(sockFD, &rs);
	                    		connections--;
	                		}
    					}
    				}
    				break;
    				case F_WRITING:
    				{
    					auto input = u->GetLineFromFile();
	                
	                	// if exit is sent, exit after next write
	                	if (input->find("exit") == 0)
	                    	u->Done();
	                
	                	// write to webpage
	                	u->GoToWeb(input, true);
	                	u->Write(input);
	                
	                	u->TransferState();
	                	FD_CLR(sockFD, &ws);
	                	FD_SET(sockFD, &rs);
    					
    				}
    				break;
    				default:
    					break;
    			}
    		};
    		while(connections > 0)
    		{
    			memcpy(&rfds, &rs, sizeof(rfds));
        		memcpy(&wfds, &ws, sizeof(wfds));
        		select(nfds, &rfds, &wfds, NULL, 0);
    			ForAllUserDo(Handler);
    		}
    		
		}
		void ConnectAllUsers()
		{
			auto f = LAMBDA_u{
				u->connect();
			};
			ForAllUserDo(f);
		}
};

template<typename Out>
void split(const std::string &s, char delim, Out result) {
    std::stringstream ss(s);
    std::string item;
    while (std::getline(ss, item, delim)) {
        *(result++) = item;
    }
}

std::vector<std::string> split(const std::string &s, char delim) {
    std::vector<std::string> elems;
    split(s, delim, std::back_inserter(elems));
    return elems;
}

class CGI
{
	private:
		UserPool pool;
		PASS(GETS_ARGS) GetArgsFromGET()
		{
			STR fromGET(getenv("QUERY_STRING"));
			PASS(GETS_ARGS) ans = MAKE_PASS_EMPTY(GETS_ARGS);
            auto vec = split(fromGET,'&');
            for(const auto& s : vec)
            {
                auto v = split(s,'=');
                if(v.size()==2)
                    DEPASS(ans)[v[0]] = v[1];
            }
			return move(ans);
		}
		void AddUserByGET(GETS_ARGS& gets)
		{
			for(int i=1;i<=MAX_USERS;i++)
			{
				STR IPKey("h");
				STR I = to_string(i);
				IPKey.append(I);
				if(!gets[IPKey].empty())
				{
					STR portKey("p"),fileKey("f");
					portKey.append(I);
					fileKey.append(I);
					pool.AddUser(gets[IPKey],gets[portKey],gets[fileKey]);
				}
				else
					break;
			}
		}
		CGI()
		{
			auto gets = GetArgsFromGET();
			AddUserByGET(*gets);
		}
	public:
        static void Start()
        {
            CGI cgi;
            cgi.Go();
        }
		void Go()
		{
			auto forFuckingCompiler = pool.GetTableInHTML();
			_GoToWeb(forFuckingCompiler);
			pool.ConnectAllUsers();
			pool.HandleAllReqFromUsers();
		}
};


int main()
{
	//send header, table from pool, frames,footer
	_GoToWeb(HEADER);
	_GoToWeb(BEGIN_BODY);
    CGI::Start();
	_GoToWeb(END_BODY);
}
