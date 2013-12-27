"use strict";

define(["umbra"], function (u) {
    ///
    /// Defines one entry for console
    var consoleEntryClass = function (category, message, importance) {
        this.date = new Date();
        this.category = category
        this.message = message;
        this.importance = importance;
    };

    ///
    /// Defines the console
    var consoleClass = function () {
        /// Stores the reference to the domElement where console is located
        this.domContent = {};
        this.logEntries = [];
    };

    consoleClass.prototype =
	{
	    log: function (category, message, importance) {
	        var entry = new consoleEntryClass(category, message, importance);
	        this.logEntries.push(entry);
	        this.__addToDom(entry);
	    },

	    assignViewPoint: function (viewPoint) {
	        this.domContent = viewPoint.domContent;
	    },

	    logAjaxError: function (error, status, thrown) {
	        var response = $.parseJSON(error.responseText);
	        this.log("Workspace", "AJAX Error: " + status + " [" + thrown + "]: \r\n" + response.message, "Error");
	    },

	    __addToDom: function (entry) {
	        var row = $('<tr class="consoleentry"><td class="date"></td><td class="category"></td><td class="content"></td><td class="importance"></td></tr>');
	        row.find(".date").text(entry.date.format("dd.mm.yyyy HH:MM"));
	        row.find(".category").text(entry.category);
	        row.find(".content").html($("<pre></pre>").text(entry.message));
	        row.find(".importance").text(entry.importance);

	        // Append directly
	        var headerDom = this.domContent.find(".header");
	        headerDom.after(row);
	    }
	};

    u.umbra.addViewType(
		new u.ViewType(
			"BurnSystems.WebServer.Umbra.Requests.ConsoleUmbraRequest",
			function (data) {
			    data.viewPoint.domContent.html(
					'<table><tr class="header"><th>Date</th><th>Category</th><th>Content</th><th>Importance</th></tr></table>');
			    consoleInstance.assignViewPoint(data.viewPoint);
			}));

    // Returns the console
    var consoleInstance = new consoleClass();
    u.umbra.addPlugin("Umbra.Console", consoleInstance);

    var result =
	{
	    console: consoleInstance,
	    ConsoleEntry: consoleEntryClass
	};

    return result;
});