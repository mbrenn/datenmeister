"use strict";

requirejs.config(
{
    //By default load any module IDs from js/lib
    baseUrl: 'js/lib/',
});

requirejs(['umbra'],
	function (umbra) {
	    var workSpace = new umbra.WorkSpace();
	    workSpace.create($("body"));
	    workSpace.hide(workSpace.areaTop);

	    /* 
         * Demo-Stuff
         */
	    var topView = new umbra.View("Top 1", "top1", "This is conteeeent");
	    var topView2 = new umbra.View("Top 2", "top2", "This is <b>MORE</b> conteeeent");
	    workSpace.areaRight.addView(topView);
	    workSpace.areaRight.addView(topView2);

	    var content = "";
	    for (var i = 1; i < 100; i++) {
	        content += "<p>This is the maincontent of the content, a longer text than required but a nice content. This is true!</p>";
	    }

	    var centerView = new umbra.View("Center 1", "center1", content);
	    var centerView2 = new umbra.View("Center 2", "center2", "This is <b>MORE</b> conteeeent!");
	    workSpace.areaCentered.addView(centerView);
	    workSpace.areaCentered.addView(centerView2);

	    workSpace.areaCentered.focusView(centerView);
	    workSpace.areaRight.focusView(topView);

	    var viewPoint = workSpace.openView(
            "bottom",
            "Console",
            "<div class=\"umbra_console\">C</div>",
            "umbra.console", ["plugins/umbra.console"],
            "BurnSystems.WebServer.Umbra.Requests.ConsoleUmbraRequest");
	    workSpace.findArea("bottom").focusView(viewPoint.getView());
	    workSpace.loadContent("framework/Version", "centered",
			{
			    success: function (area, view) {
			        area.focusView(view);
			    }
			});

	    var ribbonBar = workSpace.getRibbonBar();
	    var startTab = ribbonBar.addTab("START");
	    var helpTab = ribbonBar.addTab("HELP");
	    var file = startTab.addGroup("File");
	    var content = startTab.addGroup("Content");

	    file.addElement(new umbra.RibbonButton("New", function () { alert('NEW'); }));
	    file.addElement(new umbra.RibbonButton("Open", function () { alert('Open'); }));
	    file.addElement(new umbra.RibbonButton("Save", function () { alert('Save'); }));

	    content.addElement(new umbra.RibbonButton("Cut", function () { alert('Cut'); }));
	    content.addElement(new umbra.RibbonButton("Copy", function () { alert('Copy'); }));
	    content.addElement(new umbra.RibbonButton("Paste", function () { alert('Paste'); }));

	    var helpGroup = helpTab.addGroup("?");
	    helpGroup.addElement(new umbra.RibbonButton("Version", function () {
	        workSpace.loadContent("framework/Version", "centered",
           {
               success: function (area, view) {
                   area.focusView(view);
               }
           });
	    }));

	    /*
         * End of Demo stuff
         */

	    workSpace.loadContent("treeview/window/", "left",
			{
			    success: function (area, view) {
			        area.focusView(view);
			    }
			});

	    umbra.umbra.eventbus.ItemSelected(
            function (data) {
                workSpace.loadContent("detail/entities" + data.path, "centered",
                    {
                        success: function (area, view) {
                            area.focusView(view);
                        }
                    });
            });
	});