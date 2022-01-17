var StateSet = function(stateTable,viewFunc,user) {   
    this.prevState = -1;
    this.state = 0;
    this.user = user;
    this.stateTable = stateTable;
    this.viewFunc = viewFunc;
};

// pred and action are zero argument function
// stateTable => {pred,action,toState}
// viewFunc => [func] each func maps to state(0...n)
StateSet.prototype.update = function(action) {
    var go = false;
    var testAndset = function(data){
	if(data.pred()){
	    go = data.action;
	    this.state = data.toState;
	    return true;
	}
	else {
	    return false;
	}
    };
    for(var i in this.stateTable){
	if(this.stateTable[i]){
	    break;
	}
    }
    
    if(this.state != this.prevState){
	this.viewFunc[this.state]();
	this.prevState = this.state;
    }
    go();
};
