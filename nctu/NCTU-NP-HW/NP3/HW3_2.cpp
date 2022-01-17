#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/wait.h>
#include <netinet/in.h>
#include <signal.h>
#include <unistd.h>
#include <cstring>
#include <string>
#include <thread>
#include <memory>
#include <iostream>
#include <sstream>
#include <fstream>
#include <vector>
#include <map>
#include <utility>
#include <sys/stat.h>
#include <fcntl.h>
using namespace std;

#define MAX_SIZE 15001
#define DEFAULT_PORT 4411

//#define DEBUG 1

/// HW3 Related
#define PROTOCOL "HTTP/1.1"
#define SERVER "NP_SERVER/44.10"
#define HEADER_SIZE 8192

#define BUFFER_DONT_RESET (1 << 0)

char buffer[MAX_SIZE];

#define LAMBDA_u [&](const auto& u)

#define MAKE_REF(name,init) auto* name = &(init)
#define CHANGE_REF(name,val) name = &(val)

#define STR string
#define MY_KEY string
#define MY_VAL string
#define MIME_TABLE map<string,string>

#define PASS(x) unique_ptr<x>
#define DEPASS(x) (*x)
#define MAKE_PASS(x,y) make_unique<x>(y)
#define MAKE_PASS_EMPTY(x) make_unique<x>()
const STR MIME_CGI(".cgi");
const MIME_TABLE MIME = {{".cgi",MIME_CGI},{".txt","text/plain"},{".html","text/html"},{".htm","text/html"},{".jpg","image/jpeg"},{".jpeg","image/jpeg"},{".gif","image/gif"},{".png","image/png"},{".ico","image/jpeg"} };

PASS(STR) MakeHeader(int status,const STR& desc,const STR& mime)
{
    const auto Is_CGI = [](auto& s){return !MIME_CGI.compare(s);};
    const STR HEADER_END("\r\n");
    const STR SPACE(" "); 
    vector<STR> header({PROTOCOL,SPACE,to_string(status),SPACE,desc,HEADER_END,
                        "Server: ",SERVER,HEADER_END,
                        "Connection: close",HEADER_END});
    if(!Is_CGI(mime))
    {
        header.push_back("Content-Type: ");
        header.push_back(mime);
        header.push_back(HEADER_END);
        header.push_back("\n");
    }
    PASS(STR) ans = MAKE_PASS_EMPTY(STR);
    for(const auto& s : header)
        ans->append(s);
    return move(ans);
}

PASS(STR) MakeErrorPage(int status,const STR& desc)
{
    STR SPACE(" ");
    const STR HTML_KEY(".html");
    auto ans = MakeHeader(status, desc, MIME.at(HTML_KEY));
    vector<STR> page({"<html><title>",to_string(status),SPACE,desc,"</title>",
                      "<body><h1>", to_string(status), SPACE, desc, "</h1></body></html>"});
    for(const auto& s : page)
        ans->append(s);
    return move(ans);
}


STR GetExt(STR& s)
{
    STR tmp(s.substr(s.find(".")));
    auto pos = tmp.find("?");
    STR key;
    if(pos!=STR::npos)
    {
        tmp.erase(pos);
        key.assign(tmp);
    }
    else
        key.assign(tmp);
        cout << key << '\n';
    return MIME.at(key);
}

