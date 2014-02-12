/// <reference path="../backbone/backbone.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "../dejs/dejs.ajax", "datenmeister.objects", "datenmeister.navigation"], function(require, exports, __ajax__, __d__, __navigation__) {
    // The Server API
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

    var ServerAPI = (function (_super) {
        __extends(ServerAPI, _super);
        function ServerAPI(connection) {
            _super.call(this);

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

        /*
        * Encodes the URI twice to transmit via QueryString
        * Mono HttpWebserver decodes the uri once when it has been received.
        */
        ServerAPI.prototype.__doubleEncodeUri = function (uri) {
            if (true) {
                // When connected to IIS, just a single encoding is necessary
                return encodeURIComponent(uri);
            } else {
                // When connected to Mono, a double encoding is necessary
                return encodeURIComponent(encodeURIComponent(uri));
            }
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
                    var newList = new Array();
                    _.each(value, function (v) {
                        var c = tthis.convertToJsonObject(v);
                        newList.push(c);
                    });

                    result.set(key, newList);
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
                url: this.__getUrl() + "extent/GetObject?uri=" + this.__doubleEncodeUri(uri),
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
                url: this.__getUrl() + "extent/GetObjectsInExtent?uri=" + this.__doubleEncodeUri(uri),
                prefix: 'loadobjects_',
                success: function (data) {
                    if (success !== undefined) {
                        data.extent = new d.JsonExtentObject(data.extent);

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
                url: this.__getUrl() + "extent/DeleteObject?uri=" + this.__doubleEncodeUri(uri),
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
                url: this.__getUrl() + "extent/EditObject?uri=" + this.__doubleEncodeUri(uri),
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
                url: this.__getUrl() + "extent/AddObject?uri=" + this.__doubleEncodeUri(uri),
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

        /*
        * Sends the object to server and waits for answer
        * The answer will be converted to a d.JsonExtentObject
        * @actionUrl: This url will be directly used to trigger the object
        */
        ServerAPI.prototype.sendObjectToServer = function (actionUrl, data, success, fail) {
            if (actionUrl === undefined || actionUrl.length == 0 || actionUrl[0] == '/') {
                throw "Action URL should not be undefined, '' or start with '/'";
            }

            alert('will be sent');

            var tthis = this;
            ajax.performRequest({
                url: this.__getUrl() + actionUrl,
                method: 'post',
                data: data.toJSON(),
                success: function (responseData) {
                    if (success !== undefined) {
                        success(responseData);
                    }
                },
                fail: function (responseData) {
                    if (fail !== undefined) {
                        fail(responseData);
                    }

                    tthis.onError(responseData);
                }
            });
        };

        /*
        * Throws the fail event
        * */
        ServerAPI.prototype.onError = function (data) {
            this.trigger('error', data);
            alert('FAIL');
        };
        return ServerAPI;
    })(Backbone.Model);
    exports.ServerAPI = ServerAPI;
});
//# sourceMappingURL=datenmeister.serverapi.js.map
