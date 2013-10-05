import table = require('datenmeister.datatable');
import d = require("datenmeister.objects");
import t = require('lib/dejs.table');
export declare class FormViewOptions extends table.ViewOptions {
    public allowNewProperty: boolean;
}
export declare class DataForm extends table.DataView {
    public object: d.JsonExtentObject;
    public domElement: JQuery;
    public options: FormViewOptions;
    constructor(object: d.JsonExtentObject, domElement: JQuery, options?: FormViewOptions);
    public autoGenerateFields(): void;
    public render(): DataForm;
    public createNewPropertyRow(table: t.Table, lastRow: JQuery): void;
}
