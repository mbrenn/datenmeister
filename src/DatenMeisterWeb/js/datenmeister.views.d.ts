import api = require("datenmeister.serverapi");
import d = require("datenmeister.objects");
import t = require("datenmeister.datatable");
export declare function prepareForViewChange(): void;
export declare class BackButtonView extends Backbone.View {
    constructor(options: Backbone.ViewOptions);
}
export declare class ServerConnectionView extends Backbone.View {
    public onConnect: (settings: api.ServerSettings) => any;
    public formDom: JQuery;
    constructor(options: Backbone.ViewOptions);
    public render(): Backbone.View;
    public connect(): boolean;
}
export interface DefaultTableViewOptions extends Backbone.ViewOptions {
    extentElement?: d.JsonExtentData;
    tableOptions?: t.ViewOptions;
    url?: string;
}
export declare class DefaultTableView extends Backbone.View {
    public extentElement: d.JsonExtentData;
    public tableOptions: t.ViewOptions;
    public url: string;
    public data: d.JsonExtentData;
    constructor(options?: DefaultTableViewOptions);
    public loadAndRender(): DefaultTableView;
    public render(): DefaultTableView;
    public showObjects(): t.DataTable;
}
export declare class ExtentTableView extends DefaultTableView {
    constructor(options?: DefaultTableViewOptions);
}
export declare class AllExtentsView extends DefaultTableView {
    constructor(options?: DefaultTableViewOptions);
}
export interface DetailViewOptions extends Backbone.ViewOptions {
    object?: d.JsonExtentObject;
    url?: string;
}
export interface FormViewOptions extends Backbone.ViewOptions {
    viewUrl?: string;
    viewObject?: d.JsonExtentObject;
}
export declare class DetailView extends Backbone.View {
    public object: d.JsonExtentObject;
    public url: string;
    public urlView: string;
    public formOptions: FormViewOptions;
    public viewUrl: string;
    public viewObject: d.JsonExtentObject;
    constructor(options: FormViewOptions);
    public loadAndRender(): void;
    public render(): DetailView;
}
export declare class ViewSelectorModel extends Backbone.Model {
    public getCurrentView(): string;
    public setCurrentView(viewUri: string): void;
}
export declare class ViewSelector extends Backbone.View {
    public model: ViewSelectorModel;
    constructor(options: Backbone.ViewOptions);
    public loadAndUpdateViews(): void;
}
