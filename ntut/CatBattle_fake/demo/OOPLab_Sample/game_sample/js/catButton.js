//catData:catData
var CatButton = function(action,moneyPair,catData,pos) {
    Button.call(this,action);
    
    this.moneyPair = moneyPair;
    this.price = catData.paraList.price ? catData.paraList.price : 0;
    this.graphs = [];
    catData.btnGraph.forEach(function(addr){
	var pic = new Framework.Sprite(addr);
	pic.position.x = pos.x;
	pic.position.y = pos.y;
	this.graphs.push(pic);
    }.bind(this));
    this.myGraph = this.graphs[0];
    
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
    var pricePos = {x:pos.x+27,y:pos.y+25};
    this.printPrice = new PrintNum(9,13,0,pricePos,numList,symbolAddr);
    this.printPrice.setPrint(this.price);

    this.speed = catData.paraList.speed;
    this.goal = 100;
    this.myTimer = new MyTimer(this.speed);
    this.prevState = -1;
    this.state = 0;
    this.count = 0;
    this.myWidth = 64;
    this.myHeight = 64;

    this.couldDo = function(){
	return this.moneyPair.nowMoney >= this.price;
    };
    
    this.update = function(){
	var counter = function(){
	    if(this.state == 1 || this.state == 2){
	    	this.printPrice.update();
		if(this.couldDo()){
		    this.state = 2;
		    this._enable = true;
		}
		else {
		    this.state = 1;
		    this._enable = false;
		}
	    }else if(this.state == 0 && this.count >= this.goal){
		this.state = 1;
		this.count = 0;
		console.log("ok");
	    }else{
		this.count += 1;
		this.state = 0;
	    }
	}.bind(this);
	this.myTimer.timer(counter);

	if(this.state != this.prevState){
	    console.log("change");
	    this.myGraph = this.graphs[this.state];
	    this.prevState = this.state;
	}
    };
    this.draw = function(ctx){
	this.myGraph.draw(ctx);
	if(this.state != 0)
		this.printPrice.draw(ctx);
	if(this.state == 0){
	    var x = this.myGraph.upperLeft.x+6;
	    var y = this.myGraph.upperLeft.y+44;
	    ctx.fillStyle = "#00FFFF";
	    ctx.fillRect(x,y,this.count*(50/100),5);
	}
    };
};
CatButton.prototype = Button.prototype;
