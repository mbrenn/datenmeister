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
export declare class JsonExtentFieldInfo {
    public name: string;
    public title: string;
    public width: number;
    constructor(name?: string, title?: string);
}
export declare class JsonExtentObject {
    public id: string;
    public values: any;
    public extentUri: string;
    constructor();
    public getUri(): string;
}
export declare class JsonExtentData {
    public extent: ExtentInfo;
    public columns: JsonExtentFieldInfo[];
    public objects: JsonExtentObject[];
}
import t = require('lib/dejs.table');
export declare class ServerAPI {
    public connectionInfo: ServerSettings;
    constructor(connection: ServerSettings);
    public __getUrl(): string;
    public getServerInfo(success: (info: ServerInfo) => void, fail?: () => void): void;
    public getExtentInfo(success: (info: ExtentInfo[]) => void, fail?: () => void): void;
    public getObjectsInExtent(uri: string, success: (extentData: JsonExtentData) => void, fail?: () => void): void;
    public deleteObject(uri: string, success: () => void, fail?: () => void): void;
    public editObject(uri: string, data: any, success: () => void, fail?: () => void): void;
    public addObject(uri: string, data: any, success: (data: JsonExtentObject) => void, fail?: () => void): void;
}
export declare module Forms {
    class ServerConnectionForm {
        public onConnect: (settings: ServerSettings) => any;
        public formDom: JQuery;
        constructor(formDom);
        public bind(): void;
    }
}
export declare module Gui {
    function showExtents(domElement: JQuery): void;
    function showObjectsByUri(uri: string, domElement: JQuery): void;
    function showObjects(data: JsonExtentData, domElement: JQuery): void;
    class DataTable {
        public domTable: JQuery;
        public columns: JsonExtentFieldInfo[];
        public objects: JsonExtentObject[];
        public itemClickedEvent: (object: JsonExtentObject) => void;
        public allowEdit: boolean;
        public allowDelete: boolean;
        public allowNew: boolean;
        public extent: ExtentInfo;
        public table: t.Table;
        constructor(extent: ExtentInfo, domTable: JQuery);
        public defineColumns(columns: JsonExtentFieldInfo[]): void;
        public addObject(object: JsonExtentObject): void;
        public renderTable(): void;
        public createRow(object: JsonExtentObject): void;
        public createCreateButton(): void;
        public createNewElement(inputs: JQuery[], cells: JQuery[]): void;
        public createReadField(object: JsonExtentObject, field: JsonExtentFieldInfo): JQuery;
        public createWriteField(object: JsonExtentObject, field: JsonExtentFieldInfo): JQuery;
        public setValueByWriteField(object: JsonExtentObject, field: JsonExtentFieldInfo, dom: JQuery): void;
        public setItemClickedEvent(clickedEvent: (object: JsonExtentObject) => void): void;
        public triggerDelete(object: JsonExtentObject): void;
    }
}
