"use strict";

define(["umbra.viewpoint"], function (ViewPointClass) {

    ///////////////////////////////////////////
    // Definition of Area class	
    // @name Name of the DOM element where area has been created
    // @token Name of the Area within the workspace
    var AreaClass = function (domName, token) {
        this.width = 0;
        this.height = 0;
        this.name = domName;
        this.token = token;

        // Stores the list of views
        this.viewPoints = [];
        this.activeViewPoint = undefined;
        this.isVisible = true;
    };

    AreaClass.prototype =
	{
	    getEffectiveHeight: function () {
	        if (this.isVisible) {
	            return this.height;
	        }

	        return 0;
	    },

	    getEffectiveWidth: function () {
	        if (this.isVisible) {
	            return this.width;
	        }

	        return 0;
	    },


	    // Adds a view to area and 
	    addView: function (view) {
	        var domTabContent = this.addTabForView(view);

	        var viewPoint = new ViewPointClass(view, domTabContent);
	        this.viewPoints.push(viewPoint);
	        viewPoint.area = this;

	        view.isVisible = false;
	        view.areaAttached = this;

	        var targetDomContent = $("#" + this.name + " .content");
	        ViewPointClass.lastDomId++;
	        viewPoint.domContent = $('<div id="viewpoint_' + ViewPointClass.lastDomId + '"></div>');
	        viewPoint.domContent.html(view.content)
	        viewPoint.domContent.hide();

	        targetDomContent.append(viewPoint.domContent);

	        return viewPoint;
	    },

	    // Focuses a view, so content is shown in the area window
	    focusView: function (view) {
	        // Deactivate old view
	        this.clearView();

	        // Activate new view
	        var viewPoint = this.findViewPoint(view);
	        if (viewPoint === undefined) {
	            alert('View is not within this area');
	            return;
	        }

	        this.activeViewPoint = viewPoint;
	        view.isVisible = true;
	        this.activeViewPoint.domContent.show();
	        viewPoint.domRegisterTab.addClass("selected");

	        this.updateLayout();
	    },

	    clearView: function () {
	        var targetDomContent = $("#" + this.name + " .content");

	        // Deactivate old view
	        if (this.activeViewPoint !== undefined) {
	            this.activeViewPoint.getView().isVisible = false;
	            this.activeViewPoint.domContent.hide();
	            this.activeViewPoint.domRegisterTab.removeClass("selected");
	        }
	    },

	    // Removes a certain view from area
	    removeView: function (view) {
	        var foundViewIndex = -1;

	        for (var i = 0; i < this.viewPoints.length; i++) {
	            if (this.viewPoints[i].getView() === view) {
	                foundViewIndex = i;
	            }
	        }

	        if (view.isVisible) {
	            // Ok, first focus another view
	            var toFocusView = this.viewPoints[foundViewIndex + 1];
	            if (toFocusView === undefined) {
	                toFocusView = this.viewPoints[foundViewIndex - 1];
	            }

	            if (toFocusView === undefined) {
	                this.clearView();
	            }
	            else {
	                this.focusView(toFocusView.getView());
	            }
	        }

	        if (foundViewIndex === -1) {
	            alert('View not found in viewPoint');
	        }

	        var viewPoint = this.viewPoints[foundViewIndex];
	        var domAreaContent = $("#" + this.name + " .content");
	        var domTabs = $("#" + this.name + " .tabs");
	        var domTab = $("#" + view.name + "_tab");

	        // Remove from viewpoints
	        this.viewPoints.splice(foundViewIndex, 1);

	        // Removes domtab
	        domTab.remove();

	        // Remove from content
	        viewPoint.domContent.remove();
	    },

	    // Adds DOM for tab in view, adds it to area and returns DOM of tab.
	    addTabForView: function (view) {
	        var title = view.title;
	        var domTabContent = $('<div class="tab" id="' + view.name + "_tab" + '">'
				+ '<a id="' + view.name + "_tab_a" + '">...</a>'
				+ '<div class="closed"><img src="i/cross.png" alt="Close" id="' + view.name + "_tab_c" + '" /></div>'
				+ '</div>');
	        $("#" + this.name + " .tabs").append(domTabContent);
	        $("#" + view.name + "_tab_a").text(view.title);

	        var _this = this;
	        $("#" + view.name + "_tab").click(function () {
	            _this.focusView(view);
	        });

	        $("#" + view.name + "_tab_c").click(function () {
	            _this.removeView(view);
	        });

	        return domTabContent;
	    },

	    getViewPoints: function () {
	        return this.viewPoints;
	    },

	    getViews: function () {
	        var result = [];
	        for (var i = 0; i < this.viewPoints.length; i++) {
	            result[i] = this.viewPoints[i].getView();
	        }

	        return result;
	    },

	    findViewPoint: function (view) {
	        for (var i = 0; i < this.viewPoints.length; i++) {
	            if (this.viewPoints[i].getView() === view) {
	                return this.viewPoints[i];
	            }
	        }
	    },

	    updateLayout: function () {
	        var domArea = $("#" + this.name);
	        var domContent = $("#" + this.name + " .content");
	        var domTab = $("#" + this.name + " .tabs");

	        var totalHeight = domArea.height();
	        totalHeight -= domTab.height() + 13;

	        domContent.height(totalHeight);
	    }
	};

    return AreaClass;
});