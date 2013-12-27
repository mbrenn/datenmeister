"use strict";

define([], function () {

    ///////////////////////////////////////////
    // Definition of RibbonGroup class	
    var RibbonGroupClass = function (title) {
        this.title = title;
        this.elements = [];
        this.domContent = {};
    };

    RibbonGroupClass.prototype =
    {
        addElement: function (element) {
            this.elements.push(element);

            this.domContent.append(element.domElement);
        }
    };


    return RibbonGroupClass;
});