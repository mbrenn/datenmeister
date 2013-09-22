/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />

import api = require("datenmeister.serverapi");
import t = require("datenmeister.datatable");
import d = require("datenmeister.objects");
import forms = require("datenmeister.forms");
import gui = require("datenmeister.gui");

// Serverconnection form
export function init() {
    var router = new AppRouter();

    if (!Backbone.history.start({ pushState: false })) {
        Backbone.history.navigate("login", { trigger: true });
    }

}

class AppRouter extends Backbone.Router {
    constructor(options?: Backbone.RouterOptions) {
        if (options === undefined) {
            options = { routes: null };
        }

        options.routes =
        {
            "login": "showLoginForm",
            "all": "showExtents"
        };

        super(options);
    }

    showExtents() {
    }

    showLoginForm() {

        var type: Backbone.ViewOptions =
            {
                el: "#serverconnectionview"
            };

        var form = new forms.ServerConnectionView(type);

        form.onConnect = function (settings) {
            gui.showExtents($("#extent_list_table"));
        };
    }
}