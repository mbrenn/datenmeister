"use strict";

define([], function () {

    ///////////////////////////////////////////
    // Definition of ViewType-Class 

    // Initializes a new function
    // @token Token of the view type
    // @initFunction This function will be called with 1 argument with four properties, 
    // when a view has been created.
    //  view: View, where the new content shall be shown
    //  area: Area, where the new view has been inserted
    //  viewPoint: ViewPoint of content
    //  workSpace: Reference to workspace
    //  umbra: Reference to umbra instance
    //  settings: User-Defined structure that is created for the loadContent class
    //  data: UserData that is sent by the AJAX-Request. See BaseUmbraRequest.UserData
    var ViewTypeClass = function (token, initFunction) {
        this.token = token;
        this.init = initFunction;
    };

    ViewTypeClass.prototype =
    {

    };


    return ViewTypeClass;
});