// The Server API
define(["require", "exports", "lib/dejs.ajax", "datenmeister.objects", "datenmeister.navigation"], function(require, exports, __ajax__, __d__, __navigation__) {
    var ajax = __ajax__;
    var d = __d__;
    var navigation = __navigation__;

    /*
    * Gets the server API, will request loadng of server settings if required
    */
    function getAPI() {
        if (singletonAPI == undefined) {
            var serverSettings = retrieveServerSettings();
            if (serverSettings == undefined) {
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

    /*
    Retrieves the server settings from browser storage
    */
    function retrieveServerSettings() {
        var serverAddress = window.sessionStorage.getItem("serverapi_currentserveraddress");
        if (serverAddress == undefined) {
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

        ServerAPI.prototype.getObject = function (uri, success) {
            ajax.performRequest({
                url: this.__getUrl() + "extent/GetObject?uri=" + encodeURIComponent(uri),
                success: function (data) {
                    if (success !== undefined) {
                        var result = new d.JsonExtentObject();

                        // Sets id and extentUri of object
                        result.id = data.id;
                        result.extentUri = data.extentUri;

                        // Sets values of the complete object
                        _.each(data.values, function (value, key, list) {
                            result.set(key, value);
                        });

                        // Returns result
                        success(result);
                    }
                }
            });
        };

        ServerAPI.prototype.getObjectsInExtent = function (uri, success, fail) {
            ajax.performRequest({
                url: this.__getUrl() + "extent/GetObjectsInExtent?uri=" + uri,
                prefix: 'loadobjects_',
                success: function (data) {
                    if (success !== undefined) {
                        data.extent = new d.JsonExtentFieldInfo(data.extent);

                        for (var m = 0; m < data.columns.length; m++) {
                            var columnBackbone = new d.JsonExtentFieldInfo(data.columns[m]);

                            data.columns[m] = columnBackbone;
                        }

                        for (var n = 0; n < data.objects.length; n++) {
                            var currentObject = data.objects[n];
                            var result = new d.JsonExtentObject();
                            result.id = currentObject.id;
                            _.each(currentObject.values, function (value, key, list) {
                                result.set(key, value);
                            });
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
            ajax.performRequest({
                url: this.__getUrl() + "extent/AddObject?uri=" + encodeURIComponent(uri),
                prefix: 'editobject_',
                method: 'post',
                data: data,
                success: function (data) {
                    if (success !== undefined) {
                        success(data.element);
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
