/// <reference path="../backbone/backbone.d.ts" />
/// <reference path="../underscore/underscore.d.ts" />
/// <reference path="../dejs/dejs.table.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.objects", "datenmeister.serverapi", "datenmeister.fieldinfo", '../dejs/dejs.table'], function(require, exports, __d__, __api__, __fi__, __t__) {
    var d = __d__;
    var api = __api__;
    var fi = __fi__;
    var t = __t__;

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

    /*
    * This handler is offering the switch from edit to view by providing methods.
    */
    var DataViewEditHandler = (function (_super) {
        __extends(DataViewEditHandler, _super);
        function DataViewEditHandler() {
            this.newPropertyInfos = new Array();
            this.writeFields = new Array();

            _super.call(this);
        }
        /*
        * Adds a new property field that will be evaluated when the edit or creation of a new object has been finished.
        * The new property fields consists of a field containing the key and a field containing the value
        */
        DataViewEditHandler.prototype.addNewPropertyField = function (newField) {
            this.newPropertyInfos.push(newField);
        };

        DataViewEditHandler.prototype.switchToEdit = function () {
            // Is currently in reading mode, switch to writing mode
            this.writeFields = new Array();
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
        };

        // Stores the changes, being done by user or webscripts into object
        DataViewEditHandler.prototype.storeChangesInObject = function () {
            var fieldInfos = this.view.getFieldInfos();

            for (var n = 0; n < this.columnDoms.length; n++) {
                var column = fieldInfos[n];

                var renderer = fi.getRendererByObject(column);
                renderer.setValueByWriteField(this.currentObject, column, this.writeFields[n], this.view);
            }
        };

        // Switches the current form (whether table or form) to read
        // Sets necessary information to field, if necessary
        DataViewEditHandler.prototype.switchToRead = function () {
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
                } else {
                    tthis.currentObject.set(key, value);

                    var fieldInfo = new d.JsonExtentObject();
                    fi.General.setName(fieldInfo, key);
                    fi.General.setTitle(fieldInfo, key);
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
            api.getAPI().editObject(this.currentObject.getUri(), this.currentObject, function () {
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
        };

        // Binds the current form to the switch to an edit view
        // @view: View to be attached
        // @editButton: Button that shall be used to switch between both views
        // @object: The object that shall store the object data
        // @columnDoms: An array of Dom-Elements, that store the DOM-content being created by 'createReadField' and 'createWriteField'.
        //              The content will be erased during each switch
        DataViewEditHandler.prototype.bindToEditButton = function (view, editButton, object, columnDoms) {
            var tthis = this;

            this.columnDoms = columnDoms;
            this.currentObject = object;
            this.view = view;
            this.editButton = editButton;

            var currentlyInEdit = false;
            var writeFields;

            editButton.click(function () {
                if (!currentlyInEdit) {
                    tthis.switchToEdit();
                    currentlyInEdit = true;
                } else {
                    tthis.switchToRead();
                    currentlyInEdit = false;
                }

                // No bubbling
                return false;
            });
        };
        return DataViewEditHandler;
    })(Backbone.Model);
    exports.DataViewEditHandler = DataViewEditHandler;

    var DataView = (function () {
        function DataView(domElement, viewInfo) {
            this.domElement = domElement;
            this.viewInfo = viewInfo;
            if (this.viewInfo === undefined) {
                this.viewInfo = fi.TableView.create();
            }
        }
        /*
        * Sets the field information objects
        */
        DataView.prototype.setFieldInfos = function (fieldInfos) {
            return fi.View.setFieldInfos(this.viewInfo, fieldInfos);
        };

        /*
        * Adds a field info to the current data view
        */
        DataView.prototype.addFieldInfo = function (fieldInfo) {
            return fi.View.pushFieldInfo(this.viewInfo, fieldInfo);
        };

        DataView.prototype.getFieldInfos = function () {
            return fi.View.getFieldInfos(this.viewInfo);
        };

        /*
        * Defines the function that will be executed when user clicks on a certain object
        */
        DataView.prototype.setItemClickedEvent = function (clickedEvent) {
            this.itemClickedEvent = clickedEvent;
        };

        /*
        * This method is not implemented by DataView, it has to be implemented by subclasses
        */
        DataView.prototype.convertViewToObject = function () {
            throw "Not implemented";
        };

        /*
        * Evaluates the response from a server action, that has direct consequences for models
        * and/or views
        */
        DataView.prototype.evaluateActionResponse = function (data) {
            if (data.actions !== undefined) {
                for (var n = 0; n < data.actions.length; n++) {
                    this.executeAction(data.actions[n]);
                }
            }
        };

        DataView.prototype.executeAction = function (action) {
            if (action.type === 'RefreshBrowserWinder') {
                window.location.reload(true);
            }
        };
        return DataView;
    })();
    exports.DataView = DataView;

    var DataTable = (function (_super) {
        __extends(DataTable, _super);
        function DataTable(extent, domTable, viewInfo) {
            _super.call(this, domTable, viewInfo);

            this.objects = new Array();
            this.extent = extent;

            if (viewInfo === undefined) {
                this.viewInfo = fi.TableView.create();
            } else {
                this.viewInfo = viewInfo;
            }

            if (fi.View.getAllowDelete(this.viewInfo) === undefined) {
                fi.View.setAllowDelete(this.viewInfo, true);
            }

            if (fi.View.getAllowEdit(this.viewInfo) === undefined) {
                fi.View.setAllowEdit(this.viewInfo, true);
            }

            if (fi.View.getAllowNew(this.viewInfo) === undefined) {
                fi.View.setAllowNew(this.viewInfo, true);
            }
        }
        /*
        * Performs an auto-generation of
        */
        DataTable.prototype.autoGenerateColumns = function () {
            var tthis = this;

            var fieldInfos = tthis.getFieldInfos();

            // Goes through every object
            _.each(this.objects, function (obj) {
                for (var key in obj.attributes) {
                    if (!(_.some(fieldInfos, function (info) {
                        return fi.General.getName(info) == key;
                    }))) {
                        // No, so create new field info
                        var fieldInfo = new d.JsonExtentObject();
                        fi.General.setName(fieldInfo, key);
                        fi.General.setTitle(fieldInfo, key);
                        fi.General.setReadOnly(fieldInfo, false);
                        tthis.addFieldInfo(fieldInfo);
                    }
                }
            });
        };

        DataTable.prototype.addObject = function (object) {
            this.objects.push(object);
        };

        // Renders the table for the given objects
        DataTable.prototype.render = function () {
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
                tthis.table.addColumn(fi.General.getTitle(fieldInfos[n]));
            }

            // For the last column, containing all the settings
            tthis.table.addColumnHtml("");

            for (var m = 0; m < this.objects.length; m++) {
                var object = this.objects[m];
                this.createRow(object);
            }

            if (fi.View.getAllowNew(this.viewInfo)) {
                this.createCreateButton();
            }
        };

        DataTable.prototype.createRow = function (object) {
            var tthis = this;
            var values = object.values;
            var currentRow = tthis.table.addRow();

            var columnDoms = new Array();

            var fieldInfos = tthis.getFieldInfos();
            for (var n = 0; n < this.getFieldInfos().length; n++) {
                var fieldInfo = fieldInfos[n];
                var renderer = fi.getRendererByObject(fieldInfo);
                columnDoms.push(tthis.table.addColumnJQuery(renderer.createReadField(object, fieldInfos[n], this)));
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

            if (fi.View.getAllowDelete(this.viewInfo)) {
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

            if (fi.View.getAllowEdit(this.viewInfo)) {
                var editColumn = $("<a class='btn btn-default'>EDIT</a>");

                var handler = new DataViewEditHandler();
                handler.bindToEditButton(this, editColumn, object, columnDoms);

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
        };

        // Reads the values from the inputfields and
        // performs a request on server to add the values to database
        // Also, the form is resetted, the uploaded information is shown as read-only fields
        // and the 'CREATE' button is reinserted
        DataTable.prototype.createNewElement = function (inputs, cells) {
            var tthis = this;
            var value = new d.JsonExtentObject();

            var fieldInfos = tthis.getFieldInfos();

            for (var n = 0; n < fieldInfos.length; n++) {
                var column = fieldInfos[n];
                var input = inputs[n];
                var renderer = fi.getRendererByObject(column);

                renderer.setValueByWriteField(value, column, input, this);
            }

            api.getAPI().addObject(this.extent.get('uri'), value.attributes, function (data) {
                tthis.createRow(data);

                tthis.createCreateButton();
            }, function () {
            });
        };

        DataTable.prototype.convertViewToObject = function () {
            throw "Not implemented";
        };
        return DataTable;
    })(DataView);
    exports.DataTable = DataTable;
});
//# sourceMappingURL=datenmeister.datatable.js.map
