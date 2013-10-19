var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", 'datenmeister.datatable', "datenmeister.objects", "datenmeister.serverapi", 'lib/dejs.table', 'datenmeister.navigation'], function(require, exports, __dt__, __d__, __api__, __t__, __navigation__) {
    var dt = __dt__;
    var d = __d__;
    var api = __api__;
    var t = __t__;
    var navigation = __navigation__;

    var FormViewOptions = (function (_super) {
        __extends(FormViewOptions, _super);
        function FormViewOptions() {
            _super.apply(this, arguments);
        }
        return FormViewOptions;
    })(dt.ViewOptions);
    exports.FormViewOptions = FormViewOptions;

    var DataForm = (function (_super) {
        __extends(DataForm, _super);
        function DataForm(object, domElement, options) {
            _super.call(this, domElement, options);

            this.object = object;
            this.fieldInfos = new Array();

            if (this.options.allowNewProperty !== undefined) {
                this.options.allowNewProperty = options.allowNewProperty;
            }
        }
        /*
        * Autogenerates the fields by evaluating the contents of given object
        */
        DataForm.prototype.autoGenerateFields = function () {
            var keys = _.keys(this.object.attributes);
            for (var i = 0, len = keys.length; i < len; i++) {
                var k = keys[i];
                var v = this.object.attributes[k];

                var fieldInfo = new d.JsonExtentFieldInfo();
                fieldInfo.setTitle(k);
                fieldInfo.setName(k);
                this.fieldInfos.push(fieldInfo);
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

            table.addHeaderRow();
            table.addColumn("Key");
            table.addColumn("Value");

            _.each(this.fieldInfos, function (f) {
                table.addRow();

                table.addColumn(f.get('title'));
                columnDoms.push(table.addColumnJQuery(tthis.createReadField(tthis.object, f)));
            });

            if (this.options.allowEdit || this.options.allowDelete) {
                var lastRow = table.addRow();
                table.addColumn("");

                var div = $("<div class='lastcolumn'></div>");

                if (this.options.allowDelete) {
                    var deleteButton = $("<button class='btn btn-default'>DELETE</button>");
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

                if (this.options.allowEdit) {
                    var editButton = $("<button class='btn btn-default'>EDIT</button>");
                    var newPropertyRows = new Array();

                    var handler = new dt.DataViewEditHandler();
                    handler.bindToEditButton(this, editButton, this.object, columnDoms);
                    handler.bind('editModeChange', function (inEditMode) {
                        if (tthis.options.allowNewProperty === true) {
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

                    div.append(editButton);
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

            var keyElement = this.createWriteField(undefined, undefined);
            var valueElement = this.createWriteField(undefined, undefined);
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
        return DataForm;
    })(dt.DataView);
    exports.DataForm = DataForm;
});
//# sourceMappingURL=datenmeister.dataform.js.map
