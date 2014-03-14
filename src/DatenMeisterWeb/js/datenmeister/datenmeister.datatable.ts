/// <reference path="../backbone/backbone.d.ts" />
/// <reference path="../underscore/underscore.d.ts" />
/// <reference path="../dejs/dejs.table.d.ts" />

import d = require("datenmeister.objects");
import api = require("datenmeister.serverapi");
import fi = require("datenmeister.fieldinfo");
import fo = require("datenmeister.fieldinfo.objects");
import t = require('../dejs/dejs.table');

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
        this.newPropertyInfos = new Array<NewPropertyFields>();
        this.writeFields = new Array<JQuery>();

        super();
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
        var fieldInfos = this.view.getFieldInfos();

        for (var n = 0; n < this.columnDoms.length; n++) {
            var dom = this.columnDoms[n];
            var column = fieldInfos[n];
            var renderer = fi.getRendererByObject(column);

            dom.empty();

            var writeField = renderer.createWriteField(this.currentObject, column, this.view);
            this.writeFields.push(writeField);
            dom.append(writeField);
            this.editButton.html("ACCEPT");
        }

        this.trigger('editModeChange', true);
    }

    // Stores the changes, being done by user or webscripts into object
    storeChangesInObject(): void {
        var fieldInfos = this.view.getFieldInfos();

        for (var n = 0; n < this.columnDoms.length; n++) {
            var column = fieldInfos[n];

            var renderer = fi.getRendererByObject(column);
            renderer.setValueByWriteField(this.currentObject, column, this.writeFields[n], this.view);
        }
    }

    // Switches the current form (whether table or form) to read
    // Sets necessary information to field, if necessary
    switchToRead(): void {
        var tthis = this;

        // Stores the changes into the given object
        this.storeChangesInObject();

        // Goes through the new properties being created during the detail view
        _.each(this.newPropertyInfos, function (info) {
            // This is quite special
            var key = info.keyField.val();
            var value = info.valueField.val();

            if (_.isEmpty(key)) {
                // No key had been entered, let's remove the row
                info.rowDom.remove();
            }
            else {
                tthis.currentObject.set(key, value);

                var fieldInfo = new d.JsonExtentObject();
                fo.General.setName(fieldInfo, key);
                fo.General.setTitle(fieldInfo, key);
                tthis.view.addFieldInfo(fieldInfo);
                tthis.columnDoms.push(info.valueField.parent());

                // making the key field as read only
                var keyValue = info.keyField.val();
                info.keyField.before($("<div></div>").text(keyValue));
                info.keyField.remove();
            }
        });

        tthis.newPropertyInfos.length = 0;
        var fieldInfos = this.view.getFieldInfos();

        // Changes the object on server
        api.getAPI().editObject(
            this.currentObject.getUri(),
            this.currentObject,
            function () {
                for (var n = 0; n < tthis.columnDoms.length; n++) {
                    var dom = tthis.columnDoms[n];
                    var column = fieldInfos[n];
                    var renderer = fi.getRendererByObject(column);
                    dom.empty();
                    dom.append(renderer.createReadField(tthis.currentObject, column, this.view));
                }

                tthis.editButton.html("EDIT");
            });

        // Throw the necessary event
        this.trigger('editModeChange', false);
    }

    // Binds the current form to the switch to an edit view
    // @view: View to be attached
    // @editButton: Button that shall be used to switch between both views
    // @object: The object that shall store the object data
    // @columnDoms: An array of Dom-Elements, that store the DOM-content being created by 'createReadField' and 'createWriteField'. 
    //              The content will be erased during each switch
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

export class DataView implements fi.IDataView {

    itemClickedEvent: (object: d.JsonExtentObject) => void;

    viewInfo: d.JsonExtentObject;
    domElement: JQuery;

    constructor(domElement: JQuery, viewInfo?: d.JsonExtentObject) {
        this.domElement = domElement;
        this.viewInfo = viewInfo;
        if (this.viewInfo === undefined) {
            this.viewInfo = fo.TableView.create();
        }
    }

    /*
     * Sets the field information objects
     */
    setFieldInfos(fieldInfos: Array<d.JsonExtentObject>) {

        return fo.View.setFieldInfos(this.viewInfo, fieldInfos);
    }

    /*
     * Adds a field info to the current data view
     */
    addFieldInfo(fieldInfo: d.JsonExtentObject) {
        return fo.View.pushFieldInfo(this.viewInfo, fieldInfo);
    }

    getFieldInfos(): Array<d.JsonExtentObject> {
        return fo.View.getFieldInfos(this.viewInfo);
    }

    /*
     * Defines the function that will be executed when user clicks on a certain object
     */
    setItemClickedEvent(clickedEvent: (object: d.JsonExtentObject) => void): void {
        this.itemClickedEvent = clickedEvent;
    }

    /*
     * This method is not implemented by DataView, it has to be implemented by subclasses
     */
    convertViewToObject(): d.JsonExtentObject {
        throw "Not implemented";
    }

