var CtrlEnemys = function(cons,forChar,pos){
    this.myMemory = new CharMemory(cons,forChar);
    this.CDtime = 0.05;
    this.easyMonsProb = 0.8;
    this.fullHP=100;
    this.myTimer = new MyTimer(this.CDtime);
    for(var i = 0;i<11;i++){
        var a = new Framework.AnimationSprite({url:EnemyDatas[i].graph, loop:true , speed:1});
        forChar.attach(a);
        forChar.detach(a);
    }
    this.pos = pos;

};

CtrlEnemys.prototype.update = function(){
    var addEnemy = function(){
        var hp = this.myMemory.getTowerHP();
        if(this.fullHP*3/4-1 <= hp &&  hp <= this.fullHP*3/4+1){
            this.easyMonsProb = 0.7;
            this.myTimer.setSpeed(0.06);
        }
        else if(this.fullHP/2-1 <= hp &&  hp <= this.fullHP/2+1){
            this.easyMonsProb = 0.6;
            this.myTimer.setSpeed(0.08);
        }
        else if(this.fullHP/4-1 <= hp &&  hp <= this.fullHP/4+1){
            this.easyMonsProb = 0.5;
            this.myTimer.setSpeed(0.1);
        }
        var easyIndex = function(){
            var items = [0,1,2,3,4];
            return items[Math.floor(Math.random()*items.length)];
        }
        var hardIndex = function(){
            var items = [5,6,7,8,9];
            return items[Math.floor(Math.random()*items.length)];
        }
        var index = Math.random() < this.easyMonsProb ? easyIndex() :  hardIndex();
        if(Math.random() >= 0.2)
        this.myMemory.addChar(EnemyDatas[index],this.pos);
    }.bind(this);
    this.myTimer.timer(addEnemy);
}
