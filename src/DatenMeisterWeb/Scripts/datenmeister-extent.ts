
export class ExtentOverview {
    loadTable(extentUri: string) {
        $.get(
            "/api/Extent/Get",
            { uri: extentUri }).done(function (data) {
                // Clears the table
                $("#extentTable").html("");

                var htmlTable = $("<table class='table table-striped'></table>");

                var htmlRow = $("<tr></tr>");
                for (var columnNr in data.Columns) {
                    var htmlTd = $("<th></th>");
                    htmlTd.text(data.Columns[columnNr]);
                    htmlRow.append(htmlTd);
                }

                htmlTable.append(htmlRow);
                for (var rowNr in data.Elements) {
                    var row = data.Elements[rowNr];
                    htmlRow = $("<tr></tr>");
                    for (var colName in row) {
                        var value = row[colName];
                        var htmlTd = $("<td></td>");
                        htmlTd.text(value);
                        htmlRow.append(htmlTd);
                    }

                    htmlTable.append(htmlRow);
                }

                $("#extentTable").append(htmlTable);
            });
    }
}
