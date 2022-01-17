#include <arpa/inet.h>
#include <errno.h>
#include <fcntl.h>
#include <netdb.h>
#include <netinet/in.h>
#include <signal.h>
#include <stdarg.h>
#include <stdio.h> 
#include <stdlib.h>
#include <string.h>
#include <sys/types.h> 
#include <sys/socket.h> 
#include <sys/time.h> 
#include <sys/ipc.h> 
#include <sys/shm.h> 
#include <sys/stat.h>
#include <unistd.h>

#define MAX_LINE 1024
#define MAX_PARAMETERS 5

enum USER_STATUS {OFFLINE, CONNECTING , READING, WRITING};

/*
    void FD_ZERO(fd_set *set);          clear Set<fd>
    void FD_SET(int fd, fd_set *set);   add a fd to Set<fd>
    void FD_CLR(int fd, fd_set *set);   remove a fd from Set<fd>
    int  FD_ISSET(int fd, fd_set *set); check whether Set<fd> contains fd or not
*/

struct User
{
    USER_STATUS status;
    int csock;
    int fileFd;
    struct sockaddr_in fsin;
    char hostInput[MAX_LINE];
    char portInput[MAX_LINE];
    char fileNameInput[MAX_LINE];
    char socksHostInput[MAX_LINE];
    char socksPortInput[MAX_LINE];
    unsigned char vnReply;
    unsigned char cdReply;
    unsigned char portReply[2];
    unsigned char ipReply[4];
};

int readline(int fd, char * ptr, int maxlen){ 
    int  n, rc;
    char  c; 
    
    for (n = 1; n < maxlen; n++) { 
        if ( (rc = read(fd, &c, 1)) == 1) { 
            *ptr++ = c; 
            if (c == '\n')      break;
        }
        else if (rc == 0) { 
            if (n == 1)
                return(0);  /* EOF, no data read */ 
            else                                  
                break;  /* EOF, some data was read */ 
        }
        else{
            *ptr = 0;
            return(-1);  /* error */ 
        }
    } 
    *ptr = 0;
    
    return(n); 
}
void writeTo(int socketfd, char* msg){
    write(socketfd, msg, strlen(msg));
}

const char * globalPercentSymbol = "% ";

