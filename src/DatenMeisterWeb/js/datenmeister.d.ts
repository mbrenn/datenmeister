/// <reference path="lib/jquery.d.ts" />
/// <reference path="lib/dejs.ajax.d.ts" />
export declare class ServerSettings {
    public serverAddress: string;
}
export declare class ServerInfo {
    public serverAddress: string;
    public serverName: string;
}
export declare class ExtentInfo {
    public name: string;
    public url: string;
}
export declare class ObjectData {
    public values: any;
}
export declare class ExtentData {
    public dataObjects: ObjectData;
}
export declare class ServerAPI {
    public connectionInfo: ServerSettings;
    constructor(connection: ServerSettings);
    public __getUrl(): string;
    public getServerInfo(success: (info: ServerInfo) => void, fail: () => void): void;
}
export declare module Forms {
    class ServerConnectionForm {
        public onConnect: (settings: ServerSettings) => any;
        public formDom: JQuery;
        constructor(formDom);
        public bind(): void;
    }
}
