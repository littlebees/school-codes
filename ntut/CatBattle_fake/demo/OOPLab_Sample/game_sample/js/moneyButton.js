var MoneyButton = function(moneyPair,pos) {
    this.moneyPair = moneyPair;
    this.price = 100;
    this.goalMoney = 2000;
    this.quantity = 1000;
    this.arise = 1.5;
    Button.call(this,function(){
	this.moneyPair.nowMoney -= this.price;
	this.price *= this.arise;
	if(this.moneyPair.maxMoney + this.quantity > this.goalMoney){
	    this.moneyPair.maxMoney = this.goalMoney;
	}else {
	    this.moneyPair.maxMoney += this.quantity;
	}
    }.bind(this));
    var numList = [define.imagePath+'costON0.bmp',
		   define.imagePath+'costON1.bmp',
		   define.imagePath+'costON2.bmp',
		   define.imagePath+'costON3.bmp',
		   define.imagePath+'costON4.bmp',
		   define.imagePath+'costON5.bmp',
		   define.imagePath+'costON6.bmp',
		   define.imagePath+'costON7.bmp',
		   define.imagePath+'costON8.bmp',
		   define.imagePath+'costON9.bmp'];
    var symbolAddr = define.imagePath+'costON10.bmp';
    var pricePos = {x:pos.x+30,y:pos.y+30};
    this.printPrice = new PrintNum(9,13,0,pricePos,numList,symbolAddr);
    this.printPrice.setPrint(this.price);
    this.graphs = [new Framework.Sprite(define.imagePath + 'upgrade0.bmp'),
		   new Framework.Sprite(define.imagePath + 'upgrade2.bmp'),
		   new Framework.Sprite(define.imagePath + 'upgrade1.bmp')];
    this.graphs.forEach(function(i){
	i.position.x = pos.x;
	i.position.y = pos.y
    });
    this.myGraph = this.graphs[0];

    this.prevState = -1;
    this.state = 0;

    this.myWidth = 115;
    this.myHeight = 100;

    this.update = function(){
	if(this.moneyPair.maxMoney >= this.goalMoney){
	    this.state = 2;
	    this._enable = false;
	}
	else if(this.moneyPair.nowMoney >= this.price){
	    this.state = 1;
	    this._enable = true;
	}
	else {
	    this.state = 0;
	}
	
	if(this.state != this.prevState){
	    //this.forButton.detach(this.myGraph);
	    this.myGraph = this.graphs[this.state];
	    //this.forButton.attach(this.myGraph);
	    this.prevState = this.state;
	}
	this.printPrice.setPrint(this.price);
	this.printPrice.update();
    };
    this.draw = function(ctx){
	this.myGraph.draw(ctx);
	if(this.moneyPair.maxMoney < this.goalMoney)
	    this.printPrice.draw(ctx);
    };
};
MoneyButton.prototype = Button.prototype;
