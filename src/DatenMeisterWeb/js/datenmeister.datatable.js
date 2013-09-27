/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
/// <reference path="lib/dejs.table.d.ts" />
/// <reference path="datenmeister.objects.ts" />
define(["require", "exports", "datenmeister.objects", "datenmeister.serverapi", 'lib/dejs.table'], function(require, exports, __d__, __api__, __t__) {
    var d = __d__;
    var api = __api__;
    var t = __t__;

    var TableOptions = (function () {
        function TableOptions() {
        }
        return TableOptions;
    })();
    exports.TableOptions = TableOptions;

    var DataTable = (function () {
        function DataTable(extent, domTable, options) {
            this.domTable = domTable;
            this.columns = new Array();
            this.objects = new Array();
            this.extent = extent;

            if (options === undefined) {
                this.options = new TableOptions();
            } else {
                this.options = options;
            }

            if (this.options.allowDelete === undefined) {
                this.options.allowDelete = true;
            }

            if (this.options.allowEdit === undefined) {
                this.options.allowEdit = true;
            }

            if (this.options.allowNew === undefined) {
                this.options.allowNew = true;
            }
        }
        DataTable.prototype.defineColumns = function (columns) {
            this.columns = columns;
        };

        DataTable.prototype.addObject = function (object) {
            this.objects.push(object);
        };

        // Renders the table for the given objects
        DataTable.prototype.renderTable = function () {
            var tthis = this;

            var tableOptions = new t.TableOptions();
            tableOptions.cssClass = "table";
            this.table = new t.Table(this.domTable, tableOptions);

            /*
            * Creates headline
            */
            tthis.table.addHeaderRow();
            for (var n = 0; n < this.columns.length; n++) {
                tthis.table.addColumn(this.columns[n].getTitle());
            }

            if (this.options.allowDelete) {
                tthis.table.addColumnHtml("");
            }

            if (this.options.allowEdit) {
                tthis.table.addColumnHtml("");
            }

            for (var m = 0; m < this.objects.length; m++) {
                var object = this.objects[m];
                this.createRow(object);
            }

            if (this.options.allowNew) {
                this.createCreateButton();
            }
        };

        DataTable.prototype.createRow = function (object) {
            var tthis = this;
            var values = object.values;
            var currentRow = tthis.table.addRow();

            var columnDoms = new Array();

            for (var n = 0; n < tthis.columns.length; n++) {
                columnDoms.push(tthis.table.addColumnJQuery(tthis.createReadField(object, tthis.columns[n])));
            }

            if (tthis.itemClickedEvent !== undefined) {
                currentRow.click(function () {
                    tthis.itemClickedEvent(object);
                });
            }

            if (tthis.options.allowDelete) {
                var delColumn = tthis.table.addColumnHtml("<em>DEL</em>");
                var clicked = false;
                delColumn.click(function () {
                    if (!clicked) {
                        delColumn.html("<em>SURE?</em>");
                        clicked = true;
                    } else {
                        api.singletonAPI.deleteObject(object.getUri(), function () {
                            currentRow.remove();
                        });
                    }
                });
            }

            if (tthis.options.allowEdit) {
                var editColumn = tthis.table.addColumnHtml("<em>EDIT</em>");
                var currentlyInEdit = false;
                var writeFields;

                editColumn.click(function () {
                    if (!currentlyInEdit) {
                        // Is currently in reading mode, switch to writing mode
                        writeFields = new Array();
                        for (var n = 0; n < columnDoms.length; n++) {
                            var dom = columnDoms[n];
                            var column = tthis.columns[n];
                            dom.empty();

                            var writeField = tthis.createWriteField(object, column);
                            writeFields.push(writeField);
                            dom.append(writeField);
                            editColumn.html("<em>ACCEPT</em>");
                        }

                        currentlyInEdit = true;
                    } else {
                        for (var n = 0; n < columnDoms.length; n++) {
                            var column = tthis.columns[n];
                            tthis.setValueByWriteField(object, column, writeFields[n]);
                        }

                        api.singletonAPI.editObject(object.getUri(), object, function () {
                            for (var n = 0; n < columnDoms.length; n++) {
                                var dom = columnDoms[n];
                                var column = tthis.columns[n];
                                dom.empty();
                                dom.append(tthis.createReadField(object, column));
                            }

                            editColumn.html("<em>EDIT</em>");
                        });

                        currentlyInEdit = false;
                    }
                });
            }
        };

        // Creates the create button at the end of the table
        DataTable.prototype.createCreateButton = function () {
            var tthis = this;
            var row = tthis.table.addRow();

            var newCells = new Array();
            var newInputs = new Array();

            // Adds create text
            var createDom = $("<em>CREATE</em>");
            createDom.click(function () {
                newInputs.length = 0;

                var newObject = new d.JsonExtentObject();
                for (var n = 0; n < tthis.columns.length; n++) {
                    newCells[n].empty();

                    var dom = tthis.createWriteField(newObject, tthis.columns[n]);
                    newInputs.push(dom);

                    newCells[n].append(dom);
                }

                var okDom = $("<button>OK</button>");
                okDom.click(function () {
                    tthis.createNewElement(newInputs, newCells);
                    row.remove();
                });

                newCells[tthis.columns.length].append(okDom);
            });

            var cell = tthis.table.addColumnJQuery(createDom);
            newCells.push(cell);

            for (var n = 0; n < this.columns.length - 1; n++) {
                cell = tthis.table.addColumn("");
                newCells.push(cell);
            }

            // Last cell, containing OK button
            newCells.push(tthis.table.addColumn(""));
        };

        // Reads the values from the inputfields and
        // performs a request on server to add the values to database
        // Also, the form is resetted, the uploaded information is shown as read-only fields
        // and the 'CREATE' button is reinserted
        DataTable.prototype.createNewElement = function (inputs, cells) {
            var tthis = this;
            var value = new d.JsonExtentObject();
            for (var n = 0; n < this.columns.length; n++) {
                var column = this.columns[n];
                var input = inputs[n];
                this.setValueByWriteField(value, column, input);
            }

            api.singletonAPI.addObject(this.extent.get('uri'), value.attributes, function (data) {
                tthis.createRow(value);

                tthis.createCreateButton();
            }, function () {
            });
        };

        DataTable.prototype.createReadField = function (object, field) {
            var span = $("<span />");
            var value = object.get(field.getName());
            if (value === undefined || value === null) {
                span.html("<em>undefined</em>");
            } else {
                span.text(value);
            }

            return span;
        };

        DataTable.prototype.createWriteField = function (object, field) {
            var value = object.get(field.getName());
            var inputField = $("<input type='text' />");
            if (value !== undefined && value !== null) {
                inputField.val(value);
            }

            return inputField;
        };

        DataTable.prototype.setValueByWriteField = function (object, field, dom) {
            object.set(field.getName(), dom.val());
        };

        DataTable.prototype.setItemClickedEvent = function (clickedEvent) {
            this.itemClickedEvent = clickedEvent;
        };

        // Called, when user wants to delete one object and has clicked on the delete icon.
        // This method executes the server request.
        DataTable.prototype.triggerDelete = function (object) {
        };
        return DataTable;
    })();
    exports.DataTable = DataTable;
});
//# sourceMappingURL=datenmeister.datatable.js.map
