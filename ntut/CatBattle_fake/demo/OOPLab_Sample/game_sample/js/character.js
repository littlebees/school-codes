var Character = function(charData,mem) {   
    // file_list:0~2 -> 走路 3~5 -> 攻擊
    // para_list: atk,def,hp
    var self = this;
    this.hp = charData.paraList.hp;
    this.atk = charData.paraList.atk;
    this.def = charData.paraList.def;
    this.speed = charData.paraList.speed;
    this.step = charData.paraList.step;
    this.file_list = charData.graph;
    // 敵人
    this.others = new Set();
    // 在 cats 中的位置
    this.mem = mem;
    // 圖片
    this.sprite = new Framework.AnimationSprite({url:this.file_list, loop:true , speed:this.spped}); 
    this.sprite.position = {x: 0, y: 0};
    this.sprite.scale = 1;
    // 圖片的狀態
    this.action_state = 0;
    this.prev_state = 1;
    //timer
    this.myTimer = new MyTimer(this.speed);
    this.myTimer2 = new MyTimer(this.speed);
    this.toDelete = [];
    this.attacked = function(atk){
	this.hp -= atk;
    };
    this.attack = ((function(atk){
	return function(other){
	    return function(){
		if(other.hp <= 0){self.toDelete.push(other);}
		else{other.attacked(atk)}
		console.log(other.hp);
	    };
	};
    })(this.atk));
};

Character.prototype.run = function() {
    this.myTimer.timer(this.move);
};


Character.prototype.dead = function() {
    this.mem.deleteChar(this);
};

Character.prototype.addOther = function(other){
    this.others.add(other);
};

Character.prototype.hit = function() {
    this.others.forEach(function(item){
	 this.myTimer2.timer(this.attack(item));
    }.bind(this));
    this.toDelete.forEach(function(obj){this.others.delete(obj);}.bind(this));
};

Character.prototype.update =  function(){
        /*
      var needDie = function(){
      return this.hp <= 0;
      }.bind(this);
      var die = function(){
      this.dead();
      }.bind(this);
      var needAttack = function(){
      return this.others.length > 0;
      }.bind(this);
      var attack = function(){
      this.hit();
      }.bind(this);
      var needRun = function(){
      return true;
      };
      var run = function(){
      this.run();
      }.bind(this);
      [{pred:needDie,action:die,toState:2},
       {pred:needAttack,action:attack,toState:1},
       {pred:needRun,action:run,toState:0}]

      var whenRun = function(){
      this.sprite.start({ from: 0, to: 2, loop: true , speed:this.speed});
      }.bind(this);
      var whenAttack = function(){
      this.sprite.start({ from: 3, to: 5, loop: true , speed:this.speed});
      }.bind(this);
      var whenDie = function(){
      this.deadAni();
      }.bind(this);
     */
    if(this.hp <= 0){
	this.action_state = 2;
	this.action = this.dead;
    }
    else if(this.others.size > 0){
	this.action_state = 1;
	this.action = this.hit;
    }
    else{
	this.action_state = 0;
	this.action = this.run;
    }
    if(this.prev_state != this.action_state)
    {
	if(this.action_state == 0){
	    this.sprite.start({ from: 0, to: 2, loop: true , speed:this.speed});
	}
	else if(this.action_state == 1){
	    this.sprite.start({ from: 3, to: 5, loop: true , speed:this.speed});
	}
	else{
	    this.deadAni();
	    this.action();
	}
	this.prev_state = this.action_state
    }
    if(this.action_state != 2){
	this.action();
    }
};
