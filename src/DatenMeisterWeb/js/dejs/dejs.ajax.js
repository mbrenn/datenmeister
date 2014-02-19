/// <reference path="../jquery/jquery.d.ts" />
define(["require", "exports"], function(require, exports) {
    var AjaxSettings = (function () {
        function AjaxSettings() {
        }
        return AjaxSettings;
    })();
    exports.AjaxSettings = AjaxSettings;

    function reportError(serverResponse) {
        try  {
            var data = $.parseJSON(serverResponse);

            if (data !== undefined && data !== null && data["message"] !== undefined) {
                $("#errorlog").text(data["message"]);
            } else {
                $("#errorlog").text("Unknown Message: " + serverResponse);
            }
        } catch (exc) {
            alert(exc);
        }
    }
    exports.reportError = reportError;

    /*
    * Loads HTML content directly from webserver
    *
    * data.url: URL being used
    * data.domTarget: Target DOM element for content
    * data.success: function being executed in case of success
    */
    function loadWebContent(data) {
        $.ajax(data.url, undefined).done(function (responseData) {
            data.domTarget.html(responseData);

            if (data.success !== undefined) {
                data.success(responseData);
            }
        }).fail(function (responseData) {
            exports.reportError(responseData.responseText);
        });
    }
    exports.loadWebContent = loadWebContent;

    /*
    * Performs a request with certain default actions
    * data: Data-structure
    * data.url: URL being used
    * data.method: HTTP-Method being used
    * data.data: Data being send with the request.
    The data gets stringified, if contentType is 'application/json'
    * data.contentType: Content Type of the request
    * data.success: Function being called in case of success with result
    * data.fail: Function being called in case of non-success
    * data.prefix: Prefix for message being used for translation (register_, login_, etc)
    *              $("#" + prefix + "success") and $("#" + prefix + "nosuccess") will be used for success
    *              and non-success messages.
    *              $("#" + prefix + "domsuccess") is the element being shown in case of success
    *              $("#" + prefix + "domnosuccess_" + errorcode) is the element being shown in case of nonsuccess
    *              $("#" + prefix + "button") is the button being used to activate and not activate
    */
    function performRequest(data) {
        if (data === undefined) {
            data = {};
        }

        if (data.url === undefined) {
            throw "No url had been given";
        }

        // Cache DOM elements
        var domTextSuccess = $("." + data.prefix + "success");
        var domTextSuccessFadeOut = $("." + data.prefix + "successfadeout");
        var domTextNoSuccess = $("." + data.prefix + "nosuccess");
        var domSuccess = $("." + data.prefix + "domsuccess");
        var domNosuccess = $("." + data.prefix + "domnosuccess");
        var domButton = $("." + data.prefix + "button");

        var ajaxSettings = {};
        ajaxSettings.headers = { 'X-Requested-With': 1 };

        if (data.method !== undefined) {
            ajaxSettings.type = data.method;
        }

        if (data.data !== undefined) {
            // Checks, if contentType is 'application/json', if yes, stringify the data
            if (data.contentType !== undefined && data.contentType.toLowerCase() == 'application/json') {
                ajaxSettings.data = JSON.stringify(data.data);
            } else {
                ajaxSettings.data = data.data;
            }
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
        domNosuccess.hide();
        domButton.attr("disabled", "disabled");

        // Perform AJAX request
        $.ajax(data.url, ajaxSettings).done(function (resultData) {
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
                domNosuccess.show();

                exports.showFormFailure(data.prefix, data.fail);

                $("#errorlog").text(resultData.error.message);
            }
        }).fail(function (failData) {
            // Not successful :-(
            domButton.removeAttr("disabled");
            domNosuccess.show();

            exports.showFormFailure(data.prefix, data.fail);

            exports.reportError(failData.responseText);
        });
    }
    exports.performRequest = performRequest;

    function showFormFailure(prefix, failFunction) {
        //alert(message);
    }
    exports.showFormFailure = showFormFailure;
});
