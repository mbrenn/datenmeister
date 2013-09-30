
import table = require ('datenmeister.datatable');
import d = require("datenmeister.objects");
import api = require("datenmeister.serverapi");
import t = require('lib/dejs.table');
import navigation = require('datenmeister.navigation');

export class DataForm extends table.DataView {
    object: d.JsonExtentObject;
    domElement: JQuery;

    constructor(object: d.JsonExtentObject, domElement: JQuery, options?: table.ViewOptions) {
        super(domElement, options);

        this.object = object;
        this.fieldInfos = new Array<d.JsonExtentFieldInfo>();
    }

    /*
     * Autogenerates the fields by evaluating the contents of given object
     */ 
    autoGenerateFields(): void {
        var keys = _.keys(this.object.attributes);
        for (var i = 0, len = keys.length; i < len; i++) {
            var k = keys[i];
            var v = this.object.attributes[k];

            var fieldInfo = new d.JsonExtentFieldInfo();
            fieldInfo.setTitle(k);
            fieldInfo.setName(k);
            this.fieldInfos.push(fieldInfo);
        }
    }

    /*
     * Renders the form
     */
    render(): DataForm {
        var tthis = this;
        var columnDoms = new Array<JQuery>();

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
                    }
                    else {
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
    }
}