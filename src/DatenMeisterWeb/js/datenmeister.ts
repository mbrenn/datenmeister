/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />

import api = require("datenmeister.serverapi");
import t = require("datenmeister.datatable");
import d = require("datenmeister.objects");
import views = require("datenmeister.views");
import navigation = require("datenmeister.navigation");
import forms = require('datenmeister.dataform');

// Serverconnection form
export function init() {
    var router = new AppRouter();

    if (!Backbone.history.start({ pushState: false })) {
        navigation.to("login");
    } else {
        navigation.add(Backbone.history.getFragment());
    }

    new views.BackButtonView(
        {
            el: "#backview"
        });
}

var loginForm: views.ServerConnectionView;

class AppRouter extends Backbone.Router {
    constructor(options?: Backbone.RouterOptions) {
        if (options === undefined) {
            options = { routes: null };
        }

        options.routes =
        {
            "login": "showLoginForm",
            "all": "showAllExtents",
            "extent/*extent": "showExtent",
            "view/*object": "showObject"
        };

        super(options);
    }

    showAllExtents(): views.AllExtentsView {
        return new views.AllExtentsView({
            el: "#extentlist"
        });
    }

    showExtent(extentUri: string): views.ExtentTableView {
        return new views.ExtentTableView(
            {
                el: "#objectlist",
                url: extentUri
            });
    }

    showObject(objectUri: string): views.DetailView {

        var options = new forms.FormViewOptions();
        options.allowDelete = true;
        options.allowEdit = true;
        options.allowNew = true;
        options.allowNewProperty = true;

        return new views.DetailView(
            {
                el: "#detailview",
                url: objectUri,
                options: options
            });
    }

    showLoginForm(): views.ServerConnectionView {
        if (loginForm == undefined) {
            loginForm = new views.ServerConnectionView({
                el: "#serverconnectionview"
            });

            loginForm.onConnect = function (settings) {
                navigation.to("all");
            };
        }
        else {
            loginForm.render();
        }

        return loginForm;
    }
}