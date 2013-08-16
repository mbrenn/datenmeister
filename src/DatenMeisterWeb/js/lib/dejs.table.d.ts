/// <reference path="jquery.d.ts" />
export declare class CellOptions {
    public asHeader: boolean;
}
export declare class Table {
    public __domElement: JQuery;
    public __currentTable: JQuery;
    public __currentRow: JQuery;
    public __isHeaderRow: boolean;
    constructor(domElement: JQuery);
    public addRow(): JQuery;
    public addHeaderRow(): JQuery;
    public addColumn(content: string);
    public addColumnHtml(content: string);
    public __createColumn(options);
}
