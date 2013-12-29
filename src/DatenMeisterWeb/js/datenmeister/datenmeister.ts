/// <reference path="../dejs/dejs.ajax.d.ts" />
/// <reference path="../backbone/backbone.d.ts" />
/// <reference path="../underscore/underscore.d.ts" />

import api = require("datenmeister.serverapi");
import t = require("datenmeister.datatable");
import d = require("datenmeister.objects");
import views = require("datenmeister.views");
import navigation = require("datenmeister.navigation");
import forms = require('datenmeister.dataform');

// Serverconnection form
export function init() {
    var router = new AppRouter();

    // Binds the login
    router.bind('login', function () {

    });


    if (!Backbone.history.start({ pushState: false })) {
        navigation.to("login");
    } else {
        navigation.add(Backbone.history.getFragment());
    }

    new views.BackButtonView(
        {
            el: "#backview"
        });

    $("#delete_storage_link").click(function () {
        api.deleteBrowserStorage();
        alert('Storage deleted');
    });
}

var loginForm: views.ServerConnectionView;

class AppRouter extends Backbone.Router {

    isLoggedIn: boolean;

    constructor(options?: Backbone.RouterOptions) {
        if (options === undefined) {
            options = { routes: null };
        }

        options.routes =
        {
            "login": "showLoginForm",
            "all": "showAllExtents",
            "extent/*extent(/:viewUri)": "showExtent",
            "view/:objectUri(/:viewUri)": "showObject"
        };

        super(options);
    }

    showAllExtents(): views.AllExtentsView {
        if (this.triggerLoginEvent() === false) {
            return;
        }

        return new views.AllExtentsView({
            el: "#extentlist"
        });
    }

    showExtent(extentUri: string): views.ExtentTableView {
        if (this.triggerLoginEvent() === false) {
            return;
        }

        return new views.ExtentTableView(
            {
                el: "#objectlist",
                url: extentUri
            });
    }

    showObject(objectUri: string, viewUri: string): views.DetailView {        
        if (this.triggerLoginEvent() === false) {
            return;
        }

        var options = new forms.FormViewOptions();
        options.allowDelete = true;
        options.allowEdit = true;
        options.allowNew = true;
        options.allowNewProperty = true;

        return new views.DetailView(
            {
                el: "#detailview",
                url: objectUri,
                options: options,
                viewUrl: viewUri
            });
    }

    showLoginForm(): views.ServerConnectionView {
        var tthis = this;
        if (this.triggerLogoutEvent() === false) {
            return;
        }

        if (loginForm == undefined) {
            loginForm = new views.ServerConnectionView({
                el: "#serverconnectionview"
            });

            loginForm.onConnect = function (settings) {
                tthis.triggerLoginEvent();
                navigation.to("all");
            };
        }
        else {
            loginForm.render();
        }

        return loginForm;
    }

    /*
     * Triggers the login event, if it has not been triggered before
     */
    triggerLoginEvent(): boolean {
        if (api.getAPI() === undefined) {
            return false;
        }

        if (this.isLoggedIn !== true) {
            this.isLoggedIn = true;
            this.trigger('login');
        }

        return true;
    }

    /*
     * Triggers the logout event, if it has not been triggered before
     */
    triggerLogoutEvent(): boolean {
        if (this.isLoggedIn === true) {
            this.isLoggedIn = false;
            this.trigger('logout');
        }

        return true;
    }
     
}