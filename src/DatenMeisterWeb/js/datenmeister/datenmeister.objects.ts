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

// Defines the information for a column that has been received from server
export class JsonExtentFieldInfo extends JsonExtentObject {
    /* 
     * Gets the name of the property
     */
    getName(): string {
        return this.get('name');
    }

    /* 
     * Gets the title of property as shown in table or views
     */
    getTitle(): string {
        return this.get('title');
    }

    /*
     * Gets the read-only status
     */
    getReadOnly(): boolean {
        return this.get('isReadonly');
    }

    setName(name: string): Backbone.Model {
        return this.set('name', name);
    }

    setTitle(title: string): Backbone.Model {
        return this.set('title', title);
    }

    /*
     * Sets the read only
     */
    setReadOnly(isReadOnly: boolean): Backbone.Model {
        return this.set('isReadonly', isReadOnly);
    }

    constructor(attributes?: any) {
        super(attributes);
    }
}

// Result from GetObjectsInExtent
export class JsonExtentData {
    extent: ExtentInfo;    
    // columns: Array<JsonExtentFieldInfo>;
    objects: Array<JsonExtentObject>;
}
