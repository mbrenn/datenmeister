/// <reference path="../dejs/dejs.ajax.d.ts" />
/// <reference path="../backbone/backbone.d.ts" />
/// <reference path="../underscore/underscore.d.ts" />

import api = require("datenmeister.serverapi");
import t = require("datenmeister.datatable");
import d = require("datenmeister.objects");
import views = require("datenmeister.views");
import navigation = require("datenmeister.navigation");
import forms = require('datenmeister.dataform');
import fi = require('datenmeister.fieldinfo');
import fo = require('datenmeister.fieldinfo.objects');

// Initializes the whole application by creating the form
export function init() {
    var router = new AppRouter();

    // Binds the login
    router.bind('login', function () {

    });

    // Performs the login screen, where user can connect to a database server
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

        views.prepareForViewChange();

        // Creates new form for NewExtentForm
        new views.CreateNewExtentView({ el: ".newextent_form" });

        // Creates form for all extents
        return new views.AllExtentsView({
            el: "#extentlist"
        });
    }

    showExtent(extentUri: string): views.ExtentTableView {
        if (this.triggerLoginEvent() === false) {
            return;
        }

        views.prepareForViewChange();

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

        var options = fo.FormView.create();
        fo.FormView.setAllowDelete(options, true);
        fo.FormView.setAllowEdit(options, true);
        fo.FormView.setAllowNew(options, true);
        fo.FormView.setAllowNewProperty(options, true);

        views.prepareForViewChange();

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

        var loginForm = new views.ServerConnectionView({
            el: ".serverconnectionview"
        });

        views.prepareForViewChange();
        $(".serverconnection_container").show();

        loginForm.onConnect = function (settings) {
            tthis.triggerLoginEvent();
            navigation.to("all");
        };

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