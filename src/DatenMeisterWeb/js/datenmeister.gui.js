/// <reference path="lib/jquery.d.ts" />
define(["require", "exports", "datenmeister.serverapi", "datenmeister.objects", "datenmeister.datatable"], function(require, exports, __api__, __d__, __t__) {
    var api = __api__;
    var d = __d__;
    var t = __t__;

    // Shows the extents of the server at the given DOM element
    function showExtents(domElement) {
        domElement.empty();

        api.singletonAPI.getObjectsInExtent("datenmeister:///pool", function (data) {
            var table = exports.showObjects(data, $("#extent_list_table"), {
                allowNew: false,
                allowEdit: false,
                allowDelete: false
            });

            table.setItemClickedEvent(function (object) {
                exports.showObjectsByUri(object.get('uri'), $("#object_list_table"));
            });
        }, function () {
        });
    }
    exports.showExtents = showExtents;

    function showObjectsByUri(uri, domElement) {
        api.singletonAPI.getObjectsInExtent(uri, function (data) {
            exports.showObjects(data, domElement);
        });
    }
    exports.showObjectsByUri = showObjectsByUri;

    // Shows the object of an extent in a table, created into domElement
    function showObjects(data, domElement, options) {
        if (options === undefined) {
            options = new t.TableOptions();
        }

        var table = new t.DataTable(data.extent, domElement);

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
        var columns = new Array();
        for (var n = 0; n < data.columns.length; n++) {
            var column = data.columns[n];
            var info = new d.JsonExtentFieldInfo(column.name);
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

        return table;
    }
    exports.showObjects = showObjects;
});
//# sourceMappingURL=datenmeister.gui.js.map
