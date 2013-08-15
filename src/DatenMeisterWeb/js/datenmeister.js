/// <reference path="lib/jquery.d.ts" />
/// <reference path="lib/dejs.ajax.d.ts" />
define(["require", "exports", "lib/dejs.ajax"], function(require, exports, __ajax__) {
    var ServerSettings = (function () {
        function ServerSettings() {
        }
        return ServerSettings;
    })();
    exports.ServerSettings = ServerSettings;

    var ServerInfo = (function () {
        function ServerInfo() {
        }
        return ServerInfo;
    })();
    exports.ServerInfo = ServerInfo;

    var ExtentInfo = (function () {
        function ExtentInfo() {
        }
        return ExtentInfo;
    })();
    exports.ExtentInfo = ExtentInfo;

    var ObjectData = (function () {
        function ObjectData() {
        }
        return ObjectData;
    })();
    exports.ObjectData = ObjectData;

    var ExtentData = (function () {
        function ExtentData() {
        }
        return ExtentData;
    })();
    exports.ExtentData = ExtentData;

    var ajax = __ajax__;

    // Serverconnection form
    // The Server API
    var ServerAPI = (function () {
        function ServerAPI(connection) {
            this.connectionInfo = connection;

            if (this.connectionInfo.serverAddress[connection.serverAddress.length - 1] != '/') {
                this.connectionInfo.serverAddress += '/';
            }
        }
        ServerAPI.prototype.__getUrl = function () {
            return this.connectionInfo.serverAddress;
        };

        ServerAPI.prototype.getServerInfo = function (success, fail) {
            ajax.performRequest({
                url: this.__getUrl() + "extent/GetServerInfo",
                prefix: 'serverconnection_',
                fail: function () {
                    if (fail !== undefined) {
                        fail();
                    }
                },
                success: function (data) {
                    if (success !== undefined) {
                        success(null);
                    }
                }
            });
        };
        return ServerAPI;
    })();
    exports.ServerAPI = ServerAPI;

    (function (Forms) {
        var ServerConnectionForm = (function () {
            function ServerConnectionForm(formDom) {
                this.formDom = formDom;
            }
            ServerConnectionForm.prototype.bind = function () {
                var tthis = this;
                $(".serveraddress", tthis.formDom).val(window.localStorage.getItem("serverconnection_serveraddress"));

                $(".btn-primary", this.formDom).click(function () {
                    var settings = new ServerSettings();
                    settings.serverAddress = $(".serveraddress", tthis.formDom).val();

                    window.localStorage.setItem("serverconnection_serveraddress", settings.serverAddress);

                    // Check, if connection is possible
                    var serverAPI = new ServerAPI(settings);
                    serverAPI.getServerInfo(function (info) {
                        if (tthis.onConnect !== undefined) {
                            tthis.onConnect(settings);
                        }
                    }, function () {
                    });

                    return false;
                });
            };
            return ServerConnectionForm;
        })();
        Forms.ServerConnectionForm = ServerConnectionForm;
    })(exports.Forms || (exports.Forms = {}));
    var Forms = exports.Forms;
});