    /*
     * Evaluates the response from a server action, that has direct consequences for models
     * and/or views
     */
    evaluateActionResponse(data: fi.ClientActionInformation): void {
        if (data.actions !== undefined) {
            for (var n = 0; n < data.actions.length; n++) {
                this.executeAction(data.actions[n]);
            }
        }
    }

    executeAction(action: any): void {
        if (action.type === 'RefreshBrowserWinder') {
            window.location.reload(true);
        }
    }
}

export class DataTable extends DataView {

    objects: Array<d.JsonExtentObject>;

    extent: d.ExtentInfo;

    table: t.Table;

    constructor(extent: d.ExtentInfo, domTable: JQuery, viewInfo?: d.JsonExtentObject) {
        super(domTable, viewInfo);

        this.objects = new Array<any>();
        this.extent = extent;

        if (viewInfo === undefined) {
            this.viewInfo = fo.TableView.create();
        }
        else {
            this.viewInfo = viewInfo;
        }

        if (fo.TableView.getAllowDelete(this.viewInfo) === undefined) {
            fo.TableView.setAllowDelete(this.viewInfo, true);
        }

        if (fo.TableView.getAllowEdit(this.viewInfo) === undefined) {
            fo.TableView.setAllowEdit(this.viewInfo, true);
        }

        if (fo.TableView.getAllowNew(this.viewInfo) === undefined) {
            fo.TableView.setAllowNew(this.viewInfo, true);
        }
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
                var fieldInfos = tthis.getFieldInfos();
                if (!(_.some(fieldInfos, function (info) {
                    return fo.General.getName(<d.JsonExtentObject> info) == key;
                }))) {
                    // No, so create new field info
                    var fieldInfo = new d.JsonExtentObject();
                    fo.General.setName(fieldInfo, key);
                    fo.General.setTitle(fieldInfo, key);
                    fo.General.setReadOnly(fieldInfo, false);
                    tthis.addFieldInfo(fieldInfo);
                }
            }
        });
    }

    addObject(object: d.JsonExtentObject) {
        this.objects.push(object);
    }

    // Renders the table for the given objects
    render(): void {
        var tthis = this;

        var fieldInfos = tthis.getFieldInfos();

        var tableOptions = new t.TableOptions();
        tableOptions.cssClass = "table table-hover table-striped";

        this.domElement.empty();
        this.table = new t.Table(this.domElement, tableOptions);

        /*
         * Creates headline
         */
        tthis.table.addHeaderRow();

        for (var n = 0; n < fieldInfos.length; n++) {
            tthis.table.addColumn(fo.General.getTitle(fieldInfos[n]));
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
        if (fo.TableView.getAllowNew(this.viewInfo)) {
            this.createCreateButton();
        }
    }

    createRow(object: d.JsonExtentObject): void {
        var tthis = this;
        var values = object.values
        var currentRow = tthis.table.addRow();

        var columnDoms = new Array<JQuery>();

        var fieldInfos = tthis.getFieldInfos();
        for (var n = 0; n < this.getFieldInfos().length; n++) {
            var fieldInfo = fieldInfos[n];
            var renderer = fi.getRendererByObject(fieldInfo);
            columnDoms.push(
                tthis.table.addColumnJQuery(
                    renderer.createReadField(object, fieldInfos[n], this)));
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
        if (fo.TableView.getAllowDelete(this.viewInfo)) {
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
        if (fo.TableView.getAllowEdit(this.viewInfo)) {
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
        var fieldInfos = tthis.getFieldInfos();
        var createDom = $("<button class='btn btn-default'>CREATE</button>");
        createDom.click(function () {
            newInputs.length = 0;

            var newObject = new d.JsonExtentObject();
            for (var n = 0; n < fieldInfos.length; n++) {
                newCells[n].empty();

                var renderer = fi.getRendererByObject(fieldInfos[n]);

                var dom = renderer.createWriteField(newObject, fieldInfos[n], this);
                newInputs.push(dom);

                newCells[n].append(dom);
            }

            var okDom = $("<div class='lastcolumn'><button class='btn btn-default'>ADD</button></div>");
            okDom.click(function () {
                tthis.createNewElement(newInputs, newCells);
                row.remove();

                return false;
            });

            newCells[fieldInfos.length].append(okDom);

            return false;
        });

        var cell = tthis.table.addColumnJQuery(createDom);
        newCells.push(cell);

        for (var n = 0; n < fieldInfos.length; n++) {
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

        var fieldInfos = tthis.getFieldInfos();

        for (var n = 0; n < fieldInfos.length; n++) {
            var column = fieldInfos[n];
            var input = inputs[n];
            var renderer = fi.getRendererByObject(column);

            renderer.setValueByWriteField(value, column, input, this);
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

    convertViewToObject(): d.JsonExtentObject {
        throw "Not implemented";
    }
}