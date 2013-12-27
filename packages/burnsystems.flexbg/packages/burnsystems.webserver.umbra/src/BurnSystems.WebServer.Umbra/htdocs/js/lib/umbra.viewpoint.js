"use strict";

define([], function () {

    ///////////////////////////////////////////
    // Definition of ViewPoint-Class 

    // The viewPoint contains the information how a specific view is embedded
    // into the DOM
    var ViewPointClass = function (view, domRegisterTab) {
        this.view = view;
        this.domRegisterTab = domRegisterTab;
        this.domContent = {};
    };

    ViewPointClass.prototype =
	{
	    getView: function () {
	        return this.view;
	    }
	};

    ViewPointClass.lastDomId = 1;

    return ViewPointClass;
});