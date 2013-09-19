/// <reference path="lib/require.d.ts" />
/// <reference path="datenmeister.ts" />

import dm = require("datenmeister");
import forms = require("datenmeister.forms");
import gui = require("datenmeister.gui");

$(document).ready(
    function () {
        var type: Backbone.ViewOptions =
            {
                el: "#server_selection"
            };

        var form = new forms .ServerConnectionView(type);

        form.onConnect = function (settings) {
            gui.showExtents($("#extent_list_table"));
        };
    });
