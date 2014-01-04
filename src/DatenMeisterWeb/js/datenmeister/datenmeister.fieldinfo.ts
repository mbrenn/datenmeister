
import d = require("datenmeister.objects");

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
    
    export function getTitle(value: d.JsonExtentObject)
    {
        return value.get("title");
    }
    
    export function getName(value: d.JsonExtentObject)
    {
        return value.get("name");
    }
    
    export function getReadOnly(value: d.JsonExtentObject)
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
    
    export function getTitle(value: d.JsonExtentObject)
    {
        General.getTitle(value);
    }
    
    export function setName(value : d.JsonExtentObject, name: string)
    {
        General.setName(value, name);
    }
    
    export function getName(value: d.JsonExtentObject)
    {
        General.getName(value);
    }
    
    export function setReadOnly(value: d.JsonExtentObject, isReadOnly: boolean)
    {
        General.setReadOnly(value, isReadOnly);
    }
    
    export function getReadOnly(value: d.JsonExtentObject)
    {
        General.getReadOnly(value);
    }
    
    export function setWidth(value : d.JsonExtentObject, width : number)
    {
        value.set("width", width);
    }
    
    export function getWidth(value : d.JsonExtentObject)
    {
        return value.get("width");
    }
    
    export function setHeight(value : d.JsonExtentObject, height : number)
    {
        value.set("height", height);
    }
    
    export function getHeight(value : d.JsonExtentObject)
    {
        return value.get("height");
    }
}