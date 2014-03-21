/// <reference path="../backbone/backbone.d.ts" />
import __d__ = require('datenmeister.objects');

export module Comment {
    export var TypeName='Comment';

    export function create(title?: any, comment?: any) {
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


    export function getComment(item: __d__.JsonExtentObject) {
        return item.get('comment');
    }

    export function setComment(item : __d__.JsonExtentObject, value: any) {
        item.set('comment', value);
    }

    export function getTitle(item: __d__.JsonExtentObject) {
        return item.get('title');
    }

    export function setTitle(item : __d__.JsonExtentObject, value: any) {
        item.set('title', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

}

export module General {
    export var TypeName='General';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'General');
        return result;
    }


    export function getTitle(item: __d__.JsonExtentObject) {
        return item.get('title');
    }

    export function setTitle(item : __d__.JsonExtentObject, value: any) {
        item.set('title', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
    }

}

export module TextField {
    export var TypeName='TextField';

    export function create(title?: any, name?: any) {
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


    export function getWidth(item: __d__.JsonExtentObject) {
        return item.get('width');
    }

    export function setWidth(item : __d__.JsonExtentObject, value: any) {
        item.set('width', value);
    }

    export function getHeight(item: __d__.JsonExtentObject) {
        return item.get('height');
    }

    export function setHeight(item : __d__.JsonExtentObject, value: any) {
        item.set('height', value);
    }

    export function getTitle(item: __d__.JsonExtentObject) {
        return item.get('title');
    }

    export function setTitle(item : __d__.JsonExtentObject, value: any) {
        item.set('title', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function isReadOnly(item: __d__.JsonExtentObject) {
        return item.get('isReadOnly');
    }

    export function setReadOnly(item : __d__.JsonExtentObject, value: any) {
        item.set('isReadOnly', value);
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

export module View {
    export var TypeName='View';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'View');
        return result;
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

}

