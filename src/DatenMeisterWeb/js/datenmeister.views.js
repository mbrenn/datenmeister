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
                tthis.trigger('rowclicked', object);
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

            this.bind('rowclicked', function (clickedObject) {
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

            this.bind('rowclicked', function (clickedObject) {
                var route = "extent/" + encodeURIComponent(clickedObject.get('uri'));
                navigation.to(route);
            });
        }
        return AllExtentsView;
    })(DefaultTableView);
    exports.AllExtentsView = AllExtentsView;

    var FormViewOptions = (function () {
        function FormViewOptions() {
        }
        return FormViewOptions;
    })();
    exports.FormViewOptions = FormViewOptions;

    var DetailView = (function (_super) {
        __extends(DetailView, _super);
        function DetailView(options) {
            _.extend(this, options);

            _super.call(this, options);

            if (this.url !== undefined && this.object === undefined) {
                this.loadAndRender();
            } else if (this.object !== undefined && this.formOptions !== undefined) {
                this.render();
            } else {
                throw "ExtentTableView has no url and no object to render";
            }
        }
        DetailView.prototype.loadAndRender = function () {
            var tthis = this;
            api.getAPI().getObject(this.url, function (object) {
                tthis.object = object;
                tthis.render();
            });
        };

        DetailView.prototype.render = function () {
            exports.prepareForViewChange();

            this.$(".form").empty();

            var form = new forms.DataForm(this.object, this.$(".form"));

            form.autoGenerateFields();
            form.render();

            this.$el.show();
            return this;
        };
        return DetailView;
    })(Backbone.View);
    exports.DetailView = DetailView;
});
//# sourceMappingURL=datenmeister.views.js.map
