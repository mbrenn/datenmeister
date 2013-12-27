$(document).ready(function () {
    $("#teststart").click(function () {
        var dataToBeSent =
            {
                prop: "Hallo",
                numberProp: 2133,
                arrayProp: [223, 321, 231, 21],
                substructure: {
                    name: "Brenn",
                    prename: "Martin"
                }
            };

        $.ajax({
            type: 'POST',
            url: 'controller/jsontest/Load',
            data: JSON.stringify(dataToBeSent),
            headers: { 'Content-Type': 'application/json' },
            success: function (data) {
                alert(data);
            }
        });

    });
});