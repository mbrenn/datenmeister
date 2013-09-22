/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.forms", "datenmeister.gui"], function(require, exports, __forms__, __gui__) {
    
    
    
    var forms = __forms__;
    var gui = __gui__;

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
            var type = {
                el: "#serverconnectionview"
            };

            var form = new forms.ServerConnectionView(type);

            form.onConnect = function (settings) {
                gui.showExtents($("#extent_list_table"));
            };
        };
        return AppRouter;
    })(Backbone.Router);
});
//# sourceMappingURL=datenmeister.js.map
