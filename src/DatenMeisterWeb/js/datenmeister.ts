/// <reference path="lib/jquery.d.ts" />

export module DatenMeister {
    export class ServerSettings {
        serverAddress: String;
    }

    export class ServerInfo {
        serverAddress: String;
        serverName: String;
    }

    export class ExtentInfo {
        name: String;
        url: String;
    }

    export class ObjectData {
        values: any;
    }

    export class ExtentData {
        dataObjects: ObjectData;
    }


    // The Server API
    export class ServerAPI {
        connectionInfo: ServerSettings;

        constructor(connection: ServerSettings) {
            this.connectionInfo = connection;
        }

        getServerInfo(callback: (info: ServerInfo) => void) {
            
            var info = new ServerInfo();
            callback(info);
        }
    }
}