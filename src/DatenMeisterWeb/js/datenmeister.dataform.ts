
import dt = require ('datenmeister.datatable');
import d = require("datenmeister.objects");
import api = require("datenmeister.serverapi");
import t = require('lib/dejs.table');
import navigation = require('datenmeister.navigation');

export class FormViewOptions extends dt.ViewOptions {
    allowNewProperty: boolean;
}

export class DataForm extends dt.DataView {
    object: d.JsonExtentObject;
    domElement: JQuery;
    options: FormViewOptions;

    /* 
     * Stores the information whether the form shall be used to create a new item.
     * A 'create' form is per default 'writable'. And a new item will be created, when user clicks on 'create'. 
     */
    createNewItem: boolean; 

    constructor(object: d.JsonExtentObject, domElement: JQuery, options?: FormViewOptions) {
        super(domElement, options);

        this.object = object;
        this.fieldInfos = new Array<d.JsonExtentFieldInfo>();

        if (this.options.allowNewProperty !== undefined) {
            this.options.allowNewProperty = options.allowNewProperty;
        }
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
                var newPropertyRows = new Array<JQuery>();

                var handler = new dt.DataViewEditHandler();
                handler.bindToEditButton(this, editButton, this.object, columnDoms);
                handler.bind(
                    'editModeChange', 
                    function (inEditMode) {
                        // Called, if the user switches from view mode to edit mode or back
                        if (tthis.options.allowNewProperty === true) {
                            if (inEditMode) {
                                tthis.createNewPropertyRow(table, lastRow, handler);
                            }
                            else {
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
    }

    createNewPropertyRow(table: t.Table, lastRow: JQuery, handler: dt.DataViewEditHandler): void {
        var tthis = this;
        var newPropertyRow = table.insertRowBefore(lastRow);

        var keyElement = this.createWriteField(undefined, undefined);
        var valueElement = this.createWriteField(undefined, undefined);
        handler.addNewPropertyField(
            {
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
    }
}