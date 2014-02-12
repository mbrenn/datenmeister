var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.serverapi", "datenmeister.objects", "datenmeister.datatable", "datenmeister.dataform", "datenmeister.navigation", "datenmeister.fieldinfo"], function(require, exports, __api__, __d__, __t__, __forms__, __navigation__, __fi__) {
    var api = __api__;
    var d = __d__;
    var t = __t__;
    var forms = __forms__;
    var navigation = __navigation__;
    var fi = __fi__;

    /*
    * Has to be called before every view,
    * resets the visibility for each form
    */
    function prepareForViewChange() {
        $("#content > div").hide();
    }
    exports.prepareForViewChange = prepareForViewChange;

    /*
    * Connects a simple backbutton with navigation.back call on click
    */
    var BackButtonView = (function (_super) {
        __extends(BackButtonView, _super);
        function BackButtonView(options) {
            _super.call(this, options);
            this.delegateEvents({
                "click .backbutton": function () {
                    navigation.back();
                    return false;
                }
            });
        }
        return BackButtonView;
    })(Backbone.View);
    exports.BackButtonView = BackButtonView;

    /*
    * Defines the view which allows the user to connect to the server .
    * This interface can also be seen as login screen
    */
    var ServerConnectionView = (function (_super) {
        __extends(ServerConnectionView, _super);
        function ServerConnectionView(options) {
            _super.call(this, options);

            this.delegateEvents({
                "click .serverconnection_button": this.connect
            });

            this.render();
        }
        ServerConnectionView.prototype.render = function () {
            var templatedElement = _.template($("#serverconnectiontemplate").html(), {});
            this.$el.html(templatedElement);

            this.$(".serveraddress").val(window.localStorage.getItem("serverconnection_serveraddress"));
            return this;
        };

        ServerConnectionView.prototype.connect = function () {
            var tthis = this;
            var settings = new api.ServerSettings();
            settings.serverAddress = $(".serveraddress", this.formDom).val();

            // Stores the selected
            window.localStorage.setItem("serverconnection_serveraddress", settings.serverAddress);

            // Check, if connection is possible
            var serverAPI = new api.ServerAPI(settings);
            serverAPI.getServerInfo(function (info) {
                if (tthis.onConnect !== undefined) {
                    tthis.onConnect(settings);
                }
            }, function () {
            });

            return false;
        };
        return ServerConnectionView;
    })(Backbone.View);
    exports.ServerConnectionView = ServerConnectionView;

    /*
    * Creates a view by loading or using the given extent data and creating a datatable
    * by using field information.
    */
    var DefaultTableView = (function (_super) {
        __extends(DefaultTableView, _super);
        function DefaultTableView(options) {
            _.extend(this, options);

            _super.call(this, options);

            if (this.url !== undefined && this.extentElement === undefined) {
                this.loadAndRender();
            } else if (this.extentElement !== undefined && this.viewObject !== undefined) {
                this.render();
            } else {
                throw "ExtentTableView has no url and no object to render";
            }
        }
        DefaultTableView.prototype.loadAndRender = function () {
            var tthis = this;

            api.getAPI().getObjectsInExtent(this.url, function (data) {
                tthis.data = data;
                tthis.render();
            });
            return this;
        };

        DefaultTableView.prototype.render = function () {
            var tthis = this;

            this.$(".datatable").empty();

            var table = this.showObjects();
            this.$el.show();

            return this;
        };

        DefaultTableView.prototype.showObjects = function () {
            var tthis = this;
            var table = new t.DataTable(this.data.extent, this.$(".datatable"), this.viewObject);

            for (var n = 0; n < this.data.objects.length; n++) {
                var func = function (obj) {
                    table.addObject(obj);
                };

                func(this.data.objects[n]);
            }

            if (this.columns === undefined) {
                table.autoGenerateColumns();
            } else {
                table.setFieldInfos(this.columns);
            }

            // Sets the 'item clicked' event
            table.setItemClickedEvent(function (object) {
                tthis.trigger('itemclicked', object);
            });

            table.render();

            return table;
        };
        return DefaultTableView;
    })(Backbone.View);
    exports.DefaultTableView = DefaultTableView;

    /*
    * Create a table for an extent list.
    * When the user clicks a certain event, he is forwarded to the detail view
    * He also gets a selector to switch views
    */
    var ExtentTableView = (function (_super) {
        __extends(ExtentTableView, _super);
        function ExtentTableView(options) {
            var tthis = this;
            _super.call(this, options);

            tthis.bind('itemclicked', function (clickedObject) {
                var route = "view/" + encodeURIComponent(clickedObject.extentUri + "#" + clickedObject.id);
                navigation.to(route);
            });

            // Updates the selector view
            var viewSelectorModel = new ViewSelectorModel();
            viewSelectorModel.setCurrentView(this.viewUrl);
            var viewSelector = new ViewSelector({
                el: this.$(".view_selector"),
                model: viewSelectorModel
            });

            viewSelector.unbind('viewselected');
            viewSelector.bind('viewselected', function (viewUrl) {
                var route = "extent/" + encodeURIComponent(tthis.url);
                if (!_.isEmpty(viewUrl)) {
                    route += "/" + encodeURIComponent(viewUrl);
                }

                navigation.to(route);
            });
        }
        return ExtentTableView;
    })(DefaultTableView);
    exports.ExtentTableView = ExtentTableView;

    /*
    * Creates a list of all extents.
    * When the user clicks one view, he will be forwarded to a list of all elements of the extent
    */
    var AllExtentsView = (function (_super) {
        __extends(AllExtentsView, _super);
        function AllExtentsView(options) {
            // Defines the default url
            this.url = "datenmeister:///pools";

            _super.call(this, options);

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
        return AllExtentsView;
    })(DefaultTableView);
    exports.AllExtentsView = AllExtentsView;

    var DetailView = (function (_super) {
        __extends(DetailView, _super);
        function DetailView(options) {
            var tthis = this;
            _.extend(this, options);

            _super.call(this, options);

            if (!_.isEmpty(this.url) && _.isEmpty(this.object)) {
                this.loadAndRender();
            } else if (this.object !== undefined && this.viewObject !== undefined) {
                this.render();
            } else {
                throw "ExtentTableView has no url and no object to render";
            }

            this.bind('itemclicked', function (clickedObject) {
                var route = "view/" + encodeURIComponent(clickedObject.extentUri + "#" + clickedObject.id);
                navigation.to(route);
            });

            // Loads the view selection
            var viewSelectorModel = new ViewSelectorModel();
            viewSelectorModel.setCurrentView(this.viewUrl);

            var viewSelector = new ViewSelector({
                el: this.$(".view_selector"),
                model: viewSelectorModel
            });

            viewSelector.unbind('viewselected');
            viewSelector.bind('viewselected', function (viewUrl) {
                var route = "view/" + encodeURIComponent(tthis.url);
                if (!_.isEmpty(viewUrl)) {
                    route += "/" + encodeURIComponent(viewUrl);
                }

                navigation.to(route);
            });
        }
        DetailView.prototype.loadAndRender = function () {
            var tthis = this;
            var urls = new Array();
            urls.push(this.url);

            if (!_.isEmpty(this.viewUrl)) {
                urls.push(this.viewUrl);
            }

            api.getAPI().getObjects(urls, function (objects) {
                tthis.object = objects[0];

                if (tthis.viewUrl !== undefined) {
                    tthis.viewObject = objects[1];
                }

                tthis.render();
            });
        };

        DetailView.prototype.render = function () {
            var tthis = this;

            this.$el.empty();

            var form = new forms.DataForm(this.object, this.$el, this.viewObject);

            if (this.viewObject === undefined) {
                form.autoGenerateFields();
            } else {
                form.setFieldInfos(this.viewObject.get('fieldinfos'));
            }

            form.render();

            this.$el.show();

            form.setItemClickedEvent(function (object) {
                tthis.trigger('itemclicked', object);
            });

            return this;
        };
        return DetailView;
    })(Backbone.View);
    exports.DetailView = DetailView;

    /*
    * Defines the model for the ViewSelector View.
    * Just the chosen view as string
    */
    var ViewSelectorModel = (function (_super) {
        __extends(ViewSelectorModel, _super);
        function ViewSelectorModel() {
            _super.apply(this, arguments);
        }
        ViewSelectorModel.prototype.getCurrentView = function () {
            return this.get('currentView');
        };

        ViewSelectorModel.prototype.setCurrentView = function (viewUri) {
            this.set('currentView', viewUri);
        };
        return ViewSelectorModel;
    })(Backbone.Model);
    exports.ViewSelectorModel = ViewSelectorModel;

    /*
    * Returns a view containing a drop down, where user can select
    * the view to be applied.
    * - Throws an 'change:currentView' event, when user has selected a specific view
    */
    var ViewSelector = (function (_super) {
        __extends(ViewSelector, _super);
        function ViewSelector(options) {
            _super.call(this, options);

            // Loads the views
            this.loadAndUpdateViews();
        }
        ViewSelector.prototype.loadAndUpdateViews = function () {
            var tthis = this;

            var select = this.$el;
            select.empty();
            select.append($("<option class='default' value=''>--- Default view ---</option>"));

            api.getAPI().getObjectsInExtent("datenmeister:///defaultviews/", function (views) {
                _.each(views.objects, function (view) {
                    var name = view.get('name');
                    var option = $("<option></option>");
                    option.val(view.getUri());
                    option.text(name);

                    if (view.getUri() === tthis.model.getCurrentView()) {
                        option.attr('selected', 'selected');
                    }

                    select.append(option);
                });
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
                } else {
                    // Default selection, when no view has been selected
                    tthis.trigger('viewselected', null);
                }
            });
        };
        return ViewSelector;
    })(Backbone.View);
    exports.ViewSelector = ViewSelector;

    var CreateNewExtentView = (function (_super) {
        __extends(CreateNewExtentView, _super);
        function CreateNewExtentView(options) {
            var view = fi.FormView.create();
            fi.View.setAllowEdit(view, false);
            fi.View.setAllowNew(view, false);
            fi.View.setAllowDelete(view, false);
            fi.View.setStartInEditMode(view, true);
            fi.FormView.setShowColumnHeaders(view, false);

            fi.View.pushFieldInfo(view, fi.Comment.create("Information", "Please give a title and filename for the new extent (without file extension)"));
            fi.View.pushFieldInfo(view, fi.TextField.create("Name", "name"));
            fi.View.pushFieldInfo(view, fi.TextField.create("Filename", "filename"));
            fi.View.pushFieldInfo(view, fi.ActionButton.create("Create", "extent/Create"));

            this.viewObject = view;
            this.object = new d.JsonExtentObject();

            // This method already calls renders
            _super.call(this, options);
        }
        return CreateNewExtentView;
    })(DetailView);
    exports.CreateNewExtentView = CreateNewExtentView;
});
//# sourceMappingURL=datenmeister.views.js.map
