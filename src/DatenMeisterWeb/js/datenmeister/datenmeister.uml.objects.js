define(["require", "exports", 'datenmeister.objects'], function(require, exports, __d__) {
    (function (NamedElement) {
        NamedElement.TypeName = 'NamedElement';

        function create() {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'NamedElement');
            return result;
        }
        NamedElement.create = create;

        function getName(item) {
            return item.get('name');
        }
        NamedElement.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        NamedElement.setName = setName;
    })(exports.NamedElement || (exports.NamedElement = {}));
    var NamedElement = exports.NamedElement;

    (function (Type) {
        Type.TypeName = 'Type';

        function create() {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'Type');
            return result;
        }
        Type.create = create;

        function getName(item) {
            return item.get('name');
        }
        Type.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        Type.setName = setName;
    })(exports.Type || (exports.Type = {}));
    var Type = exports.Type;

    (function (Property) {
        Property.TypeName = 'Property';

        function create() {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'Property');
            return result;
        }
        Property.create = create;

        function getName(item) {
            return item.get('name');
        }
        Property.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        Property.setName = setName;
    })(exports.Property || (exports.Property = {}));
    var Property = exports.Property;
});
//# sourceMappingURL=datenmeister.uml.objects.js.map
