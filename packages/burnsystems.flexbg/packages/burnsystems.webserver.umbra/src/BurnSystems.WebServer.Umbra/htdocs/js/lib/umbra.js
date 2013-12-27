"use strict";

define([
    "umbra.workspace",      // 1
    "umbra.view",           // 2
    "umbra.viewpoint",      // 3
    "umbra.area",           // 4
    "umbra.viewtype",       // 5
    "umbra.ribbonbar",      // 6
    "umbra.ribbonbutton",   // 7
    "umbra.ribbonelement",  // 8
    "umbra.ribbongroup",    // 9
    "umbra.ribbontab",      // 10
    "umbra.eventbus",       // 11
    "umbra.instance",       // 12
    "dateformat"],
    function (
        umbraWorkspaceClass,        // 1
        umbraViewClass,             // 2
        umbraViewPointClass,        // 3
        umbraAreaClass,             // 4
        umbraViewTypeClass,         // 5
        umbraRibbonBarClass,        // 6
        umbraRibbonButtonClass,     // 7
        umbraRibbonElementClass,    // 8
        umbraRibbonGroupClass,      // 9
        umbraRibbonTabClass,        // 10
        umbraEventBusClass,         // 11
        umbraInstance               // 12
        ) {

        ///////////////////////////////////////////
        // Combination of everything
        var result =
            {
                getVersion: function () { return "1.0.0.0"; },

                View: umbraViewClass,
                ViewPoint: umbraViewPointClass,
                WorkSpace: umbraWorkspaceClass,
                RibbonBar: umbraRibbonBarClass,
                RibbonButton: umbraRibbonButtonClass,
                RibbonElement: umbraRibbonElementClass,
                RibbonGroup: umbraRibbonGroupClass,
                RibbonTab: umbraRibbonTabClass,
                Area: umbraAreaClass,
                EventBus: umbraEventBusClass,
                ViewType: umbraViewTypeClass,
                umbra: umbraInstance
            };

        return result;
    });