
var Button = function(action) {
    // _enable 『是說可以使用這功能的嗎』
    this._enable = false;
    this.action = action;
    this.myGraph;
    this.myWidth;
    this.myHeight;
};
Button.prototype.inside = function(e){
    var s_x = this.myGraph.position.x - this.myWidth/2;
    var s_y = this.myGraph.position.y - this.myHeight/2;
    var e_x = this.myGraph.position.x + this.myHeight/2;
    var e_y = this.myGraph.position.y + this.myWidth/2;
    return (s_x <= e.x && e.x <= e_x) && (s_y <= e.y && e.y <= e_y);
};
Button.prototype.click = function(e){
    
    if(this._enable && this.inside(e)){
	this._enable = false;
	this.action();
    }
};
