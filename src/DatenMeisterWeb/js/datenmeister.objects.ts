/// <reference path="lib/backbone.d.ts" />
/// <reference path="lib/underscore.d.ts" />

export class ExtentInfo extends Backbone.Model {
    getUri(): string {
        return <string> this.get("uri");
    }

    getType(): string {
        return <string> this.get("type");
    }
}

// Defines the information for a column that has been received from server
export class JsonExtentFieldInfo extends Backbone.Model {
    getName(): string {
        return this.get('name');
    }

    getTitle(): string {
        return this.get('title');
    }

    setName(name: string): void {
        return this.set('name', name);
    }

    setTitle(title: string): void {
        return this.set('title', title);
    }

    // Number in pixels
    width: number;

    constructor(attributes?: any, options?: any) {

        super(attributes, options);
    }
}

// Defines the information for an object within the extent
export class JsonExtentObject extends Backbone.Model {
    id: string;
    extentUri: string;

    constructor() {
        super();
    }

    // Gets the uri of the extent itself
    getUri(): string {
        return this.extentUri + "#" + this.id;
    }
}

// Result from GetObjectsInExtent
export class JsonExtentData {
    extent: ExtentInfo;
    columns: Array<JsonExtentFieldInfo>;
    objects: Array<JsonExtentObject>;
}
