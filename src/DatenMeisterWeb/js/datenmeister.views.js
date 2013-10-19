var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.serverapi", "datenmeister.datatable", "datenmeister.dataform", "datenmeister.navigation"], function(require, exports, __api__, __t__, __forms__, __navigation__) {
    var api = __api__;
    
    var t = __t__;
    var forms = __forms__;
    var navigation = __navigation__;

    /*
    * Has to be called before every view,
    * resets the visibility for each form
    */
    function prepareForViewChange() {
        $("#content > div").hide();
    }
    exports.prepareForViewChange = prepareForViewChange;

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

    /* Defines the view which allows the user to connect to the server */
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
            exports.prepareForViewChange();

            // Shows the current form
            this.$el.show();

            var tthis = this;
            this.$(".serveraddress").val(window.localStorage.getItem("serverconnection_serveraddress"));
            return this;
        };

        ServerConnectionView.prototype.connect = function () {
            var tthis = this;
            var settings = new api.ServerSettings();
            settings.serverAddress = $(".serveraddress", this.formDom).val();

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

    var DefaultTableView = (function (_super) {
        __extends(DefaultTableView, _super);
        function DefaultTableView(options) {
            _.extend(this, options);

            _super.call(this, options);

            if (this.url !== undefined && this.extentElement === undefined) {
                this.loadAndRender();
            } else if (this.extentElement !== undefined && this.tableOptions !== undefined) {
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
            exports.prepareForViewChange();
            var tthis = this;

            this.$(".datatable").empty();

            var table = this.showObjects();
            this.$el.show();

            return this;
        };

        DefaultTableView.prototype.showObjects = function () {
            var tthis = this;
            var table = new t.DataTable(this.data.extent, this.$(".datatable"), this.tableOptions);

            // Create the columns
            table.setFieldInfos(this.data.columns);

            for (var n = 0; n < this.data.objects.length; n++) {
                var func = function (obj) {
                    table.addObject(obj);
                };

                func(this.data.objects[n]);
            }

            table.setItemClickedEvent(function (object) {
                tthis.trigger('itemclicked', object);
            });

            table.render();

            return table;
        };
        return DefaultTableView;
    })(Backbone.View);
    exports.DefaultTableView = DefaultTableView;

    var ExtentTableView = (function (_super) {
        __extends(ExtentTableView, _super);
        function ExtentTableView(options) {
            _super.call(this, options);

            this.bind('itemclicked', function (clickedObject) {
                var route = "view/" + encodeURIComponent(clickedObject.extentUri + "#" + clickedObject.id);
                navigation.to(route);
            });
        }
        return ExtentTableView;
    })(DefaultTableView);
    exports.ExtentTableView = ExtentTableView;

    var AllExtentsView = (function (_super) {
        __extends(AllExtentsView, _super);
        function AllExtentsView(options) {
            // Defines the default url
            this.url = "datenmeister:///pools";

            _super.call(this, options);

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
            } else if (this.object !== undefined && this.formOptions !== undefined) {
                this.render();
            } else {
                throw "ExtentTableView has no url and no object to render";
            }

            this.bind('itemclicked', function (clickedObject) {
                var route = "view/" + encodeURIComponent(clickedObject.extentUri + "#" + clickedObject.id);
                navigation.to(route);
            });

            var viewSelectorModel = new ViewSelectorModel();
            viewSelectorModel.setCurrentView(this.viewUrl);

            var viewSelector = new ViewSelector({
                el: '#detailview',
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
            exports.prepareForViewChange();

            this.$(".form").empty();

            var form = new forms.DataForm(this.object, this.$(".form"), this.options);

            if (this.viewObject === undefined) {
                form.autoGenerateFields();
            } else {
                form.setFieldInfos(this.viewObject.get('fieldInfos'));
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

    var ViewSelector = (function (_super) {
        __extends(ViewSelector, _super);
        function ViewSelector(options) {
            _super.call(this, options);

            // Loads the views
            this.loadAndUpdateViews();
        }
        ViewSelector.prototype.loadAndUpdateViews = function () {
            var tthis = this;

            var select = this.$('.view_selector');
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
});
//# sourceMappingURL=datenmeister.views.js.map
