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

export class ExtentInfoCollection extends Backbone.Collection {
    model: ExtentInfo;
}

// Defines the information for a column that has been received from server
export class JsonExtentFieldInfo {
    name: string;
    title: string;

    // Number in pixels
    width: number;

    constructor(name?: string, title?: string) {
        if (name !== undefined) {
            this.name = name;
        }

        if (title === undefined) {
            this.title = this.name;
        }
    }
}

// Defines the information for a column that has been received from server
export class JsonExtentObject extends Backbone.Model {
    id: string;
    extentUri: string;

    constructor() {
        super();
    }

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
