// The Server API
define(["require", "exports", "lib/dejs.ajax", "datenmeister.objects"], function(require, exports, __ajax__, __d__) {
    var ajax = __ajax__;
    var d = __d__;

    exports.singletonAPI;

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

    var ServerAPI = (function () {
        function ServerAPI(connection) {
            this.connectionInfo = connection;

            if (this.connectionInfo.serverAddress[connection.serverAddress.length - 1] != '/') {
                this.connectionInfo.serverAddress += '/';
            }

            exports.singletonAPI = this;
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
