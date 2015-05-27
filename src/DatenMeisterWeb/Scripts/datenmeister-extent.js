/* Extent Table */
define(["require", "exports"], function (require, exports) {
    var ExtentObjectData = (function () {
        function ExtentObjectData() {
        }
        return ExtentObjectData;
    })();
    exports.ExtentObjectData = ExtentObjectData;
    var ExtentTableDefinition = (function () {
        function ExtentTableDefinition() {
        }
        return ExtentTableDefinition;
    })();
    exports.ExtentTableDefinition = ExtentTableDefinition;
    var ExtentTableViewRow = (function () {
        function ExtentTableViewRow() {
        }
        return ExtentTableViewRow;
    })();
    exports.ExtentTableViewRow = ExtentTableViewRow;
    /* Defines the class that creates a table
        which shows all the elements and offers an edit an delete mechanism.
        */
    var ExtentTableView = (function () {
        function ExtentTableView(domElement) {
            this.definition = new ExtentTableDefinition();
            this.domElement = domElement;
            this.orderAscending = true;
        }
        ExtentTableView.prototype.getOrderFunction = function (columnText) {
            var tthis = this;
            return function () {
                if (tthis.orderByColumn == columnText) {
                    tthis.orderAscending = !tthis.orderAscending;
                }
                else {
                    tthis.orderByColumn = columnText;
                }
                tthis.update();
            };
        };
        ExtentTableView.prototype.getEditFunction = function (obj, icon) {
            var tthis = this;
            return function () {
                if (tthis.callbackEdit === undefined) {
                    alert('callbackEdit function is not defined');
                    return;
                }
                tthis.callbackEdit(obj.id);
            };
        };
        ExtentTableView.prototype.getDeleteFunction = function (obj, icon) {
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
        };
        /* Confirms the successful delete and
                removes the line from table, if necessary*/
        ExtentTableView.prototype.onSuccessDelete = function (id) {
            for (var n in this.tableRows) {
                var row = this.tableRows[n];
                if (row.id == id) {
                    row.row.remove();
                }
            }
        };
        ExtentTableView.prototype.update = function () {
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
            this.tableRows = new Array();
            for (var rowNr in this.definition.elements) {
                var isIn = false;
                if (this.searchString === "" || this.searchString === undefined) {
                    // When string is empty, include it into the table view
                    isIn = true;
                }
                var jQueryElements = new Array();
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
            }
            if (this.orderByColumn !== "" && this.orderByColumn !== undefined) {
                this.tableRows.sort(function (a, b) {
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
            for (var n = 0; n < this.tableRows.length; n++) {
                htmlTable.append(this.tableRows[n].row);
            }
            this.domElement.append(htmlTable);
        };
        return ExtentTableView;
    })();
    exports.ExtentTableView = ExtentTableView;
    /* Extent Detail */
    var ExtentDetailDefinition = (function () {
        function ExtentDetailDefinition() {
        }
        return ExtentDetailDefinition;
    })();
    exports.ExtentDetailDefinition = ExtentDetailDefinition;
    /* Defines the class that creates a form where all the properties
        are shown and properties can be edited, delete. It is also
        possible to show this item for a new, not yet created item */
    var ExtentDetailView = (function () {
        function ExtentDetailView(domElement) {
            this.domElement = domElement;
            this.isNew = false;
        }
        ExtentDetailView.prototype.update = function () {
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
        };
        return ExtentDetailView;
    })();
    exports.ExtentDetailView = ExtentDetailView;
    /*
     * Overview configuration
     */
    var ExtentOverviewConfiguration = (function () {
        function ExtentOverviewConfiguration() {
        }
        return ExtentOverviewConfiguration;
    })();
    exports.ExtentOverviewConfiguration = ExtentOverviewConfiguration;
    var ExtentDetailConfiguration = (function () {
        function ExtentDetailConfiguration() {
        }
        return ExtentDetailConfiguration;
    })();
    exports.ExtentDetailConfiguration = ExtentDetailConfiguration;
    var ExtentDetailViewManager = (function () {
        function ExtentDetailViewManager() {
        }
        ExtentDetailViewManager.prototype.loadObject = function (config) {
            var tthis = this;
            var view = new ExtentDetailView(config.domForm);
            $.get("/api/Extent/Detail", {
                uri: config.extentUri,
                objectId: config.id
            }).done(function (definition) {
                view.definition = definition;
                view.update();
            });
        };
        return ExtentDetailViewManager;
    })();
    exports.ExtentDetailViewManager = ExtentDetailViewManager;
    var ExtentOverviewManager = (function () {
        function ExtentOverviewManager() {
        }
        ExtentOverviewManager.prototype.loadTable = function (config) {
            var tthis = this;
            var result = new ExtentTableView(config.domTable);
            $.get("/api/Extent/ExtentByUri", { uri: config.extentUri }).done(function (data) {
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
                $.post("/api/Extent/Delete", { uri: config.extentUri, objectId: id }).done(function (data) {
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
            };
            return result;
        };
        return ExtentOverviewManager;
    })();
    exports.ExtentOverviewManager = ExtentOverviewManager;
    var View = (function () {
        function View() {
        }
        View.Table = 1;
        View.Detail = 2;
        return View;
    })();
    exports.View = View;
    var DatenMeisterViewConfiguration = (function () {
        function DatenMeisterViewConfiguration() {
        }
        return DatenMeisterViewConfiguration;
    })();
    exports.DatenMeisterViewConfiguration = DatenMeisterViewConfiguration;
    var DatenMeisterViewManager = (function () {
        function DatenMeisterViewManager(configuration) {
            this.configuration = configuration;
        }
        DatenMeisterViewManager.prototype.switchView = function (view) {
            if (view === View.Table) {
                this.configuration.domDetail.hide();
                this.configuration.domTable.show();
            }
            if (view === View.Detail) {
                this.configuration.domDetail.show();
                this.configuration.domTable.hide();
            }
        };
        return DatenMeisterViewManager;
    })();
    exports.DatenMeisterViewManager = DatenMeisterViewManager;
});
//# sourceMappingURL=datenmeister-extent.js.map