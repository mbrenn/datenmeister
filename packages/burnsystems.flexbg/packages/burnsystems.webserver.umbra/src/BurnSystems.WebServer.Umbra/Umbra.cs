using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Responses;

namespace BurnSystems.WebServer.Umbra
{
    public class Umbra
    {
        /// <summary>
        /// Integrates umbra into 
        /// </summary>
        /// <param name="container">Container where umbra is integrated</param>
        public static void Integrate(ActivationContainer container)
        {
            Integrate(container, UmbraConfiguration.Default);
        }

        public static void Integrate(ActivationContainer container, UmbraConfiguration configuration)
        {
            var server = container.Get<Server>();
            Ensure.IsNotNull(server, "No Server in Actioncontainer");

            // AddWebFile(configuration, server, "index.html", "text/html", Resources_Umbra.index);

            // Add core function of umbra
            /*AddWebFile(configuration, server, "js/lib/umbra.js", "text/javascript", Resources_Umbra.js_umbra);
            AddWebFile(configuration, server, "js/lib/umbra.area.js", "text/javascript", Resources_Umbra.js_umbra_area);
            AddWebFile(configuration, server, "js/lib/umbra.dragbar.js", "text/javascript", Resources_Umbra.js_umbra_dragbar);
            AddWebFile(configuration, server, "js/lib/umbra.eventbus.js", "text/javascript", Resources_Umbra.js_umbra_eventbus);
            AddWebFile(configuration, server, "js/lib/umbra.instance.js", "text/javascript", Resources_Umbra.js_umbra_instance);
            AddWebFile(configuration, server, "js/lib/umbra.ribbonbar.js", "text/javascript", Resources_Umbra.js_umbra_ribbonbar);
            AddWebFile(configuration, server, "js/lib/umbra.ribbonbutton.js", "text/javascript", Resources_Umbra.js_umbra_ribbonbutton);
            AddWebFile(configuration, server, "js/lib/umbra.ribbonelement.js", "text/javascript", Resources_Umbra.js_umbra_ribbonelement);
            AddWebFile(configuration, server, "js/lib/umbra.ribbongroup.js", "text/javascript", Resources_Umbra.js_umbra_ribbongroup);
            AddWebFile(configuration, server, "js/lib/umbra.ribbontab.js", "text/javascript", Resources_Umbra.js_umbra_ribbontab);
            AddWebFile(configuration, server, "js/lib/umbra.view.js", "text/javascript", Resources_Umbra.js_umbra_view);
            AddWebFile(configuration, server, "js/lib/umbra.viewpoint.js", "text/javascript", Resources_Umbra.js_umbra_viewpoint);
            AddWebFile(configuration, server, "js/lib/umbra.viewtype.js", "text/javascript", Resources_Umbra.js_umbra_viewtype);
            AddWebFile(configuration, server, "js/lib/umbra.workspace.js", "text/javascript", Resources_Umbra.js_umbra_workspace);*/

            // Add Plugins
            // AddWebFile(configuration, server, "js/lib/umbra.console.js", "text/javascript", Resources_Umbra.js_umbra_console);

            // CSS Style sheet
            // AddWebFile(configuration, server, "css/umbra.css", "text/css", Resources_Umbra.css_umbra);

            // Library stupp
            /*AddWebFile(configuration, server, "js/lib/dateformat.js", "text/javascript", Files.DateFormat);
            AddWebFile(configuration, server, "js/lib/jquery.js", "text/javascript", Files.JQuery);
            AddWebFile(configuration, server, "js/lib/require.js", "text/javascript", Files.Require);*/

            // Initialization
            // AddWebFile(configuration, server, "js/lib/test.js", "text/javascript", Resources_Umbra.js_test);
            // AddWebFile(configuration, server, "js/lib/init.js", "text/javascript", Resources_Umbra.js_init);

            server.Add(
                new RelocationDispatcher(
                    DispatchFilter.ByExactUrl(configuration.WebPath), "index.html"));

            /*server.Add(
                new UmbraDispatcher(
                    DispatchFilter.ByUrl(configuration.WebPath), configuration.WebPath));*/
        }

        private static void AddWebFile(UmbraConfiguration configuration, Server server, string url, string mimeType, string content)
        {
            server.Add(
                new StaticContentResponse(
                    DispatchFilter.ByExactUrl(configuration.WebPath + url), mimeType, content));
        }
    }
}
