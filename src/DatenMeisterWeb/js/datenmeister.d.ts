/// <reference path="lib/jquery.d.ts" />
/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/dejs.table.d.ts" />
export declare class ServerSettings {
    public serverAddress: string;
}
export declare class ServerInfo {
    public serverAddress: string;
    public serverName: string;
}
export declare class ExtentInfo {
    public uri: string;
    public type: string;
}
export declare class ObjectData {
    public values: any;
}
export declare class ExtentColumnInfo {
    public name: string;
}
export declare class ExtentData {
    public columns: ExtentColumnInfo[];
    public objects: any[];
}
export declare class ServerAPI {
    public connectionInfo: ServerSettings;
    constructor(connection: ServerSettings);
    public __getUrl(): string;
    public getServerInfo(success: (info: ServerInfo) => void, fail?: () => void): void;
    public getExtentInfo(success: (info: ExtentInfo[]) => void, fail?: () => void): void;
    public getObjectsInExtent(uri: string, success: (extentData: ExtentData) => void, fail?: () => void): void;
}
export declare module Forms {
    class ServerConnectionForm {
        public onConnect: (settings: ServerSettings, api: ServerAPI) => any;
        public formDom: JQuery;
        constructor(formDom);
        public bind(): void;
    }
}
export declare module Tables {
    class ColumnDefinition {
        public title: string;
        public width: number;
        constructor(title: string);
    }
    class DataTable {
        public domTable: JQuery;
        public columns: ColumnDefinition[];
        public objects: any[];
        public itemClickedEvent: (object: any) => void;
        public isReadOnly: boolean;
        constructor(domTable: JQuery);
        public defineColumns(columns: ColumnDefinition[]): void;
        public addObject(object: any): void;
        public setItemClickedEvent(clickedEvent: (object: any) => void): void;
        public setAsReadOnly(): void;
        public renderTable(): void;
    }
}
export declare module Gui {
    function showExtents(serverConnection: ServerAPI, domElement: JQuery): void;
    function showObjectsByUri(serverConnection: ServerAPI, uri: string, domElement: JQuery): void;
    function showObjects(data: ExtentData, domElement: JQuery): void;
}
