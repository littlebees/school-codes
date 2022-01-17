var Help = Framework.exClass(Framework.GameMainMenu , {
	load: function(){
        this.photo = new Framework.Sprite(define.imagePath + 'help.png');
        
        //為了讓之後的位置較好操控, new出一個位於中心點且可以黏貼任何東西的容器
        //注意, Position都是用中心點
        this.center = new Framework.Scene();
        this.center.position = {
            x: Framework.Game.getCanvasWidth() / 2,
            y: Framework.Game.getCanvasHeight() / 2
        };


        this.photo.scale = 0.95;

        this.photo.position = {
            x: 0,
            y: 0
        };
        
        this.center.attach(this.photo);

        //rootScene為系統預設的容器, 由於其他東西都被attach到center上
        //將物件attach到center上, 順序是會影響繪製出來的效果的
        this.rootScene.attach(this.center);

    },
    
    initialize: function() {
        
    },

    update:function(){     
        this.rootScene.update();
        //this.rootScene.update(); 
    },

    draw: function(parentCtx) { 
        this.rootScene.draw(parentCtx);
        
    },

    mousedown: function(e) {
        //console.log為Browser提供的function, 可以在debugger的console內看到被印出的訊息                    
        Framework.Game.goToLevel('menu');
    },
    keydown:function(e,list){
        Framework.Game.goToLevel('menu');
    }
});