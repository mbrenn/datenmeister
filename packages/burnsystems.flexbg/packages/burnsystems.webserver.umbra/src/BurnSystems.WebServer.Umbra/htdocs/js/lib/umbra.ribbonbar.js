"use strict";

define(["umbra.ribbontab"], function (RibbonTabClass) {

    ///////////////////////////////////////////
    // Definition of Ribbonbar class	
    var RibbonBarClass = function () {
        this.height = 128;
        this.tabs = [];
        this.domElement = {};
    };

    RibbonBarClass.prototype =
    {
        /// Inits the RibbonBar Class
        init: function (domElement) {
            this.domElement = domElement;
            this.domElement.html('<div class="tabs"></div><div class="content"></div>');
            this.currentTab = undefined;
        },

        /// Adds a tab with the given title
        /// @title Name of the title
        addTab: function (title) {
            var _this = this;
            var tab = new RibbonTabClass(title);
            this.tabs.push(tab);

            // Add to DOM
            var tabDom = $('<div class="tab">' + title + '</div>');
            var tabOwnerElement = this.domElement.find(".tabs");
            tabOwnerElement.append(tabDom);
            tab.domTab = tabDom;

            var contentDom = $('<div class="tabcontent"></div>');
            var contentOwnerElement = this.domElement.find(".content");
            contentOwnerElement.append(contentDom);
            tab.domContent = contentDom;
            tab.domContent.hide();

            // Add events
            tabDom.click(function () {
                _this.selectTab(tab);
            });

            if (this.currentTab === undefined) {
                this.selectTab(tab);
            }

            return tab;
        },

        selectTab: function (tab) {
            if (this.currentTab !== undefined) {
                this.currentTab.domContent.hide();
            }

            tab.domContent.show();
            this.currentTab = tab;
        }
    };

    return RibbonBarClass;
});