#include <stdio.h>
#include <stdlib.h>
#include <sys/socket.h>
#include <sys/types.h>
#include <netdb.h>
#include <sys/wait.h>
#include <netinet/in.h>
#include <signal.h>
#include <unistd.h>
#include <cstring>
#include <string>
#include <iostream>
#include <sstream>
#include <fstream>
#include <cstdarg>
#include <algorithm>
#include <vector>
#include <list>
#include <utility>
#include <arpa/inet.h>
#include <sys/stat.h>
#include <fcntl.h>
using namespace std;

const int BUFFER_SIZE = 150001;
const short DEFAULT_PORT = 4411;
const short MAX_USER = 100;
const short MAX_USER_ID_LEN = 50;
const short MAX_DOMAIN_NAME_LEN = 1024;
const char* CONF_FILE = "socks.conf";

enum SOCKS_TYPE {CONNECT = 1, BIND,DONTCARE};

	void log(string str, bool newline=1) {
		
		cout << str;
        if (newline) {
            cout << endl;
        }
		cout.flush();
	}
	void log2(const char* format,...) {
		va_list argptr;
        va_start(argptr, format);
        vfprintf(stdout, format, argptr);
        va_end(argptr);
        fflush(stdout);
	}
    
	void err(string str) {

		log(str, true);
		perror(str.c_str());
		exit(1);
	}
    
struct MyBuffer
{
    char buffer[BUFFER_SIZE+1];
	void setBuffer(const char* ch) {
		bzero(buffer, BUFFER_SIZE);
		strcpy(buffer, ch);
	}
	
	void resetBuffer() {
		bzero(buffer, BUFFER_SIZE);
	}
	
    void writeWrapper(int sockfd, const char buffer[], size_t size,bool show) {
       ssize_t n = write(sockfd, buffer, size);
        if (n < 0)
            err("write error: " + string(buffer));
        if(show){
            log2("<CONTENT>:\t");
            for(int i=0;i<n && i<4;i++)
                printf("%x ",buffer[i]);
            log("");
        }
    }
    
