var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports", "datenmeister.serverapi", "datenmeister.datatable"], function(require, exports, __api__, __t__) {
    var api = __api__;
    
    var t = __t__;

    /*
    * Has to be called before every view,
    * resets the visibility for each form
    */
    function prepareForViewChange() {
        $("#content div").hide();
    }
    exports.prepareForViewChange = prepareForViewChange;

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

    var ExtentTableView = (function (_super) {
        __extends(ExtentTableView, _super);
        function ExtentTableView(options) {
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
        ExtentTableView.prototype.loadAndRender = function () {
            var tthis = this;

            api.singletonAPI.getObjectsInExtent(this.url, function (data) {
                tthis.data = data;
                tthis.render();
            });
            return this;
        };

        ExtentTableView.prototype.render = function () {
            var tthis = this;
            exports.prepareForViewChange();

            var table = this.showObjects();

            this.$(".datatable").empty();
            this.$(".datatable").append(table);
            this.$el.show();

            return this;
        };

        ExtentTableView.prototype.showObjects = function () {
            var tthis = this;
            var table = new t.DataTable(this.data.extent, this.$el, this.tableOptions);

            // Create the columns
            table.defineColumns(this.data.columns);

            for (var n = 0; n < this.data.objects.length; n++) {
                var func = function (obj) {
                    table.addObject(obj);
                };

                func(this.data.objects[n]);
            }

            table.setItemClickedEvent(function (object) {
                tthis.trigger('rowclicked', object);
            });

            table.renderTable();

            return table;
        };
        return ExtentTableView;
    })(Backbone.View);
    exports.ExtentTableView = ExtentTableView;

    var AllExtentsView = (function (_super) {
        __extends(AllExtentsView, _super);
        function AllExtentsView(options) {
            // Defines the default url
            this.url = "datenmeister:///pool";

            _super.call(this, options);

            if (this.tableOptions === undefined) {
                this.tableOptions = new t.TableOptions();
                this.tableOptions.allowNew = false;
                this.tableOptions.allowEdit = false;
                this.tableOptions.allowDelete = false;
            }

            this.bind('rowclicked', function (clickedObject) {
                var detailView = new ExtentTableView({
                    el: $("#objectlist"),
                    url: clickedObject.get('uri')
                });
            });
        }
        return AllExtentsView;
    })(ExtentTableView);
    exports.AllExtentsView = AllExtentsView;
});
//# sourceMappingURL=datenmeister.forms.js.map
