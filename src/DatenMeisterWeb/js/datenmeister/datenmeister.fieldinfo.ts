
import d = require("datenmeister.objects");

export interface IRenderer
{
    createReadField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject) : JQuery;
    
    createWriteField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject) : JQuery;
    
    setValueByWriteField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, dom: JQuery) : void;
}

export function getRendererByObject(object: d.JsonExtentObject): IRenderer
{
    return getRenderer(object.get("type"));
}

export function getRenderer(type: string): IRenderer
{
    if(type == Comment.TypeName)
    {
        return new Comment.Renderer();
    }
    
    if(type == TextField.TypeName)
    {
        return new TextField.Renderer();
    }
    
    return new TextField.Renderer();
}

/*
 * Defines the view element, which contains a list of field information. 
 * - fieldinfos: Array of fieldinfos to be added
 */
export module View
{
    export function create() : d.JsonExtentObject
    {
        var result = new d.JsonExtentObject();
        result.set('fieldinfos', new Array<d.JsonExtentObject>());
        return result;
    }
    
    export function getFieldInfos(value: d.JsonExtentObject) : Array<d.JsonExtentObject>
    {
        return <Array<d.JsonExtentObject>> this.get('fieldinfos') ;
    }
    
    export function pushFieldInfo(value: d.JsonExtentObject, fieldInfo: d.JsonExtentObject) : void
    {
        var infos = <Array<d.JsonExtentObject>> value.get('fieldinfos')
        if (infos === undefined)
        {
            infos = new Array <d.JsonExtentObject>();
        }
        
        infos.push(fieldInfo);
        setFieldInfos(value, infos);
    }
    
    export function setFieldInfos(value: d.JsonExtentObject, fieldInfos: Array<d.JsonExtentObject>) : void
    {
        value.set('fieldinfos', fieldInfos);
    }
}

export module FormView
{
}

/* 
 * Describes the most generic functions
 */
export module General
{    
    export function setTitle(value: d.JsonExtentObject, title : string)
    {
        value.set("title", title);
    }
    
    export function setName(value : d.JsonExtentObject, name: string)
    {
        value.set("name", name);
    }
    
    export function setReadOnly(value : d.JsonExtentObject, isReadOnly: boolean)
    {
        value.set("readonly", isReadOnly);
    }    
    
    export function getTitle(value: d.JsonExtentObject) : string
    {
        return value.get("title");
    }
    
    export function getName(value: d.JsonExtentObject) : string
    {
        return value.get("name");
    }
    
    export function isReadOnly(value: d.JsonExtentObject) : boolean
    {
        return value.get("readonly");
    }    
}

/*
 * Describes one field element, that is just a read-only description text with pre-default content
 * This field will contain the title on the left column and the commentText as Encoded Text in table
 */
export module Comment
{
    export var TypeName = "DatenMeister.DataFields.Comment";
    
    export function create(title : string, commentText? : string) : d.JsonExtentObject
    {
        var result = new d.JsonExtentObject();
        
        if (title !== undefined)
        {
            setTitle(result, title);
        }
        
        if ( commentText !== undefined )
        {
            setComment(result, commentText);
        }
        
        result.set("type", "DatenMeister.DataFields.Comment");
        
        return result;
    }
    
    export function setTitle(value: d.JsonExtentObject, title : string)
    {
        General.setTitle(value, title);
    }
    
    export function setComment(value: d.JsonExtentObject, commentText : string)
    {
        value.set("comment", commentText);
    }
    export function getTitle(value: d.JsonExtentObject) : string
    {
        return General.getTitle(value);
    }
    
    export function getComment(value: d.JsonExtentObject) : string
    {
        return value.get("comment");
    }
    
    export class Renderer implements IRenderer
    {
        createReadField(object: d.JsonExtentObject, field: d.JsonExtentObject) : JQuery {
            var result = $("<div></div>");
            result.text(Comment.getComment(field));
            return result;
        }
        
        createWriteField(object: d.JsonExtentObject, field: d.JsonExtentObject) : JQuery {
            // Same as read field
            return this.createReadField(object, field);
        }
    
        setValueByWriteField(object: d.JsonExtentObject, field: d.JsonExtentObject, dom: JQuery) : void {
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
    export var TypeName = "DatenMeister.DataFields.TextField";
    
    export function create(title : string, key : string) : d.JsonExtentObject
    {
        var result = new d.JsonExtentObject();
        
        if ( title !== undefined )
        {
            setTitle(result, title);
        }
        
        if ( key !== undefined )
        {
            setName(result, key);
        }
        
        result.set("type", "DatenMeister.DataFields.TextField");
        
        return result;
    }
    
    export function setTitle(value : d.JsonExtentObject, title : string)
    {
        General.setTitle(value, title);
    }
    
    export function getTitle(value: d.JsonExtentObject) : string
    {
        return General.getTitle(value);
    }
    
    export function setName(value : d.JsonExtentObject, name: string)
    {
        General.setName(value, name);
    }
    
    export function getName(value: d.JsonExtentObject) : string
    {
        return General.getName(value);
    }
    
    export function setReadOnly(value: d.JsonExtentObject, isReadOnly: boolean)
    {
        General.setReadOnly(value, isReadOnly);
    }
    
    export function isReadOnly(value: d.JsonExtentObject) : boolean
    {
        return General.isReadOnly(value);
    }
    
    export function setWidth(value : d.JsonExtentObject, width : number)
    {
        value.set("width", width);
    }
    
    export function getWidth(value : d.JsonExtentObject) : number
    {
        return value.get("width");
    }
    
    export function setHeight(value : d.JsonExtentObject, height : number)
    {
        value.set("height", height);
    }
    
    export function getHeight(value : d.JsonExtentObject) : number
    {
        return value.get("height");
    }
    
    export class Renderer implements IRenderer
    {
        createReadField(object: d.JsonExtentObject, field: d.JsonExtentObject) : JQuery {
            var tthis = this;
            var span = $("<span />");
            var value = object.get(General.getName(field));
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
        
        createWriteField(object: d.JsonExtentObject, field: d.JsonExtentObject) : JQuery {
            var value;

            if (object !== undefined && field !== undefined) {
                value = object.get(General.getName(field));
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
                return this.createReadField(object, field);
            }
        }
    
        setValueByWriteField(object: d.JsonExtentObject, field: d.JsonExtentObject, dom: JQuery) : void {
            if (General.isReadOnly(field) === true) {
                // Do nothing
                return;
            }

            // Reads the value
            object.set(General.getName(field), dom.val());
        }         
    }
}