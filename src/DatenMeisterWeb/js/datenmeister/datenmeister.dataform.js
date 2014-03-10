var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", 'datenmeister.datatable', "datenmeister.serverapi", '../dejs/dejs.table', 'datenmeister.navigation', 'datenmeister.fieldinfo', 'datenmeister.fieldinfo.objects'], function(require, exports, dt, api, t, navigation, fi, fo) {
    var DataForm = (function (_super) {
        __extends(DataForm, _super);
        function DataForm(object, domElement, viewInfo) {
            _super.call(this, domElement, viewInfo);

            this.object = object;
        }
        /*
        * Autogenerates the fields by evaluating the contents of given object
        */
        DataForm.prototype.autoGenerateFields = function () {
            var keys = _.keys(this.object.attributes);
            for (var i = 0, len = keys.length; i < len; i++) {
                var k = keys[i];
                var v = this.object.attributes[k];

                this.addFieldInfo(fo.TextField.create(k, k));
            }
        };

        /*
        * Renders the form
        */
        DataForm.prototype.render = function () {
            var tthis = this;
            var columnDoms = new Array();

            var tableOptions = new t.TableOptions();
            tableOptions.cssClass = "table";
            var table = new t.Table(this.domElement, tableOptions);

            // Creates Column headers for the table
            if (fo.FormView.getShowColumnHeaders(this.viewInfo) !== false) {
                table.addHeaderRow();
                table.addColumn("Key");
                table.addColumn("Value");
            }

            // Go through each field and show the field in an appropriate column
            var fieldInfos = this.getFieldInfos();
            _.each(fieldInfos, function (f) {
                var renderer = fi.getRendererByObject(f);
                table.addRow();

                table.addColumn(f.get('title'));
                columnDoms.push(table.addColumnJQuery(renderer.createReadField(tthis.object, f, this)));
            });

            // Creates the action field for Edit and Delete
            if (fo.FormView.getAllowEdit(this.viewInfo) || fo.FormView.getAllowDelete(this.viewInfo) || fo.View.getStartInEditMode(this.viewInfo)) {
                var lastRow = table.addRow();
                table.addColumn("");

                var div = $("<div class='lastcolumn'></div>");

                var deleteButton = $("<button class='btn btn-default'>DELETE</button>");
                if (fo.FormView.getAllowEdit(this.viewInfo)) {
                    var clicked = false;
                    deleteButton.click(function () {
                        if (!clicked) {
                            deleteButton.html("SURE?");
                            clicked = true;
                        } else {
                            api.getAPI().deleteObject(tthis.object.getUri(), function () {
                                navigation.back();
                            });
                        }
                    });

                    div.append(deleteButton);
                }

                if (fo.FormView.getAllowEdit(this.viewInfo) === true || fo.View.getStartInEditMode(this.viewInfo) === true) {
                    var editButton = $("<button class='btn btn-default'>EDIT</button>");
                    var newPropertyRows = new Array();

                    var handler = new dt.DataViewEditHandler();
                    handler.bindToEditButton(this, editButton, this.object, columnDoms);
                    handler.bind('editModeChange', function (inEditMode) {
                        // Called, if the user switches from view mode to edit mode or back
                        if (fo.FormView.getAllowNewProperty(tthis.viewInfo) === true) {
                            if (inEditMode) {
                                tthis.createNewPropertyRow(table, lastRow, handler);
                            } else {
                                // Remove everything and delete array
                                _.each(newPropertyRows, function (x) {
                                    x.remove();
                                });

                                newPropertyRows.length = 0;
                            }
                        }
                    });

                    if (fo.FormView.getAllowEdit(this.viewInfo) === true) {
                        // Button is only added, when user is allowed to switch between edit and non-edit
                        div.append(editButton);
                    }

                    if (fo.View.getStartInEditMode(this.viewInfo) === true) {
                        handler.switchToEdit();
                    }

                    // Stores the form handler in the instance
                    this.formHandler = handler;
                }

                table.addColumnJQuery(div);
            }

            return this;
        };

        /*
        * Creates a new property,
        * this is inserted before the 'lastRow' and will be attached to the edit handler.
        */
        DataForm.prototype.createNewPropertyRow = function (table, lastRow, handler) {
            var tthis = this;
            var newPropertyRow = table.insertRowBefore(lastRow);

            var renderer = new fi.TextField.Renderer();

            var keyElement = renderer.createWriteField(undefined, undefined, this);
            var valueElement = renderer.createWriteField(undefined, undefined, this);
            handler.addNewPropertyField({
                rowDom: newPropertyRow,
                keyField: keyElement,
                valueField: valueElement
            });

            table.addColumnJQuery(keyElement);
            table.addColumnJQuery(valueElement);

            // Adds event to insert new property row, if content is entered into fields
            var hasEntered = false;

            var changeFunction = function () {
                if (keyElement.val().length > 0 && valueElement.val().length > 0) {
                    if (!hasEntered) {
                        tthis.createNewPropertyRow(table, lastRow, handler);
                        hasEntered = true;

                        keyElement.off('keypress', changeFunction);
                        valueElement.off('keypress', changeFunction);
                    }
                }
            };

            keyElement.on('keypress', changeFunction);
            valueElement.on('keypress', changeFunction);
        };

        DataForm.prototype.convertViewToObject = function () {
            if (this.formHandler === undefined) {
                throw "Undefined Form Handler";
            }

            this.formHandler.storeChangesInObject();
            return this.formHandler.currentObject;
        };
        return DataForm;
    })(dt.DataView);
    exports.DataForm = DataForm;
});
//# sourceMappingURL=datenmeister.dataform.js.map
