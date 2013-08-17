/// <reference path="lib/jquery.d.ts" />
/// <reference path="lib/dejs.ajax.d.ts" />
/// <reference path="lib/dejs.table.d.ts" />
define(["require", "exports", "lib/dejs.ajax", 'lib/dejs.table'], function(require, exports, __ajax__, __t__) {
    var ServerSettings = (function () {
        function ServerSettings() {
        }
        return ServerSettings;
    })();
    exports.ServerSettings = ServerSettings;

    var ServerInfo = (function () {
        function ServerInfo() {
        }
        return ServerInfo;
    })();
    exports.ServerInfo = ServerInfo;

    var ExtentInfo = (function () {
        function ExtentInfo() {
        }
        return ExtentInfo;
    })();
    exports.ExtentInfo = ExtentInfo;

    var ObjectData = (function () {
        function ObjectData() {
        }
        return ObjectData;
    })();
    exports.ObjectData = ObjectData;

    // Defines the information for a column that has been received from server
    var ExtentColumnInfo = (function () {
        function ExtentColumnInfo() {
        }
        return ExtentColumnInfo;
    })();
    exports.ExtentColumnInfo = ExtentColumnInfo;

    // Result from GetObjectsInExtent
    var ExtentData = (function () {
        function ExtentData() {
        }
        return ExtentData;
    })();
    exports.ExtentData = ExtentData;

    var ajax = __ajax__;
    var t = __t__;

    // Serverconnection form
    // The Server API
    var ServerAPI = (function () {
        function ServerAPI(connection) {
            this.connectionInfo = connection;

            if (this.connectionInfo.serverAddress[connection.serverAddress.length - 1] != '/') {
                this.connectionInfo.serverAddress += '/';
            }
        }
        ServerAPI.prototype.__getUrl = function () {
            return this.connectionInfo.serverAddress;
        };

        ServerAPI.prototype.getServerInfo = function (success, fail) {
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
        };

        ServerAPI.prototype.getExtentInfo = function (success, fail) {
            ajax.performRequest({
                url: this.__getUrl() + "extent/GetExtentInfos",
                prefix: 'loadextentinfos_',
                fail: function () {
                    if (fail !== undefined) {
                        fail();
                    }
                },
                success: function (data) {
                    if (success !== undefined) {
                        success(data.extents);
                    }
                }
            });
        };

        ServerAPI.prototype.getObjectsInExtent = function (uri, success, fail) {
            ajax.performRequest({
                url: this.__getUrl() + "extent/GetObjectsInExtent?uri=" + uri,
                prefix: 'loadobjects_',
                success: function (data) {
                    if (success !== undefined) {
                        success(data);
                    }
                },
                fail: function () {
                    if (fail !== undefined) {
                        fail();
                    }
                }
            });
        };
        return ServerAPI;
    })();
    exports.ServerAPI = ServerAPI;

    // Stores the form bindings to simple, predefined forms
    (function (Forms) {
        var ServerConnectionForm = (function () {
            function ServerConnectionForm(formDom) {
                this.formDom = formDom;
            }
            ServerConnectionForm.prototype.bind = function () {
                var tthis = this;
                $(".serveraddress", tthis.formDom).val(window.localStorage.getItem("serverconnection_serveraddress"));

                $(".btn-primary", this.formDom).click(function () {
                    var settings = new ServerSettings();
                    settings.serverAddress = $(".serveraddress", tthis.formDom).val();

                    window.localStorage.setItem("serverconnection_serveraddress", settings.serverAddress);

                    // Check, if connection is possible
                    var serverAPI = new ServerAPI(settings);
                    serverAPI.getServerInfo(function (info) {
                        if (tthis.onConnect !== undefined) {
                            tthis.onConnect(settings, serverAPI);
                        }
                    }, function () {
                    });

                    return false;
                });
            };
            return ServerConnectionForm;
        })();
        Forms.ServerConnectionForm = ServerConnectionForm;
    })(exports.Forms || (exports.Forms = {}));
    var Forms = exports.Forms;

    (function (Tables) {
        var ColumnDefinition = (function () {
            function ColumnDefinition(title) {
                this.title = title;
                this.width = -1;
            }
            return ColumnDefinition;
        })();
        Tables.ColumnDefinition = ColumnDefinition;

        var DataTable = (function () {
            function DataTable(domTable) {
                this.domTable = domTable;
                this.columns = new Array();
                this.objects = new Array();
            }
            DataTable.prototype.defineColumns = function (columns) {
                this.columns = columns;
            };

            DataTable.prototype.addObject = function (object) {
                this.objects.push(object);
            };

            DataTable.prototype.setItemClickedEvent = function (clickedEvent) {
                this.itemClickedEvent = clickedEvent;
            };

            DataTable.prototype.setAsReadOnly = function () {
                this.isReadOnly = true;
            };

            // Renders the table for the given objects
            DataTable.prototype.renderTable = function () {
                var tthis = this;

                var tableOptions = new t.TableOptions();
                tableOptions.cssClass = "table";
                var table = new t.Table(this.domTable, tableOptions);

                table.addHeaderRow();
                for (var n = 0; n < this.columns.length; n++) {
                    table.addColumn(this.columns[n].title);
                }

                for (var m = 0; m < this.objects.length; m++) {
                    var object = this.objects[m];

                    var func = function (object) {
                        var currentRow = table.addRow();

                        for (var n = 0; n < tthis.columns.length; n++) {
                            var value = object[tthis.columns[n].title];
                            if (value === undefined || value === null) {
                                table.addColumnHtml("<i>undefined</i>");
                            } else {
                                table.addColumn(value);
                            }
                        }

                        if (tthis.itemClickedEvent !== undefined) {
                            currentRow.click(function () {
                                tthis.itemClickedEvent(object);
                            });
                        }
                    };

                    func(object);
                }
            };
            return DataTable;
        })();
        Tables.DataTable = DataTable;
    })(exports.Tables || (exports.Tables = {}));
    var Tables = exports.Tables;

    // Creates dynamic parts of the gui
    (function (Gui) {
        // Shows the extents of the server at the given DOM element
        function showExtents(serverConnection, domElement) {
            serverConnection.getExtentInfo(function (data) {
                var table = new Tables.DataTable(domElement);
                table.defineColumns([
                    new Tables.ColumnDefinition("uri"),
                    new Tables.ColumnDefinition("type")
                ]);

                for (var n = 0; n < data.length; n++) {
                    table.addObject(data[n]);
                }

                table.setItemClickedEvent(function (object) {
                    var uri = object.uri;
                    showObjectsByUri(serverConnection, uri, $("#object_list_table"));
                });

                table.renderTable();
            }, function () {
            });
        }
        Gui.showExtents = showExtents;

        function showObjectsByUri(serverConnection, uri, domElement) {
            serverConnection.getObjectsInExtent(uri, function (data) {
                showObjects(data, domElement);
            });
        }
        Gui.showObjectsByUri = showObjectsByUri;

        function showObjects(data, domElement) {
            var table = new Tables.DataTable(domElement);

            var columns = new Array();
            for (var n = 0; n < data.columns.length; n++) {
                var column = data.columns[n];
                var info = new Tables.ColumnDefinition(column.name);
                columns.push(info);
            }

            table.defineColumns(columns);

            for (var n = 0; n < data.objects.length; n++) {
                table.addObject(data.objects[n]);
            }

            table.renderTable();
        }
        Gui.showObjects = showObjects;
    })(exports.Gui || (exports.Gui = {}));
    var Gui = exports.Gui;
});