int main(){
	//  int lineID=0;
    char *pch;
    char queryString[MAX_LINE];
    
    //  connecting-related variables
    int conn = 0;
    User pool[MAX_PARAMETERS];
    
    //  select-related variables
    int nfds;       //  nfds: file descriptor table size
    fd_set crfds;   //  current  read file descriptor set
    fd_set arfds;   //   active  read file descriptor set
    fd_set cwfds;   //  current write file descriptor set
    fd_set awfds;   //   active write file descriptor set
    
    //  get queryString
    strcpy(queryString, getenv("QUERY_STRING"));
    //strcpy(queryString, "h1=140.113.168.190&p1=8787&f1=t1.txt&sh1=140.113.168.190&sp1=7878&h2=140.113.168.190&p2=8787&f2=t2.txt&sh2=140.113.168.190&sp2=7878&h3=140.113.168.190&p3=8787&f3=t3.txt&sh3=140.113.168.190&sp3=7878&h4=140.113.168.190&p4=8787&f4=t4.txt&sh4=140.113.168.190&sp4=7878&h5=140.113.168.190&p5=8787&f5=t5.txt&sh5=140.113.168.190&sp5=7878");
    
    //  get parameters
    pch = strtok(queryString, "&");
    while(pch!= NULL){
        int index;
        char head = pch[0];
        char neck = pch[1];
        char paraCopy[MAX_LINE];
        
        //  parse parameter
        strcpy(paraCopy, pch);
        *(strchr(paraCopy, '=')) = '\0';
        
        switch(head){
            case 'h':
                index = atoi(paraCopy+1)-1;
                strcpy(pool[index].hostInput, strchr(pch, '=')+1);
                break;
            case 'p':
                index = atoi(paraCopy+1)-1;
                strcpy(pool[index].portInput, strchr(pch, '=')+1);
                break;
            case 'f':
                index = atoi(paraCopy+1)-1;
                strcpy(pool[index].fileNameInput, strchr(pch, '=')+1);
                break;
            case 's':
                //  sh or sp
                index = atoi(paraCopy+2)-1;
                
                if(neck == 'h'){
                    //  sh
                    strcpy(pool[index].socksHostInput, strchr(pch, '=')+1);
                }
                else{
                    //  sp
                    strcpy(pool[index].socksPortInput, strchr(pch, '=')+1);
                }
                
                break;
            default:
                fprintf(stderr, "Unknown parameter!\n");
                fflush(stderr);
                break;
        }
        
        
        pch = strtok(NULL, "&");
    }
    
    //  FIXME: connect to SOCKS server
    //  initialize connecting-related variables
    int initi;
    for(initi=0; initi<MAX_PARAMETERS; initi++){
        //  default value
        pool[initi].csock = -1;
        
        //  if( strcmp(socksPortInput[initi], "") !=0 && strcmp(fileNameInput[initi], "") !=0 ){
        if(strcmp(pool[initi].fileNameInput, "") !=0 ){
            //  valid input row
            
            //  try open file
            pool[initi].fileFd = open(pool[initi].fileNameInput, O_RDONLY);
            
            if(pool[initi].fileFd != -1){
                //  open success
                struct hostent *socksHe;

                if( strcmp(pool[initi].socksHostInput, "") != 0 ){
                    //  SOCKS used
                    socksHe = gethostbyname(pool[initi].socksHostInput);
                }
                else{
                    //  normal used
                    socksHe = gethostbyname(pool[initi].hostInput);
                }
                
                if(socksHe != NULL){
                    //  gethostbyname success
                    int flag;
                    
                    //  set fsin
                    bzero(&(pool[initi].fsin),sizeof(pool[initi].fsin));
                    pool[initi].fsin.sin_family = AF_INET;
                    pool[initi].fsin.sin_addr = *((struct in_addr *)socksHe->h_addr);

                    if( strcmp(pool[initi].socksPortInput, "") != 0 ){
                        //  SOCKS used
                        pool[initi].fsin.sin_port = htons(atoi(pool[initi].socksPortInput));
                    }
                    else{
                        //  normal used
                        pool[initi].fsin.sin_port = htons(atoi(pool[initi].portInput));
                    }
                    
                    //  set csock, value and non-blocking
                    pool[initi].csock = socket(AF_INET, SOCK_STREAM, 0);
                    flag = fcntl(pool[initi].csock, F_GETFL, 0);
                    fcntl(pool[initi].csock, F_SETFL, flag | O_NONBLOCK);
                    
                    //  connect socks server
                    if ( connect(pool[initi].csock, (struct sockaddr *)&(pool[initi].fsin), sizeof(pool[initi].fsin)) == -1 ) {
                        if (errno != EINPROGRESS){
                            perror("connect error");
                            close(pool[initi].csock);
                            pool[initi].csock = -1;
                        }
                    }
                    
                    //  send SOCKS info of ip and port if needed
                    if(strcmp(pool[initi].socksHostInput, "") != 0){
                        //  SOCKS used
                        struct hostent *hostHe;
                        hostHe = gethostbyname(pool[initi].hostInput);
                        
                        if(hostHe != NULL){
                            //  gethostbyname success
                            int iPort;
                            struct sockaddr_in hfsin;
                            char hostIpString[MAX_LINE];
                            
                            //  set hfsin
                            bzero(&hfsin, sizeof(hfsin));
                            hfsin.sin_family = AF_INET;
                            hfsin.sin_addr = *((struct in_addr *)hostHe->h_addr);
                            hfsin.sin_port = htons(atoi(pool[initi].portInput));
                            
                            iPort = atoi(pool[initi].portInput);
                            inet_ntop(AF_INET, (void *)(&hfsin.sin_addr.s_addr), hostIpString, sizeof(hostIpString));
                            
                            //  SOCKS4 request
                            unsigned char end;
                            unsigned char vnRequest;
                            unsigned char cdRequest;
                            unsigned char portRequest[2];
                            unsigned char ipRequest[4];
                            
                            end = 0;    //  null
                            vnRequest = 4;
                            cdRequest = 1;
                            portRequest[0] = (int)(iPort/256);
                            portRequest[1] = iPort%256;
                            sscanf(hostIpString, "%hhu.%hhu.%hhu.%hhu", &ipRequest[0], &ipRequest[1], &ipRequest[2], &ipRequest[3]);
                            
                            //  send SOCKS4_request
                            write(pool[initi].csock, &vnRequest, 1);
                            write(pool[initi].csock, &cdRequest, 1);
                            write(pool[initi].csock, portRequest, 2);
                            write(pool[initi].csock, ipRequest, 4);
                            write(pool[initi].csock, &end, 1);
                        }
                        else{
                            //  gethostbyname fail
                            fprintf(stderr, "host '%s' gethostbyname failed\n", pool[initi].hostInput);
                            fflush(stderr);
                        }
                    }
                }
                else{
                    //  gethostbyname fail
                    fprintf(stderr, "host '%s' gethostbyname failed\n", pool[initi].socksHostInput);
                    fflush(stderr);
                }
            }
            else{
                //  open fail
                fprintf(stderr, "File '%s' open failed\n", pool[initi].fileNameInput);
                fflush(stderr);
            }
        }
    }
    
    //  print header
    printf("Content-Type: text/html\n\n");
    
    //  print static webpage head
    int swebi;
    printf("<html>\n");
    printf("<head>\n");
    printf("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=big5\" />\n");
    printf("<title>Network Programming Homework 3</title>\n");
    printf("</head>\n");
    printf("<body bgcolor=#336699>\n");
    printf("<font face=\"Courier New\" size=2 color=#FFFF99>\n");
    printf("<table width=\"800\" border=\"1\">\n");
    printf("<tr>\n");
    //   <td>140.113.210.145</td><td>140.113.210.145</td><td>140.113.210.145</td><td>140.113.210.145</td><td>140.113.210.145</td>
    for(swebi=0; swebi<MAX_PARAMETERS; swebi++){
        printf("<td>%s</td>", pool[swebi].hostInput);
    }
    printf("</tr>\n");
    printf("<tr>\n");
    //  <td valign=\"top\" id=\"m0\"></td><td valign=\"top\" id=\"m1\"></td>...<td valign=\"top\" id=\"m4\"></td>
    for(swebi=0; swebi<MAX_PARAMETERS; swebi++){
        printf("<td valign=\"top\" id=\"m%d\"></td>", swebi);
    }
    printf("</tr>\n");
    printf("</table>\n");
    
    //  initialize connection status & select-related variables
    nfds = getdtablesize();
    
    FD_ZERO(&arfds);
    FD_ZERO(&awfds);
    FD_ZERO(&crfds);
    FD_ZERO(&cwfds);
    
    int statusi;
    for(statusi=0; statusi<MAX_PARAMETERS; statusi++){
        if( pool[statusi].csock != -1 ){
            //  online connection
            pool[statusi].status = CONNECTING;
            FD_SET(pool[statusi].csock, &arfds);
            FD_SET(pool[statusi].csock, &awfds);
            conn++;
        }
        else{
            //  offline connection
            pool[statusi].status = OFFLINE;
        }
    }
    
    //  SOCKS-reply variables
    
    //  interact with server
    while(conn>0){
        //  copy afds to cfds in size of rfds
        memcpy(&crfds, &arfds, sizeof(crfds));
        memcpy(&cwfds, &awfds, sizeof(cwfds));
        
        //  listen for socket for read/write
        if(select(nfds, &crfds, &cwfds, (fd_set *)NULL, (struct timeval *)NULL) < 0){
            perror("select error");
            return -1;
        }
        
        int handlei;
        for(handlei=0; handlei<MAX_PARAMETERS; handlei++){
            if (pool[handlei].status == CONNECTING && (FD_ISSET(pool[handlei].csock, &crfds) || FD_ISSET(pool[handlei].csock, &cwfds))){
            //  if (status[handlei] == CONNECTING && FD_ISSET(csock[handlei], &crfds)){
                //  CONNECTING: initial setting -> READING
                socklen_t elen = sizeof(errno);
                if(getsockopt(pool[handlei].csock, SOL_SOCKET, SO_ERROR, &errno, &elen) < 0 || errno != 0) {
                    // non-blocking connect failed
                    perror("getsockopt error");
                    return -1;
                }
                
                if(strcmp(pool[handlei].socksHostInput, "") != 0){
                    //  SOCKS used
                    //  read SOCKS_reply
                    read(pool[handlei].csock, &(pool[handlei].vnReply), 1);
                    read(pool[handlei].csock, &(pool[handlei].cdReply), 1);
                    read(pool[handlei].csock, pool[handlei].portReply, 2);
                    read(pool[handlei].csock, pool[handlei].ipReply, 4);
                    
                    if(pool[handlei].cdReply == 90){
                        //  90, granted
                        pool[handlei].status = READING;
                        FD_CLR(pool[handlei].csock, &awfds);
                    }
                    else if(pool[handlei].cdReply == 91){
                        //  91, rejected or failed
                        pool[handlei].status = OFFLINE; 
                        FD_CLR(pool[handlei].csock, &arfds);
                        FD_CLR(pool[handlei].csock, &awfds);
                        conn--;
                        
                        close(pool[handlei].csock);
                        pool[handlei].csock = -1;
                        continue;
                    }
                }
                else{
                    //  normal used
                    pool[handlei].status = READING;
                    FD_CLR(pool[handlei].csock, &awfds);
                }
            }
            else if (pool[handlei].status == READING && FD_ISSET(pool[handlei].csock, &crfds)){
                //  READING: read result from server -("\n% ")-> WRITING
                int len;
                char serverResult[MAX_LINE];
                len = readline(pool[handlei].csock, serverResult, MAX_LINE);
                
                if(len > 0 || (len == -1 && errno == EWOULDBLOCK)){
                    //  read some data from server
                    
                    //  remove endline characters
                    strtok(serverResult, "\r\n");
                    
                    //  status changing decision
                    if( strstr(serverResult, globalPercentSymbol) != NULL ){
                        //  "% " coming
                        pool[handlei].status = WRITING;
                        FD_CLR(pool[handlei].csock, &arfds);
                        FD_SET(pool[handlei].csock, &awfds);
                    }
                    else{
                        //  print serverResult to webpage
                        //  <script>document.all['m1'].innerHTML += "This is a test program<br>";</script>
                        printf("<script>document.all['m%d'].innerHTML += \"%s<br>\";</script>\n", handlei, serverResult);
                        fflush(stdout);
                    }                    
                }
                else{
                    fprintf(stderr, "Read ServerResult Error: len=%d\n", len);
                    fflush(stderr);
                }
            }
            else if (pool[handlei].status == WRITING && FD_ISSET(pool[handlei].csock, &cwfds)){
                //  WRITING: write command to server -("exit")-> OFFLINE or -(other command)-> READING
                int len;
                char command[MAX_LINE];
                char commandCopy[MAX_LINE];
                len = readline(pool[handlei].fileFd, command, MAX_LINE);
                
                if(len > 0){
                    //  read some command from file
                    
                    //  send to server
                    writeTo(pool[handlei].csock, command);
                    
                    //  remove endline characters
                    strtok(command, "\r\n");
                    
                    //  print command to webpage
                    //  <script>document.all['m1'].innerHTML += "% <b>removetag test.html</b><br>";</script>
                    printf("<script>document.all['m%d'].innerHTML += \"%% <b>%s</b><br>\";</script>\n", handlei, command);
                    fflush(stdout);

                    if( strcmp(command, "exit") == 0 ){
                        //  exit
                        pool[handlei].status = OFFLINE;
                        FD_CLR(pool[handlei].csock, &awfds);
                        conn--;
                    }
                    else{
                        //  other command
                        pool[handlei].status = READING;
                        FD_CLR(pool[handlei].csock, &awfds);
                        FD_SET(pool[handlei].csock, &arfds);
                    }
                }
                else{
                    fprintf(stderr, "Read Command Error: len=%d\n", len);
                    fflush(stderr);
                }
            }
        }
    }
    
    //  print static webpage tail
    printf("</font>\n");
    printf("</body>\n");
    printf("</html>\n");
    fflush(stdout);

    return 0;
}

