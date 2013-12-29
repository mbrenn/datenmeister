/// <reference path="../requirejs/require.d.ts" />
/// <reference path="datenmeister.ts" />

import dm = require("datenmeister");

$(document).ready(
    function () {

        // initializes the dom
        dm.init();
    });
