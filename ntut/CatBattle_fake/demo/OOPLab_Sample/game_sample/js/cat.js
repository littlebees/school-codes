var Cat = function(charData,obj) {   
    Character.call(this,charData,obj);

    this.sprite.position = {x:0,y:0};
    this.move = function(){
	this.sprite.position.x -= this.step;
    }.bind(this);
    
    this.deadAni = function(){
	console.log("hi2");
    };
};
Cat.prototype = Character.prototype;
