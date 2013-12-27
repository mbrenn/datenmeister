"use strict";

define(["umbra.eventbus"], function (EventBus) {

    ///////////////////////////////////////////
    // Umbra Class
    var UmbraType = function () {
        this.viewTypes = [];
        this.plugins = {};
        this.eventbus = new EventBus();
    };

    UmbraType.prototype =
    {
        addViewType: function (viewType) {
            this.viewTypes.push(viewType);
        },

        findViewType: function (token) {
            for (var i = 0; i < this.viewTypes.length; i++) {
                if (this.viewTypes[i].token == token) {
                    return this.viewTypes[i];
                }
            }
        },

        // Very simple plugin interface
        addPlugin: function (key, plugin) {
            this.plugins[key] = plugin;
        },

        getPlugin: function (key) {
            return this.plugins[key];
        }
    };

    var umbraInstance = new UmbraType();

    return umbraInstance;
});