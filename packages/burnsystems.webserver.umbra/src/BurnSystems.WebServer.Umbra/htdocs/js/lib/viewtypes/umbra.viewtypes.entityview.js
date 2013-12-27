"use strict";

requirejs.config(
{
    //By default load any module IDs from js/lib
    baseUrl: 'js/lib/',
});

define(["umbra", "umbra.instance", "plugins/umbra.console", "dejs.table"], function (u, umbraInstance, consoleLog, tableClass) {

    var _tableNumber = 0; // Incrementing tablenumber

    var entityViewTable = function (tablename) {
        _tableNumber++;

        this.tablename = "t_" + _tableNumber;
        this.nextElementName = 0;
    };

    entityViewTable.prototype = {
        getNextElementId: function () {
            this.nextElementName++;
            return "e_" + this.tablename + "_" + this.nextElementName;
        },

        createDetailTable: function (entityView, domContent, table) {
            if (table.options === undefined) {
                table.options = {};
            }

            // Create table
            var t = new tableClass(domContent);
            t.addHeaderRow();
            t.addColumn("Property:");
            t.addColumn("Value:");

            var elements = table.elements;
            var data = table.data;
            var assignResults = [];

            // Creates the table for the elements
            for (var i = 0; i < elements.length; i++) {
                var item = elements[i];
                var dataElement = data[i];

                t.addRow();

                ////////////////////////////////
                // Name of the item
                t.addColumn(item.label + ": ");

                ////////////////////////////////
                // Value of the item
                var columnDom = t.addColumn();
                var view = entityView.getView(item.dataType);
                var assignResult = view.assignHtml(item, dataElement, columnDom, this);
                assignResults.push(assignResult);
            }

            //
            // Creates the button
            t.addRow();
            t.addColumn();
            var buttonColumnDom = 
                t.addColumnHtml(
                "<span class=\"success\"></span><span class=\"nosuccess\"></span>");
            
            var buttonText = "Update";
            if (table.buttonText !== undefined && table.buttonText !== null) {
                buttonText = table.buttonText;
            }

            var buttonDom = $("<input type=\"button\" value=\"" + buttonText + "\"></input>");
            buttonDom.click(function () {
                var data = {};

                for (var i = 0; i < elements.length; i++) {
                    var element = elements[i];
                    var assignResult = assignResults[i];

                    if (assignResult === undefined) {
                        continue;
                    }

                    var result = assignResult.updateCallback();
                    if (result === undefined) {
                        continue;
                    }

                    data[element.name] = result;
                }

                buttonDom.attr("disabled", "disabled");
                $(".success", buttonColumnDom).text("");
                $(".nosuccess", buttonColumnDom).text("");

                var updateUrl = table.overrideUrl;

                $.ajax(
                    updateUrl,
                    {
                        type: 'POST',
                        data: data
                    })
                .success(function (returnData) {
                    buttonDom.removeAttr("disabled");
                    $(".success", buttonColumnDom).text("Update succeeded");

                    if (table.options.success !== undefined) {
                        table.options.success(returnData);
                    }
                })
                .error(function (jqXHR, textStatus, error) {
                    consoleLog.console.logAjaxError(jqXHR, textStatus, error);

                    buttonDom.removeAttr("disabled");
                    $(".nosuccess", buttonColumnDom).text("Update failed");
                });
            });

            buttonColumnDom.prepend(buttonDom);
        },

        createListTable: function (entityView, domContent, table, options) {
            if (options === undefined) {
                options = {};
            }

            // Create table
            var t = new tableClass(domContent);
            t.addHeaderRow();

            var elements = table.elements;
            var data = table.data;
            var assignResults = [];

            // Creates the table for the elements
            for (var i = 0; i < elements.length; i++) {
                var item = elements[i];

                // For the list, the elements will get a read-only. No support for writing
                item.readOnly = true; 
                
                // Create the header
                t.addColumn(item.label);
            }

            for (var r = 0; r < data.length; r++) {
                t.addRow();
                for (var c = 0; c < elements.length; c++) {
                    var item = elements[c];
                    var dataElement = data[r][c];
                    
                    var columnDom = t.addColumn();
                    var view = entityView.getView(item.dataType);
                    var assignResult = view.assignHtml(item, dataElement, columnDom, this);
                }
            }
        }
    };

    var entityAssignResult = function () {
        this.updateCallback = function () {
            alert('Callback function has not been set');
        };
    };

    var entityPropertyTextbox = function () {
    };

    entityPropertyTextbox.prototype =
        {
            /* 
             * config: Configuration of the item. See init.cs and EntityViewElement.ToJson
             * item: The item itself, how it had been sent by EntityViewElement.ObjectToJson
             * dom: Dom-Element, where item shall be added
             * tableInfo: Instance of entityViewTable Instance
             */
            assignHtml: function (config, item, dom, tableInfo) {
                if (config.readOnly) {
                    dom.text(item);
                }
                else {
                    var elementId = tableInfo.getNextElementId();
                    var inputElement = $("<input type=\"textbox\"></input>");
                    inputElement.attr('id', elementId);

                    inputElement.val(item);

                    if (config.width > 0) {
                        inputElement.css("width", config.width + "em");
                    }

                    if (config.height > 0) {
                        inputElement.css("width", config.height + "em");
                    }

                    dom.append(inputElement);

                }

                var result = new entityAssignResult();
                result.updateCallback = function () {
                    if (config.readOnly) {
                        return item;
                    }

                    return $("#" + elementId).val();
                }

                return result;
            }
        };

    var entityPropertyCheckbox = function () {
    };

    entityPropertyCheckbox.prototype =
        {
            assignHtml: function (config, item, dom, tableInfo) {
                var elementId = tableInfo.getNextElementId();
                var inputElement = $("<input type=\"checkbox\"></input>");
                inputElement.attr('id', elementId);

                if (item == "True") {
                    inputElement.attr('checked', true);
                }

                if (config.readOnly) {
                    inputElement.attr('readonly', true);
                }

                dom.append(inputElement);

                var result = new entityAssignResult();
                result.updateCallback = function () {
                    if (config.readOnly) {
                        return item == "True";
                    }

                    return $("#" + elementId).attr('checked') == 'checked';
                }

                return result;
            }
        };

    ///
    /// Defines one entry for console
    var entityViewClass = function () {
    };

    entityViewClass.prototype =
        {
            processTables: function (tables, domContent) {
                for (var t = 0; t < tables.length; t++) {
                    var table = tables[t];

                    var viewTable = new entityViewTable();

                    if (table.type == "detail") {
                        viewTable.createDetailTable(this, domContent, table);
                    }
                    else if (table.type == "list") {
                        viewTable.createListTable(this, domContent, table);
                    }
                }
            },

            getView: function (datatype) {
                if (datatype == "String") {
                    return new entityPropertyTextbox();
                }

                if (datatype == "Integer") {
                    return new entityPropertyTextbox();
                }

                if (datatype == "Boolean") {
                    return new entityPropertyCheckbox();
                }

                if (datatype == "DateTime") {
                    return new entityPropertyTextbox();
                }

                throw datatype + " type is not supported";
            }
        };

    var result = {
        EntityViewClass: entityViewClass
    };

    var tableNumber = 0;

    u.umbra.addViewType(
        new u.ViewType(
            "BurnSystems.Umbra.DetailView.EntityView",
            function (info) {
                var entityView = new entityViewClass();

                entityView.processTables(info.userData.tables, info.viewPoint.domContent);

            }));

    return result;
});