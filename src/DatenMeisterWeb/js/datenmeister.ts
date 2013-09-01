/// <reference path="lib/jquery.d.ts" />
/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/dejs.table.d.ts" />

export class ServerSettings {
    serverAddress: string;
}

export class ServerInfo {
    serverAddress: string;
    serverName: string;
}

export class ExtentInfo {
    uri: string;
    type: string;
}

// Defines the information for a column that has been received from server
export class JsonExtentFieldInfo {
    name: string;
    title: string;

    // Number in pixels
    width: number;

    constructor(name?: string, title?: string) {
        if (name !== undefined) {
            this.name = name;
        }

        if (title === undefined) {
            this.title = this.name;
        }
    }
}

// Defines the information for a column that has been received from server
export class JsonExtentObject {
    id: string;
    values: any;
    extentUri: string;

    constructor() {
        this.values = {};
    }

    getUri(): string {
        return this.extentUri + "#" + this.id;
    }
}

// Result from GetObjectsInExtent
export class JsonExtentData {
    extent: ExtentInfo;
    columns: Array<JsonExtentFieldInfo>;
    objects: Array<JsonExtentObject>;
}

import ajax = require("lib/dejs.ajax");
import t = require('lib/dejs.table');

// Serverconnection form

// The Server API
var singletonAPI: ServerAPI;

export class ServerAPI {
    connectionInfo: ServerSettings;

    constructor(connection: ServerSettings) {
        this.connectionInfo = connection;

        if (this.connectionInfo.serverAddress[connection.serverAddress.length - 1] != '/') {
            this.connectionInfo.serverAddress += '/';
        }

        singletonAPI = this;
    }

    __getUrl(): string {
        return this.connectionInfo.serverAddress;
    }

    getServerInfo(success: (info: ServerInfo) => void , fail?: () => void ) {
        ajax.performRequest({
            url: this.__getUrl() + "extent/GetServerInfo",
            prefix: 'serverconnection_',
            fail: function () {
                if (fail !== undefined) {
                    fail();
                }
            },
            success: function (data) {
                if (success !== undefined) {
                    success(null);
                }
            }
        });
    }

    getObjectsInExtent(uri: string, success: (extentData: JsonExtentData) => void, fail?: () => void) {
        ajax.performRequest({
            url: this.__getUrl() + "extent/GetObjectsInExtent?uri=" + uri,
            prefix: 'loadobjects_',
            success: function (data: any) {
                if (success !== undefined) {
                    for (var n = 0; n < data.objects.length; n++) {
                        var currentObject = data.objects[n];
                        var result = new JsonExtentObject();
                        result.id = currentObject.id;
                        result.values = currentObject.values;
                        data.objects[n] = result;
                        data.objects[n].extentUri = uri;
                    }

                    success(data);
                }
            },
            fail: function () {
                if (fail !== undefined) {
                    fail();
                }
            }
        });
    }

    deleteObject(uri: string, success: () => void, fail?: () => void) {
        ajax.performRequest({
            url: this.__getUrl() + "extent/DeleteObject?uri=" + encodeURIComponent(uri),
            prefix: 'deleteobject_',
            method: 'post',
            success: function () {
                if (success !== undefined) {
                    success();
                }
            },
            fail: function () {
                if (fail !== undefined) {
                    fail();
                }
            }
        });
    }

    editObject(uri: string, data: any, success: () => void, fail?: () => void) {
        ajax.performRequest({
            url: this.__getUrl() + "extent/EditObject?uri=" + encodeURIComponent(uri),
            prefix: 'editobject_',
            method: 'post',
            data: data,
            success: function () {
                if (success !== undefined) {
                    success();
                }
            },
            fail: function () {
                if (fail !== undefined) {
                    fail();
                }
            }
        });   
    }

    addObject(uri: string, data: any, success: (data: JsonExtentObject) => void, fail?: () => void) {
        ajax.performRequest({
            url: this.__getUrl() + "extent/AddObject?uri=" + encodeURIComponent(uri),
            prefix: 'editobject_',
            method: 'post',
            data: data,
            success: function (data) {
                if (success !== undefined) {
                    success(<JsonExtentObject> data.element);
                }
            },
            fail: function () {
                if (fail !== undefined) {
                    fail();
                }
            }
        });
    }
}

// Stores the form bindings to simple, predefined forms
export module Forms {
    export class ServerConnectionForm {

        // Call back, when user has depressed connect button
        onConnect: (settings: ServerSettings) => any;

        formDom: JQuery;

        constructor(formDom) {
            this.formDom = formDom;
        }

