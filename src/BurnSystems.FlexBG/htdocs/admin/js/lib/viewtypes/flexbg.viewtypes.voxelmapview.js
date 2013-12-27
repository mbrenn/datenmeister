"use strict";

define(["umbra", "umbra.instance", "viewtypes/umbra.viewtypes.entityview", "dejs.table"], function (u, umbraInstance, entityViewClass, tableClass) {


    u.umbra.addViewType(
        new u.ViewType(
            "BurnSystems.FlsxBG.DetailView.VoxelMapView",
            function (info) {
                info.viewPoint.domContent.html(
                    "<h1>Map Information</h1>");

                var tables = info.userData.tables;
                tables[0].options = {
                    success: function (data) {
                        if (data.success === true) {
                            resultDiv.text("");

                            var table = new tableClass(resultDiv);
                            table.addHeaderRow();
                            table.addColumn("Height");
                            table.addColumn("FieldType");

                            for (var n = 0; n < data.fieldInfos.length; n++) {
                                var info = data.fieldInfos[n];
                                table.addRow();
                                table.addColumn(info.changeHeight);
                                table.addColumn(info.fieldType);
                            }
                        }
                        else {
                            result.resultDiv.text("No Fieldinfos retrieved");
                        }
                    }
                };

                var entityView = new entityViewClass.EntityViewClass();
                entityView.processTables(tables, info.viewPoint.domContent);

                var resultDiv = $("<div></div>");
                info.viewPoint.domContent.append(resultDiv);

            }));
});