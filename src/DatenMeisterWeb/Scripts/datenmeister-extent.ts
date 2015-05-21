
export class ExtentTable {

    columns: Array<string>;

    elements: Array<Array<string>>;
}

class ExtentTableViewRow {
    row: JQuery;
    
    /**
     * Stores the key to be used for sorting
     */
    key: string;
}

export class ExtentTableView {
    domElement: JQuery;

    searchString: string;

    orderByColumn: string;

    orderAscending: boolean;

    data: ExtentTable;

    constructor(domElement: JQuery) {
        this.data = new ExtentTable();
        this.domElement = domElement;
    }

    update(): void {
        // Clears the table
        this.domElement.html("");

        var htmlTable = $("<table class='table table-striped'></table>");

        var htmlRow = $("<tr></tr>");
        for (var columnNr in this.data.columns) {
            var htmlTd = $("<th></th>");
            htmlTd.text(this.data.columns[columnNr]);
            htmlRow.append(htmlTd);
        }        

        htmlTable.append(htmlRow);

        var rows = new Array<ExtentTableViewRow>();

        for (var rowNr in this.data.elements) {
            var isIn = false;
            if (this.searchString === "" || this.searchString === undefined) {
                // When string is empty, include it into the table view
                isIn = true;
            }

            var jQueryElements = new Array<JQuery>();
            var row = this.data.elements[rowNr];
            htmlRow = $("<tr></tr>");
            for (var colName in row) {
                var value = row[colName];
                var htmlTd = $("<td></td>");
                htmlTd.text(value);
                htmlRow.append(htmlTd);

                if (value.indexOf(this.searchString) !== -1) {
                    // The search string is in the table
                    isIn = true;
                }
            }

            if (isIn) {
                // Add only the rows, which are included
                var elementRow = new ExtentTableViewRow();
                elementRow.row = htmlRow;
                if (this.searchString !== "" && this.searchString !== undefined) {
                    elementRow.key = row[this.searchString];
                }
                rows.push(elementRow);
            }

            // Sort, if necessary
            var tthis = this;
            if (this.searchString !== "" && this.searchString !== undefined) {
                rows.sort(
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
                    }
                    );
            }


            // Create the table
            for (var n = 0; n < rows.length; n++) {
                htmlTable.append(rows[n].row);
            }
        }

        this.domElement.append(htmlTable);
    }
}

export class ExtentOverviewConfiguration {
    extentUri: string;
    domTable: JQuery;
    domSearchString: JQuery;
}

export class ExtentOverviewManager {    
    loadTable(config: ExtentOverviewConfiguration): ExtentTableView {
        var result = new ExtentTableView(config.domTable);

        $.get(
            "/api/Extent/Get",
            { uri: config.extentUri }).done(function (data) {

                result.data.columns = data.Columns;
                result.data.elements = data.Elements;
                result.update();
            });

        if (config.domSearchString !== undefined) {
            config.domSearchString.on('input', function () {
                result.searchString = config.domSearchString.val();
                result.update();
            });
        }

        return result;
    }
}
