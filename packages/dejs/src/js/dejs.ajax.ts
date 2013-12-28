/// <reference path="jquery.d.ts" />

export class AjaxSettings {
    type: string;

    data: any;

    prefix: string;

    contentType: string;
}

export function reportError(serverResponse: string) {
    var data = $.parseJSON(serverResponse);

    if (data !== undefined && data !== null && data["message"] !== undefined) {
        $("#errorlog").text(data["message"]);
    }
    else {
        $("#errorlog").text("Unknown Message: " + serverResponse);
    }
}

/*
 * Loads HTML content directly from webserver
 *
 * data.url: URL being used
 * data.domTarget: Target DOM element for content
 * data.success: function being executed in case of success
 */
export function loadWebContent(data: any) {
    $.ajax(data.url, undefined)
        .success(function (responseData) {
            data.domTarget.html(responseData);

            if (data.success !== undefined) {
                data.success(responseData);
            }
        })
        .fail(function (responseData) {
            reportError(responseData.responseText);
        });
}

export interface PerformRequestSettings {
    url?: string;
    method?: string;
    data?: any;
    contentType?: string;
    success?: (data: any) => void;
    fail?: (data: any) => void;
    prefix?: string;
}
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
export function performRequest(data: PerformRequestSettings) {
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

    var ajaxSettings = new AjaxSettings();

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
    $.ajax(
            data.url,
            ajaxSettings)
        .success(function (resultData) {
            if (resultData.success) {
                // Ok, we have a successful request
                domButton.removeAttr("disabled");

                domTextSuccessFadeOut.show();
                domTextSuccessFadeOut.fadeOut(3000);

                domSuccess.show();

                if (data.success !== undefined) {
                    data.success(resultData);
                }
            }
            else {
                // Request done, but not successful
                domButton.removeAttr("disabled");
                domNosuccess.show();

                showFormFailure(data.prefix, data.fail);

                $("#errorlog").text(resultData.error.message);
            }
        })
        .fail(function (failData) {
            // Not successful :-(
            domButton.removeAttr("disabled");
            domNosuccess.show();

            showFormFailure(data.prefix, data.fail);

            reportError(failData.responseText);
        });
}

export function showFormFailure(prefix: string, failFunction: (data: any) => void) {
    //alert(message);
}
