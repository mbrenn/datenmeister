
import api = require("datenmeister.serverapi");
import d = require("datenmeister.objects");
import t = require("datenmeister.datatable");
import forms = require("datenmeister.dataform");
import navigation = require("datenmeister.navigation");
import fi = require("datenmeister.fieldinfo");

/* 
 * Has to be called before every view,
 * resets the visibility for each form
 */
export function prepareForViewChange() {
    $("#content > div").hide();
}

/* 
 * Connects a simple backbutton with navigation.back call on click
 */ 
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

/* 
 * Defines the view which allows the user to connect to the server . 
 * This interface can also be seen as login screen
 */
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

        var templatedElement = _.template($("#serverconnectiontemplate").html(), {} );
        this.$el.html(templatedElement);

        this.$(".serveraddress").val(window.localStorage.getItem("serverconnection_serveraddress"));
        return this;
    }

    connect(): boolean {
        var tthis = this;
        var settings = new api.ServerSettings();
        settings.serverAddress = $(".serveraddress", this.formDom).val();

        // Stores the selected 
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

/* 
 * Defines the arguments to create a view, which contains a table for a list of object. 
 * - extentElement: contains an array of all elements to be shown
 * - url: can be used alternatively to download the extent data
 * - tableOptions: Additional options (like: Edit allowed, etc)
 */
export interface DefaultTableViewOptions extends Backbone.ViewOptions {
    extentElement?: d.JsonExtentData;
    url?: string;
    viewObject?: d.JsonExtentObject;
}

/*
 * Creates a view by loading or using the given extent data and creating a datatable 
 * by using field information.
 */
export class DefaultTableView extends Backbone.View {
    extentElement: d.JsonExtentData;
    url: string;
    data: d.JsonExtentData;  // Result from query, which will be shown render
    columns: Array<d.JsonExtentObject>;

    // Defines the view url and or the viewObject. 
    // If no viewObject has been given, a default view will be generated
    viewUrl: string;
    viewObject: d.JsonExtentObject;

    constructor(options?: DefaultTableViewOptions) {
        _.extend(this, options);

        super(options);

        if (this.url !== undefined && this.extentElement === undefined) {
            this.loadAndRender();
        }
        else if (this.extentElement !== undefined && this.viewObject !== undefined) {
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

                tthis.data = data; // Stores the objects
                tthis.render()
            });
        return this;
    }

    render(): DefaultTableView {
        var tthis = this;

        this.$(".datatable").empty();

        var table = this.showObjects();
        this.$el.show();

        return this;
    }

    showObjects(): t.DataTable {
        var tthis = this;
        var table = new t.DataTable(this.data.extent, this.$(".datatable"), this.viewObject);
        
        // Adds the objects
        for (var n = 0; n < this.data.objects.length; n++) {
            var func = function (obj: d.JsonExtentObject) {
                table.addObject(obj);
            }

            func(this.data.objects[n]);
        }

        // Create the columns
        if (this.columns === undefined) {
            table.autoGenerateColumns();
        }
        else {
            table.setFieldInfos(this.columns);
        }

        // Sets the 'item clicked' event
        table.setItemClickedEvent(function (object: d.JsonExtentObject) {
            tthis.trigger('itemclicked', object);
        });

        table.render();

        return table;
    }
}

/* 
 * Create a table for an extent list. 
 * When the user clicks a certain event, he is forwarded to the detail view
 * He also gets a selector to switch views
 */
export class ExtentTableView extends DefaultTableView {
    constructor(options?: DefaultTableViewOptions) {
        var tthis = this;
        super(options);

       tthis.bind('itemclicked', (clickedObject) => {
            var route = "view/" + encodeURIComponent(clickedObject.extentUri + "#" + clickedObject.id);
            navigation.to(route);
        });

        // Updates the selector view
        var viewSelectorModel = new ViewSelectorModel();
        viewSelectorModel.setCurrentView(this.viewUrl);
        var viewSelector = new ViewSelector(
            {
                el: this.$(".view_selector"),
                model: viewSelectorModel
            });

        viewSelector.unbind('viewselected');
        viewSelector.bind('viewselected', (viewUrl: string) => {
            var route = "extent/" + encodeURIComponent(tthis.url);
            if (!_.isEmpty(viewUrl)) {
                route += "/" + encodeURIComponent(viewUrl);
            }

            navigation.to(route);
        });
    }
}

/* 
 * Creates a list of all extents. 
 * When the user clicks one view, he will be forwarded to a list of all elements of the extent
 */
