
var catButton = function(action,pred,cat_graphs,pos) {
    // _enable 『是說可以使用這功能的嗎』
    this._enable = true;
    this.action = action;
    this.couldDo = pred;
    this.myGraph = new Framework.Sprite(cat_graphs[0]);
    this.myGraph.position.x = pos.x;
    this.myGraph.position.y = pos.y;
    this.speed = 5;
    this.goal = 100;
    this.myTimer = new MyTimer(this.speed);
    this.prevState = -1;
    this.state = 0;
    this.count = 0;
    this.myWidth = 60;
    this.myHeight = 50;

    this.inside = function(e){
	var s_x = this.myGraph.position.x - this.myWidth/2;
	var s_y = this.myGraph.position.y - this.myHeight/2;
	var e_x = this.myGraph.position.x + this.myHeight/2;
	var e_y = this.myGraph.position.y + this.myWidth/2;
	if((s_x <= e.x && e.x <= e_x) && (s_y <= e.y && e.y <= e_y)){return true;}
	else{return false;}
    };
    this.counter = function(){
	if(this.count >= this.goal || this._enable == true){
	    this._enable = true;
	    this.state = 1;
	    this.count = 0;
	    console.log("ok");
	}else{
	    this.count += 1;
	    this.state = 0;
	}
    }.bind(this);
    this.click = function(e){
	if(this._enable && this.inside(e) && this.couldDo()){
	    this._enable = false;
	    this.action();
	    this.state = 0;
	}
     };
    this.progress_cat = function(){
	this.myTimer.timer(this.counter);
    };
    this.update = function(){
	
	if(this.count <= this.goal && this._enable == false){
	    this.progress_cat();
	}
	if(this.state != this.prevState){
	    if(this.state == 0){
		this.myGraph = new Framework.Sprite(cat_graphs[0]);
		this.myGraph.position.x = pos.x;
		this.myGraph.position.y = pos.y;
	    }else if(this.state == 1){
		this.myGraph = new Framework.Sprite(cat_graphs[0]);
		this.myGraph.position.x = pos.x;
		this.myGraph.position.y = pos.y;
	    }
	    this.prevState = this.state;
	}
    };
};
