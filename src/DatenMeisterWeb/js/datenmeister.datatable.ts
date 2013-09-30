/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
/// <reference path="lib/dejs.table.d.ts" />
/// <reference path="datenmeister.objects.ts" />

import d = require("datenmeister.objects");
import api = require("datenmeister.serverapi");
import t = require('lib/dejs.table');

export class ViewOptions {
    allowEdit: boolean;
    allowNew: boolean;
    allowDelete: boolean;
}

export class DataView {

    options: ViewOptions;
    domElement: JQuery;
    fieldInfos: Array<d.JsonExtentFieldInfo>;

    constructor(domElement: JQuery, options: ViewOptions) {
        this.domElement = domElement;
        this.options = options;
        if (this.options === undefined) {
            this.options = new ViewOptions();
            this.options.allowDelete = true;
            this.options.allowNew = true;
            this.options.allowEdit = true;
        }
    }

    /*
     * Sets the field information objects
     */
    setFieldInfos(fieldInfos: Array<d.JsonExtentFieldInfo>) {
        this.fieldInfos = fieldInfos;
    }

    createReadField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery {
        var span = $("<span />");
        var value = object.get(field.getName());
        if (value === undefined || value === null) {
            span.html("<em>undefined</em>");
        }
        else {
            span.text(value);
        }

        return span;
    }

    createWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery {
        var value = object.get(field.getName());
        var inputField = $("<input type='text' />");
        if (value !== undefined && value !== null) {
            inputField.val(value);
        }

        return inputField;
    }

    setValueByWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo, dom: JQuery): void {
        object.set(field.getName(), dom.val());
    }

    createEventsForEditButton(editButton: JQuery, object: d.JsonExtentObject, columnDoms: Array<JQuery>) {
        var tthis = this;
        var currentlyInEdit = false;
        var writeFields: Array<JQuery>;

        editButton.click(function () {
            if (!currentlyInEdit) {
                // Is currently in reading mode, switch to writing mode
                writeFields = new Array<JQuery>();
                for (var n = 0; n < columnDoms.length; n++) {
                    var dom = columnDoms[n];
                    var column = tthis.fieldInfos[n];
                    dom.empty();

                    var writeField = tthis.createWriteField(object, column);
                    writeFields.push(writeField);
                    dom.append(writeField);
                    editButton.html("ACCEPT");
                }

                currentlyInEdit = true;
            }
            else {
                // Is currently in writing mode, 
                // upload to server
                // switch to reading mode
                for (var n = 0; n < columnDoms.length; n++) {
                    var column = tthis.fieldInfos[n];
                    tthis.setValueByWriteField(object, column, writeFields[n]);
                }

                api.getAPI().editObject(
                    object.getUri(),
                    object,
                    function () {
                        for (var n = 0; n < columnDoms.length; n++) {
                            var dom = columnDoms[n];
                            var column = tthis.fieldInfos[n];
                            dom.empty();
                            dom.append(tthis.createReadField(object, column));
                        }

                        editButton.html("EDIT");
                    });

                currentlyInEdit = false;
            }

            // No bubbling
            return false;
        });
    }

}

export class DataTable extends DataView{

    objects: Array<d.JsonExtentObject>;

    itemClickedEvent: (object: d.JsonExtentObject) => void;

    extent: d.ExtentInfo;

    table: t.Table;

