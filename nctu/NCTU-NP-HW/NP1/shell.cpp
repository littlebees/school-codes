#include "MyInclude.h"
char input[BUFFER_SIZE],output[BUFFER_SIZE];
//char args[1000][BUFFER_SIZE];
//int argc=0;
int clientFD;
char* comd[BUFFER_SIZE];
char* prog[BUFFER_SIZE];
int progL=0;
char myPath[BUFFER_SIZE/100];
char myRoot[BUFFER_SIZE/100];
char myPATH_MEM[BUFFER_SIZE];
char tmp[BUFFER_SIZE];
map<string,string> env;


class ENDSERVER : public exception {};
class NOCOMMAND : public exception
{
  public:
      char var[BUFFER_SIZE];
      int len;
      NOCOMMAND(OLD_STR s)
      {
        strcpy(var,s);
        len=strlen(var);
      }
  };
class ILLEGAL_PATH : public exception {};

struct Waiting
{
        int times;
        char data[BUFFER_SIZE];
};
Waiting waitingLine[1000];
int waitL=0;

int subProcess(COMD comd,int len,OLD_STR path,int in,int out)
{
    pid_t pid;
    int status;
    char *old_path = getenv("PATH");

    if ((pid = fork ()) == 0)
    {
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

int scanner()
{
    int len=0;
    if((comd[0]=strtok(input," "))!=NULL)
    {
        len=1;
        for(;(comd[len]=strtok(NULL," "))!=NULL;len++);
    }
    return len;
}

bool send_all(int socket, void *buffer, size_t length)
{
    char *ptr = (char*) buffer;
    while (length > 0)
    {
        int i = send(socket, ptr, length,0);
        if (i < 1) return false;
        ptr += i;
        length -= i;
    }
    return true;
}

void PrintOutput(bool p,bool e=false)
{
    memset(tmp,0,sizeof tmp);
    strcpy(tmp,output);
    if(p&&!e&&tmp[0]!='\0')
    {
        send_all(clientFD, tmp, strlen(tmp));
    } else if(e)
    {
        send_all(clientFD, tmp, strlen(tmp));
    }
    memset(output,0,sizeof output);
}
void WaitOutput(int t,char* ptr=output)
{
    waitingLine[waitL].times = t;
    strcpy(waitingLine[waitL].data,ptr);
    //cout << R++ << ": " << strlen(output) << " , " << strlen(waitingLine[waitL].data) << '\n';
    waitL++;
}
void MinusWaitingTime()
{
    for(int i=0;i<waitL;i++)
        waitingLine[i].times--;
}
bool HasZeroOne()
{
    for(int i=0;i<waitL;i++)
        if(waitingLine[i].times==0)
            return true;
    return false;
}

void DeleteFromWaitingLine()
{
    int minus = 0;
    for(int i=0;i<waitL;i++)
        if(waitingLine[i].times==0)
            minus++;
    for(int i=0;i<waitL-minus;i++)
        if(waitingLine[i].times==0)
        {
            for(int j=i+1;j<waitL;j++)
                if(waitingLine[j].times!=0)
                {
                    memset(waitingLine[i].data,0,sizeof waitingLine[i].data);
                    waitingLine[i].times = waitingLine[j].times;
                    strcpy(waitingLine[i].data,waitingLine[j].data);
                    memset(waitingLine[j].data,0,sizeof waitingLine[j].data);
                    waitingLine[j].times=0;
                }
        }
        else
            continue;
    waitL=waitL-minus;
}
void LoadWaitedData()
{
    memset(tmp,0,sizeof tmp);
    for(int i=0;i<waitL;i++)
        if(waitingLine[i].times==0)
        {
            char* d = waitingLine[i].data;
            strcat(tmp,d);
        }
}
void ClearProg()
{
   for(int i=0;i<progL;i++)
     prog[i]=NULL;
   progL=0;
}
void ClearWaitingLine()
{
    for(int i=0;i<waitL;i++)
    {
        waitingLine[i].times=0;
        memset(waitingLine[i].data,0,sizeof waitingLine[i].data);
    }
    waitL=0;
}
void ErrorMsg(OLD_STR s,char* ret)
{
  sprintf(ret, "Unknown command: [%s].\n",s);
}

OLD_STR getPATHforEXEC(bool debug=false)
{
    if(!debug)
    {
        memset(tmp,0,sizeof tmp);
        strcpy(tmp,myPath);
        memset(myPATH_MEM,0,sizeof myPATH_MEM);
        //parse
        char* toks[BUFFER_SIZE];
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
    else
        return myPath;
}
string getPATHforPrint()
{
    string s(myPath);
    return s+"\n";
}
void SetPath(char* val,bool debug=false)
{
    if(!debug)
    {
        memset(tmp,0,sizeof tmp);
        strcpy(tmp,val);
        char* toks[BUFFER_SIZE/10];
        int l=0;
        for(toks[l]=strtok(tmp,":");(toks[++l]=strtok(NULL,":"))!=NULL;);
        for(int i=0;i<l;i++)
        {
            if(toks[i][0]=='/')
            {
                memset(output,0,sizeof output);
                sprintf(output,"ILLEGAL PATH:[%s].\n",toks[i]);
                throw ILLEGAL_PATH();
            }

        }

        memset(myPath,0,sizeof myPath);
        strcpy(myPath,val);
    }
}
int pp[2],pp2[2];
void RescueWaitingLine()
{
    for(int i=0;i<waitL;i++)
        waitingLine[i].times++;
}
void EXEC()
{
    if(progL <= 0)
        return;
    memset(tmp,0,sizeof tmp);
    MinusWaitingTime();
    if(HasZeroOne())
    {
        LoadWaitedData();
    }

    int a = pipe(pp);
    int b = pipe(pp2);
    if(a==-1||b==-1)
    {
        cout << "Pipe die\n";
        cout << strerror(errno) << '\n';
    }
        write(pp2[1],tmp,strlen(tmp));
        memset(tmp,0,sizeof tmp);
    close(pp2[1]);
    try{
      subProcess(prog,progL,getPATHforEXEC(),pp2[0],pp[1]);
    } catch(NOCOMMAND &e) {
      close(pp[1]);
      close(pp[0]);
      close(pp2[0]);
      ClearProg();
      ErrorMsg(e.var,output);
      throw;
    }
    DeleteFromWaitingLine();
    close(pp[1]);
    memset(output,0,sizeof output);
    while(read(pp[0],output, sizeof output)>0);
    close(pp[0]);
    close(pp2[0]);
}

void WriteTo(char* s)
{
    ofstream myfile;
    myfile.open(s);
    myfile << output;
    myfile.close();
}

void FindVar(char* s)
{
    string ss(s);
    if(!strcmp(s,"PATH"))
    {
        string tmp="PATH=";
        strcpy(output,(tmp+getPATHforPrint()).c_str());
    }
    else
    {
        strcpy(output,(ss+"="+env[ss]+"\n").c_str());
    }

}

void SetVar(char* var, char* val)
{
    if(!strcmp(var,"PATH"))
        SetPath(val);
    else
    {
        string a(var),b(val);
        env[a]=b;
    }
}

void Interpret(int l)
{
    bool inPipe = false;
    for(int i=0;i<l;i++)
    {
        if(comd[i][0]=='|')
        {
            inPipe = true;
            int pipeL = strlen(comd[i]),waitTime;
            try{
                if(pipeL>1)
                {
                    waitTime = atoi(comd[i]+1);
                }
                else
                {
                    waitTime = 1;
                }
                    EXEC();
                    WaitOutput(waitTime);
                    PrintOutput(false);
            } catch(NOCOMMAND &e) {
                  RescueWaitingLine();
                throw;
            }
            ClearProg();
        }
        else if(comd[i][0]=='>')
        {
            EXEC();
            WriteTo(comd[i+1]);
            PrintOutput(false);
            i++;
            ClearProg();
        }
        else if(!strcmp(comd[i],"printenv"))
        {
            FindVar(comd[i+1]);
            PrintOutput(true);
            i++;
            ClearProg();
        }
        else if(!strcmp(comd[i],"setenv"))
        {
            try{
                SetVar(comd[i+1],comd[i+2]);
                PrintOutput(false);
                i=i+2;
            } catch(ILLEGAL_PATH &e) {
                PrintOutput(true,true);
            }
            ClearProg();
        }
        else if(!strcmp(comd[i],"exit"))
        {
            ClearProg();
            throw ENDSERVER();
        }
        else
        {
            prog[progL++]=comd[i];
        }
    }
    try{
        EXEC();
    } catch(NOCOMMAND &e) {
        if(inPipe)
          RescueWaitingLine();
        throw;
    }
    PrintOutput(true);
    ClearProg();
}
int recv_all(int socket, void *buffer, size_t length)
{
    char *ptr = (char*) buffer;
    int len=0;
    while (length > 0)
    {
        int i = recv(socket, ptr, length,0);
        len+=i;
        if(ptr[i]==10 || ptr[i-1]==10)
        {
            return len;
        }

        if (i < 1) return -1;
        ptr += i;
        length -= i;
    }
}

void ReceiveCommand()
{
    memset(tmp,0,sizeof tmp);
    memset(input,0,sizeof input);
    int len = recv_all(clientFD,tmp,sizeof tmp);
    tmp[len-2]='\0';
    strcpy(input,tmp);
}
void Welcome()
{
  strcpy(output,"****************************************\n** Welcome to the information server. **\n****************************************\n");
  PrintOutput(true);
}
void Prompt()
{
    strcpy(output,"% ");
    PrintOutput(true);
}
void Bye()
{
    strcpy(output,"Bye\n");
    PrintOutput(true);
}
void Go()
{
    strcpy(myPath,getenv("PATH"));
    strcpy(myPath,"bin:.");
    strcpy(myRoot,getenv("HOME"));
    strcat(myRoot,"/ras");
    chdir(myRoot);
    Welcome();

        try {
            while(1)
            {
                Prompt();
                int l;
                ReceiveCommand();
                if(input[0]!=10)
                {

                    l = scanner();
                    try{
                      Interpret(l);
                    } catch(NOCOMMAND &e)
                    {
                        PrintOutput(true,true);
                    }
                }
            }
        } catch(ENDSERVER &e) {
            shutdown(clientFD,2);
        }
}
int main()
{
    int client,portNum=2017;
    struct sockaddr_in server_addr;
    socklen_t size;
    client = socket(AF_INET, SOCK_STREAM, 0);
    if (client < 0) 
    {
        cout << "\nError establishing socket..." << endl;
        exit(1);
    }
    server_addr.sin_family = AF_INET;
    server_addr.sin_addr.s_addr = htons(INADDR_ANY);
    server_addr.sin_port = htons(portNum);
    if ((bind(client, (struct sockaddr*)&server_addr,sizeof(server_addr))) < 0) 
    {
        cout << "=> Error binding connection, the socket has already been established..." << endl;
        return -1;
    }
    size = sizeof(server_addr);
    listen(client, 1);
    while(1)
    {
        clientFD = accept(client,(struct sockaddr *)&server_addr,&size);
        dup2(clientFD,STDERR_FILENO);
        Go();
        ClearProg();
        ClearWaitingLine();
        memset(tmp,0,sizeof tmp);
        memset(output,0,sizeof output);
        cout << "close\n";
    }
    close(client);
}
