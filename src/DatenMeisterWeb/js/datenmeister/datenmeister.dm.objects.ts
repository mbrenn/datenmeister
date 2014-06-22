/// <reference path="../backbone/backbone.d.ts" />
import __d__ = require('datenmeister.objects');

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

