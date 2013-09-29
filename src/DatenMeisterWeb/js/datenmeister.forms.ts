
import api = require("datenmeister.serverapi");
import d = require("datenmeister.objects");
import t = require("datenmeister.datatable");
import navigation = require("datenmeister.navigation");

/* 
 * Has to be called before every view,
 * resets the visibility for each form
 */
export function prepareForViewChange() {
    $("#content > div").hide();
}

export class BackButtonView extends Backbone.View {
    constructor(options: Backbone.ViewOptions) {
        super(options);
        this.delegateEvents(
            {
                "click .backbutton": function () {
                    navigation.back();
                    return false;
                }
            });
    }
}

/* Defines the view which allows the user to connect to the server */
export class ServerConnectionView extends Backbone.View {

    // Call back, when user has depressed connect button
    onConnect: (settings: api.ServerSettings) => any;

    formDom: JQuery;

    constructor(options: Backbone.ViewOptions) {
        super(options);

        this.delegateEvents(
            {
                "click .serverconnection_button": this.connect
            });

        this.render();
    }

    render(): Backbone.View {
        prepareForViewChange();

        // Shows the current form
        this.$el.show();

        var tthis = this;
        this.$(".serveraddress").val(window.localStorage.getItem("serverconnection_serveraddress"));
        return this;
    }

    connect(): boolean {
        var tthis = this;
        var settings = new api.ServerSettings();
        settings.serverAddress = $(".serveraddress", this.formDom).val();

        window.localStorage.setItem("serverconnection_serveraddress", settings.serverAddress);

        // Check, if connection is possible
        var serverAPI = new api.ServerAPI(settings);
        serverAPI.getServerInfo(
            function (info) {
                // Yes, do the connect
                if (tthis.onConnect !== undefined) {
                    tthis.onConnect(settings);
                }
            },
            function () {
            });

        return false;
    }
}

export interface ExtentTableViewOptions extends Backbone.ViewOptions {
    extentElement?: d.JsonExtentData;
    tableOptions?: t.TableOptions;
    url?: string;
}

export class ExtentTableView extends Backbone.View {
    extentElement: d.JsonExtentData;
    tableOptions: t.TableOptions;
    url: string;
    data: d.JsonExtentData;  // Result from query, which will be shown render

    constructor(options?: ExtentTableViewOptions) {
        _.extend(this, options);

        super(options);

        if (this.url !== undefined && this.extentElement === undefined) {
            this.loadAndRender();
        }
        else if (this.extentElement !== undefined && this.tableOptions !== undefined) {
            this.render();
        }
        else {
            throw "ExtentTableView has no url and no object to render";
        }
    }

    loadAndRender(): ExtentTableView {
        var tthis = this;

        api.getAPI().getObjectsInExtent(
            this.url,
            function (data) {

                tthis.data = data;
                tthis.render()
            });
        return this;
    }

    render(): ExtentTableView {
        prepareForViewChange();
        var tthis = this;

        this.$(".datatable").empty();

        var table = this.showObjects();
        this.$el.show();

        return this;
    }

    showObjects(): t.DataTable {
        var tthis = this;
        var table = new t.DataTable(this.data.extent, this.$(".datatable"), this.tableOptions);

        // Create the columns
        table.defineColumns(this.data.columns);

        // Adds the objects
        for (var n = 0; n < this.data.objects.length; n++) {
            var func = function (obj: d.JsonExtentObject) {
                table.addObject(obj);
            }

            func(this.data.objects[n]);
        }

        table.setItemClickedEvent(function (object: d.JsonExtentObject) {
            tthis.trigger('rowclicked', object);
        });

        table.renderTable();

        return table;
    }
}

export class AllExtentsView extends ExtentTableView {
    constructor(options?: ExtentTableViewOptions) {

        // Defines the default url
        this.url = "datenmeister:///pool";

        super(options);

        if (this.tableOptions === undefined) {
            this.tableOptions = new t.TableOptions();
            this.tableOptions.allowNew = false;
            this.tableOptions.allowEdit = false;
            this.tableOptions.allowDelete = false;
        }

        this.bind('rowclicked', function (clickedObject) {
            var route = "extent/" + encodeURIComponent(clickedObject.get('uri'));
            navigation.to(route);
        });
    }
}