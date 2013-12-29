/// <reference path="../backbone/backbone.d.ts" />
/// <reference path="../underscore/underscore.d.ts" />
/// <reference path="../dejs/dejs.table.d.ts" />
/// <reference path="datenmeister.objects.ts" />

import d = require("datenmeister.objects");
import api = require("datenmeister.serverapi");
import t = require('../dejs/dejs.table');

/* 
 * Defines the view options for a table or detail view as a complete table
 */
export class ViewOptions {
    allowEdit: boolean = true;
    allowNew: boolean = true;
    allowDelete: boolean = true;
}

/*
 * Supports the insertion of new properties by collecting them into the View 
 * When called by createEventsForEditButton, this information is used to send out new properties
 */
export class NewPropertyFields {
    keyField: JQuery;
    valueField: JQuery;
    rowDom: JQuery; // Stores the complete row, that may be deleted, if key is empty
}

/* 
 * This handler is offering the switch from edit to view by providing methods.
 */
export class DataViewEditHandler extends Backbone.Model {

    // Stores the object that will be evaluated
    currentObject: d.JsonExtentObject;

    // Stores the 'input' fields
    writeFields: Array<JQuery>;

    // Stores the td-Elements
    columnDoms: Array<JQuery>;

    // Stores the JQuery DOM item of the edit button
    editButton: JQuery;

    // Stores the view being used
    view: DataView;

    // Array of new properties
    newPropertyInfos: Array<NewPropertyFields>;

    constructor() {
        super();
        this.newPropertyInfos = new Array<NewPropertyFields>();
        this.writeFields = new Array<JQuery>();
    }

    /*
     * Adds a new property field that will be evaluated when the edit or creation of a new object has been finished.
     * The new property fields consists of a field containing the key and a field containing the value
     */
    addNewPropertyField(newField: NewPropertyFields): void {
        this.newPropertyInfos.push(newField);
    }

    switchToEdit(): void {
        // Is currently in reading mode, switch to writing mode
        this.writeFields = new Array<JQuery>();
        for (var n = 0; n < this.columnDoms.length; n++) {
            var dom = this.columnDoms[n];
            var column = this.view.fieldInfos[n];
            dom.empty();

            var writeField = this.view.createWriteField(this.currentObject, column);
            this.writeFields.push(writeField);
            dom.append(writeField);
            this.editButton.html("ACCEPT");
        }

        this.trigger('editModeChange', true);
    }

    switchToRead(): void {
        var tthis = this;

        // Is currently in writing mode, 
        // upload to server
        // switch to reading mode
        for (var n = 0; n < this.columnDoms.length; n++) {
            var column = this.view.fieldInfos[n];
            this.view.setValueByWriteField(this.currentObject, column, this.writeFields[n]);
        }

        // Goes through the new properties being created during the detail view
        _.each(this.newPropertyInfos, function (info) {
            var key = info.keyField.val();
            var value = info.valueField.val();

            if (_.isEmpty(key)) {
                // No key had been entered, let's remove the row
                info.rowDom.remove();
            }
            else {
                tthis.currentObject.set(key, value);

                var fieldInfo = new d.JsonExtentFieldInfo();
                fieldInfo.setName(key);
                fieldInfo.setTitle(key);
                tthis.view.fieldInfos.push(fieldInfo);
                tthis.columnDoms.push(info.valueField.parent());

                // making the key field as read only
                var keyValue = info.keyField.val();
                info.keyField.before($("<div></div>").text(keyValue));
                info.keyField.remove();
            }
        });

        tthis.newPropertyInfos.length = 0;

        api.getAPI().editObject(
            this.currentObject.getUri(),
            this.currentObject,
            function () {
                for (var n = 0; n < tthis.columnDoms.length; n++) {
                    var dom = tthis.columnDoms[n];
                    var column = tthis.view.fieldInfos[n];
                    dom.empty();
                    dom.append(tthis.view.createReadField(tthis.currentObject, column));
                }

                tthis.editButton.html("EDIT");
            });

        // Throw the necessary event
        this.trigger('editModeChange', false);
    }

