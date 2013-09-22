import api = require("datenmeister.serverapi");
export declare function prepareForViewChange(): void;
export declare class ServerConnectionView extends Backbone.View {
    public onConnect: (settings: api.ServerSettings) => any;
    public formDom: JQuery;
    constructor(options: Backbone.ViewOptions);
    public render(): Backbone.View;
    public connect(): boolean;
}
