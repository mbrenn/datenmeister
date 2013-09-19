/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />


import api = require("datenmeister.serverapi");
import t = require("datenmeister.datatable");
import d = require("datenmeister.objects");

// Serverconnection form
export function init() {
    var router = new AppRouter();

    Backbone.history.start();
}

class AppRouter extends Backbone.Router {
    constructor(options?: Backbone.RouterOptions) {
        this.route("all", "showExtents");
        super(options);
    }

    showExtents() {
    }
}