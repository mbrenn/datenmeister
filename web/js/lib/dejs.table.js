"use strict";

define([],
    function () {
        var tableClass = function (domElement) {
            if (domElement === undefined) {
                throw ("First parameter 'domElement' is not defined.");
            }
            this.__domElement = domElement;
            this.__currentTable = undefined;
            this.__currentRow = undefined;
            this.__isHeaderRow = false;
            
            this.__init();
        };

        tableClass.prototype =
            {
                __init: function () {
                    this.__currentTable = $("<table></table>");
                    this.__domElement.append(this.__currentTable);
                },

                addRow: function () {
                    this.__isHeaderRow = false;

                    this.__currentRow = $("<tr></tr>");
                    this.__currentTable.append(this.__currentRow);

                    return this.__currentRow;
                },

                addHeaderRow: function () {
                    this.__isHeaderRow = true;

                    this.__currentRow = $("<tr></tr>");
                    this.__currentTable.append(this.__currentRow);

                    return this.__currentRow;
                },

                addColumn: function (content, options) {
                    var currentColumn = this.__createColumn(options);
                    currentColumn.text(content);

                    return currentColumn;
                },

                addColumnHtml: function (content, options) {
                    var currentColumn = this.__createColumn(options);
                    currentColumn.html(content);

                    return currentColumn;
                },

                __createColumn: function (options) {
                    if (this.__currentRow === undefined) {
                        throw ("Row has not been started");
                    }

                    if (options == undefined) {
                        options = {};
                    }

                    var currentColumn;

                    if (this.__isHeaderRow || options.asHeader === true) {
                        currentColumn = $("<th></th>");
                    }
                    else {
                        currentColumn = $("<td></td>");
                    }

                    this.__currentRow.append(currentColumn);

                    return currentColumn;
                }
            };

        return tableClass;
    }
    );