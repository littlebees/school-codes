#include <windows.h>
#include <list>
#include <string.h>
#include <stdlib.h>
using namespace std;

#include "resource.h"

#define SERVER_PORT 7799
#define BUFLEN 10000
#define WM_SOCKET_NOTIFY (WM_USER + 1)
#define MAXFORM 5
#define MAXCMD 15000
#define LAMNDA_u [&](const auto& u)
#define OLD_S char*
const OLD_S Q_MARK = " /?";
const OLD_S AND_MARK = "&";
const OLD_S CGI_FORM_NAME = "form_get.htm";
const OLD_S CGI_FILE_NAME = "hw3.cgi";
OLD_S HTTP_OK = "HTTP/1.0 200 OK\n\n";
OLD_S ERROR_404 = "HTTP/1.0 404 Not Found\n";
OLD_S HTML_HEAD = "<html>\n<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=big5\" />\n";
OLD_S HTML_TITLE = "<title>Network Programming Homework 3</title>\n</head>\n<body bgcolor=#336699>\n";
OLD_S TABLE_HEAD = "<font face=\"Courier New\" size=2 color=#FFFF99>\n<table width=\"800\" border=\"1\">\n<tr>\n";
OLD_S HTML_ROW = "\n<tr>\n";
OLD_S TABLE_END = "</tr>\n</table>\n";
OLD_S RES_TEMPLETE = "<script>document.all['%s'].innerHTML += \"%s<br/>\";</script>\n\0";
OLD_S HTTP_GET_ACTION = "GET";
enum RES_TYPE  { CGI,CGI_FORM };
int websock, connect_cnt;
int Send(int fd, OLD_S s)
{
	return send(fd, s, strlen(s), 0);
}
bool PartialCompare(char* a, const char* b)
{
	return !strncmp(a, b, strlen(b));
}
char *str_replace(char *source, char *find, char *replace);
struct client
{
	char hostIP[30];
	int port;
	char fileName[30];
	int sockfd;
	int filefd;
	FILE *fd;
	char terminalID[3];
	char cmdBuf[MAXCMD];
	char printCmd[MAXCMD];
	bool status;
	bool unsend;
	int wNeed;
	int Connect()
	{
		//handle socket
		struct sockaddr_in client_sin;
		struct hostent *he;
		if ((he = gethostbyname(hostIP)) == NULL) exit(1);
		sockfd = socket(AF_INET, SOCK_STREAM, 0);
		memset(&client_sin, 0, sizeof(client_sin));
		client_sin.sin_family = AF_INET;
		client_sin.sin_addr = *((struct in_addr *)he->h_addr);
		client_sin.sin_port = htons(port);
		if (connect(sockfd, (struct sockaddr *)&client_sin, sizeof(client_sin)) < 0) {
			if (errno != EINPROGRESS) return 0;
		}
		//open file
		fd = fopen(fileName, "r");
		//cli->filefd = fileno(cli->fd);
		return 1;
	}

	int recv_msg()
	{
		char buf[MAXCMD], msg[MAXCMD], *tmp, *result;
		int len, i;

		len = recv(sockfd, buf, sizeof(buf) - 1, 0);
		if (len < 0) return -1;
		buf[len] = 0;
		if (len > 0)
		{
			result = str_replace(buf, "<", "&lt;");
			result = str_replace(result, ">", "&gt;");
			for (tmp = strtok(result, "\n\r"); tmp; tmp = strtok(NULL, "\n\r"))
			{
				if (strncmp(tmp, "% ", 2) == 0) {
					status = 1;
					//printf("write unlock<br/>");
					sprintf(msg, "<script>document.all['%s'].innerHTML += \"%s\";</script>\n", terminalID, tmp);
					send(websock, msg, strlen(msg), 0);
				}
				else {
					sprintf(msg, "<script>document.all['%s'].innerHTML += \"%s<br/>\";</script>\n", terminalID, tmp);  // echo input
					send(websock, msg, strlen(msg), 0);
				}
			}
		}
		fflush(stdout);
		return len;
	} //end recv_msg
	template<typename Lambda>
	void Apply(Lambda f)
	{
		f(this);
	}
};

struct UserPool
{
	client cli_table[MAXFORM];
	template<typename Lambda>
	void SelectDo(WPARAM &wParam, Lambda f)
	{
		for (int i = 0; i < MAXFORM; i++) {
			if (cli_table[i].sockfd == wParam)
			{
				cli_table[i].Apply(f);
			}
		}
	}
	template<typename Lambda>
	void ForAllDo(Lambda f)
	{
		for (int i = 0; i < MAXFORM; i++)
			cli_table[i].Apply(f);
	}
};
UserPool pool;

void response(int, HWND hwnd, RES_TYPE);
void get_query(char *get, HWND hwndEdit);



