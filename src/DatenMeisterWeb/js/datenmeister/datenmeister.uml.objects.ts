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

export module Class {
    export var TypeName='Class';

    export function create() {
        var result = new __d__.JsonExtentObject();
        result.set('type', 'Class');
        return result;
    }


    export function isAbstract(item: __d__.JsonExtentObject) {
        return item.get('isAbstract');
    }

    export function setAbstract(item : __d__.JsonExtentObject, value: any) {
        item.set('isAbstract', value);
    }

    export function getOwnedAttribute(item: __d__.JsonExtentObject) {
        return item.get('ownedAttribute');
    }

    export function setOwnedAttribute(item : __d__.JsonExtentObject, value: any) {
        item.set('ownedAttribute', value);
    }

    export function pushOwnedAttribute(item : __d__.JsonExtentObject, value: any) {
        var a = <Array<any>> item.get('ownedAttribute');
        if (a === undefined) {
            a = new Array<any>();
        }

        a.push(value);
        item.set('ownedAttribute', a);
    }

    export function getName(item: __d__.JsonExtentObject) {
        return item.get('name');
    }

    export function setName(item : __d__.JsonExtentObject, value: any) {
        item.set('name', value);
    }

}