    ssize_t readWrapper(int sockfd) {
        resetBuffer();
        ssize_t n = read(sockfd, buffer, sizeof(buffer));
        if (n < 0)
            err("read error");
        else if (n == 0)
            return -1;
        return n;
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
struct Packet
{
    unsigned short vn,cd;
    unsigned short dst_port;
    char dst_ip[INET_ADDRSTRLEN];
    char user_id[MAX_USER_ID_LEN+1];
    bool Set(char socksreq[262]) 
    {
        vn = socksreq[0],cd = socksreq[1];
        dst_port = (unsigned short) ntohs(*(unsigned short*)&socksreq[2]);
        sprintf(dst_ip, "%u.%u.%u.%u", (unsigned char)socksreq[4], (unsigned char)socksreq[5], (unsigned char)socksreq[6], (unsigned char)socksreq[7]);
        char* user_id_pch = strtok(&socksreq[8], "\0");
        strncpy(user_id, (user_id_pch == NULL ? "(no name)" : user_id_pch), MAX_USER_ID_LEN);  // USER_ID
        log2("<D_IP>\t:%s\n<D_Port>\t:%hu\n<Command>\t:%s\n", dst_ip, dst_port,cd == 1 ? "Connect" : "Bind");
        return isAllowedToConnect();
    }
bool isAllowedToConnect() {
    SOCKS_TYPE type = (SOCKS_TYPE)cd;
    char* ip = dst_ip;
    ifstream rulefile(CONF_FILE, ifstream::in);
    string ruletype, ruleip;
    if (!rulefile) {
        printf("socks.conf open filed\n");
        return true;
    }
    auto IP = split(ip,'.');
    while (!rulefile.eof()) {
        rulefile >> ruletype >> ruleip;
        size_t ruleipsize = ruleip.size();
        auto ruleIP = split(ruleip,'.');
        
        if ((ruletype == "c" && type == CONNECT) || (ruletype == "b" && type==BIND)) {
            for(int i=0;i<4;i++)
                if(!ruleIP[i].compare("*") || !ruleIP[i].compare(IP[i]))
                    continue;
                else
                    return false;
            return true;
        }
        
    }
    return false;
}
int connectDestHost() {
    int status;
    struct addrinfo hints, *res;
    memset(&hints, 0, sizeof(hints));
    hints.ai_family = AF_INET;
    hints.ai_socktype = SOCK_STREAM;
    hints.ai_flags = AI_PASSIVE;
    hints.ai_protocol = IPPROTO_TCP;
    
    char port[6];
    sprintf(port, "%hu", dst_port);
    if ((status = getaddrinfo(dst_ip, port, &hints, &res)) != 0) {
        log("getaddrinfo: " + string(gai_strerror(status)));
        return -1;
    }
    
    int rsock = socket(AF_INET, SOCK_STREAM, 0);
    if (rsock == -1) {
        log("Socket error: " + string(strerror(errno)));
        return -1;
    }
    
    if (::connect(rsock, res->ai_addr, res->ai_addrlen) == -1) {
        log("Connect error: " + string(strerror(errno)));
        return -1;
    }
    
    freeaddrinfo(res);
    return rsock;
}
};
    
class Server
{
    MyBuffer B;
    int ssock;
    Server(int s):ssock(s) {}
void processRequest() {
    B.readWrapper(ssock);
    
    char socksreq[262];
    memcpy(socksreq, B.buffer, 262);
    
    if (socksreq[0] != 4)
        return;
    log2("SOCKS Request:\n<S_IP>\t:%s\n<S_Port>\t:%hu\n", clientAddr.c_str(),clientPort);
    Packet pkt;

    if (!pkt.Set(socksreq)) {

        log("<Reply>\t:Reject\n");
        sendSockResponse(false, DONTCARE, socksreq);
        return;
    }
    
    log("<Reply>\t:Accept\n");
    switch (pkt.cd) {
        case CONNECT: // CONNECT
        {
            int rsock = -1;

            if ((rsock = pkt.connectDestHost()) == -1) {
                sendSockResponse(false, CONNECT, socksreq);
                return;
            }
            sendSockResponse(true, CONNECT, socksreq);
            redirectData(rsock);

            close(rsock);
            break;
        }
            
        case BIND: // BIND
        {
            int bindsock = 0;
            if ((bindsock = socket(AF_INET, SOCK_STREAM, 0)) < 0) {
                err("[SOCKS_BIND] Cannot open socket!");
            }
            
            struct sockaddr_in ftpsv_addr, ftpcli_addr;
            socklen_t ftpcli_len, ftpsv_len;
            bzero((char*)&ftpsv_addr, sizeof(ftpsv_addr));
            ftpsv_addr.sin_family = AF_INET;
            ftpsv_addr.sin_addr.s_addr = htonl(INADDR_ANY);
            ftpsv_addr.sin_port = 0;
            
            if (::bind(bindsock, (struct sockaddr*) &ftpsv_addr, sizeof(ftpsv_addr)) < 0)
                err("[SOCKS_BIND] Bind error");
            ftpsv_len = sizeof(ftpsv_addr);
            ::getsockname(bindsock, (struct sockaddr*) &ftpsv_addr, &ftpsv_len);
            
            if (::listen(bindsock, 5) < 0)
                err("[SOCKS_BIND] listen error");

            *((unsigned short*)&socksreq[2]) = (ftpsv_addr.sin_port);
            for(int i=4;i<8;i++)
                socksreq[i] = 0x0;
            sendSockResponse(true, BIND, socksreq);

            ftpcli_len = sizeof(ftpcli_addr);
            int newbindsock = ::accept(bindsock, (struct sockaddr *) &ftpcli_addr, &ftpcli_len);
            if (newbindsock == -1)
                err("[SOCKS_BIND] accept error");
            
            char ftpcli_addr_str[INET_ADDRSTRLEN];
            unsigned short ip[4];
            inet_ntop(AF_INET, &(ftpcli_addr.sin_addr), ftpcli_addr_str, INET_ADDRSTRLEN);
            sscanf(ftpcli_addr_str, "%hu.%hu.%hu.%hu", &ip[0], &ip[1], &ip[2], &ip[3]);
            
            *((unsigned short*)&socksreq[2]) = (ftpcli_addr.sin_port);
            for(int i=0;i<4;i++)
                socksreq[4+i] = (unsigned char)ip[i];

            sendSockResponse(true, BIND, socksreq);
            redirectData(newbindsock);
            close(newbindsock);
            
            break;
        }
    }
}

void sendSockResponse(bool isGranted, SOCKS_TYPE type, char* socksreq) {

    char socksresp[8];
    socksresp[0] = 0x00;
    socksresp[1] = isGranted ? 0x5A : 0x5B;
    switch (type) {
        case CONNECT:
        case BIND:
            for (int i = 2; i <= 7; i++) {
                socksresp[i] = socksreq[i];
            }
            break;
            
        default:
            break;
    }
    B.writeWrapper(ssock, socksresp, 8,false);
}


void redirectData(int rsock) {
    int nfds = FD_SETSIZE;
    fd_set rfds,wfds,rs,ws;
    
    FD_ZERO(&rfds);
    FD_ZERO(&wfds);
    FD_ZERO(&rs);
    FD_ZERO(&ws);
    
    FD_SET(ssock, &rs);
    FD_SET(ssock, &ws);
    FD_SET(rsock, &rs);
    FD_SET(rsock, &ws);
    
    char rsockbuffer[BUFFER_SIZE];
    char ssockbuffer[BUFFER_SIZE];
    size_t rsockbuffersize = 0;
    size_t ssockbuffersize = 0;
    bzero(rsockbuffer, BUFFER_SIZE);
    bzero(ssockbuffer, BUFFER_SIZE);

    bool rsockbufempty = true;
    bool ssockbufempty = true;
    struct timeval timeout;
    timeout.tv_sec = 1200;
    int toCheck = 0;
    bool show1 = true;
    
    while (1) {
        
        memcpy(&rfds, &rs, sizeof(rfds));
        memcpy(&wfds, &ws, sizeof(wfds));

        toCheck = select(nfds, &rfds, NULL, NULL, &timeout);
        if (toCheck == 0) {
            break;
        }

        if (ssockbufempty && FD_ISSET(ssock, &rfds)) {

            ssockbuffersize = B.readWrapper(ssock);
            char* readStr = B.buffer;
            for (int i = 0; i < ssockbuffersize; i++) {
                ssockbuffer[i] = readStr[i];
            }
            ssockbufempty = false;
        }
        
        if (rsockbufempty && FD_ISSET(rsock, &rfds)) {

            rsockbuffersize = B.readWrapper(rsock);
            char* readStr = B.buffer;
            for (int i = 0; i < rsockbuffersize; i++) {
                rsockbuffer[i] = readStr[i];
            }
            rsockbufempty = false;
        }
        
        toCheck = select(nfds, NULL, &wfds, NULL, &timeout);
        if (toCheck == 0) {
            break;
        }
        
        if (!ssockbufempty && FD_ISSET(rsock, &wfds)) {
            B.writeWrapper(rsock, ssockbuffer, ssockbuffersize,show1);
            bzero(ssockbuffer, BUFFER_SIZE);
            ssockbufempty = true,show1=false;
        }

        if (!rsockbufempty && FD_ISSET(ssock, &wfds)) {
            B.writeWrapper(ssock, rsockbuffer, rsockbuffersize,false);
            bzero(rsockbuffer, BUFFER_SIZE);
            rsockbufempty = true;
        }
    }
}
public:
    string clientAddr = "(null)";
    unsigned short clientPort = 0;
static void Go(int ssss,string addr,unsigned short port) {
    Server s(ssss);
    s.clientAddr = addr;
    s.clientPort = port;
    s.processRequest();
}
};
int main(int argc, const char * argv[]) {
	
	int sockfd, newsockfd, childpid, portnum;
	struct sockaddr_in client_addr, serv_addr;
    char client_addr_str[INET_ADDRSTRLEN]; // client ip addr
    int client_port;   // client port
	socklen_t client_addr_len;
    
    portnum = argc < 2 ?  DEFAULT_PORT : atoi(argv[1]);
    
    // SIGCHLD to prevnet zombie process
	struct sigaction signal_action;
    signal_action.sa_handler = SIG_DFL;
	signal_action.sa_flags = SA_NOCLDWAIT | SA_RESTART;
    sigaction(SIGCHLD, &signal_action, NULL);
    
	
	if ((sockfd = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		err("Cannot open socket!");

	bzero((char*)&serv_addr, sizeof(serv_addr));
	serv_addr.sin_family = AF_INET;
	serv_addr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_addr.sin_port = htons(portnum);
	
	if (::bind(sockfd, (struct sockaddr*) &serv_addr, sizeof(serv_addr)) < 0)
		err("Bind error");
	
	listen(sockfd, 30);
	
	while (1) {
		client_addr_len = sizeof(client_addr);
		newsockfd = ::accept(sockfd, (struct sockaddr *) &client_addr, &client_addr_len);
        
        inet_ntop(AF_INET, &(client_addr.sin_addr), client_addr_str, INET_ADDRSTRLEN);
        client_port = client_addr.sin_port;

        
		if (newsockfd < 0)
			err("accept error");
        if ((childpid = fork()) < 0) {
            err("fork error");
        } else if (childpid == 0) { // child process
            close(sockfd);
            Server::Go(newsockfd,string(client_addr_str),client_port);
            exit(EXIT_SUCCESS);
        } else {
            close(newsockfd);
        }
	}
	return 0;
}

