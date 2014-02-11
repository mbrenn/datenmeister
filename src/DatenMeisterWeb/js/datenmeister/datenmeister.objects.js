/// <reference path="../backbone/backbone.d.ts" />
/// <reference path="../underscore/underscore.d.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports"], function(require, exports) {
    var ExtentInfo = (function (_super) {
        __extends(ExtentInfo, _super);
        function ExtentInfo() {
            _super.apply(this, arguments);
        }
        ExtentInfo.prototype.getUri = function () {
            return this.get("uri");
        };

        ExtentInfo.prototype.getType = function () {
            return this.get("type");
        };
        return ExtentInfo;
    })(Backbone.Model);
    exports.ExtentInfo = ExtentInfo;

    // Defines the information for an object within the extent
    var JsonExtentObject = (function (_super) {
        __extends(JsonExtentObject, _super);
        function JsonExtentObject(attributes) {
            _super.call(this, attributes);
        }
        // Gets the uri of the extent itself
        JsonExtentObject.prototype.getUri = function () {
            return this.extentUri + "#" + this.id;
        };
        return JsonExtentObject;
    })(Backbone.Model);
    exports.JsonExtentObject = JsonExtentObject;

    // Result from GetObjectsInExtent
    var JsonExtentData = (function () {
        function JsonExtentData() {
        }
        return JsonExtentData;
    })();
    exports.JsonExtentData = JsonExtentData;
});
//# sourceMappingURL=datenmeister.objects.js.map
