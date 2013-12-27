"use strict";

define(["umbra.ribbontab"], function (RibbonTabClass) {

    ///////////////////////////////////////////
    // Definition of Dragbar class	
    var DragBarClass = function (direction, domElement, dragEvent) {
        this.isHorizontal = direction === "h";
        this.isVertical = direction === "v";
        this.domElement = domElement;
        this.dragEvent = dragEvent;

        this.lastX = 0;
        this.lastY = 0;
        this.isMouseDown = false;

        var _this = this;
        domElement.mousedown(
            function (data) {
                if (data.which == 1) {
                    _this.lastX = data.pageX;
                    _this.lastY = data.pageY;
                    _this.isMouseDown = true;

                    data.preventDefault();
                    data.stopPropagation();
                }
            });

        $(document).mousemove(
            function (data) {
                if (_this.isMouseDown) {
                    // Change
                    var changeX = _this.lastX - data.pageX;
                    var changeY = _this.lastY - data.pageY;

                    var realChange = 0;
                    if (_this.isVertical) {
                        realChange = changeX;
                    }

                    if (_this.isHorizontal) {
                        realChange = changeY;
                    }

                    dragEvent(this, realChange);

                    _this.lastX = data.pageX;
                    _this.lastY = data.pageY;

                    data.preventDefault();
                    data.stopPropagation();
                }
            });

        $(document).mouseup(
            function (data) {
                if (data.which == 1 && _this.isMouseDown) {
                    _this.isMouseDown = false;

                    data.preventDefault();
                    data.stopPropagation();
                }
            });

    };

    DragBarClass.prototype =
    {
    };

    return DragBarClass;
});