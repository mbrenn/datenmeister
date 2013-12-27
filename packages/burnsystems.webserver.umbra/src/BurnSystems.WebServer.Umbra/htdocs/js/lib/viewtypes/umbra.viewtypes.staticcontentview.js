"use strict";

define(["umbra", "umbra.instance"], function (u, umbraInstance) {


    u.umbra.addViewType(
        new u.ViewType(
            "BurnSystems.Umbra.StaticContentView",
            function (info) {
                info.viewPoint.domContent.html(
                    "<h1>" + info.userData.headline + "</h1><p>" + info.userData.content + "</p>");
            }));
});