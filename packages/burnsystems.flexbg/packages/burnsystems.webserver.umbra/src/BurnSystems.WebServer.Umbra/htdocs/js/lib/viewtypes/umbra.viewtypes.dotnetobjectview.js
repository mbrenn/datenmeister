"use strict";

define(["umbra", "umbra.instance"], function (u, umbraInstance) {
    ///
    /// Defines one entry for console
    var objectViewClass = function () {
    };

    objectViewClass.prototype =
        {
        };


    var result = {
        ObjectViewClass: objectViewClass
    };

    u.umbra.addViewType(
        new u.ViewType(
            "BurnSystems.Umbra.DetailView.DotNetObjectView",
            function (info) {
                info.viewPoint.domContent.html(
                    'DotNetObjectView');

                var objectView = new objectViewClass();
                objectView.domElement = info.viewPoint.domContent;

                var typeDom = $("<h2></h2>");
                typeDom.text(info.userData.type);

                var table = $("<table><th>Name:</th><th>Value:</th></table>");
                var properties = info.userData.properties;
                
                // Creates table
                for (var i = 0; i < properties.length; i++) {
                    var tr = $("<tr></tr>");

                    var pair = properties[i];

                    var nameCell = $("<td></td>");
                    nameCell.text(pair.name);
                    var valueCell = $("<td></td>");
                    valueCell.text(pair.value);

                    tr.append(nameCell);
                    tr.append(valueCell);

                    table.append(tr);
                }

                objectView.domElement.html(typeDom);
                objectView.domElement.append(table);
            }));

    return result;
});