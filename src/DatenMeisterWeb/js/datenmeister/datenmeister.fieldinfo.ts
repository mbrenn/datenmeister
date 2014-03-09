
import d = require("datenmeister.objects");
import fo = require("datenmeister.fieldinfo.objects");
import serverapi = require("datenmeister.serverapi");

/*
 * Defines the interface for the view objects, which contain
 * creates the view for the browser. 
 * For example dt.DataForm and dt.DataTable should implement this interface
 */
export interface IDataView
{
    /*
     * Converts the view content to a JsonExtentObject object, that contains
     * all the properties of the form. 
     */
    convertViewToObject(): d.JsonExtentObject;

    /*
     * Evaluates the response from a server action.
     * This might lead to a model update, change of view or many other things.
     */
    evaluateActionResponse(data: ClientActionInformation): void;
}

/*
 * Contains actions which were sent back from server to client
 * The actions contain the information what to be done on browser
 */ 
export class ClientActionInformation {
    actions: Array<any>;
}

/*
 * Defines the renderer interface for the objects
 */
export interface IRenderer
{
    createReadField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, form: IDataView) : JQuery;
    
    createWriteField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, form: IDataView) : JQuery;
    
    setValueByWriteField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, dom: JQuery, form: IDataView) : void;
}

export function getRendererByObject(object: d.JsonExtentObject): IRenderer
{
    return getRenderer(object.get("type"));
}

/* 
 * Gets the renderer for a specific object type
 */
export function getRenderer(type: string): IRenderer
{
    if(type == fo.Comment.TypeName)
    {
        return new Comment.Renderer();
    }
    
    if (type == fo.TextField.TypeName)
    {
        return new TextField.Renderer();
    }
    
    if(type == fo.ActionButton.TypeName)
    {
        return new ActionButton.Renderer();
    }
    
    return new TextField.Renderer();
}

/*
 * Describes one field element, that is just a read-only description text with pre-default content
 * This field will contain the title on the left column and the commentText as Encoded Text in table
 */
export module Comment
{    
    export class Renderer implements IRenderer
    {
        createReadField(object: d.JsonExtentObject, field: d.JsonExtentObject, form: IDataView) : JQuery {
            var result = $("<div></div>");
            result.text(fo.Comment.getComment(field));
            return result;
        }
        
        createWriteField(object: d.JsonExtentObject, field: d.JsonExtentObject, form: IDataView) : JQuery {
            // Same as read field
            return this.createReadField(object, field, form);
        }
    
        setValueByWriteField(object: d.JsonExtentObject, field: d.JsonExtentObject, dom: JQuery, form: IDataView) : void {
            // Just a comment, nothing to do here
            return;
        }
    }
}

/*
 * Describes one textfield element, which has an input field and can be shown to the user.
 * It also contains information about height and width of the field.
 * - title: Title being shown to user
 * - name: Name of the property, where value will be stored
 * - readonly: True, if field is read-only
 * - width: Width of the element
 * - height: Height of the element
 */
export module TextField
{    
    export class Renderer implements IRenderer
    {
        createReadField(object: d.JsonExtentObject, field: d.JsonExtentObject, form: IDataView) : JQuery {
            var tthis = this;
            var span = $("<span />");
            var value = object.get(fo.General.getName(field));
            if (value === undefined || value === null) {
                span.html("<em>undefined</em>");
            }
            else if (_.isArray(value)) {
                span.text('Array with ' + value.length + " items:");
                var ul = $("<ul></ul>");
                _.each(value, function (item: d.JsonExtentObject) {
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
            }
            else {
                span.text(value);
            }

            return span;
        }
        
        createWriteField(object: d.JsonExtentObject, field: d.JsonExtentObject, form: IDataView) : JQuery {
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
            }
            else {
                return this.createReadField(object, field, form);
            }
        }
    
        setValueByWriteField(object: d.JsonExtentObject, field: d.JsonExtentObject, dom: JQuery) : void {
            if (fo.General.isReadOnly(field) === true) {
                // Do nothing
                return;
            }

            // Reads the value
            object.set(fo.General.getName(field), dom.val());
        }         
    }
}

/*
 * Defines the module for an action button, which sends the form data to a certain 
 * form
 */
export module ActionButton {

    export class Renderer implements IRenderer {
        createReadField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, form: IDataView): JQuery {
            var tthis = this;
            var inputField = $("<input type='button'></input>");
            inputField.attr('value', fo.ActionButton.getText(fieldInfo));
            inputField.click(function () {
                tthis.onClick(object, fieldInfo, form);
            });

            return inputField;
        }

        createWriteField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, form: IDataView): JQuery {
            return this.createReadField(object, fieldInfo, form);
        }

        setValueByWriteField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, dom: JQuery, form: IDataView): void {
            // Buttons don't have action stuff
            return;
        }

        onClick(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, form: IDataView): void {
            var convertedObject = form.convertViewToObject();

            // Everything seemed to be successful, now send back to server
            serverapi.getAPI().sendObjectToServer(
                fo.ActionButton.getClickUrl(fieldInfo),
                convertedObject,
                function (data) { form.evaluateActionResponse(data); });
        }
    }
}