/// <reference path="../dejs/dejs.ajax.d.ts" />
/// <reference path="../backbone/backbone.d.ts" />
/// <reference path="../underscore/underscore.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.serverapi", "datenmeister.views", "datenmeister.navigation", 'datenmeister.fieldinfo.objects'], function(require, exports, api, views, navigation, fo) {
    // Initializes the whole application by creating the form
    function init() {
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

        new views.BackButtonView({
            el: "#backview"
        });

        $("#delete_storage_link").click(function () {
            api.deleteBrowserStorage();
            alert('Storage deleted');
        });
    }
    exports.init = init;

    var AppRouter = (function (_super) {
        __extends(AppRouter, _super);
        function AppRouter(options) {
            if (options === undefined) {
                options = { routes: null };
            }

            options.routes = {
                "login": "showLoginForm",
                "all": "showAllExtents",
                "extent/*extent(/:viewUri)": "showExtent",
                "view/:objectUri(/:viewUri)": "showObject"
            };

            _super.call(this, options);
        }
        AppRouter.prototype.showAllExtents = function () {
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
        };

        AppRouter.prototype.showExtent = function (extentUri) {
            if (this.triggerLoginEvent() === false) {
                return;
            }

            views.prepareForViewChange();

            return new views.ExtentTableView({
                el: "#objectlist",
                url: extentUri
            });
        };

        AppRouter.prototype.showObject = function (objectUri, viewUri) {
            if (this.triggerLoginEvent() === false) {
                return;
            }

            var options = fo.FormView.create();
            fo.View.setAllowDelete(options, true);
            fo.View.setAllowEdit(options, true);
            fo.View.setAllowNew(options, true);
            fo.FormView.setAllowNewProperty(options, true);

            views.prepareForViewChange();

            return new views.DetailView({
                el: "#detailview",
                url: objectUri,
                options: options,
                viewUrl: viewUri
            });
        };

        AppRouter.prototype.showLoginForm = function () {
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
        };

        /*
        * Triggers the login event, if it has not been triggered before
        */
        AppRouter.prototype.triggerLoginEvent = function () {
            if (api.getAPI() === undefined) {
                return false;
            }

            if (this.isLoggedIn !== true) {
                this.isLoggedIn = true;
                this.trigger('login');
            }

            return true;
        };

        /*
        * Triggers the logout event, if it has not been triggered before
        */
        AppRouter.prototype.triggerLogoutEvent = function () {
            if (this.isLoggedIn === true) {
                this.isLoggedIn = false;
                this.trigger('logout');
            }

            return true;
        };
        return AppRouter;
    })(Backbone.Router);
});
//# sourceMappingURL=datenmeister.js.map
