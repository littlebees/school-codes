import sys,re,argparse
from argparse import ArgumentParser
from prettytable import PrettyTable
class NoFound(Exception):
    pass
class WrongFormat(Exception):
    pass

class Parser:
    class LogLine:
        def __init__(self,line):
            def logParser(line):
                ATTAACK_MSG_RE = r'Invalid user [a-zA-Z0-9]+ '
                ATTAACK_MSG_RE2 = r'Invalid user '
                TIME_RE = r' 2[0-3]|[0-1][0-9]:[0-5][0-9]:[0-5][0-9] '
                def divideByRE(paternStr,line):
                    a = re.search(paternStr,line)
                    if a is None:
                        raise NoFound()
                    else:
                        b = re.split(paternStr, line)
                        return (a.group(0),b[0],b[1])
                
                userPre, datetimePre, _ = divideByRE(ATTAACK_MSG_RE, line)
                time, date, _ = divideByRE(TIME_RE, datetimePre)
                _, _, userName = divideByRE(ATTAACK_MSG_RE2, userPre)
                from dateutil import parser
                return (userName[:-1],parser.parse(date+time))
            self.userName, self.date = logParser(line)
        def __eq__(self,other):
            return self.date == other.date
        def __lt__(self,other):
            return self.date < other.date

    def dateParser(self,d):
        from datetime import datetime
        DATE_PATTEN = '%Y-%m-%d-%H:%M:%S' 
        try:
            return datetime.strptime(d,DATE_PATTEN)
        except Exception:
            raise WrongFormat()

    def __init__(self,fName):
        import datetime,math
        self.logPath = fName
        self.after = datetime.datetime.min
        self.before = datetime.datetime.max
        self.most = None
        self.moreThan = 0
        self.ByName = self.rev = False
    def After(self,stamp):
        self.after = self.dateParser(stamp)
    def Before(self,stamp):
        self.before = self.dateParser(stamp)
    def Most(self,num):
        self.most = int(num)
    def MoreAttack(self,num):
        self.moreThan = int(num)
    def SortByUserName(self):
        self.ByName = True
    def ReverseCurrentOrder(self):
        self.rev = True
    def Go(self):
        with open(self.logPath) as f:
            rows = []
            for l in f.readlines():
                try:
                    rows.append(self.LogLine(l))
                except NoFound:
                    pass
            # filter based on before and after
            rows = filter(lambda r: self.after <= r.date and r.date <= self.before, rows)
            # count
            counts = {}
            for r in rows:
                if r.userName in counts:
                    counts[r.userName] += 1
                else:
                    counts[r.userName] = 1
            # fliter based on most or moreThan
            counts = {k:v for (k,v) in counts.items() if self.moreThan <= v}
            # most
            myList = []
            myList = sorted(counts.items(), key=lambda r:r[1],reverse=True)
            mostList = myList[:self.most]


            # sort in UsernameOrder or countOrder
            if self.ByName:
                myList = sorted(mostList, key=lambda r:r[0])
            else:
                myList = sorted(mostList, key=lambda r:(r[1],r[0]), reverse=True)
            # reverse it or not
            if self.rev:
                myList.reverse()
            else:
                pass
            # use prettytable to print
            from prettytable import PrettyTable
            table = PrettyTable(('user','count'))
            for x in myList:
                table.add_row(x)
            print(table)

def main():
    parser = ArgumentParser(description="Auth log parser.",prog="log.py")
    parser.add_argument("filename",help="Log file path.")
    parser.add_argument("-u",help="Summary failed login log and sort logby user",action='store_true')
    parser.add_argument("--after",help="Filter log after date. format YYYY-MM-DD-HH:MM:SS",dest="AFTER")
    parser.add_argument("--before",help="Filter log before date. format YYYY-MM-DD-HH:MM:SS",dest="BEFORE")
    parser.add_argument("-n",help="Show only the user of most N-th times",dest="N")
    parser.add_argument("-t",help="Show only the user of attacking equal of more than T times",dest="T")
    parser.add_argument("-r",help="Sort in reverse order",action='store_true')
    args = parser.parse_args()
    try:
        logP = Parser(args.filename)
        if args.r: logP.ReverseCurrentOrder()
        if args.u: logP.SortByUserName()
        if args.AFTER: logP.After(args.AFTER)
        if args.BEFORE: logP.Before(args.BEFORE)
        if args.N: logP.Most(args.N)
        if args.T: logP.MoreAttack(args.T)
        logP.Go()
    except WrongFormat:
        print("Illegal time format")
    except (OSError, IOError):
        print("file not exist")
if __name__ == "__main__":
    main()
