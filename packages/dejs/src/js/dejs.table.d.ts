/// <reference path="jquery.d.ts" />
export declare class CellOptions {
    public asHeader: boolean;
}
export declare class TableOptions {
    public cssClass: string;
}
export declare class Table {
    public __domElement: JQuery;
    public __currentTable: JQuery;
    public __currentRow: JQuery;
    public __isHeaderRow: boolean;
    constructor(domElement: JQuery, options?: TableOptions);
    public getTableDom(): JQuery;
    public addRow(): JQuery;
    public addHeaderRow(): JQuery;
    public insertRowAfter(lastRow: JQuery): JQuery;
    public insertRowBefore(lastRow: JQuery): JQuery;
    public addColumn(content: string): JQuery;
    public addColumnHtml(content: string): JQuery;
    public addColumnJQuery(content: JQuery, options?: CellOptions): JQuery;
    public __createColumn(options): JQuery;
}
