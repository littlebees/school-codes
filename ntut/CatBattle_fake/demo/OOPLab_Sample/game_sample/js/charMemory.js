// currying!!
var CharMemory = function(consChar,printTo){   
    this.container = new Set();
    this.myCanva = printTo;
    this.user = this;
    this.consChar = consChar;
    this.toAddList = new TodoList();
    //this.toDieList = new TodoList();
};

CharMemory.prototype.addChar = function(dataChar,pos,func){
    this.toAddList.addEvent(function(){
	var role = new this.consChar(dataChar,this.user);
    if(pos){
        role.sprite.position = pos;
    }
    if(func){
        role.deadAni = func;
    }
	this.container.add(role);
	this.myCanva.attach(role.sprite);
    }.bind(this));
};

CharMemory.prototype.deleteChar = function(target){
    //this.toDieList.addEvent(function(){
	this.myCanva.detach(target.sprite);
	this.container.delete(target);
    //}.bind(this));
};

CharMemory.prototype.updateChars = function(){
    
    //this.toDieList.execEvents();
    this.container.forEach(function(aChar){if(aChar){aChar.update();}});
    this.toAddList.execEvents();
};

CharMemory.prototype.forEachChars = function(f){
    this.container.forEach(f);
};

CharMemory.prototype.getTowerHP = function(){
    return this.container.values().next().value;
};
