var MyGame = Framework.Class(Framework.Level , {
	load: function(){
	    var self = this;

	    // 遊戲屬性
	    this.bg_pic = new Framework.Sprite(define.imagePath + 'stage2.bmp');
	    this.stage_pic = new Framework.Sprite(define.imagePath + 'ec044_n_en.png');
	    this.stage_pic.position = {x:128,y:32};
	    this.bg_pic.position = {x:0,y:0};
	 
	    this.bg = new Framework.Scene();
	    this.bg.attach(this.bg_pic);
	    this.bg.position = {x:312,y:256};
	    this.container_test = new Framework.Scene();
	    this.container_test.position = {x:400,y:100};

	    this.catFactory = new CtrlCats(Cat,CatDatas,this.container_test,this.rootScene,{x:300,y:465});
	    this.enemyFactory = new CtrlEnemys(Enemy,this.container_test,{x:-780,y:0});
	    this.enemys = this.enemyFactory.myMemory;
	    this.cats = this.catFactory.myMemory;
	    this.charList = [this.cats,this.enemys];

	    this.buttons=[];
	    
	    // attach 到 rootSence

	    this.bg.attach(this.container_test);
	    this.rootScene.attach(this.bg);
	    this.rootScene.attach(this.stage_pic);
	    this._ctx;
	    //

	    // 屬於內部判斷的屬性
	    this.istouch=false;
	    this.currentTouch ={x:0,y:0};
	    this.prevousTouch ={x:0,y:0};

	    this.rootScene.attach(this.catFactory);
	},
    
    initialize: function() {
        this.enemys.addChar(TowerDatas[1],{x:-780,y:-80},function(){Framework.Game.goToLevel('thanks');});
	    this.cats.addChar(TowerDatas[0],{x:0,y:-80},function(){Framework.Game.goToLevel('over');});
	    this.charList.forEach(function(list){list.updateChars();});   
    },
    
    update: function() {
	var self = this;
        //this.rootScene.update();
	var combine_pair = function(cat){
	    if(cat){
		var meetEnemy = function(enemy){
		    var is_encounter = function(cat,enemy) {
			var a = cat.sprite.upperLeft.x <= enemy.sprite.upperRight.x;
			var b = cat.sprite.upperRight.x >= enemy.sprite.upperLeft.x;
			return a && b;
    		    };
		    if(enemy && is_encounter(cat,enemy)){
			cat.addOther(enemy);
			enemy.addOther(cat);
		    }
		};
		this.enemys.forEachChars(meetEnemy);
	    }
	}.bind(this);
	this.cats.forEachChars(combine_pair);
	this.catFactory.update();
	this.enemyFactory.update();
	this.charList.forEach(function(list){list.updateChars();});
	this.draw(this._ctx);
    },

    draw: function (parentCtx) {
        this._ctx = parentCtx;
        this.rootScene.draw();
    },

    keydown:function(e, list){
	if(e.key === 'Up') {
		this.cats[0].action_state = 0;
        }
	if(e.key === 'Down') {
     		
        }
	if(e.key === 'Left') {
	console.log(this.bg.position.x-10);
	  if(this.bg.position.x-10 >= 312){
            this.bg.position.x -= 10;
        }
	}
	if(e.key === 'Right') {
		console.log(this.bg.position.x+10);
            if(this.bg.position.x+10 <= 512){
            this.bg.position.x += 10;
        }
       }
	if(e.key === 'Enter'){
		this.catFactory.cheatCD();
	}
	if(e.key === 'Space'){
		this.catFactory.cheatMoney();
	}
    },
    mousedown:function(e){
	this.previousTouch = { x: e.x, y: e.y };
	this.istouch=true;
	this.catFactory.mousedown(e);
    },
    mouseup:function(e){
       this.istouch=false;
       this.catFactory.mouseup(e);
    },
    mousemove:function(e){
	if(this.istouch){
            this.currentTouch = { x: e.x, y: e.y };
	      var tmpX = this.bg.position.x + this.currentTouch.x-this.previousTouch.x;
            if (tmpX >= 312 && tmpX <= 512) {
                this.bg.position.x = tmpX; 
            }
        this.previousTouch = this.currentTouch;	
	}
	this.catFactory.mousemove(e);
     },
    touchstart: function (e) {
        this.click({ x: e.touches[0].clientX, y: e.touches[0].clientY });
    },
    
    click: function (e) {  
    this.catFactory.click(e);
	}
});

