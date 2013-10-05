/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.views", "datenmeister.navigation", 'datenmeister.dataform'], function(require, exports, __views__, __navigation__, __forms__) {
    
    
    
    var views = __views__;
    var navigation = __navigation__;
    var forms = __forms__;

    // Serverconnection form
    function init() {
        var router = new AppRouter();

        if (!Backbone.history.start({ pushState: false })) {
            navigation.to("login");
        } else {
            navigation.add(Backbone.history.getFragment());
        }

        new views.BackButtonView({
            el: "#backview"
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
                "view/*object": "showObject"
            };

            _super.call(this, options);
        }
        AppRouter.prototype.showAllExtents = function () {
            return new views.AllExtentsView({
                el: "#extentlist"
            });
        };

        AppRouter.prototype.showExtent = function (extentUri) {
            return new views.ExtentTableView({
                el: "#objectlist",
                url: extentUri
            });
        };

        AppRouter.prototype.showObject = function (objectUri) {
            var options = new forms.FormViewOptions();
            options.allowDelete = true;
            options.allowEdit = true;
            options.allowNew = true;
            options.allowNewProperty = true;

            return new views.DetailView({
                el: "#detailview",
                url: objectUri,
                options: options
            });
        };

        AppRouter.prototype.showLoginForm = function () {
            if (loginForm == undefined) {
                loginForm = new views.ServerConnectionView({
                    el: "#serverconnectionview"
                });

                loginForm.onConnect = function (settings) {
                    navigation.to("all");
                };
            } else {
                loginForm.render();
            }

            return loginForm;
        };
        return AppRouter;
    })(Backbone.Router);
});
//# sourceMappingURL=datenmeister.js.map
