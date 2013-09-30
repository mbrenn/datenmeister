var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", 'datenmeister.datatable', "datenmeister.objects", "datenmeister.serverapi", 'lib/dejs.table', 'datenmeister.navigation'], function(require, exports, __table__, __d__, __api__, __t__, __navigation__) {
    var table = __table__;
    var d = __d__;
    var api = __api__;
    var t = __t__;
    var navigation = __navigation__;

    var DataForm = (function (_super) {
        __extends(DataForm, _super);
        function DataForm(object, domElement, options) {
            _super.call(this, domElement, options);

            this.object = object;
            this.fieldInfos = new Array();
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

                table.addColumn(f.getTitle());
                columnDoms.push(table.addColumnJQuery(tthis.createReadField(tthis.object, f)));
            });

            if (this.options.allowEdit || this.options.allowDelete) {
                table.addRow();
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
                    this.createEventsForEditButton(editButton, this.object, columnDoms);

                    div.append(editButton);
                }

                table.addColumnJQuery(div);
            }

            return this;
        };
        return DataForm;
    })(table.DataView);
    exports.DataForm = DataForm;
});
//# sourceMappingURL=datenmeister.dataform.js.map
