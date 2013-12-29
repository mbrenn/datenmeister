/// <reference path="../jquery/jquery.d.ts" />

export class CellOptions
{
	asHeader : boolean;
}

export class TableOptions {
    cssClass: string
}

export class Table {
    __domElement: JQuery;
    __currentTable: JQuery;
    __currentRow: JQuery;
    __isHeaderRow: boolean;

    constructor(domElement: JQuery, options?: TableOptions) {
        this.__domElement = domElement;
        this.__currentTable = $("<table></table>");
        this.__domElement.append(this.__currentTable);

        if (options !== undefined) {
            if (options.cssClass !== undefined && options.cssClass !== null) {
                this.__currentTable.addClass(options.cssClass);
            }
        }
    }

    getTableDom(): JQuery {
        return this.__currentTable;
    }

    addRow(): JQuery {
        this.__isHeaderRow = false;

        this.__currentRow = $("<tr></tr>");
        this.__currentTable.append(this.__currentRow);

        return this.__currentRow;
    }

    addHeaderRow(): JQuery {
        this.__isHeaderRow = true;

        this.__currentRow = $("<tr></tr>");
        this.__currentTable.append(this.__currentRow);

        return this.__currentRow;
    }

    insertRowAfter(lastRow: JQuery): JQuery{
        this.__isHeaderRow = false;

        this.__currentRow = $("<tr></tr>");
        lastRow.after(this.__currentRow);

        return this.__currentRow;
    }

    insertRowBefore(lastRow: JQuery): JQuery {
        this.__isHeaderRow = false;

        this.__currentRow = $("<tr></tr>");
        lastRow.before(this.__currentRow);

        return this.__currentRow;
    }

    addColumn(content: string): JQuery;
    addColumn(content: string, options?: CellOptions): JQuery {
        var currentColumn = this.__createColumn(options);
        currentColumn.text(content);

        return currentColumn;
    }

    addColumnHtml(content: string): JQuery;
    addColumnHtml(content: string, options?: CellOptions): JQuery {
        var currentColumn = this.__createColumn(options);
        currentColumn.html(content);

        return currentColumn;
    }

    addColumnJQuery(content: JQuery, options?: CellOptions): JQuery {
        var currentColumn = this.__createColumn(options);
        currentColumn.append(content);

        return currentColumn;
    }

    __createColumn(options): JQuery {
        if (this.__currentRow === undefined) {
            throw ("Row has not been started");
        }

        if (options == undefined) {
            options = {};
        }

        var currentColumn;

        if (this.__isHeaderRow || options.asHeader === true) {
            currentColumn = $("<th></th>");
        }
        else {
            currentColumn = $("<td></td>");
        }

        this.__currentRow.append(currentColumn);

        return currentColumn;
    }
}