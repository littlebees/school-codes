var Wallet = function(nowM,maxM) {   
    this.moneyPair = {nowMoney:nowM,maxMoney:maxM};
    this.speed = 1;
    this.myTimer = new MyTimer(this.speed);
    var numList = [define.imagePath+'wallet0.bmp',
		   define.imagePath+'wallet1.bmp',
		   define.imagePath+'wallet2.bmp',
		   define.imagePath+'wallet3.bmp',
		   define.imagePath+'wallet4.bmp',
		   define.imagePath+'wallet5.bmp',
		   define.imagePath+'wallet6.bmp',
		   define.imagePath+'wallet7.bmp',
		   define.imagePath+'wallet8.bmp',
		   define.imagePath+'wallet9.bmp'];
    var symbolAddr1 = define.imagePath+'wallet10.bmp';
    var symbolAddr2 = define.imagePath+'wallet11.bmp';
    var printPos = {x:740,y:6};
    this.printMoney = new PrintNum(15,21,0,printPos,numList,symbolAddr1);
    this.printMoney.setPrint(this.moneyPair.nowMoney);
    this.printGoal = new PrintNum(15,21,1,{x:printPos.x+15,y:printPos.y},numList,symbolAddr2);
    this.printGoal.setPrint(this.moneyPair.maxMoney);
};

Wallet.prototype.update = function() {
    var addMoney = function(){
	if(this.moneyPair.nowMoney < this.moneyPair.maxMoney){
	    this.moneyPair.nowMoney += 1;
	}
	this.printMoney.setPrint(this.moneyPair.nowMoney);
	this.printMoney.update();
	this.printGoal.setPrint(this.moneyPair.maxMoney);
	this.printGoal.update();
    }.bind(this);
    this.myTimer.timer(addMoney);
};

Wallet.prototype.draw = function(ctx){
    this.printMoney.draw(ctx);
    this.printGoal.draw(ctx);
};
