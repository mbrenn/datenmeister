var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.serverapi"], function(require, exports, __api__) {
    var api = __api__;

    var ServerConnectionView = (function (_super) {
        __extends(ServerConnectionView, _super);
        function ServerConnectionView(options) {
            _super.call(this, options);

            this.delegateEvents({
                "click .serverconnection_button": this.connect
            });

            this.render();
        }
        ServerConnectionView.prototype.render = function () {
            var tthis = this;
            this.$(".serveraddress").val(window.localStorage.getItem("serverconnection_serveraddress"));
            return this;
        };

        ServerConnectionView.prototype.connect = function () {
            var tthis = this;
            var settings = new api.ServerSettings();
            settings.serverAddress = $(".serveraddress", this.formDom).val();

            window.localStorage.setItem("serverconnection_serveraddress", settings.serverAddress);

            // Check, if connection is possible
            var serverAPI = new api.ServerAPI(settings);
            serverAPI.getServerInfo(function (info) {
                if (tthis.onConnect !== undefined) {
                    tthis.onConnect(settings);
                }
            }, function () {
            });

            return false;
        };
        return ServerConnectionView;
    })(Backbone.View);
    exports.ServerConnectionView = ServerConnectionView;
});
//# sourceMappingURL=datenmeister.forms.js.map
