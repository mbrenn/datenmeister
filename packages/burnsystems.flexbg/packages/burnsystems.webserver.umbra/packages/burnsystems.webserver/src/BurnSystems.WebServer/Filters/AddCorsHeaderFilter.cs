using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Filters
{
    /// <summary>
    /// Adds the cors header to request.
    /// Access-Control-Allow-Origin: *
    /// </summary>
    public class AddCorsHeaderFilter : IRequestFilter
    {
        public string AllowOrigins
        {
            get;
            set;
        }

        public string AllowHeaders
        {
            get;
            set;
        }

        public string AllowMethods
        {
            get;
            set;
        }

        public AddCorsHeaderFilter(
            string allowOrigins = "*", 
            string allowHeaders = "Content-Type",
            string allowMethods = "GET, POST")
        {
            this.AllowOrigins = allowOrigins;
            this.AllowHeaders = allowHeaders;
            this.AllowMethods = allowMethods;
        }

        public void BeforeDispatch(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation information, out bool cancel)
        {
            information.Context.Response.AddHeader("Access-Control-Allow-Origin", this.AllowOrigins);
            information.Context.Response.AddHeader("Access-Control-Allow-Headers", this.AllowHeaders);
            information.Context.Response.AddHeader("Access-Control-Allow-Methods", this.AllowMethods);
            cancel = false;
        }

        public void AfterDispatch(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation information, out bool cancel)
        {
            cancel = false;
        }

        public void AfterRequest(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation information)
        {
        }
    }
}
