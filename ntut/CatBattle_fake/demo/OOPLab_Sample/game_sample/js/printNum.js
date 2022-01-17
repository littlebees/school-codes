//succData:{speed,goal}
var PrintNum = function(MW,MH,dir,pos,numList,symAddr) {
    this.symGraph = symAddr ? new Framework.Sprite(symAddr) : false;
    this.numList = numList.map(function(i){return (new Framework.Sprite(i));});
    this.MW = MW;
    this.MH = MH;
    this.pos = pos;
    
    this.toPrint = [0];
    this.theNum = 0;
    this.myDraw;
    if(dir == 0)
	this.myDraw =  function(ctx){
	    var start = this.toPrint.length-1;
	    var myPos = this.symGraph ? {x:this.pos.x-this.MW,y:this.pos.y} : this.pos;
	    for(var i = start;i>=0;i--){
		var toDraw = this.numList[this.toPrint[i]];
		toDraw.position = myPos;
		toDraw.draw(ctx);
		myPos = {x:myPos.x-this.MW,y:this.pos.y};
	    }
	    if(this.symGraph){
		var myPos = this.pos;
		this.symGraph.position = myPos;
		this.symGraph.draw(ctx);
	    }
	};
    else
	this.myDraw = function(ctx){
	    var len = this.toPrint.length;
	    for(var i = 0;i<len;i++){
		var myPos = {x:this.MW*i+this.pos.x,y:this.pos.y};
		var toDraw = this.numList[this.toPrint[i]];
		toDraw.position = myPos;
		toDraw.draw(ctx);
	    }
	    if(this.symGraph){
		this.symGraph.position = {x:this.pos.x+this.MW*len,y:this.pos.y};
		this.symGraph.draw(ctx);
	    }
	};
};

PrintNum.prototype.setPrint = function(num) {
    this.theNum = num;
};

PrintNum.prototype.update = function(){
    this.toPrint = this.theNum.toString(10).split('').map(function(c){return parseInt(c,10)});
};

PrintNum.prototype.draw = function(ctx){
    this.myDraw(ctx);
};

PrintNum.prototype.getLastPos = function(){
    return this.pos;
};
