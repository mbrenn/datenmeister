import table = require('datenmeister.datatable');
import d = require("datenmeister.objects");
export declare class DataForm extends table.DataView {
    public object: d.JsonExtentObject;
    public domElement: JQuery;
    constructor(object: d.JsonExtentObject, domElement: JQuery, options?: table.ViewOptions);
    public autoGenerateFields(): void;
    public render(): DataForm;
}
