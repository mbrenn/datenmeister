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

    // Defines the information for a column that has been received from server
    var JsonExtentFieldInfo = (function () {
        function JsonExtentFieldInfo(name, title) {
            if (name !== undefined) {
                this.name = name;
            }

            if (title === undefined) {
                this.title = this.name;
            }
        }
        return JsonExtentFieldInfo;
    })();
    exports.JsonExtentFieldInfo = JsonExtentFieldInfo;

    // Defines the information for a column that has been received from server
    var JsonExtentObject = (function () {
        function JsonExtentObject() {
        }
        JsonExtentObject.prototype.getUri = function () {
            return this.extentUri + "#" + this.id;
        };
        return JsonExtentObject;
    })();
    exports.JsonExtentObject = JsonExtentObject;

    // Result from GetObjectsInExtent
    var JsonExtentData = (function () {
        function JsonExtentData() {
        }
        return JsonExtentData;
    })();
    exports.JsonExtentData = JsonExtentData;

    var ajax = __ajax__;
    var t = __t__;

    // Serverconnection form
    // The Server API
    var singletonAPI;

    var ServerAPI = (function () {
        function ServerAPI(connection) {
            this.connectionInfo = connection;

            if (this.connectionInfo.serverAddress[connection.serverAddress.length - 1] != '/') {
                this.connectionInfo.serverAddress += '/';
            }

            singletonAPI = this;
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
        };

        ServerAPI.prototype.deleteObject = function (uri, success, fail) {
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
        };

        ServerAPI.prototype.editObject = function (uri, data, success, fail) {
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
                            tthis.onConnect(settings);
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

    // Creates dynamic parts of the gui
    (function (Gui) {
        // Shows the extents of the server at the given DOM element
        function showExtents(domElement) {
            singletonAPI.getExtentInfo(function (data) {
                var table = new DataTable(domElement);
                table.allowDelete = false;
                table.allowEdit = false;
                table.allowNew = false;
                table.defineColumns([
                    new JsonExtentFieldInfo("uri"),
                    new JsonExtentFieldInfo("type")
                ]);

                for (var n = 0; n < data.length; n++) {
                    var obj = new JsonExtentObject();
                    obj.id = data[n].uri;
                    obj.values = data[n];
                    table.addObject(obj);
                }

                table.setItemClickedEvent(function (object) {
                    var uri = object.values.uri;
                    showObjectsByUri(uri, $("#object_list_table"));
                });

                table.renderTable();
            }, function () {
            });
        }
        Gui.showExtents = showExtents;

        function showObjectsByUri(uri, domElement) {
            singletonAPI.getObjectsInExtent(uri, function (data) {
                showObjects(data, domElement);
            });
        }
        Gui.showObjectsByUri = showObjectsByUri;

        // Shows the object of an extent in a table, created into domElement
        function showObjects(data, domElement) {
            var table = new DataTable(domElement);

            var columns = new Array();
            for (var n = 0; n < data.columns.length; n++) {
                var column = data.columns[n];
                var info = new JsonExtentFieldInfo(column.name);
                columns.push(info);
            }

            table.defineColumns(columns);
            for (var n = 0; n < data.objects.length; n++) {
                var func = function (obj) {
                    table.addObject(obj);
                };

                func(data.objects[n]);
            }

            table.setItemClickedEvent(function (data) {
            });

            domElement.empty();
            table.renderTable();
        }
        Gui.showObjects = showObjects;

        var DataTable = (function () {
            function DataTable(domTable) {
                this.allowEdit = true;
                this.allowDelete = true;
                this.allowNew = true;
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

            // Renders the table for the given objects
            DataTable.prototype.renderTable = function () {
                var tthis = this;

                var tableOptions = new t.TableOptions();
                tableOptions.cssClass = "table";
                var table = new t.Table(this.domTable, tableOptions);

                /*
                * Creates headline
                */
                table.addHeaderRow();
                for (var n = 0; n < this.columns.length; n++) {
                    table.addColumn(this.columns[n].title);
                }

                if (this.allowDelete) {
                    table.addColumnHtml("");
                }

                if (this.allowEdit) {
                    table.addColumnHtml("");
                }

                for (var m = 0; m < this.objects.length; m++) {
                    var object = this.objects[m];

                    var func = function (object) {
                        var values = object.values;
                        var currentRow = table.addRow();

                        var columnDoms = new Array();

                        for (var n = 0; n < tthis.columns.length; n++) {
                            columnDoms.push(table.addColumnJQuery(tthis.createReadField(object, tthis.columns[n])));
                        }

                        if (tthis.itemClickedEvent !== undefined) {
                            currentRow.click(function () {
                                tthis.itemClickedEvent(object);
                            });
                        }

                        if (tthis.allowDelete) {
                            var delColumn = table.addColumnHtml("<em>DEL</em>");
                            var clicked = false;
                            delColumn.click(function () {
                                if (!clicked) {
                                    delColumn.html("<em>SURE?</em>");
                                    clicked = true;
                                } else {
                                    singletonAPI.deleteObject(object.getUri(), function () {
                                        currentRow.remove();
                                    });
                                }
                            });
                        }

                        if (tthis.allowEdit) {
                            var editColumn = table.addColumnHtml("<em>EDIT</em>");
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

                                    singletonAPI.editObject(object.getUri(), object.values, function () {
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

                    func(object);
                }

                if (this.allowNew) {
                    var row = table.addRow();

                    var newCells = new Array();

                    // Adds create text
                    var createDom = $("<em>CREATE</em>");
                    createDom.click(function () {
                        var newObject = new JsonExtentObject();
                        newObject.values = {};
                        for (var n = 0; n < tthis.columns.length; n++) {
                            newCells[n].empty();

                            var dom = tthis.createWriteField(newObject, tthis.columns[n]);
                            newCells[n].append(dom);
                        }

                        var okDom = $("<button>OK</button>");
                        okDom.click(function () {
                            alert('OK');
                        });

                        newCells[tthis.columns.length].append(okDom);
                    });

                    var cell = table.addColumnJQuery(createDom);
                    newCells.push(createDom);

                    for (var n = 0; n < this.columns.length - 1; n++) {
                        cell = table.addColumn("");
                        newCells.push(cell);
                    }

                    // Last cell, containing OK button
                    newCells.push(table.addColumn(""));
                }
            };

            DataTable.prototype.createReadField = function (object, field) {
                var span = $("<span />");
                var value = object.values[field.name];
                if (value === undefined || value === null) {
                    span.html("<em>undefined</em>");
                } else {
                    span.text(value);
                }

                return span;
            };

            DataTable.prototype.createWriteField = function (object, field) {
                var value = object.values[field.name];
                var inputField = $("<input type='text' />");
                if (value !== undefined && value !== null) {
                    inputField.val(value);
                }

                return inputField;
            };

            DataTable.prototype.setValueByWriteField = function (object, field, dom) {
                object.values[field.name] = dom.val();
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
        Gui.DataTable = DataTable;
    })(exports.Gui || (exports.Gui = {}));
    var Gui = exports.Gui;
});