    bindToEditButton(view: DataView, editButton: JQuery, object: d.JsonExtentObject, columnDoms: Array<JQuery>): void {
        var tthis = this;

        this.columnDoms = columnDoms;
        this.currentObject = object;
        this.view = view;
        this.editButton = editButton;

        var currentlyInEdit = false;
        var writeFields: Array<JQuery>;

        editButton.click(function () {
            if (!currentlyInEdit) {
                tthis.switchToEdit();
                currentlyInEdit = true;
            }
            else {
                tthis.switchToRead();
                currentlyInEdit = false;
            }

            // No bubbling
            return false;
        });
    }
}

export class DataView {

    itemClickedEvent: (object: d.JsonExtentObject) => void;

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

    /*
     * Defines the function that will be executed when user clicks on a certain object
     */
    setItemClickedEvent(clickedEvent: (object: d.JsonExtentObject) => void): void {
        this.itemClickedEvent = clickedEvent;
    }

    createReadField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery {
        var tthis = this;
        var span = $("<span />");
        var value = object.get(field.get('name'));
        if (value === undefined || value === null) {
            span.html("<em>undefined</em>");
        }
        else if (_.isArray(value)) {
            span.text('Array with ' + value.length + " items:");
            var ul = $("<ul></ul>");
            _.each(value, function (item: d.JsonExtentFieldInfo) {
                var div = $("<li></li>");
                div.text(JSON.stringify(item.toJSON()) + " | " + item.id);
                div.click(function () {
                    if (tthis.itemClickedEvent !== undefined) {
                        tthis.itemClickedEvent(item);
                    }
                });

                ul.append(div);
            });

            span.append(ul);
        }
        else {
            span.text(value);
        }

        return span;
    }

    createWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo): JQuery {
        var value;

        if (object !== undefined && field !== undefined) {
            value = object.get(field.getName());
        }

        // Checks, if writing is possible
        var offerWriting = true;
        if (_.isArray(value) || _.isObject(value)) {
            offerWriting = false;
        }

        // Creates the necessary controls
        if (offerWriting) {
            // Offer writing
            var inputField = $("<input type='text' />");
            if (value !== undefined && value !== null) {
                inputField.val(value);
            }

            return inputField;
        }
        else {
            return this.createReadField(object, field);

        }
    }

    setValueByWriteField(object: d.JsonExtentObject, field: d.JsonExtentFieldInfo, dom: JQuery): void {
        if (field.getReadOnly() === true) {
            // Do nothing
            return;
        }

        // Reads the value
        object.set(field.getName(), dom.val());
    }
}

export class DataTable extends DataView{

    objects: Array<d.JsonExtentObject>;

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

    /* 
     * Performs an auto-generation of 
     */
    autoGenerateColumns(): void {
        var tthis = this;

        // Goes through every object
        _.each(this.objects, function (obj: d.JsonExtentObject) {
            // Goes through the attributes    
            for (var key in obj.attributes) {
                // Checks, if already in
                if (!(_.some(tthis.fieldInfos, function (info) {
                    return info.attributes.name == key;
                }))) {
                    // No, so create new field info
                    var fieldInfo = new d.JsonExtentFieldInfo();
                    fieldInfo.setName(key);
                    fieldInfo.setTitle(key);
                    fieldInfo.setReadOnly(false);
                    tthis.fieldInfos.push(fieldInfo);
                }
            }
        });
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

        // Adds last line for adding new items, if necessary
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

            var handler = new DataViewEditHandler();
            handler.bindToEditButton(this, editColumn, object, columnDoms);

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
            function (data: d.JsonExtentObject) {
                tthis.createRow(data);

                tthis.createCreateButton();
            },
            function () {
            });
    }
}