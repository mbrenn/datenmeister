﻿/// <reference path="jquery.d.ts" />
define(["require", "exports"], function(require, exports) {
    (function (DeJS) {
        var AjaxSettings = (function () {
            function AjaxSettings() {
            }
            return AjaxSettings;
        })();

        var AjaxModules = (function () {
            function AjaxModules() {
            }
            AjaxModules.prototype.reportError = function (serverResponse) {
                var data = $.parseJSON(serverResponse);

                if (data !== undefined && data !== null && data["message"] !== undefined) {
                    $("#errorlog").text(data["message"]);
                } else {
                    $("#errorlog").text("Unknown Message: " + serverResponse);
                }
            };

            /*
            * Loads HTML content directly from webserver
            *
            * data.url: URL being used
            * data.domTarget: Target DOM element for content
            * data.success: function being executed in case of success
            */
            AjaxModules.prototype.loadWebContent = function (data) {
                var tthis = this;
                $.ajax(data.url, undefined).success(function (responseData) {
                    data.domTarget.html(responseData);

                    if (data.success !== undefined) {
                        data.success(responseData);
                    }
                }).fail(function (responseData) {
                    tthis.reportError(responseData.responseText);
                });
            };

            /*
            * Performs a request with certain default actions
            * data: Data-structure
            * data.url: URL being used
            * data.method: HTTP-Method being used
            * data.data: Data being send with the request
            * data.success: Function being called in case of success with result
            * data.fail: Function being called in case of non-success
            * data.prefix: Prefix for message being used for translation (register_, login_, etc)
            *              $("#" + prefix + "success") and $("#" + prefix + "nosuccess") will be used for success
            *              and non-success messages.
            *              $("#" + prefix + "domsuccess") is the element being shown in case of success
            *              $("#" + prefix + "domnosuccess_" + errorcode) is the element being shown in case of nonsuccess
            *              $("#" + prefix + "button") is the button being used to activate and not activate
            */
            AjaxModules.prototype.performRequest = function (data) {
                if (data === undefined) {
                    data = {};
                }

                if (data.url === undefined) {
                    throw "No url had been given";
                }

                var tthis = this;

                // Cache DOM elements
                var domTextSuccess = $("#" + data.prefix + "success");
                var domTextSuccessFadeOut = $("#" + data.prefix + "successfadeout");
                var domTextNoSuccess = $("#" + data.prefix + "nosuccess");
                var domSuccess = $("#" + data.prefix + "domsuccess");
                var domButton = $("#" + data.prefix + "button");

                var ajaxSettings = new AjaxSettings();

                if (data.method !== undefined) {
                    ajaxSettings.type = data.method;
                }

                if (data.data !== undefined) {
                    ajaxSettings.data = data.data;
                }

                if (data.prefix === undefined) {
                    data.prefix = '';
                }

                if (data.contentType !== undefined) {
                    ajaxSettings.contentType = data.contentType;
                }

                // Reset all content fields
                domTextSuccess.text("");
                domTextNoSuccess.text("");
                domSuccess.hide();
                domButton.attr("disabled", "disabled");

                // Perform AJAX request
                $.ajax(data.url, ajaxSettings).success(function (resultData) {
                    if (resultData.success) {
                        // Ok, we have a successful request
                        domButton.removeAttr("disabled");

                        domTextSuccessFadeOut.show();
                        domTextSuccessFadeOut.fadeOut(3000);

                        domSuccess.show();

                        if (data.success !== undefined) {
                            data.success(resultData);
                        }
                    } else {
                        // Request done, but not successful
                        domButton.removeAttr("disabled");

                        tthis.showFormFailure(data.prefix, data.fail);

                        $("#errorlog").text(resultData.error.message);
                    }
                }).fail(function (failData) {
                    // Not successful :-(
                    domButton.removeAttr("disabled");

                    tthis.showFormFailure(data.prefix, data.fail);

                    tthis.reportError(failData.responseText);
                });
            };

            AjaxModules.prototype.showFormFailure = function (prefix, message) {
                alert(message);
            };
            return AjaxModules;
        })();
        DeJS.AjaxModules = AjaxModules;

        DeJS.ajax = new AjaxModules();
    })(exports.DeJS || (exports.DeJS = {}));
    var DeJS = exports.DeJS;
});
