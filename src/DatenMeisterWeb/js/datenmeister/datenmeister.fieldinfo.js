define(["require", "exports", "datenmeister.fieldinfo.objects", "datenmeister.serverapi"], function(require, exports, fo, serverapi) {
    

    /*
    * Contains actions which were sent back from server to client
    * The actions contain the information what to be done on browser
    */
    var ClientActionInformation = (function () {
        function ClientActionInformation() {
        }
        return ClientActionInformation;
    })();
    exports.ClientActionInformation = ClientActionInformation;

    

    function getRendererByObject(object) {
        return exports.getRenderer(object.get("type"));
    }
    exports.getRendererByObject = getRendererByObject;

    /*
    * Gets the renderer for a specific object type
    */
    function getRenderer(type) {
        if (type == fo.Comment.TypeName) {
            return new Comment.Renderer();
        }

        if (type == fo.TextField.TypeName) {
            return new TextField.Renderer();
        }

        if (type == fo.ActionButton.TypeName) {
            return new ActionButton.Renderer();
        }

        return new TextField.Renderer();
    }
    exports.getRenderer = getRenderer;

    /*
    * Describes one field element, that is just a read-only description text with pre-default content
    * This field will contain the title on the left column and the commentText as Encoded Text in table
    */
    (function (Comment) {
        var Renderer = (function () {
            function Renderer() {
            }
            Renderer.prototype.createReadField = function (object, field, form) {
                var result = $("<div></div>");
                result.text(fo.Comment.getComment(field));
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
        var Renderer = (function () {
            function Renderer() {
            }
            Renderer.prototype.createReadField = function (object, field, form) {
                var tthis = this;
                var span = $("<span />");
                var value = object.get(fo.General.getName(field));
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
                    value = object.get(fo.General.getName(field));
                }

                // Checks, if writing is possible
                var offerWriting = true;
                if (_.isArray(value) || _.isObject(value)) {
                    offerWriting = false;
                }

                // Creates the necessary controls
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
                if (fo.General.isReadOnly(field) === true) {
                    // Do nothing
                    return;
                }

                // Reads the value
                object.set(fo.General.getName(field), dom.val());
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
        var Renderer = (function () {
            function Renderer() {
            }
            Renderer.prototype.createReadField = function (object, fieldInfo, form) {
                var tthis = this;
                var inputField = $("<input type='button'></input>");
                inputField.attr('value', fo.ActionButton.getText(fieldInfo));
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
                serverapi.getAPI().sendObjectToServer(fo.ActionButton.getClickUrl(fieldInfo), convertedObject, function (data) {
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
