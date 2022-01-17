//建構子,catDatas:{graph,btnGraph,paraList}
var CtrlCats = function(cons,catDatas,forChar,forButton,pos){
    this.myMemory = new CharMemory(cons,forChar);
    this.myWallet = new Wallet(0,500);
    //this.pos = pos;
    this.moneyButton = new MoneyButton(this.myWallet.moneyPair,{x:57.5,y:465});
    this.myButtons = [];
    this.items = 5;
    this.interval = 5 + 32;
    this.istouch=false;
    this.nowList = 0;
    this.previousTouch;
    
    var posList = [];
    var pos = {x:300,y:465};
    for(var i = 0;i<5;i++){
        var toAdd = (37*2)*i;
        var myPos = {x:pos.x+toAdd,y:pos.y};
        posList.push(myPos);
    }

    var addChar = function(charData,charMem){
        return function(){
            charMem.addChar(charData);
            if(this.price){
                this.moneyPair.nowMoney -= this.price;
                this.state = 0;
            }
        };
    };
    var tmpList = [];
    for(var i = 0;i<10;i++){
        var a = new Framework.AnimationSprite({url:CatDatas[i].graph, loop:true , speed:1});
        tmpList.push(new CatButton(addChar(CatDatas[i],this.myMemory),this.myWallet.moneyPair,CatDatas[i],posList[i % this.items]));
        forChar.attach(a);
        forChar.detach(a);
    }

    this.myButtons.push(tmpList.slice(0,5));
    this.myButtons.push(tmpList.slice(5,10));
    this.pages = 2;
    var lX = pos.x - this.interval;
    var rX = lX + (this.interval*2)*this.items;
    var lY = pos.y - this.interval;
    var rY = lY + (this.interval*2);
    this.inside = function(e){
        return (lX <= e.x && e.x <= rX) && (lY <= e.y && e.y <= rY);
    };
};

CtrlCats.prototype.update = function(){
    this.moneyButton.update();
    this.myButtons.forEach(function(btns){
	btns.forEach(function(btn){
	    btn.update();
	});
    });
    this.myWallet.update();
}

CtrlCats.prototype.draw = function(ctx){
    this.myButtons[this.nowList].forEach(function(btn){
	btn.draw(ctx);
    });
    this.moneyButton.draw(ctx);
    this.myWallet.draw(ctx);
};

CtrlCats.prototype.click = function(e){
    this.moneyButton.click(e);
    this.myButtons[this.nowList].forEach(function(btn){
	btn.click(e);
    });
};

CtrlCats.prototype.mouseup = function(e){
       this.istouch=false;
};

CtrlCats.prototype.mousemove = function(e){
    
    if(this.istouch && this.inside(e)){
        this.currentTouch = { x: e.x, y: e.y };
	var tmpY = this.currentTouch.y-this.previousTouch.y;
        if (Math.abs(tmpY) >= 2) {
            this.nowList = (this.nowList+1)%this.pages;
        }
        this.previousTouch = this.currentTouch;	
    }
};
CtrlCats.prototype.mousedown = function(e){
    this.previousTouch = { x: e.x, y: e.y };
    this.istouch=true;
};
CtrlCats.prototype.cheatCD = function(e){
    this.myButtons.forEach(function(btns){
    btns.forEach(function(btn){
        btn.count = btn.goal;
    });
    });
};
CtrlCats.prototype.cheatMoney = function(e){
    this.myWallet.moneyPair.nowMoney = this.myWallet.moneyPair.maxMoney;
};