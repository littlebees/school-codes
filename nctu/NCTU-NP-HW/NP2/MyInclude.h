#include <iostream>
#include <fstream>
#include <map> // conflit
#include <string>
#include <vector>
#include <list>
#include <exception>
#include <errno.h>   // For errno
#include <sys/wait.h>
#include <signal.h>   // For kill() to work
#include <string.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <stdlib.h>
#include <unistd.h>
using namespace std;
typedef char * const * COMD;
typedef char const * OLD_STR;
const int BUFFER_SIZE=10000;
const int COMD_NUMBER=10000;