BOOL CALLBACK MainDlgProc(HWND, UINT, WPARAM, LPARAM);
int EditPrintf(HWND, TCHAR *, ...);
list<SOCKET> Socks;

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance,
	LPSTR lpCmdLine, int nCmdShow)
{

	return DialogBox(hInstance, MAKEINTRESOURCE(ID_MAIN), NULL, MainDlgProc);
}

BOOL CALLBACK MainDlgProc(HWND hwnd, UINT Message, WPARAM wParam, LPARAM lParam)
{
	WSADATA wsaData;

	static HWND hwndEdit;
	static SOCKET msock, ssock;
	static struct sockaddr_in sa;

	int err;


	switch (Message)
	{
	case WM_INITDIALOG:
		hwndEdit = GetDlgItem(hwnd, IDC_RESULT);
		break;
	case WM_COMMAND:
		switch (LOWORD(wParam))
		{
		case ID_LISTEN:

			WSAStartup(MAKEWORD(2, 0), &wsaData);

			//create master socket
			msock = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

			if (msock == INVALID_SOCKET) {
				EditPrintf(hwndEdit, TEXT("=== Error: create socket error ===\r\n"));
				WSACleanup();
				return TRUE;
			}

			err = WSAAsyncSelect(msock, hwnd, WM_SOCKET_NOTIFY, FD_ACCEPT);

			if (err == SOCKET_ERROR) {
				EditPrintf(hwndEdit, TEXT("=== Error: select error ===\r\n"));
				closesocket(msock);
				WSACleanup();
				return TRUE;
			}

			//fill the address info about server
			sa.sin_family = AF_INET;
			sa.sin_port = htons(SERVER_PORT);
			sa.sin_addr.s_addr = INADDR_ANY;

			//bind socket
			err = bind(msock, (LPSOCKADDR)&sa, sizeof(struct sockaddr));

			if (err == SOCKET_ERROR) {
				EditPrintf(hwndEdit, TEXT("=== Error: binding error ===\r\n"));
				WSACleanup();
				return FALSE;
			}

			err = listen(msock, 2);

			if (err == SOCKET_ERROR) {
				EditPrintf(hwndEdit, TEXT("=== Error: listen error ===\r\n"));
				WSACleanup();
				return FALSE;
			}
			else {
				EditPrintf(hwndEdit, TEXT("=== Server START ===\r\n"));
			}

			break;
		case ID_EXIT:
			EndDialog(hwnd, 0);
			break;
		};
		break;

	case WM_CLOSE:
		EndDialog(hwnd, 0);
		break;

	case WM_SOCKET_NOTIFY:
		switch (WSAGETSELECTEVENT(lParam))

		{
		case FD_ACCEPT: {
			int n, i;
			char get[BUFLEN];
			ssock = accept(msock, NULL, NULL);
			Socks.push_back(ssock);
			EditPrintf(hwndEdit, TEXT("=== Accept one new client(%d), List size:%d ===\r\n"), ssock, Socks.size());
			if ((n = recv(ssock, get, BUFLEN, 0)) > 0);
			EditPrintf(hwndEdit, TEXT("%s\r\n"), get);
			if (PartialCompare(get, HTTP_GET_ACTION)) {
				if (PartialCompare(get + 5, CGI_FORM_NAME)) {
					response(ssock, hwnd, CGI_FORM);
					closesocket(ssock);
				}
				if (PartialCompare(get + 5, CGI_FILE_NAME)) {
					get_query(get, hwndEdit);
					response(ssock, hwnd, CGI);
					websock = ssock;
				}
			}
			break; }
		case FD_READ: {
			auto WhenRead = LAMNDA_u{
				int n;
				if ((n = u->recv_msg()) == 0) {
					closesocket(u->sockfd);
					connect_cnt--;
				}

				if (u->status) {
					WSAAsyncSelect(u->sockfd, hwnd, WM_SOCKET_NOTIFY, FD_WRITE);
				}

				if (u->fd == NULL) {
					return;
				}

				if (!u->unsend) {
					if (fgets(u->cmdBuf, MAXCMD, u->fd) == NULL) {
						fclose(u->fd);
					}
					else if (n > 0) {
						u->unsend = 1;
						u->wNeed = strlen(u->cmdBuf);
						strncpy(u->printCmd, u->cmdBuf, strlen(u->cmdBuf));
						strtok(u->printCmd, "\r\n");
						EditPrintf(hwndEdit, TEXT("=== %s ===\r\n"), u->printCmd);
					}
				}

				if (connect_cnt == 0)
					closesocket(websock);
			};
			pool.SelectDo(wParam, WhenRead);
			break; }
		case FD_WRITE: {
			char msg[MAXCMD];
			auto WhenWrite = LAMNDA_u{
				int n;
				if (u->status && u->unsend) {

					n = send(u->sockfd, u->cmdBuf, u->wNeed, 0);
					u->wNeed -= n;
					strncpy(u->cmdBuf, u->cmdBuf + n, u->wNeed);
					if (n <= 0 || u->wNeed == 0) {
						u->unsend = 0;
						u->status = 0;
						WSAAsyncSelect(u->sockfd, hwnd, WM_SOCKET_NOTIFY, FD_READ);
					}
					if (u->wNeed == 0) {
						sprintf(msg, RES_TEMPLETE, u->terminalID, u->printCmd);
						msg[strlen(msg)] = 0;
						Send(websock, msg);
					}

				}
			};
			pool.SelectDo(wParam, WhenWrite);
			break; }
		case FD_CLOSE: {
			break; }
		};
		break;

	default:
		return FALSE;
	};

	return TRUE;
}

