/// <reference path="../requirejs/require.d.ts" />
/// <reference path="datenmeister.ts" />
define(["require", "exports", "datenmeister"], function(require, exports, __dm__) {
    var dm = __dm__;

    $(document).ready(function () {
        // initializes the dom
        dm.init();
    });
});
//# sourceMappingURL=init.js.map
