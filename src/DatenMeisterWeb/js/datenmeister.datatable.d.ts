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
export declare class NewPropertyFields {
    public keyField: JQuery;
    public valueField: JQuery;
    public rowDom: JQuery;
}
export declare class DataViewEditHandler extends Backbone.Model {
    public currentObject: d.JsonExtentObject;
    public writeFields: JQuery[];
    public columnDoms: JQuery[];
    public editButton: JQuery;
    public view: DataView;
    public newPropertyInfos: NewPropertyFields[];
    constructor();
    public addNewPropertyField(newField: NewPropertyFields): void;
    public switchToEdit(): void;
    public switchToRead(): void;
    public bindToEditButton(view: DataView, editButton: JQuery, object: d.JsonExtentObject, columnDoms: JQuery[]): void;
}
export declare class DataView {
    public itemClickedEvent: (object: d.JsonExtentObject) => void;
    public options: ViewOptions;
    public domElement: JQuery;
    public fieldInfos: d.JsonExtentFieldInfo[];
    constructor(domElement: JQuery, options: ViewOptions);
    public setFieldInfos(fieldInfos: d.JsonExtentFieldInfo[]): void;
    public setItemClickedEvent(clickedEvent: (object: d.JsonExtentObject) => void): void;
    public createReadField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery;
    public createWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery;
    public setValueByWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo, dom: JQuery): void;
}
export declare class DataTable extends DataView {
    public objects: d.JsonExtentObject[];
    public extent: d.ExtentInfo;
    public table: t.Table;
    constructor(extent: d.ExtentInfo, domTable: JQuery, options?: ViewOptions);
    public defineFieldInfos(fieldInfos: d.JsonExtentFieldInfo[]): void;
    public addObject(object: d.JsonExtentObject): void;
    public render(): void;
    public createRow(object: d.JsonExtentObject): void;
    public createCreateButton(): void;
    public createNewElement(inputs: JQuery[], cells: JQuery[]): void;
}
