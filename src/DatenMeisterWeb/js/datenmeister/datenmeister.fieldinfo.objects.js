define(["require", "exports", 'datenmeister.objects'], function(require, exports, __d__) {
    (function (Comment) {
        Comment.TypeName = 'Comment';

        function create(title, comment) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'Comment');
            if (title !== undefined) {
                result.set('title', title);
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

        function getTitle(item) {
            return item.get('title');
        }
        Comment.getTitle = getTitle;

        function setTitle(item, value) {
            item.set('title', value);
        }
        Comment.setTitle = setTitle;

        function getName(item) {
            return item.get('name');
        }
        Comment.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        Comment.setName = setName;

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

        function create() {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'General');
            return result;
        }
        General.create = create;

        function getTitle(item) {
            return item.get('title');
        }
        General.getTitle = getTitle;

        function setTitle(item, value) {
            item.set('title', value);
        }
        General.setTitle = setTitle;

        function getName(item) {
            return item.get('name');
        }
        General.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        General.setName = setName;

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

    (function (TextField) {
        TextField.TypeName = 'TextField';

        function create(title, name) {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'TextField');
            if (title !== undefined) {
                result.set('title', title);
            }

            if (name !== undefined) {
                result.set('name', name);
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

        function getTitle(item) {
            return item.get('title');
        }
        TextField.getTitle = getTitle;

        function setTitle(item, value) {
            item.set('title', value);
        }
        TextField.setTitle = setTitle;

        function getName(item) {
            return item.get('name');
        }
        TextField.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        TextField.setName = setName;

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

    (function (View) {
        View.TypeName = 'View';

        function create() {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'View');
            return result;
        }
        View.create = create;

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

        function getTitle(item) {
            return item.get('title');
        }
        View.getTitle = getTitle;

        function setTitle(item, value) {
            item.set('title', value);
        }
        View.setTitle = setTitle;

        function getName(item) {
            return item.get('name');
        }
        View.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        View.setName = setName;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        View.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        View.setReadOnly = setReadOnly;
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

        function getTitle(item) {
            return item.get('title');
        }
        FormView.getTitle = getTitle;

        function setTitle(item, value) {
            item.set('title', value);
        }
        FormView.setTitle = setTitle;

        function getName(item) {
            return item.get('name');
        }
        FormView.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        FormView.setName = setName;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        FormView.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        FormView.setReadOnly = setReadOnly;
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

        function getTitle(item) {
            return item.get('title');
        }
        TableView.getTitle = getTitle;

        function setTitle(item, value) {
            item.set('title', value);
        }
        TableView.setTitle = setTitle;

        function getName(item) {
            return item.get('name');
        }
        TableView.getName = getName;

        function setName(item, value) {
            item.set('name', value);
        }
        TableView.setName = setName;

        function isReadOnly(item) {
            return item.get('isReadOnly');
        }
        TableView.isReadOnly = isReadOnly;

        function setReadOnly(item, value) {
            item.set('isReadOnly', value);
        }
        TableView.setReadOnly = setReadOnly;
    })(exports.TableView || (exports.TableView = {}));
    var TableView = exports.TableView;
});
//# sourceMappingURL=datenmeister.fieldinfo.objects.js.map
