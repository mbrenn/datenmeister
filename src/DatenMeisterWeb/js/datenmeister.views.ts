
import api = require("datenmeister.serverapi");
import d = require("datenmeister.objects");
import t = require("datenmeister.datatable");
import forms = require("datenmeister.dataform");
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

export interface DefaultTableViewOptions extends Backbone.ViewOptions {
    extentElement?: d.JsonExtentData;
    tableOptions?: t.ViewOptions;
    url?: string;
}

export class DefaultTableView extends Backbone.View {
    extentElement: d.JsonExtentData;
    tableOptions: t.ViewOptions;
    url: string;
    data: d.JsonExtentData;  // Result from query, which will be shown render

    constructor(options?: DefaultTableViewOptions) {
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

    loadAndRender(): DefaultTableView {
        var tthis = this;

        api.getAPI().getObjectsInExtent(
            this.url,
            function (data) {

                tthis.data = data;
                tthis.render()
            });
        return this;
    }

    render(): DefaultTableView {
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
        table.setFieldInfos(this.data.columns);

        // Adds the objects
        for (var n = 0; n < this.data.objects.length; n++) {
            var func = function (obj: d.JsonExtentObject) {
                table.addObject(obj);
            }

            func(this.data.objects[n]);
        }

        table.setItemClickedEvent(function (object: d.JsonExtentObject) {
            tthis.trigger('itemclicked', object);
        });

        table.render();

        return table;
    }
}

export class ExtentTableView extends DefaultTableView {
    constructor(options?: DefaultTableViewOptions) {
        super(options);

        this.bind('itemclicked', function (clickedObject) {
            var route = "view/" + encodeURIComponent(clickedObject.extentUri + "#" + clickedObject.id);
            navigation.to(route);
        });
    }
}

export class AllExtentsView extends DefaultTableView {
    constructor(options?: DefaultTableViewOptions) {

        // Defines the default url
        this.url = "datenmeister:///pools";

        super(options);

        if (this.tableOptions === undefined) {
            this.tableOptions = new t.ViewOptions();
            this.tableOptions.allowNew = false;
            this.tableOptions.allowEdit = false;
            this.tableOptions.allowDelete = false;
        }

        this.bind('itemclicked', function (clickedObject) {
            var route = "extent/" + encodeURIComponent(clickedObject.get('uri'));
            navigation.to(route);
        });
    }
}

export interface DetailViewOptions extends Backbone.ViewOptions {
    object?: d.JsonExtentObject;
    url?: string;
}

export interface FormViewOptions extends Backbone.ViewOptions{
    viewUrl?: string;
    viewObject?: d.JsonExtentObject;
}

export class DetailView extends Backbone.View {
    object: d.JsonExtentObject;
    url: string;
    urlView: string;
    formOptions: FormViewOptions;

    // Defines the view url and or the viewObject. 
    // If no viewObject has been given, a default view will be generated
    viewUrl: string;
    viewObject: d.JsonExtentObject;

    constructor(options: FormViewOptions) {
        var tthis = this;
        _.extend(this, options);

        super(options);

        if (!_.isEmpty(this.url) && _.isEmpty(this.object)) {
            this.loadAndRender();
        }
        else if (this.object !== undefined && this.formOptions !== undefined) {
            this.render();
        }
        else {
            throw "ExtentTableView has no url and no object to render";
        }

        this.bind('itemclicked', function (clickedObject) {
            var route = "view/" + encodeURIComponent(clickedObject.extentUri + "#" + clickedObject.id);
            navigation.to(route);
        });

        var viewSelectorModel = new ViewSelectorModel();
        viewSelectorModel.setCurrentView(this.viewUrl);

        var viewSelector = new ViewSelector(
            {
                el: '#detailview',
                model: viewSelectorModel           
            });

        viewSelector.unbind('viewselected');
        viewSelector.bind('viewselected', function (viewUrl: string) {
            var route = "view/" + encodeURIComponent(tthis.url);
            if (!_.isEmpty(viewUrl)) {
                route += "/" + encodeURIComponent(viewUrl);
            }

            navigation.to(route);
        });
    }

    loadAndRender() {
        var tthis = this;
        var urls = new Array<string>();
        urls.push(this.url);

        if (!_.isEmpty(this.viewUrl)) {
            urls.push(this.viewUrl);
        }

        api.getAPI().getObjects(urls, function (objects: Array<d.JsonExtentObject>) {
            tthis.object = objects[0];

            if (tthis.viewUrl !== undefined) {
                tthis.viewObject = <d.JsonExtentFieldInfo> objects[1];
            }

            tthis.render();
        });
    }

    render(): DetailView {
        var tthis = this;
        prepareForViewChange();

        this.$(".form").empty();

        var form = new forms.DataForm(this.object, this.$(".form"), this.options);

        if (this.viewObject === undefined) {
            form.autoGenerateFields();
        }
        else {
            form.setFieldInfos(this.viewObject.get('fieldInfos'));
        }

        form.render();

        this.$el.show();
        
        form.setItemClickedEvent(function (object: d.JsonExtentObject) {
            tthis.trigger('itemclicked', object);
        });

        return this;
    }
}

export class ViewSelectorModel extends Backbone.Model {
    getCurrentView(): string {
        return this.get('currentView');
    }

    setCurrentView(viewUri: string): void {
        this.set('currentView', viewUri);
    }
}

export class ViewSelector extends Backbone.View {

    model: ViewSelectorModel;

    constructor(options: Backbone.ViewOptions) {
        super(options);

        // Loads the views
        this.loadAndUpdateViews();
    }

    loadAndUpdateViews() {
        var tthis = this;

        var select = this.$('.view_selector');
        select.empty();
        select.append($("<option class='default' value=''>--- Default view ---</option>"));

        api.getAPI().getObjectsInExtent(
            "datenmeister:///defaultviews/",
            function (views: d.JsonExtentData) {
                _.each(
                    views.objects,
                    function (view) {
                        var name = view.get('name');
                        var option = $("<option></option>");
                        option.val(view.getUri());
                        option.text(name);

                        if (view.getUri() === tthis.model.getCurrentView()) {
                            option.attr('selected', 'selected');
                        }

                        select.append(option);
                    })
            });

        this.model.bind('change:currentView', function (model, newCurrentView) {

            // Selects the correct item
            $("option", select).each(function () {
                this.selected = ((this.value == newCurrentView) ? "selected" : "");
            });
        });

        select.unbind('change');
        select.bind('change', function () {
            var selectedView = select.val();
            tthis.model.setCurrentView(selectedView);
            if (!_.isEmpty(selectedView)) {
                tthis.trigger('viewselected', selectedView);
            }
            else {
                // Default selection, when no view has been selected
                tthis.trigger('viewselected', null);
            }
        });
    }
}