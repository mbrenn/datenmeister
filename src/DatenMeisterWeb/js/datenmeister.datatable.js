/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
/// <reference path="lib/dejs.table.d.ts" />
/// <reference path="datenmeister.objects.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.objects", "datenmeister.serverapi", 'lib/dejs.table'], function(require, exports, __d__, __api__, __t__) {
    var d = __d__;
    var api = __api__;
    var t = __t__;

    var ViewOptions = (function () {
        function ViewOptions() {
        }
        return ViewOptions;
    })();
    exports.ViewOptions = ViewOptions;

    /*
    * Supports the insertion of new properties by collecting them into the View
    * When called by createEventsForEditButton, this information is used to send out new properties
    */
    var NewPropertyFields = (function () {
        function NewPropertyFields() {
        }
        return NewPropertyFields;
    })();
    exports.NewPropertyFields = NewPropertyFields;

    var DataView = (function () {
        function DataView(domElement, options) {
            this.domElement = domElement;
            this.options = options;
            this.newPropertyInfos = new Array();
            if (this.options === undefined) {
                this.options = new ViewOptions();
                this.options.allowDelete = true;
                this.options.allowNew = true;
                this.options.allowEdit = true;
            }
        }
        /*
        * Defines the function that will be executed when user clicks on a certain object
        */
        DataView.prototype.setItemClickedEvent = function (clickedEvent) {
            this.itemClickedEvent = clickedEvent;
        };

        /*
        * Sets the field information objects
        */
        DataView.prototype.setFieldInfos = function (fieldInfos) {
            this.fieldInfos = fieldInfos;
        };

        /*
        * Adds a new property field that will be evaluated when the edit or creation of a new object has been finished.
        * The new property fields consists of a field containing the key and a field containing the value
        */
        DataView.prototype.addNewPropertyField = function (newField) {
            this.newPropertyInfos.push(newField);
        };

        DataView.prototype.createReadField = function (object, field) {
            var tthis = this;
            var span = $("<span />");
            var value = object.get(field.getName());
            if (value === undefined || value === null) {
                span.html("<em>undefined</em>");
            } else if (_.isArray(value)) {
                span.text('Array with ' + value.length + " items:");
                var ul = $("<ul></ul>");
                _.each(value, function (item) {
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
            } else {
                span.text(value);
            }

            return span;
        };

        DataView.prototype.createWriteField = function (object, field) {
            var value;

            if (object !== undefined && field !== undefined) {
                value = object.get(field.getName());
            }

            var inputField = $("<input type='text' />");
            if (value !== undefined && value !== null) {
                inputField.val(value);
            }

            return inputField;
        };

        DataView.prototype.setValueByWriteField = function (object, field, dom) {
            object.set(field.getName(), dom.val());
        };

        DataView.prototype.createEventsForEditButton = function (editButton, object, columnDoms, editModeChange) {
            var tthis = this;
            var currentlyInEdit = false;
            var writeFields;

            editButton.click(function () {
                if (!currentlyInEdit) {
                    // Is currently in reading mode, switch to writing mode
                    writeFields = new Array();
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
                } else {
                    for (var n = 0; n < columnDoms.length; n++) {
                        var column = tthis.fieldInfos[n];
                        tthis.setValueByWriteField(object, column, writeFields[n]);
                    }

                    // Goes through the new properties
                    _.each(tthis.newPropertyInfos, function (info) {
                        var key = info.keyField.val();
                        var value = info.valueField.val();

                        if (_.isEmpty(key)) {
                            // No key had been entered, let's remove the row
                            info.rowDom.remove();
                        } else {
                            object.set(key, value);

                            var fieldInfo = new d.JsonExtentFieldInfo();
                            fieldInfo.setName(key);
                            fieldInfo.setTitle(key);
                            tthis.fieldInfos.push(fieldInfo);
                            columnDoms.push(info.valueField.parent());

                            // making the key field as read only
                            var keyValue = info.keyField.val();
                            info.keyField.before($("<div></div>").text(keyValue));
                            info.keyField.remove();
                        }
                    });

                    tthis.newPropertyInfos.length = 0;

                    api.getAPI().editObject(object.getUri(), object, function () {
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

                if (editModeChange !== undefined) {
                    editModeChange(currentlyInEdit);
                }

                // No bubbling
                return false;
            });
        };
        return DataView;
    })();
    exports.DataView = DataView;

    var DataTable = (function (_super) {
        __extends(DataTable, _super);
        function DataTable(extent, domTable, options) {
            _super.call(this, domTable, options);

            this.fieldInfos = new Array();
            this.objects = new Array();
            this.extent = extent;

            if (options === undefined) {
                this.options = new ViewOptions();
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
        DataTable.prototype.defineFieldInfos = function (fieldInfos) {
            this.fieldInfos = fieldInfos;
        };

        DataTable.prototype.addObject = function (object) {
            this.objects.push(object);
        };

        // Renders the table for the given objects
        DataTable.prototype.render = function () {
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

            for (var n = 0; n < tthis.fieldInfos.length; n++) {
                columnDoms.push(tthis.table.addColumnJQuery(tthis.createReadField(object, tthis.fieldInfos[n])));
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

            if (tthis.options.allowDelete) {
                var delColumn = $("<a class='btn btn-default'>DEL</a>");
                var clicked = false;
                delColumn.click(function () {
                    if (!clicked) {
                        delColumn.text("SURE?");
                        clicked = true;
                    } else {
                        api.getAPI().deleteObject(object.getUri(), function () {
                            currentRow.remove();
                        });
                    }

                    return false;
                });

                lastColumn.append(delColumn);
            }

            if (tthis.options.allowEdit) {
                var editColumn = $("<a class='btn btn-default'>EDIT</a>");
                this.createEventsForEditButton(editColumn, object, columnDoms);

                lastColumn.append(editColumn);
            }

            this.table.addColumnJQuery(lastColumn);
        };

        // Creates the create button at the end of the table
        DataTable.prototype.createCreateButton = function () {
            var tthis = this;
            var row = tthis.table.addRow();

            var newCells = new Array();
            var newInputs = new Array();

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
        };

        // Reads the values from the inputfields and
        // performs a request on server to add the values to database
        // Also, the form is resetted, the uploaded information is shown as read-only fields
        // and the 'CREATE' button is reinserted
        DataTable.prototype.createNewElement = function (inputs, cells) {
            var tthis = this;
            var value = new d.JsonExtentObject();
            for (var n = 0; n < this.fieldInfos.length; n++) {
                var column = this.fieldInfos[n];
                var input = inputs[n];
                this.setValueByWriteField(value, column, input);
            }

            api.getAPI().addObject(this.extent.get('uri'), value.attributes, function (data) {
                tthis.createRow(data);

                tthis.createCreateButton();
            }, function () {
            });
        };
        return DataTable;
    })(DataView);
    exports.DataTable = DataTable;
});
//# sourceMappingURL=datenmeister.datatable.js.map
