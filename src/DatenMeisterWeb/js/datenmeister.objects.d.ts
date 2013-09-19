/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
export declare class ExtentInfo extends Backbone.Model {
    public getUri(): string;
    public getType(): string;
}
export declare class ExtentInfoCollection extends Backbone.Collection {
    public model: ExtentInfo;
}
export declare class JsonExtentFieldInfo {
    public name: string;
    public title: string;
    public width: number;
    constructor(name?: string, title?: string);
}
export declare class JsonExtentObject extends Backbone.Model {
    public id: string;
    public extentUri: string;
    constructor();
    public getUri(): string;
}
export declare class JsonExtentData {
    public extent: ExtentInfo;
    public columns: JsonExtentFieldInfo[];
    public objects: JsonExtentObject[];
}
