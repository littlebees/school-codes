(function(window){	
    var CatDatas ={}, mainPath = 'game_sample/';
    Object.defineProperties(CatDatas, {
	0: {
	    value: {graph:[ define.imagePath+'cm000_0.png',
			       		define.imagePath+'cm000_1.png',
			       		define.imagePath+'cm000_2.png',
			       		define.imagePath+'cm000_3.png',
			       		define.imagePath+'cm000_4.png',
			       		define.imagePath+'cm000_5.png'],
		    btnGraph:[ define.imagePath+'button000_2.bmp',
			       		define.imagePath+'button000_1.bmp',
			       		define.imagePath+'button000_0.bmp'],
		    paraList:{atk:0.5,def:10,hp:50,speed:3,step:5,price:10}},
	    writable: false
	},
	1: {
	    value: {graph:[ define.imagePath+'cm001_0.png',
			       		define.imagePath+'cm001_1.png',
			       		define.imagePath+'cm001_2.png',
			       		define.imagePath+'cm001_3.png',
			       		define.imagePath+'cm001_4.png',
			       		define.imagePath+'cm001_5.png'],
		    btnGraph:[ define.imagePath+'button001_2.bmp',
			       	   define.imagePath+'button001_1.bmp',
			           define.imagePath+'button001_0.bmp'],
		    paraList:{atk:1,def:10,hp:30,speed:0.5,step:5,price:10}},
	    writable: false
	},
	2: {
	    value: {graph:[ define.imagePath+'cm002_0.png',
			       		define.imagePath+'cm002_1.png',
			       		define.imagePath+'cm002_2.png',
			       		define.imagePath+'cm002_3.png',
			       		define.imagePath+'cm002_4.png',
			       		define.imagePath+'cm002_5.png'],
		    btnGraph:[ define.imagePath+'button002_2.bmp',
			       	   define.imagePath+'button002_1.bmp',
			           define.imagePath+'button002_0.bmp'],
		    paraList:{atk:1,def:10,hp:30,speed:3,step:5,price:10}},
	    writable: false
	},
	3: {
	    value: {graph:[ define.imagePath+'cm003_0.png',
			       		define.imagePath+'cm003_1.png',
			       		define.imagePath+'cm003_2.png',
			       		define.imagePath+'cm003_3.png',
			       		define.imagePath+'cm003_4.png',
			       		define.imagePath+'cm003_5.png'],
		    btnGraph:[ define.imagePath+'button003_2.bmp',
			       	   define.imagePath+'button003_1.bmp',
			           define.imagePath+'button003_0.bmp'],
		    paraList:{atk:5,def:10,hp:15,speed:1.5,step:5,price:10}},
	    writable: false
	},
	4: {
	    value: {graph:[ define.imagePath+'cm004_0.png',
			       		define.imagePath+'cm004_1.png',
			       		define.imagePath+'cm004_2.png',
			       		define.imagePath+'cm004_3.png',
			       		define.imagePath+'cm004_4.png',
			       		define.imagePath+'cm004_5.png'],
		    btnGraph:[ define.imagePath+'button004_2.bmp',
			       	   define.imagePath+'button004_1.bmp',
			           define.imagePath+'button004_0.bmp'],
		    paraList:{atk:5,def:10,hp:15,speed:1.3,step:5,price:10}},
	    writable: false
	},
	5: {
	    value: {graph:[ define.imagePath+'cm005_0.png',
			       		define.imagePath+'cm005_1.png',
			       		define.imagePath+'cm005_2.png',
			       		define.imagePath+'cm005_3.png',
			       		define.imagePath+'cm005_4.png',
			       		define.imagePath+'cm005_5.png'],
		    btnGraph:[ define.imagePath+'button005_2.bmp',
			       	   define.imagePath+'button005_1.bmp',
			           define.imagePath+'button005_0.bmp'],
		    paraList:{atk:5,def:10,hp:15,speed:0.5,step:5,price:10}},
	    writable: false
	},
	6: {
	    value: {graph:[ define.imagePath+'cm006_0.png',
			       		define.imagePath+'cm006_1.png',
			       		define.imagePath+'cm006_2.png',
			       		define.imagePath+'cm006_3.png',
			       		define.imagePath+'cm006_4.png',
			       		define.imagePath+'cm006_5.png'],
		    btnGraph:[ define.imagePath+'button006_2.bmp',
			       	   define.imagePath+'button006_1.bmp',
			           define.imagePath+'button006_0.bmp'],
		    paraList:{atk:5,def:10,hp:15,speed:0.5,step:5,price:10}},
	    writable: false
	},
	7: {
	    value: {graph:[ define.imagePath+'cm007_0.png',
			       		define.imagePath+'cm007_1.png',
			       		define.imagePath+'cm007_2.png',
			       		define.imagePath+'cm007_3.png',
			       		define.imagePath+'cm007_4.png',
			       		define.imagePath+'cm007_5.png'],
		    btnGraph:[ define.imagePath+'button007_2.bmp',
			       	   define.imagePath+'button007_1.bmp',
			           define.imagePath+'button007_0.bmp'],
		    paraList:{atk:5,def:10,hp:15,speed:0.5,step:5,price:10}},
	    writable: false
	},
	8: {
	    value: {graph:[ define.imagePath+'cm008_0.png',
			       		define.imagePath+'cm008_1.png',
			       		define.imagePath+'cm008_2.png',
			       		define.imagePath+'cm008_3.png',
			       		define.imagePath+'cm008_4.png',
			       		define.imagePath+'cm008_5.png'],
		    btnGraph:[ define.imagePath+'button008_2.bmp',
			       	   define.imagePath+'button008_1.bmp',
			           define.imagePath+'button008_0.bmp'],
		    paraList:{atk:5,def:10,hp:15,speed:0.5,step:5,price:10}},
	    writable: false
	},
	9: {
	    value: {graph:[ define.imagePath+'cm009_0.png',
			       		define.imagePath+'cm009_1.png',
			       		define.imagePath+'cm009_2.png',
			       		define.imagePath+'cm009_3.png',
			       		define.imagePath+'cm009_4.png',
			       		define.imagePath+'cm009_5.png'],
		    btnGraph:[ define.imagePath+'button009_2.bmp',
			       	   define.imagePath+'button009_1.bmp',
			           define.imagePath+'button009_0.bmp'],
		    paraList:{atk:5,def:10,hp:15,speed:0.5,step:5,price:10}},
	    writable: false
	}
    });
    
    window.CatDatas = CatDatas;
})(window)
