import d = require("datenmeister.objects");
export declare var singletonAPI: ServerAPI;
export declare class ServerInfo {
    public serverAddress: string;
    public serverName: string;
}
export declare class ServerSettings {
    public serverAddress: string;
}
export declare class ServerAPI {
    public connectionInfo: ServerSettings;
    constructor(connection: ServerSettings);
    public __getUrl(): string;
    public getServerInfo(success: (info: ServerInfo) => void, fail?: () => void): void;
    public getObjectsInExtent(uri: string, success: (extentData: d.JsonExtentData) => void, fail?: () => void): void;
    public deleteObject(uri: string, success: () => void, fail?: () => void): void;
    public editObject(uri: string, object: Backbone.Model, success: () => void, fail?: () => void): void;
    public addObject(uri: string, data: any, success: (data: d.JsonExtentObject) => void, fail?: () => void): void;
}
