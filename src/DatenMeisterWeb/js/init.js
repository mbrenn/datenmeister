/// <reference path="lib/jquery.d.ts" />
/// <reference path="lib/require.d.ts" />
/// <reference path="datenmeister.ts" />
define(["require", "exports", "datenmeister"], function(require, exports, __dm__) {
    var dm = __dm__;

    $(document).ready(function () {
        var form = new dm.Forms.ServerConnectionForm($("#server_selection"));
        form.onConnect = function (settings) {
            $("#extent_list_table").empty();
            dm.Gui.showExtents($("#extent_list_table"));
        };

        form.bind();
    });
});