export class AllExtentsView extends DefaultTableView {
    constructor(options?: DefaultTableViewOptions) {

        // Defines the default url
        this.url = "datenmeister:///pools";

        super(options);

        if (this.viewObject === undefined) {
            this.viewObject = fi.TableView.create();
            fi.View.setAllowNew(this.viewObject, false);
            fi.View.setAllowEdit(this.viewObject, false);
            fi.View.setAllowDelete(this.viewObject, false);
        }

        this.bind('itemclicked', function (clickedObject) {
            var route = "extent/" + encodeURIComponent(clickedObject.get('uri'));
            navigation.to(route);
        });
    }
}

/* 
 * Stores the view that shall be shown that shall be shown. 
 */
export interface DetailViewOptions extends Backbone.ViewOptions{

    // Object to be shown
    object?: d.JsonExtentObject;
    
    // Url of object to be shown. The loaded object will be stored in 'object'
    url?: string;
    
    // View object to be used
    viewObject?: d.JsonExtentObject;
    
    // The url of the view object. The loaded object will be stored in 'viewObject'
    viewUrl?: string;
}

export class DetailView extends Backbone.View {
    object: d.JsonExtentObject;
    url: string;

    // Defines the view url and or the viewObject. 
    // If no viewObject has been given, a default view will be generated
    viewUrl: string;
    viewObject: d.JsonExtentObject;

    constructor(options: DetailViewOptions) {
        var tthis = this;
        _.extend(this, options);

        super(options);

        if (!_.isEmpty(this.url) && _.isEmpty(this.object)) {
            this.loadAndRender();
        }
        else if (this.object !== undefined && this.viewObject !== undefined) {
            this.render();
        }
        else {
            throw "ExtentTableView has no url and no object to render";
        }

        this.bind('itemclicked', function (clickedObject) {
            var route = "view/" + encodeURIComponent(clickedObject.extentUri + "#" + clickedObject.id);
            navigation.to(route);
        });

        // Loads the view selection
        var viewSelectorModel = new ViewSelectorModel();
        viewSelectorModel.setCurrentView(this.viewUrl);

        var viewSelector = new ViewSelector(
            {
                el: this.$(".view_selector"),
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
                tthis.viewObject = objects[1];
            }

            tthis.render();
        });
    }

    render(): DetailView {
        var tthis = this;

        this.$(".form").empty();

        var form = new forms.DataForm(this.object, this.$el, this.viewObject);

        if (this.viewObject === undefined) {
            form.autoGenerateFields();
        }
        else {
            form.setFieldInfos(this.viewObject.get('fieldinfos'));
        }

        form.render();

        this.$el.show();
        
        form.setItemClickedEvent(function (object: d.JsonExtentObject) {
            tthis.trigger('itemclicked', object);
        });

        return this;
    }
}

/*
 * Defines the model for the ViewSelector View. 
 * Just the chosen view as string
 */
export class ViewSelectorModel extends Backbone.Model {
    getCurrentView(): string {
        return this.get('currentView');
    }

    setCurrentView(viewUri: string): void {
        this.set('currentView', viewUri);
    }
}

/*
 * Returns a view containing a drop down, where user can select
 * the view to be applied. 
 * - Throws an 'change:currentView' event, when user has selected a specific view
 */
export class ViewSelector extends Backbone.View {

    model: ViewSelectorModel;

    constructor(options: Backbone.ViewOptions) {
        super(options);

        // Loads the views
        this.loadAndUpdateViews();
    }

    loadAndUpdateViews() {
        var tthis = this;

        var select = this.$el;
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

export class CreateNewExtentView extends DetailView
{
    constructor(options: DetailViewOptions) {
        var view = fi.FormView.create();
        fi.View.setAllowEdit(view,false);
        fi.View.setAllowNew(view,false);
        fi.View.setAllowDelete(view,false);
        fi.View.setStartInEditMode(view,true);
        fi.FormView.setShowColumnHeaders(view, false);

        fi.View.pushFieldInfo(view, fi.Comment.create("Information", "Please give a title and filename for the new extent (without file extension)"));
        fi.View.pushFieldInfo(view, fi.TextField.create("Name", "name"));
        fi.View.pushFieldInfo(view, fi.TextField.create("Filename", "filename"));
        fi.View.pushFieldInfo(view, fi.ActionButton.create("Create", "/"));

        this.viewObject = view;
        this.object = new d.JsonExtentObject();

        // This method already calls renders
        super(options);
    }
}
