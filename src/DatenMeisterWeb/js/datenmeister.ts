/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />

import api = require("datenmeister.serverapi");
import t = require("datenmeister.datatable");
import d = require("datenmeister.objects");
import forms = require("datenmeister.forms");
import navigation = require("datenmeister.navigation");

// Serverconnection form
export function init() {
    var router = new AppRouter();

    if (!Backbone.history.start({ pushState: false })) {
        navigation.to("login");
    } else {
        navigation.add(Backbone.history.getFragment());
    }

    new forms.BackButtonView(
        {
            el: "#backview"
        });
}

var loginForm: forms.ServerConnectionView;

class AppRouter extends Backbone.Router {
    constructor(options?: Backbone.RouterOptions) {
        if (options === undefined) {
            options = { routes: null };
        }

        options.routes =
        {
            "login": "showLoginForm",
            "all": "showAllExtents",
            "extent/*extent": "showExtent"
        };

        super(options);
    }

    showAllExtents() {
        var allExtents = new forms.AllExtentsView({
            el: "#extentlist"
        });
    }

    showExtent(extentUri: string) {
        var detailView = new forms.ExtentTableView(
            {
                el: "#objectlist",
                url: extentUri
            });
    }

    showLoginForm() {
        if (loginForm == undefined) {
            loginForm = new forms.ServerConnectionView({
                el: "#serverconnectionview"
            });

            loginForm.onConnect = function (settings) {

                navigation.to("all");
            };
        }
        else {
            loginForm.render();
        }
    }
}