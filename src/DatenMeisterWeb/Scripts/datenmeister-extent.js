define(["require", "exports"], function (require, exports) {
    var ExtentTable = (function () {
        function ExtentTable() {
        }
        return ExtentTable;
    })();
    exports.ExtentTable = ExtentTable;
    var ExtentTableViewRow = (function () {
        function ExtentTableViewRow() {
        }
        return ExtentTableViewRow;
    })();
    var ExtentTableView = (function () {
        function ExtentTableView(domElement) {
            this.data = new ExtentTable();
            this.domElement = domElement;
        }
        ExtentTableView.prototype.update = function () {
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
            var rows = new Array();
            for (var rowNr in this.data.elements) {
                var isIn = false;
                if (this.searchString === "" || this.searchString === undefined) {
                    // When string is empty, include it into the table view
                    isIn = true;
                }
                var jQueryElements = new Array();
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
                    rows.sort(function (a, b) {
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
                for (var n = 0; n < rows.length; n++) {
                    htmlTable.append(rows[n].row);
                }
            }
            this.domElement.append(htmlTable);
        };
        return ExtentTableView;
    })();
    exports.ExtentTableView = ExtentTableView;
    var ExtentOverviewConfiguration = (function () {
        function ExtentOverviewConfiguration() {
        }
        return ExtentOverviewConfiguration;
    })();
    exports.ExtentOverviewConfiguration = ExtentOverviewConfiguration;
    var ExtentOverviewManager = (function () {
        function ExtentOverviewManager() {
        }
        ExtentOverviewManager.prototype.loadTable = function (config) {
            var result = new ExtentTableView(config.domTable);
            $.get("/api/Extent/Get", { uri: config.extentUri }).done(function (data) {
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
        };
        return ExtentOverviewManager;
    })();
    exports.ExtentOverviewManager = ExtentOverviewManager;
});
//# sourceMappingURL=datenmeister-extent.js.map