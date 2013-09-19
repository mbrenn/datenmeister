/// <reference path="lib/jquery.d.ts" />

import api = require("datenmeister.serverapi");
import d = require("datenmeister.objects");
import t = require("datenmeister.datatable");

// Shows the extents of the server at the given DOM element
export function showExtents(domElement: JQuery): void {
    domElement.empty();

    api.singletonAPI.getObjectsInExtent(
        "datenmeister:///pool",
        function (data) {
            var table = showObjects(
                data,
                $("#extent_list_table"),
                {
                    allowNew: false,
                    allowEdit: false,
                    allowDelete: false
                });

            table.setItemClickedEvent(function (object: d.JsonExtentObject) {
                showObjectsByUri(object.get('uri'), $("#object_list_table"));
            });
        },
        function () {
        });
}

export function showObjectsByUri(uri: string, domElement: JQuery): void {
    api.singletonAPI.getObjectsInExtent(
        uri,
        function (data) {
            showObjects(data, domElement);
        });
}

// Shows the object of an extent in a table, created into domElement
export function showObjects(data: d.JsonExtentData, domElement: JQuery, options?: t.TableOptions): t.DataTable {
    if (options === undefined) {
        options = new t.TableOptions();
    }

    var table = new t.DataTable(data.extent, domElement);

    // Options configuration
    if (options.allowNew === false) {
        table.allowNew = options.allowNew;
    }

    if (options.allowEdit === false) {
        table.allowEdit = options.allowEdit;
    }

    if (options.allowDelete === false) {
        table.allowDelete = options.allowDelete;
    }

    // Create the columns
    var columns = new Array<d.JsonExtentFieldInfo>();
    for (var n = 0; n < data.columns.length; n++) {
        var column = data.columns[n];
        var info = new d.JsonExtentFieldInfo(column.name);
        columns.push(info);
    }

    table.defineColumns(columns);
    for (var n = 0; n < data.objects.length; n++) {
        var func = function (obj: d.JsonExtentObject) {
            table.addObject(obj);
        }

        func(data.objects[n]);
    }

    table.setItemClickedEvent(function (data) {
    });

    domElement.empty();
    table.renderTable();

    return table;
}