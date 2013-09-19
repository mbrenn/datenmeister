
// The Server API

import ajax = require("lib/dejs.ajax");
import d = require("datenmeister.objects");

export var singletonAPI: ServerAPI;

export class ServerInfo {
    serverAddress: string;
    serverName: string;
}

export class ServerSettings {
    serverAddress: string;
}

export class ServerAPI {
    connectionInfo: ServerSettings;

    constructor(connection: ServerSettings) {
        this.connectionInfo = connection;

        if (this.connectionInfo.serverAddress[connection.serverAddress.length - 1] != '/') {
            this.connectionInfo.serverAddress += '/';
        }

        singletonAPI = this;
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

    getObjectsInExtent(uri: string, success: (extentData: d.JsonExtentData) => void, fail?: () => void) {
        ajax.performRequest({
            url: this.__getUrl() + "extent/GetObjectsInExtent?uri=" + uri,
            prefix: 'loadobjects_',
            success: function (data: any) {
                if (success !== undefined) {
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

    editObject(uri: string, data: any, success: () => void, fail?: () => void) {
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
        ajax.performRequest({
            url: this.__getUrl() + "extent/AddObject?uri=" + encodeURIComponent(uri),
            prefix: 'editobject_',
            method: 'post',
            data: data,
            success: function (data) {
                if (success !== undefined) {
                    success(<d.JsonExtentObject> data.element);
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
