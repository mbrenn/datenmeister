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

    getUri(): string {
        return this.extentUri + "#" + this.id;
    }
}

// Result from GetObjectsInExtent
export class JsonExtentData {
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

    getExtentInfo(success: (info: Array<ExtentInfo>) => void , fail?: () => void ) {
        ajax.performRequest({
            url: this.__getUrl() + "extent/GetExtentInfos",
            prefix: 'loadextentinfos_',
            fail: function () {
                if (fail !== undefined)
                {
                    fail();
                }
            },
            success: function (data) {
                if (success !== undefined)
                {
                    success(<Array<ExtentInfo>> data.extents);
                }
            }
        });
    }

    getObjectsInExtent(uri: string, success: (extentData: JsonExtentData) => void , fail?: () => void ) {
        ajax.performRequest({
            url: this.__getUrl() + "extent/GetObjectsInExtent?uri=" + uri,
            prefix: 'loadobjects_',
            success: function (data: any) {
                if (success !== undefined)
                {
                    for (var n = 0; n < data.objects.length; n++)
                    {
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
                if (fail !== undefined)
                {
                    fail();
                }
            }
        });
    }

    deleteObject(uri: string, success: () => void , fail?: () => void ) {
        ajax.performRequest({
            url: this.__getUrl() + "extent/DeleteObject?uri=" + encodeURIComponent(uri),
            prefix: 'deleteobject_',
            success: function () {
                if (success !== undefined)
                {
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
    // Shows the extents of the server at the given DOM element
    export function showExtents(domElement: JQuery) {
        singletonAPI.getExtentInfo(
            function (data) {
                var table = new DataTable(domElement);
                table.defineColumns(
                    [
                        new JsonExtentFieldInfo("uri"),
                        new JsonExtentFieldInfo("type"),
                    ]);

                for (var n = 0; n < data.length; n++)
                {
                    var obj = new JsonExtentObject()
                    obj.id = data[n].uri;
                    obj.values = data[n];
                    table.addObject(obj);
                }

                table.setItemClickedEvent(function (object: JsonExtentObject) {
                    var uri = object.values.uri;
                    showObjectsByUri(uri, $("#object_list_table"));
                });

                table.renderTable();
            },
            function () { }
            );
    }

    export function showObjectsByUri(uri: string, domElement: JQuery) {
        singletonAPI.getObjectsInExtent(
            uri,
            function (data) {
                showObjects(data, domElement);
            });
    }

    // Shows the object of an extent in a table, created into domElement
    export function showObjects(data: JsonExtentData, domElement: JQuery) {
        var table = new DataTable(domElement);

        var columns = new Array<JsonExtentFieldInfo>();
        for (var n = 0; n < data.columns.length; n++)
        {
            var column = data.columns[n];
            var info = new JsonExtentFieldInfo(column.name);
            columns.push(info);
        }

        table.defineColumns(columns);
        for (var n = 0; n < data.objects.length; n++)
        {
            var func = function (obj: JsonExtentObject) {
                table.addObject(obj);
            }

            func(data.objects[n]);
        }

        table.setItemClickedEvent(function (data) {
        });

        domElement.empty();
        table.renderTable();
    }    

    export class DataTable {
        domTable: JQuery;

        columns: Array<JsonExtentFieldInfo>;

        objects: Array<JsonExtentObject>;

        itemClickedEvent: (object: JsonExtentObject) => void;

        allowEdit: boolean = true;
        allowDelete: boolean = true;
        allowNew: boolean = true;

        constructor(domTable: JQuery) {
            this.domTable = domTable;
            this.columns = new Array<JsonExtentFieldInfo>();
            this.objects = new Array<any>();
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
            var table = new t.Table(this.domTable, tableOptions);

            /*
             * Creates headline
             */
            table.addHeaderRow();
            for (var n = 0; n < this.columns.length; n++)
            {
                table.addColumn(this.columns[n].title);
            }
            
            if (this.allowDelete)
            {
                table.addColumnHtml("");
            }

            if (this.allowEdit)
            {
                table.addColumnHtml("");
            }

            /*
             * Creates object list
             */
            for (var m = 0; m < this.objects.length; m++)
            {
                var object = this.objects[m];

                var func = function (object: JsonExtentObject) {
                    var values = object.values
                    var currentRow = table.addRow();

                    var columnDoms = new Array<JQuery>();

                    for (var n = 0; n < tthis.columns.length; n++)
                    {
                        columnDoms.push(
                            table.addColumnJQuery(
                                tthis.createReadField(object, tthis.columns[n])));
                    }

                    if (tthis.itemClickedEvent !== undefined)
                    {
                        currentRow.click(function () {
                            tthis.itemClickedEvent(object);
                        });
                    }
                    
                    if (tthis.allowDelete)
                    {
                        var delColumn = table.addColumnHtml("<em>DEL</em>");
                        var clicked = false;
                        delColumn.click(function () {
                            if (!clicked)
                            {
                                delColumn.html("<em>Sure?</em>");
                                clicked = true;
                            }
                            else
                            {
                                singletonAPI.deleteObject(object.getUri(), function () {
                                    alert('DELETE SUCCEEDED');
                                });
                            }
                        });
                    }

                    if (tthis.allowEdit)
                    {
                        var editColumn = table.addColumnHtml("<em>EDIT</em>");
                        var currentlyInEdit = false;
                        editColumn.click(function () {
                            if (!currentlyInEdit)
                            {
                                for (var n = 0; n < columnDoms.length; n++)
                                {
                                    var dom = columnDoms[n];
                                    var column = tthis.columns[n];
                                    dom.empty();

                                    dom.append(tthis.createWriteField(object, column));
                                }

                                currentlyInEdit = true;
                            }
                            else
                            {
                                for (var n = 0; n < columnDoms.length; n++)
                                {
                                    var dom = columnDoms[n];
                                    var column = tthis.columns[n];
                                    dom.empty();

                                    dom.append(tthis.createReadField(object, column));
                                }

                                currentlyInEdit = false;
                            }
                        });
                    }
                }

                func(object);
            }
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
            if (value !== undefined && value !== null)
            {
                inputField.val(value);
            }

            return inputField;
        }

        setItemClickedEvent(clickedEvent: (object: JsonExtentObject) => void ) {
            this.itemClickedEvent = clickedEvent;
        }

        // Called, when user wants to delete one object and has clicked on the delete icon. 
        // This method executes the server request. 
        triggerDelete(object: JsonExtentObject): void {
        }

        
    }
}