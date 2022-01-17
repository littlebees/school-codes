#!/usr/bin/python3

import getpass
import sys,re, os
import requests as req
import pytesseract
from PIL import Image
from bs4 import BeautifulSoup as bs
from prettytable import PrettyTable
class ShowUsage(Exception):
    pass
class ReTry(Exception):
    pass
class WrongPW(Exception):
    pass
USAGE = """\
usage: main.py [-h] username

Web crawler for NCTU class schedule.

positional arguments:
    username    username of NCTU portal

optional arguments:
    -h, --help show this help message and exit\
"""
def getPW():
    return getpass.getpass('Password: ')
def tryToGetID():
    if len(sys.argv) is not 2:
        raise ShowUsage()
    idORnot = sys.argv[1]
    if re.search("^[0-9]", idORnot) is None:
        raise ShowUsage()
    else:
        return idORnot
def printTable(data):
    def collector(tr):
        def cleaner(s):
            ans = ''
            for c in s:
                if c not in '\t\n\r\xa0 ':
                    ans += c
            return ans
        ans = []
        for t in tr.find_all('td'):
            ans.append(cleaner(t.text))
        return ans

    tree = bs(data,'html.parser')
    table = (tree.find_all('table')[1])
    for font in table.find_all('font'):
        if font.parent is not None:
            font.parent.string = font.text
        font.decompose()
    for e in table.findAll('br h2'):
            e.decompose()
    trs = table.find('td').find_all('tr')
    try:
        table = PrettyTable(collector(trs[0]))
    except IndexError:
        raise ReTry()

    for tr in trs[1:]:
        tttt = collector(tr)
        table.add_row(tttt)
    print(table)

def getTable(userId,pw):
    START_URL = 'https://portal.nctu.edu.tw/portal/login.php'
    CHECK_URL = 'https://portal.nctu.edu.tw/portal/chkpas.php?'
    RELAY_URL = 'https://portal.nctu.edu.tw/portal/relay.php?D=cos'
    TABLE_URL = 'https://course.nctu.edu.tw/adSchedule.asp'
    def saveQCODE(sess):
        PIC_URL = 'https://portal.nctu.edu.tw/captcha/pic.php'
        PATH_PIC = 'qq.png'
        r = sess.get(PIC_URL)
        with open(PATH_PIC, 'wb') as f:
            for chunk in r:
                f.write(chunk)
        return PATH_PIC

    def getQCODE(picP):
        def processPIC(imgPath):
            from PIL import Image, ImageEnhance
            img = Image.open(imgPath).convert('LA')
            img2 = ImageEnhance.Brightness(img).enhance(1.5)
            img3 = ImageEnhance.Contrast(img2).enhance(1.5)
            return [img3]
        def identifyNUMBER(img):
            CONFIG= "-psm 8 -c tessedit_char_whitelist=0123456789" 
            raw = pytesseract.image_to_string(img,config=CONFIG)
            ans = ""
            count = 0
            for c in raw:
                if c.isdigit():
                    ans += c
                    count += 1
                    if count is 4:
                        return ans
                else:
                    return ""
                    pass
            return ""

        for pp in processPIC(picP):
            b = identifyNUMBER(pp)
            if b != "":
                os.remove(picP)
                return b
        os.remove(picP)
        raise ReTry()

    sess = req.session()
    sess.get(START_URL)
    path = saveQCODE(sess)
    qcode = getQCODE(path)
    payload = {
            'username': userId,
            'password': pw,
            'seccode': qcode,
            'pwdtype':'static',
            'Submit2':'登入(Login)'
            }
    ret = sess.post(CHECK_URL,data = payload)
    if "window.location= 'login.php'" in ret.text:
        raise WrongPW()
    
    ret = sess.get(RELAY_URL)
    ret.encoding = 'big5'
    form = bs(ret.text,'html.parser')
    inputL = form.find_all('input')
    payload = {}
    for i in inputL:
        payload[i.get('id')] = i.get('value')
    payload['Chk_SSO']='on'
    ret = sess.post('https://course.nctu.edu.tw/jwt.asp',data=payload)
    ret = sess.get(TABLE_URL)
    ret.encoding = 'big5'
    return ret.text

def main():
    try:
        while True:
            try:
                userID,pw = tryToGetID(), getPW()
                while True:
                    try:
                        data = getTable(userID,pw)
                        printTable(data)
                        break
                    except ReTry:
                        continue
                break
            except WrongPW:
                print('wrong password!!') 
                continue
    except ShowUsage:
        print(USAGE)
if __name__ == "__main__":
    main()
