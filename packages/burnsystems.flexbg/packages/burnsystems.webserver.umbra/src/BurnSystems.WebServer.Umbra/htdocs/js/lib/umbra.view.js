"use strict";

define([], function () {

    ///////////////////////////////////////////
    // Definition of View-Class 

    // The view contains the information which is independent from the embedding 
    // into the DOMTree
    var ViewClass = function (title, token, content) {
        if (title === undefined) {
            title = "Unnamed";
        }

        if (content === undefined) {
            content = "No content";
        }

        this.title = title;
        this.token = token;
        this.content = content;

        ViewClass.viewCounter++;
        this.name = "view_" + ViewClass.viewCounter;

        this.content = this.content;

        this.isVisible = false;
        this.areaAttached = undefined;
    };

    ViewClass.viewCounter = 0;

    ViewClass.prototype =
    {
    };

    return ViewClass;
});