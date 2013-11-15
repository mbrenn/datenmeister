
// The Server API

import ajax = require("lib/dejs.ajax");
import d = require("datenmeister.objects");
import navigation = require("datenmeister.navigation");

/*
 * Gets the server API, will request loading of server settings if required
 */
export function getAPI()
{
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

var singletonAPI: ServerAPI;

export class ServerInfo {
    serverAddress: string;
    serverName: string;
}

export class ServerSettings {
    serverAddress: string;
}

/*
   Stores the server settings into Browser storage for reload
*/
function storeServerSettings(connectionInfo: ServerSettings): void {
    window.sessionStorage.setItem("serverapi_currentserveraddress", connectionInfo.serverAddress);
}

export function deleteBrowserStorage(): void {
    window.sessionStorage.clear();
    window.localStorage.clear();
}
/* 
  Retrieves the server settings from browser storage
*/
function retrieveServerSettings(): ServerSettings {
    var serverAddress = window.sessionStorage.getItem("serverapi_currentserveraddress");
    if (_.isEmpty(serverAddress)) {
        return undefined;
    }

    var result = new ServerSettings();
    result.serverAddress = serverAddress;
    return result;
}

export class ServerAPI {
    connectionInfo: ServerSettings;

    constructor(connection: ServerSettings) {
        this.connectionInfo = connection;

        if (this.connectionInfo.serverAddress[connection.serverAddress.length - 1] != '/') {
            this.connectionInfo.serverAddress += '/';
        }

        singletonAPI = this;

        // Stores last server address of server API into local storage to retrieve it on automatic reload   
        storeServerSettings(connection);
    }

    __getUrl(): string {
        return this.connectionInfo.serverAddress;
    }

    getServerInfo(success: (info: ServerInfo) => void, fail?: () => void) {
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
    }

    convertToJsonObject(data: any): d.JsonExtentObject {
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
            }
            else if (_.isObject(value)) {
                result.set(key, <any> tthis.convertToJsonObject(value));
            }
            else {
                result.set(key, value);
            }
        });

        // Returns result
        return result;
    }

    getObject(uri: string, success: (object: d.JsonExtentObject) => void) {
        var tthis = this;
        ajax.performRequest({
            url: this.__getUrl() + "extent/GetObject?uri=" + encodeURIComponent(uri),
            success: function (data: any) {
                if (success !== undefined) {
                    var result = tthis.convertToJsonObject(data);

                    // Returns result
                    success(result);
                }
            }
        });
    }

    getObjects(uris: Array<string>, success: (objects: Array<d.JsonExtentObject>) => void) {
        var tthis = this;
        ajax.performRequest({
            data: {
                uris: uris
            },
            url: this.__getUrl() + "extent/GetObjects",
            method: 'POST',
            contentType: 'application/json',
            success: function (data) {
                var result = new Array<d.JsonExtentObject>();
                _.each(data.objects, function (o) {
                    result.push(tthis.convertToJsonObject(o));
                });

                success(result);
            }
        });
    }

    getObjectsInExtent(uri: string, success: (extentData: d.JsonExtentData) => void, fail?: () => void) {
        var tthis = this;
        ajax.performRequest({
            url: this.__getUrl() + "extent/GetObjectsInExtent?uri=" + uri,
            prefix: 'loadobjects_',
            success: function (data: any) {
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
    }

    deleteObject(uri: string, success: () => void, fail?: () => void) {
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
    }

    editObject(uri: string, object: Backbone.Model, success: () => void, fail?: () => void) {
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
    }

    addObject(uri: string, data: any, success: (data: d.JsonExtentObject) => void, fail?: () => void) {
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
    }
}