int EditPrintf(HWND hwndEdit, TCHAR * szFormat, ...)
{
	TCHAR   szBuffer[15000];
	va_list pArgList;

	va_start(pArgList, szFormat);
	wvsprintf(szBuffer, szFormat, pArgList);
	va_end(pArgList);

	SendMessage(hwndEdit, EM_SETSEL, (WPARAM)-1, (LPARAM)-1);
	SendMessage(hwndEdit, EM_REPLACESEL, FALSE, (LPARAM)szBuffer);
	SendMessage(hwndEdit, EM_SCROLLCARET, 0, 0);
	return SendMessage(hwndEdit, EM_GETLINECOUNT, 0, 0);
}


void get_query(char *get, HWND hwndEdit)
{
	char *query;
	query = strtok(get, Q_MARK);
	query = strtok(NULL, Q_MARK);
	query = strtok(NULL, Q_MARK);
	EditPrintf(hwndEdit, TEXT("%s\r\n"), query);
	char* tmp = strtok(query, AND_MARK);
	auto SetMetaData = LAMNDA_u{
		strcpy(u->hostIP, tmp + 3);
		u->port = atoi(strtok(NULL, AND_MARK) + 3);
		strcpy(u->fileName, strtok(NULL, AND_MARK) + 3);
		tmp = strtok(NULL, AND_MARK);
	};
	pool.ForAllDo(SetMetaData);
	auto PrintUser = LAMNDA_u{
		EditPrintf(hwndEdit, TEXT("%s %d %s\r\n"), u->hostIP, u->port, u->fileName);
	};
	pool.ForAllDo(PrintUser);
}


void response(int clifd, HWND hwnd, RES_TYPE type)
{
	char buffer[BUFLEN];
	switch (type) {
	case CGI_FORM: {
		FILE *fd = fopen(CGI_FORM_NAME, "r");
		if (fd != NULL) {
			Send(clifd, HTTP_OK);
			while (fgets(buffer, BUFLEN, fd) != NULL) {
				Send(clifd, buffer);
			}
			fclose(fd);
		}
		else Send(clifd, ERROR_404); //FILE NOT FOUND
		break; }
	case CGI: {
		int i;
		connect_cnt = 0;
		char msg[BUFLEN];
		Send(clifd, HTTP_OK);
		Send(clifd, HTML_HEAD);
		Send(clifd, HTML_TITLE);
		Send(clifd, TABLE_HEAD);
		auto SetupCont = LAMNDA_u{
			if (u->port) {
				if (u->Connect())
					WSAAsyncSelect(u->sockfd, hwnd, WM_SOCKET_NOTIFY, FD_READ);
				sprintf(u->terminalID, "m%d", connect_cnt);
				connect_cnt++;
				sprintf(msg, "<td>%s</td>", u->hostIP);
				send(clifd, msg, strlen(msg), 0);
			}
		};
		pool.ForAllDo(SetupCont);
		Send(clifd, HTML_ROW);
		auto ForTableEnd = LAMNDA_u{
			if (u->port) {
				sprintf(msg, "<td valign=\"top\" id=\"%s\"></td>", u->terminalID);
				send(clifd, msg, strlen(msg), 0);
			}
		};
		pool.ForAllDo(ForTableEnd);
		Send(clifd, TABLE_END);

		break; }
	}
	return;
}

char *str_replace(char *source, char *find, char *replace)
{
	int cur_len = strlen(source);
	int dif_len = strlen(replace) - strlen(find);
	char *result;
	result = (char *)malloc((cur_len + 1) * sizeof(char));
	strcpy(result, "");

	while (*source != '\0') {
		if (strncmp(source, find, strlen(find)) == 0) {
			cur_len += dif_len;
			result = (char *)realloc(result, (cur_len + 1) * sizeof(char));
			strcat(result, replace);
			source += strlen(find);
		}
		else {
			strncat(result, source, 1);
			source++;
		}
	}
	return result;
}
