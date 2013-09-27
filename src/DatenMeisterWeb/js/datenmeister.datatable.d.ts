/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
/// <reference path="lib/dejs.table.d.ts" />
/// <reference path="datenmeister.objects.d.ts" />
import d = require("datenmeister.objects");
import t = require('lib/dejs.table');
export declare class TableOptions {
    public allowEdit: boolean;
    public allowNew: boolean;
    public allowDelete: boolean;
}
export declare class DataTable {
    public domTable: JQuery;
    public columns: d.JsonExtentFieldInfo[];
    public objects: d.JsonExtentObject[];
    public itemClickedEvent: (object: d.JsonExtentObject) => void;
    public options: TableOptions;
    public extent: d.ExtentInfo;
    public table: t.Table;
    constructor(extent: d.ExtentInfo, domTable: JQuery, options?: TableOptions);
    public defineColumns(columns: d.JsonExtentFieldInfo[]): void;
    public addObject(object: d.JsonExtentObject): void;
    public renderTable(): void;
    public createRow(object: d.JsonExtentObject): void;
    public createCreateButton(): void;
    public createNewElement(inputs: JQuery[], cells: JQuery[]): void;
    public createReadField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery;
    public createWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery;
    public setValueByWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo, dom: JQuery): void;
    public setItemClickedEvent(clickedEvent: (object: d.JsonExtentObject) => void): void;
    public triggerDelete(object: d.JsonExtentObject): void;
}