        bind() {
            var tthis = this;
            $(".serveraddress", tthis.formDom).val(window.localStorage.getItem("serverconnection_serveraddress"));

            $(".btn-primary", this.formDom).click(function () {
                var settings = new ServerSettings();
                settings.serverAddress = $(".serveraddress", tthis.formDom).val();

                window.localStorage.setItem("serverconnection_serveraddress", settings.serverAddress);

                // Check, if connection is possible
                var serverAPI = new ServerAPI(settings);
                serverAPI.getServerInfo(
                    function (info) {
                        // Yes, do the connect
                        if (tthis.onConnect !== undefined) {
                            tthis.onConnect(settings);
                        }
                    },
                    function () {
                    });

                return false;
            });
        }
    }
}

// Creates dynamic parts of the gui
export module Gui {

    export class TableOptions {
        allowEdit: boolean;
        allowNew: boolean;
        allowDelete: boolean;
    }

    // Shows the extents of the server at the given DOM element
    export function showExtents(domElement: JQuery): void {

        singletonAPI.getObjectsInExtent("datenmeister:///pool",
            function (data) {
                var table = showObjects(
                    data,
                    $("#extent_list_table"),
                    {
                        allowNew: false,
                        allowEdit: false,
                        allowDelete: false
                    });

                table.setItemClickedEvent(function (object: JsonExtentObject) {
                    showObjectsByUri(object.values.uri, $("#object_list_table"));
                });
            },
            function () {
            });
    }

    export function showObjectsByUri(uri: string, domElement: JQuery): void {
        singletonAPI.getObjectsInExtent(
            uri,
            function (data) {
                showObjects(data, domElement);
            });
    }

    // Shows the object of an extent in a table, created into domElement
    export function showObjects(data: JsonExtentData, domElement: JQuery, options?: TableOptions): DataTable {
        if (options === undefined) {
            options = new TableOptions();
        }

        var table = new DataTable(data.extent, domElement);

        // Options configuration
        if (options.allowNew === false) {
            table.allowNew = options.allowNew;
        }

        if (options.allowEdit === false) {
            table.allowEdit = options.allowEdit;
        }

        if (options.allowDelete === false) {
            table.allowDelete = options.allowDelete;
        }

        // Create the columns
        var columns = new Array<JsonExtentFieldInfo>();
        for (var n = 0; n < data.columns.length; n++) {
            var column = data.columns[n];
            var info = new JsonExtentFieldInfo(column.name);
            columns.push(info);
        }

        table.defineColumns(columns);
        for (var n = 0; n < data.objects.length; n++) {
            var func = function (obj: JsonExtentObject) {
                table.addObject(obj);
            }

            func(data.objects[n]);
        }

        table.setItemClickedEvent(function (data) {
        });

        domElement.empty();
        table.renderTable();

        return table;
    }

    export class DataTable {
        domTable: JQuery;

        columns: Array<JsonExtentFieldInfo>;

        objects: Array<JsonExtentObject>;

        itemClickedEvent: (object: JsonExtentObject) => void;

        allowEdit: boolean = true;
        allowDelete: boolean = true;
        allowNew: boolean = true;

        extent: ExtentInfo;

        table: t.Table;

        constructor(extent: ExtentInfo, domTable: JQuery) {
            this.domTable = domTable;
            this.columns = new Array<JsonExtentFieldInfo>();
            this.objects = new Array<any>();
            this.extent = extent;
        }

        defineColumns(columns: Array<JsonExtentFieldInfo>) {
            this.columns = columns;
        }

        addObject(object: JsonExtentObject) {
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

        createRow(object: JsonExtentObject): void {
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
                        singletonAPI.deleteObject(object.getUri(), function () {
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

                        singletonAPI.editObject(
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

                var newObject = new JsonExtentObject();
                newObject.values = {};
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
            var value = new JsonExtentObject();
            for (var n = 0; n < this.columns.length; n++) {
                var column = this.columns[n];
                var input = inputs[n];
                this.setValueByWriteField(value, column, input);
            }

            singletonAPI.addObject(
                this.extent.uri,
                value.values,
                function (data) {
                    tthis.createRow(value);

                    tthis.createCreateButton();
                },
                function () {
                });
        }

        createReadField(object: JsonExtentObject, field: JsonExtentFieldInfo): JQuery {
            var span = $("<span />");
            var value = object.values[field.name];
            if (value === undefined || value === null) {
                span.html("<em>undefined</em>");
            }
            else {
                span.text(value);
            }

            return span;
        }

        createWriteField(object: JsonExtentObject, field: JsonExtentFieldInfo): JQuery {
            var value = object.values[field.name];
            var inputField = $("<input type='text' />");
            if (value !== undefined && value !== null) {
                inputField.val(value);
            }

            return inputField;
        }

        setValueByWriteField(object: JsonExtentObject, field: JsonExtentFieldInfo, dom: JQuery): void {
            object.values[field.name] = dom.val();
        }

        setItemClickedEvent(clickedEvent: (object: JsonExtentObject) => void): void {
            this.itemClickedEvent = clickedEvent;
        }

        // Called, when user wants to delete one object and has clicked on the delete icon. 
        // This method executes the server request. 
        triggerDelete(object: JsonExtentObject): void {
        }
    }
}