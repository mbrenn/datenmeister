"use strict";

define([],
	function () {
	    var methods = {
	        encodeHtml: function (text) {
	            return $("<div></div>").text(text).html();
	        },

	        nl2br: function (text) {
	            return text.replace(/\n/g, "<br />");
	        }
	    };

	    return methods;
	});