/// <reference path="../backbone/backbone.d.ts" />
import __d__ = require('datenmeister.objects');

export module RecentProject {
    export var TypeName='RecentProject';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'RecentProject');
        return result;
    }


    export function getDataPath(item: __d__.JsonExtentObject) {
        return item.get('dataPath');
    }

    export function setDataPath(item : __d__.JsonExtentObject, value: any) {
        item.set('dataPath', value);
    }

}

