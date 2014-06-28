define(["require", "exports", 'datenmeister.objects'], function(require, exports, __d__) {
    (function (Comment) {
        Comment.TypeName = 'Comment';

        function create(name, comment) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'Comment');
            if (name !== undefined) {
                result.set('name', name);
            }

            if (comment !== undefined) {
                result.set('comment', comment);
            }

            return result;
        }
        Comment.create = create;

        function getComment(item) {
            return item.get('comment');
        }
        Comment.getComment = getComment;

        function setComment(item, value) {
            item.set('comment', value);
        }
        Comment.setComment = setComment;

        function getName(item) {
            return item.get('name');
        }
        Comment.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        Comment.setName = setName;

        function getBinding(item) {
            return item.get('binding');
        }
        Comment.getBinding = getBinding;

        function setBinding(item, value) {
            item.set('binding', value);
        }
        Comment.setBinding = setBinding;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        Comment.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        Comment.setReadOnly = setReadOnly;
    })(exports.Comment || (exports.Comment = {}));
    var Comment = exports.Comment;

    (function (General) {
        General.TypeName = 'General';

        function create(name, binding) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'General');
            if (name !== undefined) {
                result.set('name', name);
            }

            if (binding !== undefined) {
                result.set('binding', binding);
            }

            return result;
        }
        General.create = create;

        function getName(item) {
            return item.get('name');
        }
        General.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        General.setName = setName;

        function getBinding(item) {
            return item.get('binding');
        }
        General.getBinding = getBinding;

        function setBinding(item, value) {
            item.set('binding', value);
        }
        General.setBinding = setBinding;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        General.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        General.setReadOnly = setReadOnly;
    })(exports.General || (exports.General = {}));
    var General = exports.General;

    (function (Checkbox) {
        Checkbox.TypeName = 'Checkbox';

        function create(name, binding) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'Checkbox');
            if (name !== undefined) {
                result.set('name', name);
            }

            if (binding !== undefined) {
                result.set('binding', binding);
            }

            return result;
        }
        Checkbox.create = create;

        function getName(item) {
            return item.get('name');
        }
        Checkbox.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        Checkbox.setName = setName;

        function getBinding(item) {
            return item.get('binding');
        }
        Checkbox.getBinding = getBinding;

        function setBinding(item, value) {
            item.set('binding', value);
        }
        Checkbox.setBinding = setBinding;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        Checkbox.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        Checkbox.setReadOnly = setReadOnly;
    })(exports.Checkbox || (exports.Checkbox = {}));
    var Checkbox = exports.Checkbox;

    (function (TextField) {
        TextField.TypeName = 'TextField';

        function create(name, binding) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'TextField');
            if (name !== undefined) {
                result.set('name', name);
            }

            if (binding !== undefined) {
                result.set('binding', binding);
            }

            return result;
        }
        TextField.create = create;

        function getWidth(item) {
            return item.get('width');
        }
        TextField.getWidth = getWidth;

        function setWidth(item, value) {
            item.set('width', value);
        }
        TextField.setWidth = setWidth;

        function getHeight(item) {
            return item.get('height');
        }
        TextField.getHeight = getHeight;

        function setHeight(item, value) {
            item.set('height', value);
        }
        TextField.setHeight = setHeight;

        function getName(item) {
            return item.get('name');
        }
        TextField.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        TextField.setName = setName;

        function getBinding(item) {
            return item.get('binding');
        }
        TextField.getBinding = getBinding;

        function setBinding(item, value) {
            item.set('binding', value);
        }
        TextField.setBinding = setBinding;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        TextField.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        TextField.setReadOnly = setReadOnly;
    })(exports.TextField || (exports.TextField = {}));
    var TextField = exports.TextField;

    (function (ActionButton) {
        ActionButton.TypeName = 'ActionButton';

        function create(text, clickUrl) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'ActionButton');
            if (text !== undefined) {
                result.set('text', text);
            }

            if (clickUrl !== undefined) {
                result.set('clickUrl', clickUrl);
            }

            return result;
        }
        ActionButton.create = create;

        function getText(item) {
            return item.get('text');
        }
        ActionButton.getText = getText;

        function setText(item, value) {
            item.set('text', value);
        }
        ActionButton.setText = setText;

        function getClickUrl(item) {
            return item.get('clickUrl');
        }
        ActionButton.getClickUrl = getClickUrl;

        function setClickUrl(item, value) {
            item.set('clickUrl', value);
        }
        ActionButton.setClickUrl = setClickUrl;
    })(exports.ActionButton || (exports.ActionButton = {}));
    var ActionButton = exports.ActionButton;

    (function (ReferenceBase) {
        ReferenceBase.TypeName = 'ReferenceBase';

        function create(name, binding, referenceUrl, propertyValue) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'ReferenceBase');
            if (name !== undefined) {
                result.set('name', name);
            }

            if (binding !== undefined) {
                result.set('binding', binding);
            }

            if (referenceUrl !== undefined) {
                result.set('referenceUrl', referenceUrl);
            }

            if (propertyValue !== undefined) {
                result.set('propertyValue', propertyValue);
            }

            return result;
        }
        ReferenceBase.create = create;

        function getPropertyValue(item) {
            return item.get('propertyValue');
        }
        ReferenceBase.getPropertyValue = getPropertyValue;

        function setPropertyValue(item, value) {
            item.set('propertyValue', value);
        }
        ReferenceBase.setPropertyValue = setPropertyValue;

        function getReferenceUrl(item) {
            return item.get('referenceUrl');
        }
        ReferenceBase.getReferenceUrl = getReferenceUrl;

        function setReferenceUrl(item, value) {
            item.set('referenceUrl', value);
        }
        ReferenceBase.setReferenceUrl = setReferenceUrl;

        function getName(item) {
            return item.get('name');
        }
        ReferenceBase.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        ReferenceBase.setName = setName;

        function getBinding(item) {
            return item.get('binding');
        }
        ReferenceBase.getBinding = getBinding;

        function setBinding(item, value) {
            item.set('binding', value);
        }
        ReferenceBase.setBinding = setBinding;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        ReferenceBase.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        ReferenceBase.setReadOnly = setReadOnly;
    })(exports.ReferenceBase || (exports.ReferenceBase = {}));
    var ReferenceBase = exports.ReferenceBase;

    (function (ReferenceByValue) {
        ReferenceByValue.TypeName = 'ReferenceByValue';

        function create(name, binding, referenceUrl, propertyValue) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'ReferenceByValue');
            if (name !== undefined) {
                result.set('name', name);
            }

            if (binding !== undefined) {
                result.set('binding', binding);
            }

            if (referenceUrl !== undefined) {
                result.set('referenceUrl', referenceUrl);
            }

            if (propertyValue !== undefined) {
                result.set('propertyValue', propertyValue);
            }

            return result;
        }
        ReferenceByValue.create = create;

        function getPropertyValue(item) {
            return item.get('propertyValue');
        }
        ReferenceByValue.getPropertyValue = getPropertyValue;

        function setPropertyValue(item, value) {
            item.set('propertyValue', value);
        }
        ReferenceByValue.setPropertyValue = setPropertyValue;

        function getReferenceUrl(item) {
            return item.get('referenceUrl');
        }
        ReferenceByValue.getReferenceUrl = getReferenceUrl;

        function setReferenceUrl(item, value) {
            item.set('referenceUrl', value);
        }
        ReferenceByValue.setReferenceUrl = setReferenceUrl;

        function getName(item) {
            return item.get('name');
        }
        ReferenceByValue.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        ReferenceByValue.setName = setName;

        function getBinding(item) {
            return item.get('binding');
        }
        ReferenceByValue.getBinding = getBinding;

        function setBinding(item, value) {
            item.set('binding', value);
        }
        ReferenceByValue.setBinding = setBinding;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        ReferenceByValue.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        ReferenceByValue.setReadOnly = setReadOnly;
    })(exports.ReferenceByValue || (exports.ReferenceByValue = {}));
    var ReferenceByValue = exports.ReferenceByValue;

    (function (ReferenceByRef) {
        ReferenceByRef.TypeName = 'ReferenceByRef';

        function create(name, binding, referenceUrl, propertyValue) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'ReferenceByRef');
            if (name !== undefined) {
                result.set('name', name);
            }

            if (binding !== undefined) {
                result.set('binding', binding);
            }

            if (referenceUrl !== undefined) {
                result.set('referenceUrl', referenceUrl);
            }

            if (propertyValue !== undefined) {
                result.set('propertyValue', propertyValue);
            }

            return result;
        }
        ReferenceByRef.create = create;

        function getPropertyValue(item) {
            return item.get('propertyValue');
        }
        ReferenceByRef.getPropertyValue = getPropertyValue;

        function setPropertyValue(item, value) {
            item.set('propertyValue', value);
        }
        ReferenceByRef.setPropertyValue = setPropertyValue;

        function getReferenceUrl(item) {
            return item.get('referenceUrl');
        }
        ReferenceByRef.getReferenceUrl = getReferenceUrl;

        function setReferenceUrl(item, value) {
            item.set('referenceUrl', value);
        }
        ReferenceByRef.setReferenceUrl = setReferenceUrl;

        function getName(item) {
            return item.get('name');
        }
        ReferenceByRef.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        ReferenceByRef.setName = setName;

        function getBinding(item) {
            return item.get('binding');
        }
        ReferenceByRef.getBinding = getBinding;

        function setBinding(item, value) {
            item.set('binding', value);
        }
        ReferenceByRef.setBinding = setBinding;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        ReferenceByRef.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        ReferenceByRef.setReadOnly = setReadOnly;
    })(exports.ReferenceByRef || (exports.ReferenceByRef = {}));
    var ReferenceByRef = exports.ReferenceByRef;

    (function (MultiReferenceField) {
        MultiReferenceField.TypeName = 'MultiReferenceField';

        function create(name, binding) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'MultiReferenceField');
            if (name !== undefined) {
                result.set('name', name);
            }

            if (binding !== undefined) {
                result.set('binding', binding);
            }

            return result;
        }
        MultiReferenceField.create = create;

        function getPropertyValue(item) {
            return item.get('propertyValue');
        }
        MultiReferenceField.getPropertyValue = getPropertyValue;

        function setPropertyValue(item, value) {
            item.set('propertyValue', value);
        }
        MultiReferenceField.setPropertyValue = setPropertyValue;

        function getReferenceUrl(item) {
            return item.get('referenceUrl');
        }
        MultiReferenceField.getReferenceUrl = getReferenceUrl;

        function setReferenceUrl(item, value) {
            item.set('referenceUrl', value);
        }
        MultiReferenceField.setReferenceUrl = setReferenceUrl;

        function getName(item) {
            return item.get('name');
        }
        MultiReferenceField.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        MultiReferenceField.setName = setName;

        function getBinding(item) {
            return item.get('binding');
        }
        MultiReferenceField.getBinding = getBinding;

        function setBinding(item, value) {
            item.set('binding', value);
        }
        MultiReferenceField.setBinding = setBinding;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        MultiReferenceField.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        MultiReferenceField.setReadOnly = setReadOnly;
    })(exports.MultiReferenceField || (exports.MultiReferenceField = {}));
    var MultiReferenceField = exports.MultiReferenceField;

    (function (View) {
        View.TypeName = 'View';

        function create() {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'View');
            return result;
        }
        View.create = create;

        function getName(item) {
            return item.get('name');
        }
        View.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        View.setName = setName;

        function getFieldInfos(item) {
            return item.get('fieldInfos');
        }
        View.getFieldInfos = getFieldInfos;

        function setFieldInfos(item, value) {
            item.set('fieldInfos', value);
        }
        View.setFieldInfos = setFieldInfos;

        function pushFieldInfo(item, value) {
            var a = item.get('fieldInfos');
            if (a === undefined) {
                a = new Array();
            }

            a.push(value);
            item.set('fieldInfos', a);
        }
        View.pushFieldInfo = pushFieldInfo;

        function getStartInEditMode(item) {
            return item.get('startInEditMode');
        }
        View.getStartInEditMode = getStartInEditMode;

        function setStartInEditMode(item, value) {
            item.set('startInEditMode', value);
        }
        View.setStartInEditMode = setStartInEditMode;
    })(exports.View || (exports.View = {}));
    var View = exports.View;

    (function (FormView) {
        FormView.TypeName = 'FormView';

        function create() {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'FormView');
            result.set('allowEdit', true);
            result.set('allowDelete', true);
            result.set('allowNew', true);
            return result;
        }
        FormView.create = create;

        function getAllowEdit(item) {
            return item.get('allowEdit');
        }
        FormView.getAllowEdit = getAllowEdit;

        function setAllowEdit(item, value) {
            item.set('allowEdit', value);
        }
        FormView.setAllowEdit = setAllowEdit;

        function getAllowDelete(item) {
            return item.get('allowDelete');
        }
        FormView.getAllowDelete = getAllowDelete;

        function setAllowDelete(item, value) {
            item.set('allowDelete', value);
        }
        FormView.setAllowDelete = setAllowDelete;

        function getAllowNew(item) {
            return item.get('allowNew');
        }
        FormView.getAllowNew = getAllowNew;

        function setAllowNew(item, value) {
            item.set('allowNew', value);
        }
        FormView.setAllowNew = setAllowNew;

        function getShowColumnHeaders(item) {
            return item.get('showColumnHeaders');
        }
        FormView.getShowColumnHeaders = getShowColumnHeaders;

        function setShowColumnHeaders(item, value) {
            item.set('showColumnHeaders', value);
        }
        FormView.setShowColumnHeaders = setShowColumnHeaders;

        function getAllowNewProperty(item) {
            return item.get('allowNewProperty');
        }
        FormView.getAllowNewProperty = getAllowNewProperty;

        function setAllowNewProperty(item, value) {
            item.set('allowNewProperty', value);
        }
        FormView.setAllowNewProperty = setAllowNewProperty;

        function getName(item) {
            return item.get('name');
        }
        FormView.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        FormView.setName = setName;

        function getFieldInfos(item) {
            return item.get('fieldInfos');
        }
        FormView.getFieldInfos = getFieldInfos;

        function setFieldInfos(item, value) {
            item.set('fieldInfos', value);
        }
        FormView.setFieldInfos = setFieldInfos;

        function pushFieldInfo(item, value) {
            var a = item.get('fieldInfos');
            if (a === undefined) {
                a = new Array();
            }

            a.push(value);
            item.set('fieldInfos', a);
        }
        FormView.pushFieldInfo = pushFieldInfo;

        function getStartInEditMode(item) {
            return item.get('startInEditMode');
        }
        FormView.getStartInEditMode = getStartInEditMode;

        function setStartInEditMode(item, value) {
            item.set('startInEditMode', value);
        }
        FormView.setStartInEditMode = setStartInEditMode;
    })(exports.FormView || (exports.FormView = {}));
    var FormView = exports.FormView;

    (function (TableView) {
        TableView.TypeName = 'TableView';

        function create() {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'TableView');
            result.set('allowEdit', true);
            result.set('allowDelete', true);
            result.set('allowNew', true);
            return result;
        }
        TableView.create = create;

        function getExtentUri(item) {
            return item.get('extentUri');
        }
        TableView.getExtentUri = getExtentUri;

        function setExtentUri(item, value) {
            item.set('extentUri', value);
        }
        TableView.setExtentUri = setExtentUri;

        function getMainType(item) {
            return item.get('mainType');
        }
        TableView.getMainType = getMainType;

        function setMainType(item, value) {
            item.set('mainType', value);
        }
        TableView.setMainType = setMainType;

        function getAllowEdit(item) {
            return item.get('allowEdit');
        }
        TableView.getAllowEdit = getAllowEdit;

        function setAllowEdit(item, value) {
            item.set('allowEdit', value);
        }
        TableView.setAllowEdit = setAllowEdit;

        function getAllowDelete(item) {
            return item.get('allowDelete');
        }
        TableView.getAllowDelete = getAllowDelete;

        function setAllowDelete(item, value) {
            item.set('allowDelete', value);
        }
        TableView.setAllowDelete = setAllowDelete;

        function getAllowNew(item) {
            return item.get('allowNew');
        }
        TableView.getAllowNew = getAllowNew;

        function setAllowNew(item, value) {
            item.set('allowNew', value);
        }
        TableView.setAllowNew = setAllowNew;

        function getName(item) {
            return item.get('name');
        }
        TableView.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        TableView.setName = setName;

        function getFieldInfos(item) {
            return item.get('fieldInfos');
        }
        TableView.getFieldInfos = getFieldInfos;

        function setFieldInfos(item, value) {
            item.set('fieldInfos', value);
        }
        TableView.setFieldInfos = setFieldInfos;

        function pushFieldInfo(item, value) {
            var a = item.get('fieldInfos');
            if (a === undefined) {
                a = new Array();
            }

            a.push(value);
            item.set('fieldInfos', a);
        }
        TableView.pushFieldInfo = pushFieldInfo;

        function getStartInEditMode(item) {
            return item.get('startInEditMode');
        }
        TableView.getStartInEditMode = getStartInEditMode;

        function setStartInEditMode(item, value) {
            item.set('startInEditMode', value);
        }
        TableView.setStartInEditMode = setStartInEditMode;
    })(exports.TableView || (exports.TableView = {}));
    var TableView = exports.TableView;
});
//# sourceMappingURL=datenmeister.fieldinfo.objects.js.map
