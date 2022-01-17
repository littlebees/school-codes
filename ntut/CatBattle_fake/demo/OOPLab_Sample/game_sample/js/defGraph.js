(function(window){	
    var defGraph ={}, mainPath = 'game_sample/';
    Object.defineProperties(defGraph, {
	'catList': {
	    value: {graph:[ define.imagePath+'cm000_0.bmp',
			       define.imagePath+'cm000_1.bmp',
			       define.imagePath+'cm000_2.bmp',
			       define.imagePath+'cm000_3.bmp',
			       define.imagePath+'cm000_4.bmp',
			       define.imagePath+'cm000_5.bmp'],
		    btnGraph:[ define.imagePath+'button000_2.bmp',
			       define.imagePath+'button000_1.bmp',
			       define.imagePath+'button000_0.bmp'],
		    paraList:{atk:1,def:10,hp:10,speed:1,step:5,price:10}},
	    writable: false
	},
	'testTower': {
	    value: {graph:[ define.imagePath+'cc000_0.bmp',
			    define.imagePath+'cc000_0.bmp',
			    define.imagePath+'cc000_0.bmp',
			    define.imagePath+'cc000_0.bmp',
			    define.imagePath+'cc000_0.bmp',
			    define.imagePath+'cc000_0.bmp'],
		    paraList:{atk:0,def:10,hp:100,speed:0,step:0}},
	    writable: false
	}
    });
    
    window.defGraph = defGraph;
})(window)
