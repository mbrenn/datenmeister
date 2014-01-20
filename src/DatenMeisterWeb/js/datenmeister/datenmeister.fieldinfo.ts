
import d = require("datenmeister.objects");

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
    convertViewContentToObject() : d.JsonExtentObject;
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
    if(type == Comment.TypeName)
    {
        return new Comment.Renderer();
    }
    
    if(type == TextField.TypeName)
    {
        return new TextField.Renderer();
    }
    
    if(type == ActionButton.TypeName)
    {
        return new ActionButton.Renderer();
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
        var result = <Array<d.JsonExtentObject>> value.get('fieldinfos') ;
        if ( result === undefined )
        {
            return new Array<d.JsonExtentObject>();
        }

        return result;
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
    
    export function setAllowEdit(value: d.JsonExtentObject, allowEdit: boolean)
    {
        value.set('allowEdit', allowEdit);
    }
    
    export function getAllowEdit(value: d.JsonExtentObject) : boolean    
    {
        return value.get('allowEdit');
    }
    
    export function setAllowNew(value: d.JsonExtentObject, allowNew: boolean)
    {
        value.set('allowNew', allowNew);
    }
    
    export function getAllowNew(value: d.JsonExtentObject) : boolean    
    {
        return value.get('allowNew');
    }
    
    export function setAllowDelete(value: d.JsonExtentObject, allowDelete: boolean)
    {
        value.set('allowDelete', allowDelete);
    }
    
    export function getAllowDelete(value: d.JsonExtentObject) : boolean    
    {
        return value.get('allowDelete');
    }
    
    export function setStartInEditMode(value: d.JsonExtentObject, startInEditMode: boolean) : void
    {
        value.set('startInEditMode', startInEditMode);
    }
    
    export function getStartInEditMode(value: d.JsonExtentObject)
    {
        return value.get('startInEditMode');
    }
}

/*
 * Defines additional properties which are used for a formview and not for a table view
 * All properties derived from View are also valid
 */
export module FormView
{
    export function create()
    {
        return View.create();
    }
    
    export function setTitle(value: d.JsonExtentObject, title: string) : void
    {
        General.setTitle(value, title);
    }
    
    export function getTitle(value: d.JsonExtentObject)
    {
        return General.getTitle(value);
    }
    
    export function setShowColumnHeaders(value: d.JsonExtentObject, flag: boolean) : void
    {
        value.set('showcolumnheaders', flag);
    }
    
    export function getShowColumnHeaders(value: d.JsonExtentObject) : boolean
    {
        return value.get('showcolumnheaders');
    }
    
    export function setAllowNewProperty(value: d.JsonExtentObject, allowNewProperty: boolean) : void
    {
        value.set('allowNewProperty', allowNewProperty);
    }
    
    export function getAllowNewProperty(value: d.JsonExtentObject) : boolean
    {
        return value.get('allowNewProperty');
    }
}

/*
 * Defines additional properties which are used for a tableview and not for a form view
 * All properties derived from View are also valid
 */
export module TableView
{
    export function create() : d.JsonExtentObject
    {
        var result = View.create();
        View.setAllowDelete(result, true);
        View.setAllowNew(result, true);
        View.setAllowEdit(result, true);
        return result;
    }
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
        
        result.set("type", TypeName);
        
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
        createReadField(object: d.JsonExtentObject, field: d.JsonExtentObject, form: IDataView) : JQuery {
            var result = $("<div></div>");
            result.text(Comment.getComment(field));
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
        createReadField(object: d.JsonExtentObject, field: d.JsonExtentObject, form: IDataView) : JQuery {
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
        
        createWriteField(object: d.JsonExtentObject, field: d.JsonExtentObject, form: IDataView) : JQuery {
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
                return this.createReadField(object, field, form);
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

/*
 * Defines the module for an action button, which sends the form data to a certain 
 * form
 */
export module ActionButton
{
    export var TypeName = "DatenMeister.DataFields.ActionButton";
    
    export function create(text: string, clickUrl: string)
    {
        var result = new d.JsonExtentObject();
        setText(result, text);
        setClickUrl(result, clickUrl);
        
        result.set("type", TypeName);
        
        return result;
    }
    
    export function setText(value: d.JsonExtentObject, text: string) : void
    {
        value.set('text', text);
    }
    
    export function getText(value: d.JsonExtentObject) : string
    {
        return value.get('text');
    }
    
    export function setClickUrl(value: d.JsonExtentObject, url: string): void
    {
        value.set('clickUrl', url);
    }
    
    export function getClickUrl(value: d.JsonExtentObject): string
    {
        return value.get('clickUrl');
    }
    
    export class Renderer implements IRenderer
    { 
        createReadField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, form: IDataView) : JQuery
        {
            var tthis = this;
            var inputField = $("<input type='button'></input>");
            inputField.attr('value', getText(fieldInfo));
            inputField.click(function() { 
                tthis.onClick(object, fieldInfo, form);
            });
            
            return inputField;
        }
        
        createWriteField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, form: IDataView) : JQuery
        {
            return this.createReadField(object, fieldInfo, form);
        }
        
        setValueByWriteField(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, dom: JQuery, form: IDataView) : void
        {
            // Buttons don't have action stuff
            return;
        }
        
        onClick(object: d.JsonExtentObject, fieldInfo: d.JsonExtentObject, form: IDataView) : void
        {
            alert ( 'NEW CLICK' );
        }
    }
}