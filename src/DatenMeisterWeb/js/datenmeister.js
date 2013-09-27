/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.forms"], function(require, exports, __forms__) {
    
    
    
    var forms = __forms__;

    // Serverconnection form
    function init() {
        var router = new AppRouter();

        if (!Backbone.history.start({ pushState: false })) {
            Backbone.history.navigate("login", { trigger: true });
        }
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
                "all": "showExtents"
            };

            _super.call(this, options);
        }
        AppRouter.prototype.showExtents = function () {
        };

        AppRouter.prototype.showLoginForm = function () {
            var form = new forms.ServerConnectionView({
                el: "#serverconnectionview"
            });

            form.onConnect = function (settings) {
                var allExtents = new forms.AllExtentsView({
                    el: "#extentlist"
                });
            };
        };
        return AppRouter;
    })(Backbone.Router);
});
//# sourceMappingURL=datenmeister.js.map
