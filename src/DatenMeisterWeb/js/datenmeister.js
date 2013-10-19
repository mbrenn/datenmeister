/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.serverapi", "datenmeister.views", "datenmeister.navigation", 'datenmeister.dataform'], function(require, exports, __api__, __views__, __navigation__, __forms__) {
    var api = __api__;
    
    
    var views = __views__;
    var navigation = __navigation__;
    var forms = __forms__;

    // Serverconnection form
    function init() {
        var router = new AppRouter();

        // Binds the login
        router.bind('login', function () {
        });

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

    var loginForm;

    var AppRouter = (function (_super) {
        __extends(AppRouter, _super);
        function AppRouter(options) {
            if (options === undefined) {
                options = { routes: null };
            }

            options.routes = {
                "login": "showLoginForm",
                "all": "showAllExtents",
                "extent/*extent": "showExtent",
                "view/:objectUri(/:viewUri)": "showObject"
            };

            _super.call(this, options);
        }
        AppRouter.prototype.showAllExtents = function () {
            if (this.triggerLoginEvent() === false) {
                return;
            }

            return new views.AllExtentsView({
                el: "#extentlist"
            });
        };

        AppRouter.prototype.showExtent = function (extentUri) {
            if (this.triggerLoginEvent() === false) {
                return;
            }

            return new views.ExtentTableView({
                el: "#objectlist",
                url: extentUri
            });
        };

        AppRouter.prototype.showObject = function (objectUri, viewUri) {
            if (this.triggerLoginEvent() === false) {
                return;
            }

            var options = new forms.FormViewOptions();
            options.allowDelete = true;
            options.allowEdit = true;
            options.allowNew = true;
            options.allowNewProperty = true;

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

            if (loginForm == undefined) {
                loginForm = new views.ServerConnectionView({
                    el: "#serverconnectionview"
                });

                loginForm.onConnect = function (settings) {
                    tthis.triggerLoginEvent();
                    navigation.to("all");
                };
            } else {
                loginForm.render();
            }

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