    constructor(extent: d.ExtentInfo, domTable: JQuery, options?: ViewOptions) {
        super(domTable, options);

        this.fieldInfos = new Array<d.JsonExtentFieldInfo>();
        this.objects = new Array<any>();
        this.extent = extent;

        if (options === undefined) {
            this.options = new ViewOptions();
        }
        else {
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

    defineFieldInfos(fieldInfos: Array<d.JsonExtentFieldInfo>) {
        this.fieldInfos = fieldInfos;
    }

    addObject(object: d.JsonExtentObject) {
        this.objects.push(object);
    }

    // Renders the table for the given objects
    render() {
        var tthis = this;

        var tableOptions = new t.TableOptions();
        tableOptions.cssClass = "table table-hover table-striped";

        this.domElement.empty();
        this.table = new t.Table(this.domElement, tableOptions);

        /*
         * Creates headline
         */
        tthis.table.addHeaderRow();
        for (var n = 0; n < this.fieldInfos.length; n++) {
            tthis.table.addColumn(this.fieldInfos[n].getTitle());
        }

        // For the last column, containing all the settings
        tthis.table.addColumnHtml("");

        /*
         * Creates object list
         */
        for (var m = 0; m < this.objects.length; m++) {
            var object = this.objects[m];
            this.createRow(object);
        } // for (all objects)

        // Adds last line for adding, if necessary
        if (this.options.allowNew) {
            this.createCreateButton();
        }
    }

    createRow(object: d.JsonExtentObject): void {
        var tthis = this;
        var values = object.values
        var currentRow = tthis.table.addRow();

        var columnDoms = new Array<JQuery>();

        for (var n = 0; n < tthis.fieldInfos.length; n++) {
            columnDoms.push(
                tthis.table.addColumnJQuery(
                    tthis.createReadField(object, tthis.fieldInfos[n])));
        }

        var lastColumn = $("<div class='lastcolumn'></div>");
        if (tthis.itemClickedEvent !== undefined) {
            var detailColumn = $("<a class='btn btn-default'>DETAIL</a>");
            detailColumn.click(function () {
                tthis.itemClickedEvent(object);

                return false;
            });

            lastColumn.append(detailColumn);
        }

        // Adds delete button
        if (tthis.options.allowDelete) {
            var delColumn = $("<a class='btn btn-default'>DEL</a>");
            var clicked = false;
            delColumn.click(function () {
                if (!clicked) {
                    delColumn.text("SURE?");
                    clicked = true;
                }
                else {
                    api.getAPI().deleteObject(object.getUri(), function () {
                        currentRow.remove();
                    });
                }

                return false;
            });

            lastColumn.append(delColumn);
        }

        // Adds allow edit button
        if (tthis.options.allowEdit) {
            var editColumn = $("<a class='btn btn-default'>EDIT</a>");
            this.createEventsForEditButton(editColumn, object, columnDoms);

            lastColumn.append(editColumn);
        }

        this.table.addColumnJQuery(lastColumn);
    }

    // Creates the create button at the end of the table
    createCreateButton() {
        var tthis = this;
        var row = tthis.table.addRow();

        var newCells = new Array<JQuery>(); // Stores the 'td' elements
        var newInputs = new Array<JQuery>(); // Stores the 'input' elements

        // Adds create text
        var createDom = $("<button class='btn btn-default'>CREATE</button>");
        createDom.click(function () {
            newInputs.length = 0;

            var newObject = new d.JsonExtentObject();
            for (var n = 0; n < tthis.fieldInfos.length; n++) {
                newCells[n].empty();

                var dom = tthis.createWriteField(newObject, tthis.fieldInfos[n]);
                newInputs.push(dom);

                newCells[n].append(dom);
            }

            var okDom = $("<div class='lastcolumn'><button class='btn btn-default'>ADD</button></div>");
            okDom.click(function () {
                tthis.createNewElement(newInputs, newCells);
                row.remove();

                return false;
            });

            newCells[tthis.fieldInfos.length].append(okDom);

            return false;
        });

        var cell = tthis.table.addColumnJQuery(createDom);
        newCells.push(cell);

        for (var n = 0; n < this.fieldInfos.length; n++) {
            newCells.push(tthis.table.addColumn(""));
        }
    }

    // Reads the values from the inputfields and 
    // performs a request on server to add the values to database
    // Also, the form is resetted, the uploaded information is shown as read-only fields
    // and the 'CREATE' button is reinserted
    createNewElement(inputs: Array<JQuery>, cells: Array<JQuery>): void {
        var tthis = this;
        var value = new d.JsonExtentObject();
        for (var n = 0; n < this.fieldInfos.length; n++) {
            var column = this.fieldInfos[n];
            var input = inputs[n];
            this.setValueByWriteField(value, column, input);
        }

        api.getAPI().addObject(
            this.extent.get('uri'),
            value.attributes,
            function (data) {
                tthis.createRow(value);

                tthis.createCreateButton();
            },
            function () {
            });
    }

    setItemClickedEvent(clickedEvent: (object: d.JsonExtentObject) => void): void {
        this.itemClickedEvent = clickedEvent;
    }

    // Called, when user wants to delete one object and has clicked on the delete icon. 
    // This method executes the server request. 
    triggerDelete(object: d.JsonExtentObject): void {
    }
}