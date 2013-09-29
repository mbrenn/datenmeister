/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.forms", "datenmeister.navigation"], function(require, exports, __forms__, __navigation__) {
    
    
    
    var forms = __forms__;
    var navigation = __navigation__;

    // Serverconnection form
    function init() {
        var router = new AppRouter();

        if (!Backbone.history.start({ pushState: false })) {
            navigation.to("login");
        } else {
            navigation.add(Backbone.history.getFragment());
        }

        new forms.BackButtonView({
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
                "extent/*extent": "showExtent"
            };

            _super.call(this, options);
        }
        AppRouter.prototype.showAllExtents = function () {
            var allExtents = new forms.AllExtentsView({
                el: "#extentlist"
            });
        };

        AppRouter.prototype.showExtent = function (extentUri) {
            var detailView = new forms.ExtentTableView({
                el: "#objectlist",
                url: extentUri
            });
        };

        AppRouter.prototype.showLoginForm = function () {
            if (loginForm == undefined) {
                loginForm = new forms.ServerConnectionView({
                    el: "#serverconnectionview"
                });

                loginForm.onConnect = function (settings) {
                    navigation.to("all");
                };
            } else {
                loginForm.render();
            }
        };
        return AppRouter;
    })(Backbone.Router);
});
//# sourceMappingURL=datenmeister.js.map