int SetupConnect()
{
    int sockfd, newsockfd, childpid, portnum;
    struct sockaddr_in serv_addr;
    socklen_t clilen;
    cout << "PORT: ";
    cin >> portnum;
    if ((sockfd = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
        cerr<<("Cannot open socket!\n");
    }
    bzero((char*)&serv_addr, sizeof(serv_addr));
    serv_addr.sin_family = AF_INET;
    serv_addr.sin_addr.s_addr = htonl(INADDR_ANY);
    serv_addr.sin_port = htons(portnum);
    
    if (::bind(sockfd, (struct sockaddr*) &serv_addr, sizeof(serv_addr)) < 0) {
        cerr<<("Bind error\n");
    }
    listen(sockfd, 5);
    return sockfd;
}

PASS(STR) Read(int cFD)
{
    PASS(STR) s = MAKE_PASS_EMPTY(STR);
    s->clear();
    char t[50000];
    memset(t,0,50000);
    int l = read(cFD,t,50000);
    s->append(t,l);
    //for(int l=0;(l=read(cFD,t,500))>0;)
    //{
    //  if(l>=0)
    //  {
    //    if(t[l-1]==10)
    //    {
    //        s->append(t,l-1);
    //        break;
    //    }
    //    else
    //        s->append(t,l);
    //    memset(t,0,l);
    //  }
    //}
    return move(s);
}

void Write(int cFD,PASS(STR)& s)
{
    if(s==NULL)
        return;
    auto tt = (char*) s->c_str();
    int l=0;
    for(int remain=s->size();remain>0;)
    {
        if((l=write(cFD,tt,remain > 500 ? 500 : remain))<=remain)
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

void ParseHeader(PASS(STR)& req,STR& action,STR& param,STR& path,STR& mime)
{
    istringstream ss(DEPASS(req));
    STR path_param;
    ss>>action;
    ss>>path_param;
    auto pos = path_param.find("?");
    if(pos!=string::npos)
    {
        param.assign(path_param.substr(pos+1));
        cout << param << '\n';
        path.assign(path_param.substr(0,pos));
    }
    else
        path.assign(path_param);
    path.erase(0,1);
    if(path.empty())
        path.assign("form_get.htm");
    mime.assign(GetExt(path));
}

PASS(STR) VerifyHeader(STR& act,STR& path)
{
    const STR METHOD("Method Not Allowed"),FORBIDDEN_DIR("Forbidden (parent directory)"),FORBIDDEN_LIST("Forbidden (directory listing)"),FOUND("Not Found");
    if (act.compare("GET")) {
        auto a = MakeErrorPage(405, METHOD);
        return move(a);
    }
    
    // validate path
    int len = path.size();
    for (int i = 1; i < len; i++) {
        if (path[i-1] == '.' && path[i] == '.') {
            auto a = MakeErrorPage(403, FORBIDDEN_DIR);
            return move(a);
        }
    }
    
    struct stat statbuf;
    if (stat(path.c_str(), &statbuf) == -1) {
        auto a = MakeErrorPage(404, FOUND);
        return move(a);
    } else if (S_ISDIR(statbuf.st_mode)) {
        auto a = MakeErrorPage(403, FORBIDDEN_LIST);
        return move(a);
    }
    return move(MAKE_PASS_EMPTY(STR));
}

void HandleReq(int cFD)
{
    auto req = Read(cFD);
    STR action,param,path,mime;
    ParseHeader(req,action,param,path,mime);
    auto errorPage = VerifyHeader(action,path);
    if(errorPage->empty())
    {
        if(!param.empty())
            setenv("QUERY_STRING", param.c_str(), 1);
        auto header = MakeHeader(200,"OK",mime);
        Write(cFD,header);
        if(!MIME_CGI.compare(mime))
        {
            int pid,status;
            switch ((pid=fork())) {
            case -1:
                break;            
            case 0: // child
                dup2(cFD, STDOUT_FILENO);
                if (execl(path.c_str(), NULL) == -1) {
                    cerr<<("Child exec error\n");
                }
            default:
                waitpid(pid,&status,WNOHANG);
                break;
        }
        }
        else
        {
            int fileFd = open(path.c_str(), O_RDONLY);
            int readLen;
            char buffer[10000];
            memset(buffer,0,sizeof(buffer));
            while ((readLen = read(fileFd, buffer, sizeof(buffer))))
            {
                auto str = MAKE_PASS_EMPTY(STR);
                str->assign(buffer,readLen);
                Write(cFD,str);
            }
        }
    }
    else
    {
        Write(cFD,errorPage);
    }
    
}

int main(int argc, const char * argv[]) {
	struct sockaddr_in cli_addr;
	int fd = SetupConnect();
    auto Accept = [&](const int& cFD){
        HandleReq(cFD);
        close(cFD);
    };
    while(1)
    {
        socklen_t clilen = sizeof(cli_addr);
        int cfd = ::accept(fd, (struct sockaddr *) &cli_addr, &clilen);
        if(cfd>=0)
        {
            thread t(Accept,cfd);
            t.detach();
        }
    }
	return 0;
}
