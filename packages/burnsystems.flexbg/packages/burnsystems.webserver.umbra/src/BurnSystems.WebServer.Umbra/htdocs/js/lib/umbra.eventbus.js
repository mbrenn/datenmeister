"use strict";

define(['lychee.events'], function (Events) {

    ///////////////////////////////////////////
    // Definition of EventBus class	
    var EventBusClass = function () {

        this.__events = new Events();
    };

    EventBusClass.prototype =
    {
        ItemSelected: function (callback) {
            this.__events.bind('itemselected', callback);
        },

        onItemSelected: function (data) {
            this.__events.trigger('itemselected', [data]);
        }
    };

    return EventBusClass;
});