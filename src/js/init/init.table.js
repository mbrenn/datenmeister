define([],
    function () {
        // First load
        require(['../js/dejs.table.js'],
            function (tableClass) {
                
                var table1 = new tableClass.Table($("#table1"));
                table1.addHeaderRow();
                table1.addColumnHtml("Prename");
                table1.addColumnHtml("Name");
                table1.addColumnHtml("Street");
                table1.addRow();
                table1.addColumnHtml("John");
                table1.addColumnHtml("Wayne");
                table1.addColumnHtml("Wayne Street <em>unsure</em>");
                table1.addRow();
                table1.addColumnHtml("Another Name");
                table1.addColumnHtml("Another Prename");
                table1.addColumnHtml("<em>Also unknown</em>");

                // Antoher table
                table1 = new tableClass.Table($("#table2"));
                table1.addHeaderRow();
                table1.addColumn("Prename");
                table1.addColumn("Name");
                table1.addColumn("Street");
                table1.addRow();
                table1.addColumn("John");
                table1.addColumn("Wayne");
                table1.addColumn("Wayne Street <em>unsure</em>");
                table1.addRow();
                table1.addColumn("Another Name");
                table1.addColumn("Another Prename");
                table1.addColumn("<em>Also unknown</em>");

                table1 = new tableClass.Table($("#table3"));
                table1.addRow();
                table1.addColumn("Name", { asHeader: true });
                table1.addColumn("Wayne");
                table1.addRow();
                table1.addColumn("Prename", { asHeader: true });
                table1.addColumn("John");

            }
        );
    }
);
