(function(window){	
    var TowerDatas ={}, mainPath = 'game_sample/';
    Object.defineProperties(TowerDatas, {
	0: {
	    value: {graph:[ define.imagePath+'cc000_0.png',
			    		define.imagePath+'cc000_0.png',
			    		define.imagePath+'cc000_0.png',
			    		define.imagePath+'cc000_0.png',
			    		define.imagePath+'cc000_0.png',
			    		define.imagePath+'cc000_0.png'],
		    paraList:{atk:0,def:10,hp:100,speed:0,step:0}},
	    writable: false
	},
	1: {
	    value: {graph:[ define.imagePath+'ec037.png',
			    		define.imagePath+'ec037.png',
			    		define.imagePath+'ec037.png',
			    		define.imagePath+'ec037.png',
			    		define.imagePath+'ec037.png',
			    		define.imagePath+'ec037.png'],
		    paraList:{atk:0,def:10,hp:100,speed:0,step:0}},
	    writable: false
	},
    });
    
    window.TowerDatas = TowerDatas;
})(window)
