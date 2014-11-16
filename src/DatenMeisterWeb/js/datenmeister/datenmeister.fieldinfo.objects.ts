/// <reference path="../backbone/backbone.d.ts" />
import __d__ = require('datenmeister.objects');

export module Comment {
    export var TypeName='Comment';

    export function create(name?: any, comment?: any) {
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


    export function getComment(item: __d__.JsonExtentObject) {
        return item.get('comment');
    }

    export function setComment(item : __d__.JsonExtentObject, value: any) {
        item.set('comment', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module General {
    export var TypeName='General';

    export function create(name?: any, binding?: any) {
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


    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module Checkbox {
    export var TypeName='Checkbox';

    export function create(name?: any, binding?: any) {
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


    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module TextField {
    export var TypeName='TextField';

    export function create(name?: any, binding?: any) {
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


    export function getWidth(item: __d__.JsonExtentObject) {
        return item.get('width');
    }

    export function setWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('width', value);
    }

    export function isMultiline(item: __d__.JsonExtentObject) {
        return item.get('isMultiline');
    }

    export function setMultiline(item : __d__.JsonExtentObject, value: any) {
        item.set('isMultiline', value);
    }

    export function isDateTime(item: __d__.JsonExtentObject) {
        return item.get('isDateTime');
    }

    export function setDateTime(item : __d__.JsonExtentObject, value: any) {
        item.set('isDateTime', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module HyperLinkColumn {
    export var TypeName='HyperLinkColumn';

    export function create(name?: any, binding?: any) {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'HyperLinkColumn');
        if (name !== undefined) {
            result.set('name', name);
        }

        if (binding !== undefined) {
            result.set('binding', binding);
        }

        return result;
    }


    export function getWidth(item: __d__.JsonExtentObject) {
        return item.get('width');
    }

    export function setWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('width', value);
    }

    export function isMultiline(item: __d__.JsonExtentObject) {
        return item.get('isMultiline');
    }

    export function setMultiline(item : __d__.JsonExtentObject, value: any) {
        item.set('isMultiline', value);
    }

    export function isDateTime(item: __d__.JsonExtentObject) {
        return item.get('isDateTime');
    }

    export function setDateTime(item : __d__.JsonExtentObject, value: any) {
        item.set('isDateTime', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module DatePicker {
    export var TypeName='DatePicker';

    export function create(name?: any, binding?: any) {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'DatePicker');
        if (name !== undefined) {
            result.set('name', name);
        }

        if (binding !== undefined) {
            result.set('binding', binding);
        }

        return result;
    }


    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module ActionButton {
    export var TypeName='ActionButton';

    export function create(text?: any, clickUrl?: any) {
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


    export function getText(item: __d__.JsonExtentObject) {
        return item.get('text');
    }

    export function setText(item : __d__.JsonExtentObject, value: any) {
        item.set('text', value);
    }

    export function getClickUrl(item: __d__.JsonExtentObject) {
        return item.get('clickUrl');
    }

    export function setClickUrl(item : __d__.JsonExtentObject, value: any) {
        item.set('clickUrl', value);
    }

}

export module ReferenceBase {
    export var TypeName='ReferenceBase';

    export function create(name?: any, binding?: any, referenceUrl?: any, propertyValue?: any) {
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


    export function getPropertyValue(item: __d__.JsonExtentObject) {
        return item.get('propertyValue');
    }

    export function setPropertyValue(item : __d__.JsonExtentObject, value: any) {
        item.set('propertyValue', value);
    }

    export function getReferenceUrl(item: __d__.JsonExtentObject) {
        return item.get('referenceUrl');
    }

    export function setReferenceUrl(item : __d__.JsonExtentObject, value: any) {
        item.set('referenceUrl', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module ReferenceByConstant {
    export var TypeName='ReferenceByConstant';

    export function create(name?: any, binding?: any) {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'ReferenceByConstant');
        if (name !== undefined) {
            result.set('name', name);
        }

        if (binding !== undefined) {
            result.set('binding', binding);
        }

        return result;
    }


    export function getValues(item: __d__.JsonExtentObject) {
        return item.get('values');
    }

    export function setValues(item : __d__.JsonExtentObject, value: any) {
        item.set('values', value);
    }

    export function pushValue(item : __d__.JsonExtentObject, value: any) {
        var a = <Array<any>> item.get('values');
        if (a === undefined) {
            a = new Array<any>();
        }

        a.push(value);
        item.set('values', a);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module ReferenceByRef {
    export var TypeName='ReferenceByRef';

    export function create(name?: any, binding?: any, referenceUrl?: any, propertyValue?: any) {
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


    export function getPropertyValue(item: __d__.JsonExtentObject) {
        return item.get('propertyValue');
    }

    export function setPropertyValue(item : __d__.JsonExtentObject, value: any) {
        item.set('propertyValue', value);
    }

    export function getReferenceUrl(item: __d__.JsonExtentObject) {
        return item.get('referenceUrl');
    }

    export function setReferenceUrl(item : __d__.JsonExtentObject, value: any) {
        item.set('referenceUrl', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module ReferenceByValue {
    export var TypeName='ReferenceByValue';

    export function create(name?: any, binding?: any, referenceUrl?: any, propertyValue?: any) {
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


    export function getPropertyValue(item: __d__.JsonExtentObject) {
        return item.get('propertyValue');
    }

    export function setPropertyValue(item : __d__.JsonExtentObject, value: any) {
        item.set('propertyValue', value);
    }

    export function getReferenceUrl(item: __d__.JsonExtentObject) {
        return item.get('referenceUrl');
    }

    export function setReferenceUrl(item : __d__.JsonExtentObject, value: any) {
        item.set('referenceUrl', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module MultiReferenceField {
    export var TypeName='MultiReferenceField';

    export function create(name?: any, binding?: any, referenceUrl?: any, propertyValue?: any) {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'MultiReferenceField');
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


    export function getPropertyValue(item: __d__.JsonExtentObject) {
        return item.get('propertyValue');
    }

    export function setPropertyValue(item : __d__.JsonExtentObject, value: any) {
        item.set('propertyValue', value);
    }

    export function getReferenceUrl(item: __d__.JsonExtentObject) {
        return item.get('referenceUrl');
    }

    export function setReferenceUrl(item : __d__.JsonExtentObject, value: any) {
        item.set('referenceUrl', value);
    }

    export function getTableViewInfo(item: __d__.JsonExtentObject) {
        return item.get('tableViewInfo');
    }

    export function setTableViewInfo(item : __d__.JsonExtentObject, value: any) {
        item.set('tableViewInfo', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getBinding(item: __d__.JsonExtentObject) {
        return item.get('binding');
    }

    export function setBinding(item : __d__.JsonExtentObject, value: any) {
        item.set('binding', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

    export function getColumnWidth(item: __d__.JsonExtentObject) {
        return item.get('columnWidth');
    }

    export function setColumnWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('columnWidth', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

}

export module View {
    export var TypeName='View';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'View');
        return result;
    }


    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getFieldInfos(item: __d__.JsonExtentObject) {
        return item.get('fieldInfos');
    }

    export function setFieldInfos(item : __d__.JsonExtentObject, value: any) {
        item.set('fieldInfos', value);
    }

    export function pushFieldInfo(item : __d__.JsonExtentObject, value: any) {
        var a = <Array<any>> item.get('fieldInfos');
        if (a === undefined) {
            a = new Array<any>();
        }

        a.push(value);
        item.set('fieldInfos', a);
    }

    export function getStartInEditMode(item: __d__.JsonExtentObject) {
        return item.get('startInEditMode');
    }

    export function setStartInEditMode(item : __d__.JsonExtentObject, value: any) {
        item.set('startInEditMode', value);
    }

    export function getDoAutoGenerateByProperties(item: __d__.JsonExtentObject) {
        return item.get('doAutoGenerateByProperties');
    }

    export function setDoAutoGenerateByProperties(item : __d__.JsonExtentObject, value: any) {
        item.set('doAutoGenerateByProperties', value);
    }

}

export module FormView {
    export var TypeName='FormView';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'FormView');
        result.set('allowEdit', true);
        result.set('allowDelete', true);
        result.set('allowNew', true);
        return result;
    }


    export function getAllowEdit(item: __d__.JsonExtentObject) {
        return item.get('allowEdit');
    }

    export function setAllowEdit(item : __d__.JsonExtentObject, value: any) {
        item.set('allowEdit', value);
    }

    export function getAllowDelete(item: __d__.JsonExtentObject) {
        return item.get('allowDelete');
    }

    export function setAllowDelete(item : __d__.JsonExtentObject, value: any) {
        item.set('allowDelete', value);
    }

    export function getAllowNew(item: __d__.JsonExtentObject) {
        return item.get('allowNew');
    }

    export function setAllowNew(item : __d__.JsonExtentObject, value: any) {
        item.set('allowNew', value);
    }

    export function getShowColumnHeaders(item: __d__.JsonExtentObject) {
        return item.get('showColumnHeaders');
    }

    export function setShowColumnHeaders(item : __d__.JsonExtentObject, value: any) {
        item.set('showColumnHeaders', value);
    }

    export function getAllowNewProperty(item: __d__.JsonExtentObject) {
        return item.get('allowNewProperty');
    }

    export function setAllowNewProperty(item : __d__.JsonExtentObject, value: any) {
        item.set('allowNewProperty', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getFieldInfos(item: __d__.JsonExtentObject) {
        return item.get('fieldInfos');
    }

    export function setFieldInfos(item : __d__.JsonExtentObject, value: any) {
        item.set('fieldInfos', value);
    }

    export function pushFieldInfo(item : __d__.JsonExtentObject, value: any) {
        var a = <Array<any>> item.get('fieldInfos');
        if (a === undefined) {
            a = new Array<any>();
        }

        a.push(value);
        item.set('fieldInfos', a);
    }

    export function getStartInEditMode(item: __d__.JsonExtentObject) {
        return item.get('startInEditMode');
    }

    export function setStartInEditMode(item : __d__.JsonExtentObject, value: any) {
        item.set('startInEditMode', value);
    }

    export function getDoAutoGenerateByProperties(item: __d__.JsonExtentObject) {
        return item.get('doAutoGenerateByProperties');
    }

    export function setDoAutoGenerateByProperties(item : __d__.JsonExtentObject, value: any) {
        item.set('doAutoGenerateByProperties', value);
    }

}

export module TableView {
    export var TypeName='TableView';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'TableView');
        result.set('allowEdit', true);
        result.set('allowDelete', true);
        result.set('allowNew', true);
        return result;
    }


    export function getExtentUri(item: __d__.JsonExtentObject) {
        return item.get('extentUri');
    }

    export function setExtentUri(item : __d__.JsonExtentObject, value: any) {
        item.set('extentUri', value);
    }

    export function getMainType(item: __d__.JsonExtentObject) {
        return item.get('mainType');
    }

    export function setMainType(item : __d__.JsonExtentObject, value: any) {
        item.set('mainType', value);
    }

    export function getTypesForCreation(item: __d__.JsonExtentObject) {
        return item.get('typesForCreation');
    }

    export function setTypesForCreation(item : __d__.JsonExtentObject, value: any) {
        item.set('typesForCreation', value);
    }

    export function pushTypesForCreation(item : __d__.JsonExtentObject, value: any) {
        var a = <Array<any>> item.get('typesForCreation');
        if (a === undefined) {
            a = new Array<any>();
        }

        a.push(value);
        item.set('typesForCreation', a);
    }

    export function getAllowEdit(item: __d__.JsonExtentObject) {
        return item.get('allowEdit');
    }

    export function setAllowEdit(item : __d__.JsonExtentObject, value: any) {
        item.set('allowEdit', value);
    }

    export function getAllowDelete(item: __d__.JsonExtentObject) {
        return item.get('allowDelete');
    }

    export function setAllowDelete(item : __d__.JsonExtentObject, value: any) {
        item.set('allowDelete', value);
    }

    export function getAllowNew(item: __d__.JsonExtentObject) {
        return item.get('allowNew');
    }

    export function setAllowNew(item : __d__.JsonExtentObject, value: any) {
        item.set('allowNew', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getFieldInfos(item: __d__.JsonExtentObject) {
        return item.get('fieldInfos');
    }

    export function setFieldInfos(item : __d__.JsonExtentObject, value: any) {
        item.set('fieldInfos', value);
    }

    export function pushFieldInfo(item : __d__.JsonExtentObject, value: any) {
        var a = <Array<any>> item.get('fieldInfos');
        if (a === undefined) {
            a = new Array<any>();
        }

        a.push(value);
        item.set('fieldInfos', a);
    }

    export function getStartInEditMode(item: __d__.JsonExtentObject) {
        return item.get('startInEditMode');
    }

    export function setStartInEditMode(item : __d__.JsonExtentObject, value: any) {
        item.set('startInEditMode', value);
    }

    export function getDoAutoGenerateByProperties(item: __d__.JsonExtentObject) {
        return item.get('doAutoGenerateByProperties');
    }

    export function setDoAutoGenerateByProperties(item : __d__.JsonExtentObject, value: any) {
        item.set('doAutoGenerateByProperties', value);
    }

}

export module TreeView {
    export var TypeName='TreeView';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'TreeView');
        return result;
    }


    export function getExtentUri(item: __d__.JsonExtentObject) {
        return item.get('extentUri');
    }

    export function setExtentUri(item : __d__.JsonExtentObject, value: any) {
        item.set('extentUri', value);
    }

}

