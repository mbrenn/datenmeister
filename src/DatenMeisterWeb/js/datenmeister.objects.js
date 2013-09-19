/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
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

    var ExtentInfoCollection = (function (_super) {
        __extends(ExtentInfoCollection, _super);
        function ExtentInfoCollection() {
            _super.apply(this, arguments);
        }
        return ExtentInfoCollection;
    })(Backbone.Collection);
    exports.ExtentInfoCollection = ExtentInfoCollection;

    // Defines the information for a column that has been received from server
    var JsonExtentFieldInfo = (function () {
        function JsonExtentFieldInfo(name, title) {
            if (name !== undefined) {
                this.name = name;
            }

            if (title === undefined) {
                this.title = this.name;
            }
        }
        return JsonExtentFieldInfo;
    })();
    exports.JsonExtentFieldInfo = JsonExtentFieldInfo;

    // Defines the information for a column that has been received from server
    var JsonExtentObject = (function (_super) {
        __extends(JsonExtentObject, _super);
        function JsonExtentObject() {
            _super.call(this);
        }
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
