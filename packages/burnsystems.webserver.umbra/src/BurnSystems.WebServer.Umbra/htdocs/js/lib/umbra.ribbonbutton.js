"use strict";

define(["umbra.ribbonelement"], function (RibbonElementClass) {

    ///////////////////////////////////////////
    // Definition of RibbonButton class	
    var RibbonButtonClass = function (title, click) {

        RibbonElementClass.apply(this, arguments);

        this.title = title;
        this.click = click;

        // Create dom element
        this.domElement = $('<div class="button element"><a>' + title + '</a></div>');
        this.domElement.click(this.click);
    };

    RibbonButtonClass.prototype.prototype = RibbonElementClass.prototype;

    return RibbonButtonClass;
});