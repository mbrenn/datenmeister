/// <reference path="jquery.d.ts" />
define(["require", "exports"], function(require, exports) {
    var CellOptions = (function () {
        function CellOptions() {
        }
        return CellOptions;
    })();
    exports.CellOptions = CellOptions;

    var Table = (function () {
        function Table(domElement) {
            this.__domElement = domElement;
            this.__currentTable = $("<table></table>");
            this.__domElement.append(this.__currentTable);
        }
        Table.prototype.addRow = function () {
            this.__isHeaderRow = false;

            this.__currentRow = $("<tr></tr>");
            this.__currentTable.append(this.__currentRow);

            return this.__currentRow;
        };

        Table.prototype.addHeaderRow = function () {
            this.__isHeaderRow = true;

            this.__currentRow = $("<tr></tr>");
            this.__currentTable.append(this.__currentRow);

            return this.__currentRow;
        };

        Table.prototype.addColumn = function (content, options) {
            var currentColumn = this.__createColumn(options);
            currentColumn.text(content);

            return currentColumn;
        };

        Table.prototype.addColumnHtml = function (content, options) {
            var currentColumn = this.__createColumn(options);
            currentColumn.html(content);

            return currentColumn;
        };

        Table.prototype.__createColumn = function (options) {
            if (this.__currentRow === undefined) {
                throw ("Row has not been started");
            }

            if (options == undefined) {
                options = {};
            }

            var currentColumn;

            if (this.__isHeaderRow || options.asHeader === true) {
                currentColumn = $("<th></th>");
            } else {
                currentColumn = $("<td></td>");
            }

            this.__currentRow.append(currentColumn);

            return currentColumn;
        };
        return Table;
    })();
    exports.Table = Table;
});
