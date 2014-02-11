define(["require", "exports", "datenmeister.objects", "datenmeister.serverapi"], function(require, exports, __d__, __serverapi__) {
    var d = __d__;
    var serverapi = __serverapi__;

    function getRendererByObject(object) {
        return exports.getRenderer(object.get("type"));
    }
    exports.getRendererByObject = getRendererByObject;

    /*
    * Gets the renderer for a specific object type
    */
    function getRenderer(type) {
        if (type == Comment.TypeName) {
            return new Comment.Renderer();
        }

        if (type == TextField.TypeName) {
            return new TextField.Renderer();
        }

        if (type == ActionButton.TypeName) {
            return new ActionButton.Renderer();
        }

        return new TextField.Renderer();
    }
    exports.getRenderer = getRenderer;

    /*
    * Defines the view element, which contains a list of field information.
    * - fieldinfos: Array of fieldinfos to be added
    */
    (function (View) {
        function create() {
            var result = new d.JsonExtentObject();
            result.set('fieldinfos', new Array());
            return result;
        }
        View.create = create;

        function getFieldInfos(value) {
            var result = value.get('fieldinfos');
            if (result === undefined) {
                return new Array();
            }

            return result;
        }
        View.getFieldInfos = getFieldInfos;

        function pushFieldInfo(value, fieldInfo) {
            var infos = value.get('fieldinfos');
            if (infos === undefined) {
                infos = new Array();
            }

            infos.push(fieldInfo);
            setFieldInfos(value, infos);
        }
        View.pushFieldInfo = pushFieldInfo;

        function setFieldInfos(value, fieldInfos) {
            value.set('fieldinfos', fieldInfos);
        }
        View.setFieldInfos = setFieldInfos;

        function setAllowEdit(value, allowEdit) {
            value.set('allowEdit', allowEdit);
        }
        View.setAllowEdit = setAllowEdit;

        function getAllowEdit(value) {
            return value.get('allowEdit');
        }
        View.getAllowEdit = getAllowEdit;

        function setAllowNew(value, allowNew) {
            value.set('allowNew', allowNew);
        }
        View.setAllowNew = setAllowNew;

        function getAllowNew(value) {
            return value.get('allowNew');
        }
        View.getAllowNew = getAllowNew;

        function setAllowDelete(value, allowDelete) {
            value.set('allowDelete', allowDelete);
        }
        View.setAllowDelete = setAllowDelete;

        function getAllowDelete(value) {
            return value.get('allowDelete');
        }
        View.getAllowDelete = getAllowDelete;

        function setStartInEditMode(value, startInEditMode) {
            value.set('startInEditMode', startInEditMode);
        }
        View.setStartInEditMode = setStartInEditMode;

        function getStartInEditMode(value) {
            return value.get('startInEditMode');
        }
        View.getStartInEditMode = getStartInEditMode;
    })(exports.View || (exports.View = {}));
    var View = exports.View;

    /*
    * Defines additional properties which are used for a formview and not for a table view
    * All properties derived from View are also valid
    */
    (function (FormView) {
        function create() {
            return View.create();
        }
        FormView.create = create;

        function setTitle(value, title) {
            General.setTitle(value, title);
        }
        FormView.setTitle = setTitle;

        function getTitle(value) {
            return General.getTitle(value);
        }
        FormView.getTitle = getTitle;

        function setShowColumnHeaders(value, flag) {
            value.set('showcolumnheaders', flag);
        }
        FormView.setShowColumnHeaders = setShowColumnHeaders;

        function getShowColumnHeaders(value) {
            return value.get('showcolumnheaders');
        }
        FormView.getShowColumnHeaders = getShowColumnHeaders;

        function setAllowNewProperty(value, allowNewProperty) {
            value.set('allowNewProperty', allowNewProperty);
        }
        FormView.setAllowNewProperty = setAllowNewProperty;

        function getAllowNewProperty(value) {
            return value.get('allowNewProperty');
        }
        FormView.getAllowNewProperty = getAllowNewProperty;
    })(exports.FormView || (exports.FormView = {}));
    var FormView = exports.FormView;

    /*
    * Defines additional properties which are used for a tableview and not for a form view
    * All properties derived from View are also valid
    */
    (function (TableView) {
        function create() {
            var result = View.create();
            View.setAllowDelete(result, true);
            View.setAllowNew(result, true);
            View.setAllowEdit(result, true);
            return result;
        }
        TableView.create = create;
    })(exports.TableView || (exports.TableView = {}));
    var TableView = exports.TableView;

    /*
    * Describes the most generic functions
    */
    (function (General) {
        function setTitle(value, title) {
            value.set("title", title);
        }
        General.setTitle = setTitle;

        function setName(value, name) {
            value.set("name", name);
        }
        General.setName = setName;

        function setReadOnly(value, isReadOnly) {
            value.set("readonly", isReadOnly);
        }
        General.setReadOnly = setReadOnly;

        function getTitle(value) {
            return value.get("title");
        }
        General.getTitle = getTitle;

        function getName(value) {
            return value.get("name");
        }
        General.getName = getName;

        function isReadOnly(value) {
            return value.get("readonly");
        }
        General.isReadOnly = isReadOnly;
    })(exports.General || (exports.General = {}));
    var General = exports.General;

    /*
    * Describes one field element, that is just a read-only description text with pre-default content
    * This field will contain the title on the left column and the commentText as Encoded Text in table
    */
    (function (Comment) {
        Comment.TypeName = "DatenMeister.DataFields.Comment";

        function create(title, commentText) {
            var result = new d.JsonExtentObject();

            if (title !== undefined) {
                setTitle(result, title);
            }

            if (commentText !== undefined) {
                setComment(result, commentText);
            }

            result.set("type", Comment.TypeName);

            return result;
        }
        Comment.create = create;

        function setTitle(value, title) {
            General.setTitle(value, title);
        }
        Comment.setTitle = setTitle;

        function setComment(value, commentText) {
            value.set("comment", commentText);
        }
        Comment.setComment = setComment;
        function getTitle(value) {
            return General.getTitle(value);
        }
        Comment.getTitle = getTitle;

        function getComment(value) {
            return value.get("comment");
        }
        Comment.getComment = getComment;

        var Renderer = (function () {
            function Renderer() {
            }
            Renderer.prototype.createReadField = function (object, field, form) {
                var result = $("<div></div>");
                result.text(Comment.getComment(field));
                return result;
            };

            Renderer.prototype.createWriteField = function (object, field, form) {
                // Same as read field
                return this.createReadField(object, field, form);
            };

            Renderer.prototype.setValueByWriteField = function (object, field, dom, form) {
                // Just a comment, nothing to do here
                return;
            };
            return Renderer;
        })();
        Comment.Renderer = Renderer;
    })(exports.Comment || (exports.Comment = {}));
    var Comment = exports.Comment;

    /*
    * Describes one textfield element, which has an input field and can be shown to the user.
    * It also contains information about height and width of the field.
    * - title: Title being shown to user
    * - name: Name of the property, where value will be stored
    * - readonly: True, if field is read-only
    * - width: Width of the element
    * - height: Height of the element
    */
    (function (TextField) {
        TextField.TypeName = "DatenMeister.DataFields.TextField";

        function create(title, key) {
            var result = new d.JsonExtentObject();

            if (title !== undefined) {
                setTitle(result, title);
            }

            if (key !== undefined) {
                setName(result, key);
            }

            result.set("type", "DatenMeister.DataFields.TextField");

            return result;
        }
        TextField.create = create;

        function setTitle(value, title) {
            General.setTitle(value, title);
        }
        TextField.setTitle = setTitle;

        function getTitle(value) {
            return General.getTitle(value);
        }
        TextField.getTitle = getTitle;

        function setName(value, name) {
            General.setName(value, name);
        }
        TextField.setName = setName;

        function getName(value) {
            return General.getName(value);
        }
        TextField.getName = getName;

        function setReadOnly(value, isReadOnly) {
            General.setReadOnly(value, isReadOnly);
        }
        TextField.setReadOnly = setReadOnly;

        function isReadOnly(value) {
            return General.isReadOnly(value);
        }
        TextField.isReadOnly = isReadOnly;

        function setWidth(value, width) {
            value.set("width", width);
        }
        TextField.setWidth = setWidth;

        function getWidth(value) {
            return value.get("width");
        }
        TextField.getWidth = getWidth;

        function setHeight(value, height) {
            value.set("height", height);
        }
        TextField.setHeight = setHeight;

        function getHeight(value) {
            return value.get("height");
        }
        TextField.getHeight = getHeight;

        var Renderer = (function () {
            function Renderer() {
            }
            Renderer.prototype.createReadField = function (object, field, form) {
                var tthis = this;
                var span = $("<span />");
                var value = object.get(General.getName(field));
                if (value === undefined || value === null) {
                    span.html("<em>undefined</em>");
                } else if (_.isArray(value)) {
                    span.text('Array with ' + value.length + " items:");
                    var ul = $("<ul></ul>");
                    _.each(value, function (item) {
                        var div = $("<li></li>");
                        div.text(JSON.stringify(item.toJSON()) + " | " + item.id);

                        /*div.click(function () {
                        if (tthis.itemClickedEvent !== undefined) {
                        tthis.itemClickedEvent(item);
                        }
                        });*/
                        ul.append(div);
                    });

                    span.append(ul);
                } else {
                    span.text(value);
                }

                return span;
            };

            Renderer.prototype.createWriteField = function (object, field, form) {
                var value;

                if (object !== undefined && field !== undefined) {
                    value = object.get(General.getName(field));
                }

                // Checks, if writing is possible
                var offerWriting = true;
                if (_.isArray(value) || _.isObject(value)) {
                    offerWriting = false;
                }

                if (offerWriting) {
                    // Offer writing
                    var inputField = $("<input type='text' />");
                    if (value !== undefined && value !== null) {
                        inputField.val(value);
                    }

                    return inputField;
                } else {
                    return this.createReadField(object, field, form);
                }
            };

            Renderer.prototype.setValueByWriteField = function (object, field, dom) {
                if (General.isReadOnly(field) === true) {
                    // Do nothing
                    return;
                }

                // Reads the value
                object.set(General.getName(field), dom.val());
            };
            return Renderer;
        })();
        TextField.Renderer = Renderer;
    })(exports.TextField || (exports.TextField = {}));
    var TextField = exports.TextField;

    /*
    * Defines the module for an action button, which sends the form data to a certain
    * form
    */
    (function (ActionButton) {
        ActionButton.TypeName = "DatenMeister.DataFields.ActionButton";

        function create(text, clickUrl) {
            var result = new d.JsonExtentObject();
            setText(result, text);
            setClickUrl(result, clickUrl);

            result.set("type", ActionButton.TypeName);

            return result;
        }
        ActionButton.create = create;

        function setText(value, text) {
            value.set('text', text);
        }
        ActionButton.setText = setText;

        function getText(value) {
            return value.get('text');
        }
        ActionButton.getText = getText;

        function setClickUrl(value, url) {
            if (url === undefined || url.length == 0 || url[0] == '/') {
                throw "Action URL should not be undefined, '' or start with '/'";
            }

            value.set('clickUrl', url);
        }
        ActionButton.setClickUrl = setClickUrl;

        function getClickUrl(value) {
            return value.get('clickUrl');
        }
        ActionButton.getClickUrl = getClickUrl;

        var Renderer = (function () {
            function Renderer() {
            }
            Renderer.prototype.createReadField = function (object, fieldInfo, form) {
                var tthis = this;
                var inputField = $("<input type='button'></input>");
                inputField.attr('value', getText(fieldInfo));
                inputField.click(function () {
                    tthis.onClick(object, fieldInfo, form);
                });

                return inputField;
            };

            Renderer.prototype.createWriteField = function (object, fieldInfo, form) {
                return this.createReadField(object, fieldInfo, form);
            };

            Renderer.prototype.setValueByWriteField = function (object, fieldInfo, dom, form) {
                // Buttons don't have action stuff
                return;
            };

            Renderer.prototype.onClick = function (object, fieldInfo, form) {
                var convertedObject = form.convertViewToObject();

                // Everything seemed to be successful, now send back to server
                serverapi.getAPI().sendObjectToServer(getClickUrl(fieldInfo), convertedObject, function (data) {
                    alert('Successfully sent');
                    form.evaluateActionResponse(data);
                });
            };
            return Renderer;
        })();
        ActionButton.Renderer = Renderer;
    })(exports.ActionButton || (exports.ActionButton = {}));
    var ActionButton = exports.ActionButton;
});
//# sourceMappingURL=datenmeister.fieldinfo.js.map
