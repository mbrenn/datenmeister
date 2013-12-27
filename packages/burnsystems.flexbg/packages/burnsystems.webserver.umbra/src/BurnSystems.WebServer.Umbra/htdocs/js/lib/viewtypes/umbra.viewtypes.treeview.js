"use strict";

define(["umbra", "jquery.cookie", "jquery.hotkeys", "jquery.jstree", "umbra.instance", "plugins/umbra.console"], function (u, c, hk, jst, umbraInstance, consoleLog) {
    ///
    /// Defines one entry for console
    var treeViewClass = function () {
        this.currentPath = "/";

        // Dom-Element, where information shall be shown
        this.domElement = {};

        // Url, where entity information can be retrieved
        this.url = "";
    };

    treeViewClass.prototype =
        {

            loadAndShowChildren: function () {
                var tthis = this;

                $.ajax(
                    this.url + this.currentPath,
                    {
                    })
                    .success(function (data) {

                        var domUl = $("<ul class=\"treeview\"></ul>");

                        for (var i = 0; i < data.children.length; i++) {
                            var item = data.children[i];
                            var currentId = item.id;

                            var domli = $("<li><a class=\"list\">" + item.title + "(" + item.id + ")</a><img src=\"i/arrow_right.png\" alt=\"Open\" class=\"open\"/></li>");
                            $(".list", domli).click((function (id) {
                                return function (e) {
                                    e.preventDefault();
                                    tthis.currentPath += id + "/";
                                    tthis.loadAndShowChildren();
                                }
                            })(currentId));

                            $(".open", domli).click((function (id) {
                                return function (e) {
                                    e.preventDefault();

                                    umbraInstance.eventbus.onItemSelected(
                                        {
                                            path: tthis.currentPath + id + "/",
                                            sourceName: "umbra.viewtypes.treeview"
                                        });
                                }
                            })(currentId));

                            domUl.append(domli);
                        }

                        tthis.domElement.html("");

                        var domTitle = $("<div class=\"treeview\"><img src=\"i/arrow_right.png\" alt=\"Open\" class=\"open title\"/><h2>" + data.title + "</h2></div>");
                        $(domTitle).click(
                            function (e) {
                                e.preventDefault();

                                umbraInstance.eventbus.onItemSelected(
                                    {
                                        path: tthis.currentPath,
                                        sourceName: "umbra.viewtypes.treeview"
                                    });
                            });

                        tthis.domElement.append(domTitle);

                        if (tthis.currentPath != "/") {
                            var domBack = $("<a>Back to parent</a>");
                            domBack.click(function (e) {
                                e.preventDefault();
                                tthis.navigateBack();
                            });
                        }

                        tthis.domElement.append(domBack);

                        if (data.children.length > 0) {
                            tthis.domElement.append(domUl);
                        }
                    })
                    .error(function (x, error, data) {
                        consoleLog.console.logAjaxError(x, error, data);
                    });
            },

            navigateBack: function () {
                var lastIndexOf = this.currentPath.substring(0, this.currentPath.length - 1).lastIndexOf("/");

                this.currentPath = this.currentPath.substring(0, lastIndexOf + 1);
                if (this.currentPath == "") {
                    this.currentPath = "/";
                }

                this.loadAndShowChildren();
            }
        };

    var treeViewNodeClass = function () {
    }

    var result = {
        TreeViewClass: treeViewClass
    };

    u.umbra.addViewType(
        new u.ViewType(
            "BurnSystems.Umbra.TreeView",
            function (info) {
                info.viewPoint.domContent.html(
                    'TREEVIEW');

                var treeView = new treeViewClass();
                treeView.url = info.userData.entityUrl;
                treeView.domElement = info.viewPoint.domContent;

                // Load the entity information
                treeView.loadAndShowChildren();
            }));

    return result;
});