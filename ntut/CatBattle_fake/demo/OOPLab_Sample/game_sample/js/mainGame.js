//當有要加關卡時, 可以使用addNewLevel
//第一個被加進來的Level就是啟動點, 所以一開始遊戲就進入MyMenu
Framework.Game.addNewLevel({menu: new MyMenu()});
Framework.Game.addNewLevel({level1: new MyGame()});
Framework.Game.addNewLevel({thanks: new Thanks()});
Framework.Game.addNewLevel({over: new Over()});
Framework.Game.addNewLevel({help: new Help()});
//讓Game開始運行
Framework.Game.start();
