
/* Extent Table */

export class ExtentObjectData {
    data: Array<string>;

    id: string;
}

export class ExtentTableDefinition {

    columns: Array<string>;

    elements: Array<ExtentObjectData>;
}

export class ExtentTableViewRow {

    /**
     * The DOM being used to store the data itself
     */
    row: JQuery;

    /**
     * Stores the key to be used for sorting
     */
    key: string;

    /** 
     * Stores the id
     */
    id: string;
}

/* Defines the class that creates a table
    which shows all the elements and offers an edit an delete mechanism.
    */
export class ExtentTableView {
    domElement: JQuery;

    searchString: string;

    orderByColumn: string;

    orderAscending: boolean;

    definition: ExtentTableDefinition;

    /* Stores the rows, which were created during the update function */
    tableRows: Array<ExtentTableViewRow>;

    /* This method will be called by this class
            to trigger a deletion event. 
            The called method needs to call onSuccessDelete, 
            if this is successful */
    callbackDelete: (id: string) => void;

    /* This method will be called by this class
            when user has decided to edit a field */
    callbackEdit: (id: string) => void;

    constructor(domElement: JQuery) {
        this.definition = new ExtentTableDefinition();
        this.domElement = domElement;
        this.orderAscending = true;
    }

    getOrderFunction(columnText: string) {
        var tthis = this;
        return function () {
            if (tthis.orderByColumn == columnText) {
                tthis.orderAscending = !tthis.orderAscending;
            }
            else {
                tthis.orderByColumn = columnText;
            }

            tthis.update();
        }
    }

    getEditFunction(obj: ExtentObjectData, icon: JQuery) {
        var tthis = this;
        return function () {
            if (tthis.callbackEdit === undefined) {
                alert('callbackEdit function is not defined');
                return;
            }

            tthis.callbackEdit(obj.id);
        };
    }

    lastClickedDeleteIcon: JQuery;

    getDeleteFunction(obj: ExtentObjectData, icon: JQuery) {
        var tthis = this;
        return function () {
            if (tthis.lastClickedDeleteIcon !== undefined) {
                if (tthis.lastClickedDeleteIcon !== icon) {
                    tthis.lastClickedDeleteIcon.removeClass("glyphicon-ok");
                    tthis.lastClickedDeleteIcon.addClass("glyphicon-remove");
                }
                else {
                    // Double clicked
                    if (tthis.callbackDelete === undefined) {
                        alert('callbackDelete function is not defined');
                    }

                    tthis.callbackDelete(obj.id);
                }
            }

            icon.removeClass("glyphicon-remove");
            icon.addClass("glyphicon-ok");

            tthis.lastClickedDeleteIcon = icon;
        };
    }

    /* Confirms the successful delete and 
            removes the line from table, if necessary*/
    onSuccessDelete(id: string) {
        for (var n in this.tableRows) {
            var row = this.tableRows[n];
            if (row.id == id) {
                row.row.remove();
            }
        }
    }

    update(): void {
        var tthis = this;
        // Clears the table
        this.domElement.html("");

        var htmlTable = $("<table class='table table-striped'></table>");

        var htmlRow = $("<tr class='cursor-pointer'></tr>");
        for (var columnNr in this.definition.columns) {
            var columnText = this.definition.columns[columnNr];
            var htmlTd = $("<th></th>");
            htmlTd.text(columnText);
            htmlRow.append(htmlTd);

            if (columnText === tthis.orderByColumn) {
                if (tthis.orderAscending) {
                    htmlTd.prepend($("<span class='glyphicon glyphicon glyphicon-sort-by-alphabet'></span>"));
                }
                else {
                    htmlTd.prepend($("<span class='glyphicon glyphicon glyphicon-sort-by-alphabet-alt'></span>"));
                }
            }

            htmlTd.click(tthis.getOrderFunction(columnText));
        }

        htmlRow.append($("<th></th>"));

        htmlTable.append(htmlRow);

        this.tableRows = new Array<ExtentTableViewRow>();

        for (var rowNr in this.definition.elements) {
            var isIn = false;
            if (this.searchString === "" || this.searchString === undefined) {
                // When string is empty, include it into the table view
                isIn = true;
            }

            var jQueryElements = new Array<JQuery>();
            var row = this.definition.elements[rowNr];
            htmlRow = $("<tr></tr>");
            for (var colName in row.data) {
                var value = row.data[colName];
                var htmlTd = $("<td></td>");
                htmlTd.text(value);
                htmlRow.append(htmlTd);

                if (value.indexOf(this.searchString) !== -1) {
                    // The search string is in the table
                    isIn = true;
                }
            }

            /* Doing the command */
            var htmlEdit = $("<span class='glyphicon glyphicon-pencil cursor-pointer'></span>");
            var htmlDelete = $("<span class='glyphicon glyphicon-remove cursor-pointer'></span>");
            htmlTd = $("<td class='text-right'></td>");
            htmlTd.append(htmlEdit).append(htmlDelete);
            htmlRow.append(htmlTd);

            /* Does the edit command */
            htmlEdit.click(this.getEditFunction(row, htmlEdit));
            htmlDelete.click(this.getDeleteFunction(row, htmlDelete));

            if (isIn) {
                // Add only the rows, which are included
                var elementRow = new ExtentTableViewRow();
                elementRow.row = htmlRow;
                elementRow.id = row.id;
                if (this.orderByColumn !== "" && this.orderByColumn !== undefined) {
                    elementRow.key = row.data[this.orderByColumn];
                }

                this.tableRows.push(elementRow);
            }
        } // For each row

        if (this.orderByColumn !== "" && this.orderByColumn !== undefined) {
            this.tableRows.sort(
                function (a, b) {
                    if (a.key < b.key) {
                        return tthis.orderAscending ? -1 : 1;
                    }

                    if (a.key === b.key) {
                        return 0;
                    }

                    if (a.key > b.key) {
                        return tthis.orderAscending ? 1 : -1;
                    }
                });
        }

        // Create the table
        for (var n = 0; n < this.tableRows.length; n++) {
            htmlTable.append(this.tableRows[n].row);
        }

        this.domElement.append(htmlTable);
    }
}

