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

export class ObjectData {
    values: any;
}

// Defines the information for a column that has been received from server
export class ExtentColumnInfo {
    name: string;
}

// Result from GetObjectsInExtent
export class ExtentData {
    columns: Array<ExtentColumnInfo>;
    objects: Array<any>;
}

import ajax = require("lib/dejs.ajax");
import t = require('lib/dejs.table');

// Serverconnection form

// The Server API
export class ServerAPI {
    connectionInfo: ServerSettings;

    constructor(connection: ServerSettings) {
        this.connectionInfo = connection;

        if (this.connectionInfo.serverAddress[connection.serverAddress.length - 1] != '/') {
            this.connectionInfo.serverAddress += '/';
        }
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

    getObjectsInExtent(uri: string, success: (extentData: ExtentData) => void , fail?: () => void ) {
        ajax.performRequest({
            url: this.__getUrl() + "extent/GetObjectsInExtent?uri=" + uri,
            prefix: 'loadobjects_',
            success: function (data) {
                if (success !== undefined)
                {
                    success(<ExtentData> data);
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
}

// Stores the form bindings to simple, predefined forms
export module Forms {
    export class ServerConnectionForm {

        // Call back, when user has depressed connect button
        onConnect: (settings: ServerSettings, api: ServerAPI) => any;

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
                            tthis.onConnect(settings, serverAPI);
                        }
                    },
                    function () {
                    });

                return false;
            });
        }
    }
}

export module Tables {

    export class ColumnDefinition {
        title: string;

        // Width in percentage
        width: number;

        constructor(title: string) {
            this.title = title;
            this.width = -1;
        }
    }

    export class DataTable {
        domTable: JQuery;

        columns: Array<ColumnDefinition>;

        objects: Array<any>;

        itemClickedEvent: (object: any) => void;

        isReadOnly: boolean;

        constructor(domTable: JQuery) {
            this.domTable = domTable;
            this.columns = new Array<ColumnDefinition>();
            this.objects = new Array<any>();
        }

        defineColumns(columns: Array<ColumnDefinition>) {
            this.columns = columns;
        }

        addObject(object: any) {
            this.objects.push(object);
        }

        setItemClickedEvent(clickedEvent: (object: any) => void ) {
            this.itemClickedEvent = clickedEvent;
        }

        setAsReadOnly() {
            this.isReadOnly = true;
        }

        // Renders the table for the given objects
        renderTable() {
            var tthis = this;

            var tableOptions = new t.TableOptions();
            tableOptions.cssClass = "table";
            var table = new t.Table(this.domTable, tableOptions);

            table.addHeaderRow();
            for (var n = 0; n < this.columns.length; n++)
            {
                table.addColumn(this.columns[n].title);
            }

            for (var m = 0; m < this.objects.length; m++)
            {
                var object = this.objects[m];

                var func = function (object) {
                    var currentRow = table.addRow();

                    for (var n = 0; n < tthis.columns.length; n++)
                    {
                        var value = object[tthis.columns[n].title];
                        if (value === undefined || value === null) {
                            table.addColumnHtml("<i>undefined</i>");
                        }
                        else {
                            table.addColumn(value);
                        }
                    }

                    if (tthis.itemClickedEvent !== undefined)
                    {
                        currentRow.click(function () {
                            tthis.itemClickedEvent(object);
                        });
                    }
                }

                func(object);
            }
        }
    }
}

// Creates dynamic parts of the gui
export module Gui {
    // Shows the extents of the server at the given DOM element
    export function showExtents(serverConnection: ServerAPI, domElement: JQuery) {
        serverConnection.getExtentInfo(
            function (data) {
                var table = new Tables.DataTable(domElement);
                table.defineColumns(
                    [
                        new Tables.ColumnDefinition("uri"),
                        new Tables.ColumnDefinition("type"),
                    ]);

                for (var n = 0; n < data.length; n++)
                {
                    table.addObject(data[n]);
                }

                table.setItemClickedEvent(function (object) {
                    var uri = object.uri;
                    showObjectsByUri(serverConnection, uri, $("#object_list_table"));
                });

                table.renderTable();
            },
            function () { }
            );
    }

    export function showObjectsByUri(serverConnection: ServerAPI, uri: string, domElement: JQuery) {        
        serverConnection.getObjectsInExtent(
            uri,
            function (data) {
                showObjects(data, domElement);
            });
    }

    // Shows the object of an extent in a table, created into domElement
    export function showObjects(data: ExtentData, domElement: JQuery) {
        var table = new Tables.DataTable(domElement);
        
        var columns = new Array<Tables.ColumnDefinition>();
        for (var n = 0; n < data.columns.length; n++)
        {
            var column = data.columns[n];
            var info = new Tables.ColumnDefinition(column.name);
            columns.push(info);
        }

        table.defineColumns(columns);

        for (var n = 0; n < data.objects.length; n++)
        {
            table.addObject(data.objects[n]);
        }

        domElement.empty();
        table.renderTable();
    }
}