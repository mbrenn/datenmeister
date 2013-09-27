/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />

import api = require("datenmeister.serverapi");
import t = require("datenmeister.datatable");
import d = require("datenmeister.objects");
import forms = require("datenmeister.forms");

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
        var form = new forms.ServerConnectionView({
            el: "#serverconnectionview"
        });

        form.onConnect = function (settings) {
            var allExtents = new forms.AllExtentsView({
                el: "#extentlist"
            });
        };
    }
}