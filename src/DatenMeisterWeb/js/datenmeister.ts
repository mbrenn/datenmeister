/// <reference path="lib/jquery.d.ts" />
/// <reference path="lib/dejs.ajax.d.ts" />

export class ServerSettings {
    serverAddress: string;
}

export class ServerInfo {
    serverAddress: string;
    serverName: string;
}

export class ExtentInfo {
    name: string;
    url: string;
}

export class ObjectData {
    values: any;
}

export class ExtentData {
    dataObjects: ObjectData;
}

import ajax = require("lib/dejs.ajax");
 

// Serverconnection form

// The Server API
export class ServerAPI {
    connectionInfo: ServerSettings;

    constructor(connection: ServerSettings) {
        this.connectionInfo = connection;

        if (this.connectionInfo.serverAddress[connection.serverAddress.length -1] != '/') {
            this.connectionInfo.serverAddress += '/';
        }
    }

    __getUrl(): string {
        return this.connectionInfo.serverAddress;
    }

    getServerInfo(success: (info: ServerInfo) => void, fail: () => void) {
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
}

export module Forms {
    export class ServerConnectionForm {

        // Call back, when user has depressed connect button
        onConnect: (settings: ServerSettings) => any;

        formDom: JQuery;

        constructor(formDom) {
            this.formDom = formDom;
        }

        bind() {
            var tthis = this;
            $(".serveraddress", tthis.formDom).val(window.localStorage.getItem("serverconnection_serveraddress"));

            $(".btn-primary", this.formDom).click(function () {
                var settings = new ServerSettings();
                settings.serverAddress = $(".serveraddress", tthis.formDom).val();

                window.localStorage.setItem("serverconnection_serveraddress", settings.serverAddress);

                // Check, if connection is possible
                var serverAPI = new ServerAPI(settings);
                serverAPI.getServerInfo(function (info) {

                    // Yes, do the connect
                    if (tthis.onConnect !== undefined) {
                        tthis.onConnect(settings);
                    }
                },
                function () {
                });
                
                return false;
            });
        }
    }
}