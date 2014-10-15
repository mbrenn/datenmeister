/// <reference path="../backbone/backbone.d.ts" />
import __d__ = require('datenmeister.objects');

export module NamedElement {
    export var TypeName='NamedElement';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'NamedElement');
        return result;
    }


    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

}

export module Type {
    export var TypeName='Type';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'Type');
        return result;
    }


    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

}

export module Property {
    export var TypeName='Property';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'Property');
        return result;
    }


    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

}

