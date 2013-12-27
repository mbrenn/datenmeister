"use strict";

define(["umbra.ribbongroup"], function (RibbonGroupClass) {

    ///////////////////////////////////////////
    // Definition of RibbonGroup class	
    var RibbonTabClass = function (title) {
        this.title = title;
        this.groups = [];

        // Stores the dom element
        this.domTab = {};
        this.domContent = {};
    };

    RibbonTabClass.prototype =
    {
        addGroup: function (title) {
            var group = new RibbonGroupClass(title);
            var domGroup = $('<div class="group"><div class="header">' + title + '</div><div class="elements"></div></div>');
            this.domContent.append(domGroup);
            group.domContent = domGroup;

            return group;
        }
    };

    return RibbonTabClass;
});