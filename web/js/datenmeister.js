/// <reference path="lib/jquery.d.ts" />
var DatenMeister;
(function (DatenMeister) {
    var ServerSettings = (function () {
        function ServerSettings() {
        }
        return ServerSettings;
    })();
    DatenMeister.ServerSettings = ServerSettings;

    var ServerInfo = (function () {
        function ServerInfo() {
        }
        return ServerInfo;
    })();
    DatenMeister.ServerInfo = ServerInfo;

    var ExtentInfo = (function () {
        function ExtentInfo() {
        }
        return ExtentInfo;
    })();
    DatenMeister.ExtentInfo = ExtentInfo;

    var ObjectData = (function () {
        function ObjectData() {
        }
        return ObjectData;
    })();
    DatenMeister.ObjectData = ObjectData;

    var ExtentData = (function () {
        function ExtentData() {
        }
        return ExtentData;
    })();
    DatenMeister.ExtentData = ExtentData;

    // The Server API
    var ServerAPI = (function () {
        function ServerAPI(connection) {
            this.connectionInfo = connection;
        }
        ServerAPI.prototype.getServerInfo = function (callback) {
            var info = new ServerInfo();
            callback(info);
        };
        return ServerAPI;
    })();
    DatenMeister.ServerAPI = ServerAPI;
})(DatenMeister || (DatenMeister = {}));
