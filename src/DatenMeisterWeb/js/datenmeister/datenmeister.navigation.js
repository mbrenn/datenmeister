define(["require", "exports"], function(require, exports) {
    /// <reference path="../backbone/backbone.d.ts" />
    /*
    * Stores the last navigation points
    */
    var lastNavigations = new Array();

    function to(navigationUrl) {
        lastNavigations.push(navigationUrl);
        Backbone.history.navigate(navigationUrl, { trigger: true });
    }
    exports.to = to;

    /*
    Just adds a point to navigation history without moving to the given url
    */
    function add(navigationUrl) {
        lastNavigations.push(navigationUrl);
    }
    exports.add = add;

    function back() {
        if (lastNavigations.length <= 1) {
            // If nothing gets found, return to 'login'
            exports.to("login");
            return;
        }

        var currentRoute = lastNavigations.pop();
        var lastRoute = lastNavigations.pop();
        exports.to(lastRoute);
    }
    exports.back = back;
});
//# sourceMappingURL=datenmeister.navigation.js.map
