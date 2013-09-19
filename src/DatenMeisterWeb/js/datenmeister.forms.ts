
import api = require("datenmeister.serverapi");

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