
import api = require("datenmeister.serverapi");

/* 
 * Has to be called before every view,
 * resets the visibility for each form
 */
export function prepareForViewChange() {
    $("#content div").hide();
}

/* Defines the view which allows the user to connect to the server */
export class ServerConnectionView extends Backbone.View {

    // Call back, when user has depressed connect button
    onConnect: (settings: api.ServerSettings) => any;

    formDom: JQuery;

    constructor(options: Backbone.ViewOptions) {
        super(options);

        this.delegateEvents(
            {
                "click .serverconnection_button": this.connect
            });

        this.render();

    }

    render(): Backbone.View {
        prepareForViewChange();

        // Shows the current form
        this.$el.show();

        var tthis = this;
        this.$(".serveraddress").val(window.localStorage.getItem("serverconnection_serveraddress"));
        return this;
    }

    connect() {
        var tthis = this;
        var settings = new api.ServerSettings();
        settings.serverAddress = $(".serveraddress", this.formDom).val();

        window.localStorage.setItem("serverconnection_serveraddress", settings.serverAddress);

        // Check, if connection is possible
        var serverAPI = new api.ServerAPI(settings);
        serverAPI.getServerInfo(
            function (info) {
                // Yes, do the connect
                if (tthis.onConnect !== undefined) {
                    tthis.onConnect(settings);
                }
            },
            function () {
            });

        return false;
    }
}