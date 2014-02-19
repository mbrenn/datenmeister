/// <reference path="../jquery/jquery.d.ts" />
define(["require", "exports"], function(require, exports) {
    var String = (function () {
        function String() {
        }
        String.prototype.encodeHtml = function (text) {
            return $("<div></div>").text(text).html();
        };

        String.prototype.nl2br = function (text) {
            return text.replace(/\n/g, "<br />");
        };
        return String;
    })();
    exports.String = String;
});
