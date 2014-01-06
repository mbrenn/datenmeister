/// <reference path="../backbone/backbone.d.ts" />
/// <reference path="../underscore/underscore.d.ts" />

export class ExtentInfo extends Backbone.Model {
    getUri(): string {
        return <string> this.get("uri");
    }

    getType(): string {
        return <string> this.get("type");
    }
}

// Defines the information for an object within the extent
export class JsonExtentObject extends Backbone.Model {
    id: string;
    extentUri: string;

    constructor(attributes?: any) {
        super(attributes);
    }

    // Gets the uri of the extent itself
    getUri(): string {
        return this.extentUri + "#" + this.id;
    }
}

// Result from GetObjectsInExtent
export class JsonExtentData {
    extent: ExtentInfo;    
    // columns: Array<JsonExtentInfo>;
    objects: Array<JsonExtentObject>;
}
