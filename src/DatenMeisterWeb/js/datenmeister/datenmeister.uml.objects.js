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

    (function (Class) {
        Class.TypeName = 'Class';

        function create() {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'Class');
            return result;
        }
        Class.create = create;

        function isAbstract(item) {
            return item.get('isAbstract');
        }
        Class.isAbstract = isAbstract;

        function setAbstract(item, value) {
            item.set('isAbstract', value);
        }
        Class.setAbstract = setAbstract;

        function getOwnedAttribute(item) {
            return item.get('ownedAttribute');
        }
        Class.getOwnedAttribute = getOwnedAttribute;

        function setOwnedAttribute(item, value) {
            item.set('ownedAttribute', value);
        }
        Class.setOwnedAttribute = setOwnedAttribute;

        function pushOwnedAttribute(item, value) {
            var a = item.get('ownedAttribute');
            if (a === undefined) {
                a = new Array();
            }

            a.push(value);
            item.set('ownedAttribute', a);
        }
        Class.pushOwnedAttribute = pushOwnedAttribute;

        function getName(item) {
            return item.get('name');
        }
        Class.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        Class.setName = setName;
    })(exports.Class || (exports.Class = {}));
    var Class = exports.Class;
});
//# sourceMappingURL=datenmeister.uml.objects.js.map
