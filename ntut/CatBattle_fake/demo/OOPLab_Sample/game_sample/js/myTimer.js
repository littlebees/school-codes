var MyTimer = function(speed) {   
    this.speedCounter = 0;
    this.speed = speed;
};

MyTimer.prototype.timer = function(action) {
    var addFrame = this.speed / 30;
    this.speedCounter += addFrame;
    while(this.speedCounter > 1){
        action();
        this.speedCounter -= 1;
    }
};
MyTimer.prototype.setSpeed = function(speed) {
    this.speed = speed;
};
