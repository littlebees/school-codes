//立即執行函式, 並封裝所有變數避免衝突
var loadGameEnd;
(function(){
    //動態依序載入JS
    //ref: http://blog.darkthread.net/blogs/darkthreadtw/archive/2009/01/15/4061.aspx
    var  importJS = function(jsConf, src, lookFor) {
        var headID = document.getElementsByTagName("head")[0]; 
        var newJs = document.createElement('script');
        newJs.type = 'text/javascript';
        newJs.src= jsConf[0].src;
        headID.appendChild(newJs);
        wait_for_script_load(jsConf, function() {
            jsConf.splice(0, 1);
            if(jsConf.length > 0) {
                importJS(jsConf, lookFor);
            }else
			{
				loadGameEnd = true;
			}
        });
    }

    var wait_for_script_load = function(jsConf, callback) {
        var interval = setInterval(function() {
            if (typeof jsConf[0].lookFor === 'undefined') {
                jsConf[0].lookFor = '';
            }

            if (jsConf[0].lookFor === '') {
                clearInterval(interval);
                callback();
            } else if (eval("typeof " + jsConf[0].lookFor) !== 'undefined') {
                    clearInterval(interval);
                    callback();      
                }
            }, 50);
    }

    //陣列和載入JS檔的順序相同, lookFor為在要載入的檔案中, 
    //有用到的全域變數, importJS這個function, 會在找到lookFor的變數後
    //才會繼續loading下一個檔案, 如果沒有需要lookFor, 則以空字串代表
    var listScript = 
    [
        { src: 'game_sample/js/define.js', lookFor: 'define' },
        { src: 'game_sample/js/defGraph.js', lookFor: 'defGraph' },
        { src: 'game_sample/js/catDatas.js', lookFor: 'CatDatas' },
        { src: 'game_sample/js/enemyDatas.js', lookFor: 'EnemyDatas' },
        { src: 'game_sample/js/towerDatas.js', lookFor: 'TowerDatas' },
	    { src: 'game_sample/js/button.js', lookFor: 'Button' },
	    { src: 'game_sample/js/catButton.js', lookFor: 'CatButton' },
	    { src: 'game_sample/js/moneyButton.js', lookFor: 'MoneyButton' },
	    { src: 'game_sample/js/todoList.js',lookFor: 'TodoList' },
	    { src: 'game_sample/js/myTimer.js', lookFor: 'MyTimer' },
	    { src: 'game_sample/js/printNum.js', lookFor: 'PrintNum' },
	    { src: 'game_sample/js/wallet.js', lookFor: 'Wallet' },
        { src: 'game_sample/js/cat_button.js', lookFor: 'catButton' },
	    { src: 'game_sample/js/character.js', lookFor: 'Character' },
        { src: 'game_sample/js/cat.js', lookFor: 'Cat' },
	    { src: 'game_sample/js/enemy.js', lookFor: 'Enemy' },
	    { src: 'game_sample/js/charMemory.js', lookFor: 'CharMemory'},
        { src: 'game_sample/js/ctrlChar.js', lookFor: 'CtrlCats' },
        { src: 'game_sample/js/ctrlEnemy.js', lookFor: 'CtrlEnemys' },
        { src: 'game_sample/js/myMenu.js', lookFor: 'MyMenu' },
        { src: 'game_sample/js/thanks.js', lookFor: 'Thanks' },
        { src: 'game_sample/js/gameover.js', lookFor: 'Over' },
        { src: 'game_sample/js/help.js', lookFor: 'Help' },
        { src: 'game_sample/js/myGameLevel1.js', lookFor: 'MyGame' },
        { src: 'game_sample/js/mainGame.js'}
    ]

    importJS(listScript);
    
})();


    
