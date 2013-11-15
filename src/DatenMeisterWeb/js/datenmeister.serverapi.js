// The Server API
define(["require", "exports", "lib/dejs.ajax", "datenmeister.objects", "datenmeister.navigation"], function(require, exports, __ajax__, __d__, __navigation__) {
    var ajax = __ajax__;
    var d = __d__;
    var navigation = __navigation__;

    /*
    * Gets the server API, will request loading of server settings if required
    */
    function getAPI() {
        if (singletonAPI == undefined) {
            var serverSettings = retrieveServerSettings();
            if (serverSettings === undefined) {
                navigation.to("login");
                return undefined;
            }

            return new ServerAPI(serverSettings);
        }

        return singletonAPI;
    }
    exports.getAPI = getAPI;

    var singletonAPI;

    var ServerInfo = (function () {
        function ServerInfo() {
        }
        return ServerInfo;
    })();
    exports.ServerInfo = ServerInfo;

    var ServerSettings = (function () {
        function ServerSettings() {
        }
        return ServerSettings;
    })();
    exports.ServerSettings = ServerSettings;

    /*
    Stores the server settings into Browser storage for reload
    */
    function storeServerSettings(connectionInfo) {
        window.sessionStorage.setItem("serverapi_currentserveraddress", connectionInfo.serverAddress);
    }

    function deleteBrowserStorage() {
        window.sessionStorage.clear();
        window.localStorage.clear();
    }
    exports.deleteBrowserStorage = deleteBrowserStorage;

    /*
    Retrieves the server settings from browser storage
    */
    function retrieveServerSettings() {
        var serverAddress = window.sessionStorage.getItem("serverapi_currentserveraddress");
        if (_.isEmpty(serverAddress)) {
            return undefined;
        }

        var result = new ServerSettings();
        result.serverAddress = serverAddress;
        return result;
    }

    var ServerAPI = (function () {
        function ServerAPI(connection) {
            this.connectionInfo = connection;

            if (this.connectionInfo.serverAddress[connection.serverAddress.length - 1] != '/') {
                this.connectionInfo.serverAddress += '/';
            }

            singletonAPI = this;

            // Stores last server address of server API into local storage to retrieve it on automatic reload
            storeServerSettings(connection);
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

        ServerAPI.prototype.convertToJsonObject = function (data) {
            var tthis = this;
            var result = new d.JsonExtentObject();

            // Sets id and extentUri of object
            result.id = data.id;
            result.extentUri = data.extentUri;

            // Sets values of the complete object
            _.each(data.values, function (value, key, list) {
                if (_.isArray(value)) {
                    var list = new Array();
                    _.each(value, function (v) {
                        var c = tthis.convertToJsonObject(v);
                        list.push(c);
                    });

                    result.set(key, list);
                } else if (_.isObject(value)) {
                    result.set(key, tthis.convertToJsonObject(value));
                } else {
                    result.set(key, value);
                }
            });

            // Returns result
            return result;
        };

        ServerAPI.prototype.getObject = function (uri, success) {
            var tthis = this;
            ajax.performRequest({
                url: this.__getUrl() + "extent/GetObject?uri=" + encodeURIComponent(uri),
                success: function (data) {
                    if (success !== undefined) {
                        var result = tthis.convertToJsonObject(data);

                        // Returns result
                        success(result);
                    }
                }
            });
        };

        ServerAPI.prototype.getObjects = function (uris, success) {
            var tthis = this;
            ajax.performRequest({
                data: {
                    uris: uris
                },
                url: this.__getUrl() + "extent/GetObjects",
                method: 'POST',
                contentType: 'application/json',
                success: function (data) {
                    var result = new Array();
                    _.each(data.objects, function (o) {
                        result.push(tthis.convertToJsonObject(o));
                    });

                    success(result);
                }
            });
        };

        ServerAPI.prototype.getObjectsInExtent = function (uri, success, fail) {
            var tthis = this;
            ajax.performRequest({
                url: this.__getUrl() + "extent/GetObjectsInExtent?uri=" + uri,
                prefix: 'loadobjects_',
                success: function (data) {
                    if (success !== undefined) {
                        data.extent = new d.JsonExtentFieldInfo(data.extent);

                        for (var n = 0; n < data.objects.length; n++) {
                            var currentObject = data.objects[n];
                            var result = tthis.convertToJsonObject(currentObject);

                            data.objects[n] = result;
                            data.objects[n].extentUri = uri;
                        }

                        success(data);
                    }
                },
                fail: function () {
                    if (fail !== undefined) {
                        fail();
                    }
                }
            });
        };

        ServerAPI.prototype.deleteObject = function (uri, success, fail) {
            ajax.performRequest({
                url: this.__getUrl() + "extent/DeleteObject?uri=" + encodeURIComponent(uri),
                prefix: 'deleteobject_',
                method: 'post',
                success: function () {
                    if (success !== undefined) {
                        success();
                    }
                },
                fail: function () {
                    if (fail !== undefined) {
                        fail();
                    }
                }
            });
        };

        ServerAPI.prototype.editObject = function (uri, object, success, fail) {
            var data = object.attributes;
            ajax.performRequest({
                url: this.__getUrl() + "extent/EditObject?uri=" + encodeURIComponent(uri),
                prefix: 'editobject_',
                method: 'post',
                data: data,
                success: function () {
                    if (success !== undefined) {
                        success();
                    }
                },
                fail: function () {
                    if (fail !== undefined) {
                        fail();
                    }
                }
            });
        };

        ServerAPI.prototype.addObject = function (uri, data, success, fail) {
            var tthis = this;
            ajax.performRequest({
                url: this.__getUrl() + "extent/AddObject?uri=" + encodeURIComponent(uri),
                prefix: 'editobject_',
                method: 'post',
                data: data,
                success: function (data) {
                    if (success !== undefined) {
                        success(tthis.convertToJsonObject(data));
                    }
                },
                fail: function () {
                    if (fail !== undefined) {
                        fail();
                    }
                }
            });
        };
        return ServerAPI;
    })();
    exports.ServerAPI = ServerAPI;
});
//# sourceMappingURL=datenmeister.serverapi.js.map