/* Extent Detail */

export class ExtentDetailDefinition {
    rows: Array<string>;

    data: ExtentObjectData;
}

/* Defines the class that creates a form where all the properties
    are shown and properties can be edited, delete. It is also
    possible to show this item for a new, not yet created item */
export class ExtentDetailView {
    domElement: JQuery;

    isNew: boolean;

    definition: ExtentDetailDefinition;

    constructor(domElement: JQuery) {
        this.domElement = domElement;
        this.isNew = false;
    }

    update() {
        var tthis = this;
        // Clears the table
        this.domElement.html("");

        var htmlTable = $("<table class='table table-striped'></table>");
        for (var n in this.definition.rows) {
            var keyName = this.definition.rows[n];
            var htmlTr = $("<tr></tr>");
            var htmlTdKey = $("<td></td>");
            var htmlTdValue = $("<td></td>");
            htmlTdKey.text(keyName);
            htmlTdValue.text(this.definition.data[keyName]);

            htmlTr.append(htmlTdKey).append(htmlTdValue);
            htmlTable.append(htmlTr);
        }

        this.domElement.append(htmlTable);
    }
}

/*
 * Overview configuration
 */
export class ExtentOverviewConfiguration {
    extentUri: string;
    domTable: JQuery;
    domSearchString: JQuery;
}

export class ExtentDetailConfiguration {
    extentUri: string;
    domForm: JQuery;
    id: string;
}


export class ExtentDetailViewManager {

    loadObject(config: ExtentDetailConfiguration) {
        var tthis = this;
        var view = new ExtentDetailView(
            config.domForm);

        $.get("/api/Extent/Detail",
            {
                uri: config.extentUri,
                objectId: config.id
            })
            .done(function (definition) {
                view.definition = definition;
                view.update();
            });
    }
}

export class ExtentOverviewManager {
    searchTimeoutHandler: number;

    callbackEditObject: (extentUri: string, id: string) => void;

    loadTable(config: ExtentOverviewConfiguration): ExtentTableView {
        var tthis = this;
        var result = new ExtentTableView(config.domTable);

        $.get(
            "/api/Extent/ExtentByUri",
            { uri: config.extentUri }).done(function (data) {

                result.definition.columns = data.columns;
                result.definition.elements = data.elements;
                result.update();
            });

        if (config.domSearchString !== undefined) {
            config.domSearchString.on('input', function () {
                result.searchString = config.domSearchString.val();

                if (tthis.searchTimeoutHandler !== undefined) {
                    window.clearTimeout(tthis.searchTimeoutHandler);
                }

                tthis.searchTimeoutHandler = window.setTimeout(function () {
                    result.update();
                }, 300);
            });
        }

        // Sets the delete function
        result.callbackDelete = function (id) {
            // Call the API and remove the line from the table
            $.post(
                "/api/Extent/Delete",
                { uri: config.extentUri, objectId: id })
                .done(function (data) {
                    result.onSuccessDelete(id);
                });
        };

        result.callbackEdit = function (id) {
            if (tthis.callbackEditObject !== undefined) {
                tthis.callbackEditObject(config.extentUri, id);
            }
            else {
                alert("callbackEditObject is not set");
            }
        }

        return result;
    }
}

export class View {
    static Table: number = 1;
    static Detail: number = 2;
}

export class DatenMeisterViewConfiguration {
    domTable: JQuery;
    domDetail: JQuery;
}

export class DatenMeisterViewManager {
    configuration: DatenMeisterViewConfiguration;

    constructor(configuration: DatenMeisterViewConfiguration) {
        this.configuration = configuration;
    }

    switchView(view: number) {
        if (view === View.Table) {
            this.configuration.domDetail.hide();
            this.configuration.domTable.show();
        }

        if (view === View.Detail) {
            this.configuration.domDetail.show();
            this.configuration.domTable.hide();
        }
    }
}
