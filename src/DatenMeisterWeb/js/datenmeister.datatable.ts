/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
/// <reference path="lib/dejs.table.d.ts" />
/// <reference path="datenmeister.objects.ts" />

import d = require("datenmeister.objects");
import api = require("datenmeister.serverapi");
import t = require('lib/dejs.table');


export class TableOptions {
    allowEdit: boolean;
    allowNew: boolean;
    allowDelete: boolean;
}

export class DataTable {
    domTable: JQuery;

    columns: Array<d.JsonExtentFieldInfo>;

    objects: Array<d.JsonExtentObject>;

    itemClickedEvent: (object: d.JsonExtentObject) => void;

    allowEdit: boolean = true;
    allowDelete: boolean = true;
    allowNew: boolean = true;

    extent: d.ExtentInfo;

    table: t.Table;

    constructor(extent: d.ExtentInfo, domTable: JQuery) {
        this.domTable = domTable;
        this.columns = new Array<d.JsonExtentFieldInfo>();
        this.objects = new Array<any>();
        this.extent = extent;
    }

    defineColumns(columns: Array<d.JsonExtentFieldInfo>) {
        this.columns = columns;
    }

    addObject(object: d.JsonExtentObject) {
        this.objects.push(object);
    }

    // Renders the table for the given objects
    renderTable() {
        var tthis = this;

        var tableOptions = new t.TableOptions();
        tableOptions.cssClass = "table";
        this.table = new t.Table(this.domTable, tableOptions);

        /*
         * Creates headline
         */
        tthis.table.addHeaderRow();
        for (var n = 0; n < this.columns.length; n++) {
            tthis.table.addColumn(this.columns[n].title);
        }

        if (this.allowDelete) {
            tthis.table.addColumnHtml("");
        }

        if (this.allowEdit) {
            tthis.table.addColumnHtml("");
        }

        /*
         * Creates object list
         */
        for (var m = 0; m < this.objects.length; m++) {
            var object = this.objects[m];
            this.createRow(object);
        } // for (all objects)

        // Adds last line for adding, if necessary
        if (this.allowNew) {
            this.createCreateButton();
        }
    }

    createRow(object: d.JsonExtentObject): void {
        var tthis = this;
        var values = object.values
            var currentRow = tthis.table.addRow();

        var columnDoms = new Array<JQuery>();

        for (var n = 0; n < tthis.columns.length; n++) {
            columnDoms.push(
                tthis.table.addColumnJQuery(
                    tthis.createReadField(object, tthis.columns[n])));
        }

        if (tthis.itemClickedEvent !== undefined) {
            currentRow.click(function () {
                tthis.itemClickedEvent(object);
            });
        }

        // Adds delete button
        if (tthis.allowDelete) {
            var delColumn = tthis.table.addColumnHtml("<em>DEL</em>");
            var clicked = false;
            delColumn.click(function () {
                if (!clicked) {
                    delColumn.html("<em>SURE?</em>");
                    clicked = true;
                }
                else {
                    api.singletonAPI.deleteObject(object.getUri(), function () {
                        currentRow.remove();
                    });
                }
            });
        }

        // Adds allow edit button
        if (tthis.allowEdit) {
            var editColumn = tthis.table.addColumnHtml("<em>EDIT</em>");
            var currentlyInEdit = false;
            var writeFields: Array<JQuery>;

            editColumn.click(function () {
                if (!currentlyInEdit) {
                    // Is currently in reading mode, switch to writing mode
                    writeFields = new Array<JQuery>();
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
                }
                else {
                    // Is currently in writing mode, 
                    // upload to server
                    // switch to reading mode
                    for (var n = 0; n < columnDoms.length; n++) {
                        var column = tthis.columns[n];
                        tthis.setValueByWriteField(object, column, writeFields[n]);
                    }

                    api.singletonAPI.editObject(
                        object.getUri(),
                        object.values,
                        function () {
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
    }

    // Creates the create button at the end of the table
    createCreateButton() {
        var tthis = this;
        var row = tthis.table.addRow();

        var newCells = new Array<JQuery>(); // Stores the 'td' elements
        var newInputs = new Array<JQuery>(); // Stores the 'input' elements

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
    }

    // Reads the values from the inputfields and 
    // performs a request on server to add the values to database
    // Also, the form is resetted, the uploaded information is shown as read-only fields
    // and the 'CREATE' button is reinserted
    createNewElement(inputs: Array<JQuery>, cells: Array<JQuery>): void {
        var tthis = this;
        var value = new d.JsonExtentObject();
        for (var n = 0; n < this.columns.length; n++) {
            var column = this.columns[n];
            var input = inputs[n];
            this.setValueByWriteField(value, column, input);
        }

        api.singletonAPI.addObject(
            this.extent.getUri(),
            value.values,
            function (data) {
                tthis.createRow(value);

                tthis.createCreateButton();
            },
            function () {
            });
    }

    createReadField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery {
        var span = $("<span />");
        var value = object.get(field.name);
        if (value === undefined || value === null) {
            span.html("<em>undefined</em>");
        }
        else {
            span.text(value);
        }

        return span;
    }

    createWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery {
        var value = object.get(field.name);
        var inputField = $("<input type='text' />");
        if (value !== undefined && value !== null) {
            inputField.val(value);
        }

        return inputField;
    }

    setValueByWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo, dom: JQuery): void {
        object.set(field.name) = dom.val();
    }

    setItemClickedEvent(clickedEvent: (object: d.JsonExtentObject) => void): void {
        this.itemClickedEvent = clickedEvent;
    }

    // Called, when user wants to delete one object and has clicked on the delete icon. 
    // This method executes the server request. 
    triggerDelete(object: d.JsonExtentObject): void {
    }
}