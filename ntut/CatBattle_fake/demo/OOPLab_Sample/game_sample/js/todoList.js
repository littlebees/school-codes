var TodoList = function() {   
    this.eventList = [];
};

TodoList.prototype.totalEvents = function() {
    return this.eventList.length;
};

TodoList.prototype.addEvent = function(event){
    this.eventList.push(event);
};

TodoList.prototype.clearEvents = function(){
    this.eventList = [];
};

TodoList.prototype.isEmpty = function(){
    return this.eventList.length == 0;
};

TodoList.prototype.execEvents = function(){
    if(!this.isEmpty()){
	var go = function(event){event();};
	this.eventList.forEach(go);
	this.clearEvents();
    }
};
