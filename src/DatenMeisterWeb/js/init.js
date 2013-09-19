/// <reference path="lib/require.d.ts" />
/// <reference path="datenmeister.ts" />
define(["require", "exports", "datenmeister.forms", "datenmeister.gui"], function(require, exports, __forms__, __gui__) {
    
    var forms = __forms__;
    var gui = __gui__;

    $(document).ready(function () {
        var type = {
            el: "#server_selection"
        };

        var form = new forms.ServerConnectionView(type);

        form.onConnect = function (settings) {
            gui.showExtents($("#extent_list_table"));
        };
    });
});
//# sourceMappingURL=init.js.map
