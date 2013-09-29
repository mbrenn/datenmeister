/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
/// <reference path="lib/dejs.table.d.ts" />
/// <reference path="datenmeister.objects.d.ts" />
import d = require("datenmeister.objects");
import t = require('lib/dejs.table');
export declare class ViewOptions {
    public allowEdit: boolean;
    public allowNew: boolean;
    public allowDelete: boolean;
}
export declare class DataView {
    public options: ViewOptions;
    public domElement: JQuery;
    public fieldInfos: d.JsonExtentFieldInfo[];
    constructor(domElement: JQuery, options: ViewOptions);
    public setFieldInfos(fieldInfos: d.JsonExtentFieldInfo[]): void;
    public createReadField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery;
    public createWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery;
    public setValueByWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo, dom: JQuery): void;
    public createEventsForEditButton(editButton: JQuery, object: d.JsonExtentObject, columnDoms: JQuery[]): void;
}
export declare class DataTable extends DataView {
    public objects: d.JsonExtentObject[];
    public itemClickedEvent: (object: d.JsonExtentObject) => void;
    public extent: d.ExtentInfo;
    public table: t.Table;
    constructor(extent: d.ExtentInfo, domTable: JQuery, options?: ViewOptions);
    public defineFieldInfos(fieldInfos: d.JsonExtentFieldInfo[]): void;
    public addObject(object: d.JsonExtentObject): void;
    public render(): void;
    public createRow(object: d.JsonExtentObject): void;
    public createCreateButton(): void;
    public createNewElement(inputs: JQuery[], cells: JQuery[]): void;
    public setItemClickedEvent(clickedEvent: (object: d.JsonExtentObject) => void): void;
    public triggerDelete(object: d.JsonExtentObject): void;
}
