/// <reference path="../backbone/backbone.d.ts" />
import __d__ = require('datenmeister.objects');

export module ExtentInfo {
    export var TypeName='ExtentInfo';

    export function create(storagePath?: any, name?: any, extentType?: any, uri?: any, extentClass?: any) {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'ExtentInfo');
        if (storagePath !== undefined) {
            result.set('storagePath', storagePath);
        }

        if (name !== undefined) {
            result.set('name', name);
        }

        if (extentType !== undefined) {
            result.set('extentType', extentType);
        }

        if (uri !== undefined) {
            result.set('uri', uri);
        }

        if (extentClass !== undefined) {
            result.set('extentClass', extentClass);
        }

        return result;
    }


    export function getUri(item: __d__.JsonExtentObject) {
        return item.get('uri');
    }

    export function setUri(item : __d__.JsonExtentObject, value: any) {
        item.set('uri', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

    export function getStoragePath(item: __d__.JsonExtentObject) {
        return item.get('storagePath');
    }

    export function setStoragePath(item : __d__.JsonExtentObject, value: any) {
        item.set('storagePath', value);
    }

    export function getExtentType(item: __d__.JsonExtentObject) {
        return item.get('extentType');
    }

    export function setExtentType(item : __d__.JsonExtentObject, value: any) {
        item.set('extentType', value);
    }

    export function isPrepopulated(item: __d__.JsonExtentObject) {
        return item.get('isPrepopulated');
    }

    export function setPrepopulated(item : __d__.JsonExtentObject, value: any) {
        item.set('isPrepopulated', value);
    }

    export function getDataProviderSettings(item: __d__.JsonExtentObject) {
        return item.get('dataProviderSettings');
    }

    export function setDataProviderSettings(item : __d__.JsonExtentObject, value: any) {
        item.set('dataProviderSettings', value);
    }

    export function getExtentClass(item: __d__.JsonExtentObject) {
        return item.get('extentClass');
    }

    export function setExtentClass(item : __d__.JsonExtentObject, value: any) {
        item.set('extentClass', value);
    }

}

export module RecentProject {
    export var TypeName='RecentProject';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'RecentProject');
        return result;
    }


    export function getFilePath(item: __d__.JsonExtentObject) {
        return item.get('filePath');
    }

    export function setFilePath(item : __d__.JsonExtentObject, value: any) {
        item.set('filePath', value);
    }

    export function getCreated(item: __d__.JsonExtentObject) {
        return item.get('created');
    }

    export function setCreated(item : __d__.JsonExtentObject, value: any) {
        item.set('created', value);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

}

export module Workbench {
    export var TypeName='Workbench';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'Workbench');
        return result;
    }


    export function getPath(item: __d__.JsonExtentObject) {
        return item.get('path');
    }

    export function setPath(item : __d__.JsonExtentObject, value: any) {
        item.set('path', value);
    }

    export function getType(item: __d__.JsonExtentObject) {
        return item.get('type');
    }

    export function setType(item : __d__.JsonExtentObject, value: any) {
        item.set('type', value);
    }

    export function getInstances(item: __d__.JsonExtentObject) {
        return item.get('instances');
    }

    export function setInstances(item : __d__.JsonExtentObject, value: any) {
        item.set('instances', value);
    }

    export function pushInstance(item : __d__.JsonExtentObject, value: any) {
        var a = <Array<any>> item.get('instances');
        if (a === undefined) {
            a = new Array<any>();
        }

        a.push(value);
        item.set('instances', a);
    }

}

