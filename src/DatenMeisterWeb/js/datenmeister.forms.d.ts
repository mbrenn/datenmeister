import api = require("datenmeister.serverapi");
import d = require("datenmeister.objects");
import t = require("datenmeister.datatable");
export declare function prepareForViewChange(): void;
export declare class ServerConnectionView extends Backbone.View {
    public onConnect: (settings: api.ServerSettings) => any;
    public formDom: JQuery;
    constructor(options: Backbone.ViewOptions);
    public render(): Backbone.View;
    public connect(): boolean;
}
export interface ExtentTableViewOptions extends Backbone.ViewOptions {
    extentElement?: d.JsonExtentData;
    tableOptions?: t.TableOptions;
    url?: string;
}
export declare class ExtentTableView extends Backbone.View {
    public extentElement: d.JsonExtentData;
    public tableOptions: t.TableOptions;
    public url: string;
    public data: d.JsonExtentData;
    constructor(options?: ExtentTableViewOptions);
    public loadAndRender(): ExtentTableView;
    public render(): ExtentTableView;
    public showObjects(): t.DataTable;
}
export declare class AllExtentsView extends ExtentTableView {
    constructor(options?: ExtentTableViewOptions);
}
