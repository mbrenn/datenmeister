/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />
export declare class ExtentInfo extends Backbone.Model {
    public getUri(): string;
    public getType(): string;
}
export declare class JsonExtentObject extends Backbone.Model {
    public id: string;
    public extentUri: string;
    constructor(attributes?: any);
    public getUri(): string;
}
export declare class JsonExtentFieldInfo extends JsonExtentObject {
    public getName(): string;
    public getTitle(): string;
    public setName(name: string): void;
    public setTitle(title: string): void;
    constructor(attributes?: any);
}
export declare class JsonExtentData {
    public extent: ExtentInfo;
    public columns: JsonExtentFieldInfo[];
    public objects: JsonExtentObject[];
}
