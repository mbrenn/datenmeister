
import dt = require ('datenmeister.datatable');
import d = require("datenmeister.objects");
import api = require("datenmeister.serverapi");
import t = require('../dejs/dejs.table');
import navigation = require('datenmeister.navigation');
import fi = require('datenmeister.fieldinfo');
import fo = require('datenmeister.fieldinfo.objects');

export class DataForm extends dt.DataView implements fi.IDataView {

    /* The object, that is shown */
    object: d.JsonExtentObject;

    /* The domElement, which will contain the data form */
    domElement: JQuery;

    /* Stores the form handler, that is used for the data form */
    formHandler: dt.DataViewEditHandler;

    /* 
     * Stores the information whether the form shall be used to create a new item.
     * A 'create' form is per default 'writable'. And a new item will be created, when user clicks on 'create'. 
     */
    createNewItem: boolean;

    constructor(object: d.JsonExtentObject, domElement: JQuery, viewInfo?: d.JsonExtentObject) {
        super(domElement, viewInfo);

        this.object = object;
    }

    /*
     * Autogenerates the fields by evaluating the contents of given object
     */
    autoGenerateFields(): void {
        var keys = _.keys(this.object.attributes);
        for (var i = 0, len = keys.length; i < len; i++) {
            var k = keys[i];
            var v = this.object.attributes[k];

            this.addFieldInfo(fo.TextField.create(k, k));
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
        if (fo.View.getAllowEdit(this.viewInfo)
            || fo.View.getAllowDelete(this.viewInfo)
            || fo.View.getStartInEditMode(this.viewInfo)) {
            var lastRow = table.addRow();
            table.addColumn("");

            var div = $("<div class='lastcolumn'></div>");

            var deleteButton = $("<button class='btn btn-default'>DELETE</button>");
            if (fo.View.getAllowEdit(this.viewInfo)) {
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

            if (fo.View.getAllowEdit(this.viewInfo) === true || fo.View.getStartInEditMode(this.viewInfo) === true) {
                var editButton = $("<button class='btn btn-default'>EDIT</button>");
                var newPropertyRows = new Array<JQuery>();

                var handler = new dt.DataViewEditHandler();
                handler.bindToEditButton(this, editButton, this.object, columnDoms);
                handler.bind(
                    'editModeChange',
                    function (inEditMode) {
                        // Called, if the user switches from view mode to edit mode or back
                        if (fo.FormView.getAllowNewProperty(tthis.viewInfo) === true) {
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

                if (fo.View.getAllowEdit(this.viewInfo) === true) {
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
    }

    /*
     * Creates a new property,
     * this is inserted before the 'lastRow' and will be attached to the edit handler.
     */
    createNewPropertyRow(table: t.Table, lastRow: JQuery, handler: dt.DataViewEditHandler): void {
        var tthis = this;
        var newPropertyRow = table.insertRowBefore(lastRow);

        var renderer = new fi.TextField.Renderer();

        var keyElement = renderer.createWriteField(undefined, undefined, this);
        var valueElement = renderer.createWriteField(undefined, undefined, this);
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

    convertViewToObject(): d.JsonExtentObject {
        if (this.formHandler === undefined) {
            throw "Undefined Form Handler";
        }

        this.formHandler.storeChangesInObject();
        return this.formHandler.currentObject;
    }
}
