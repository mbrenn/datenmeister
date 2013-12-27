using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Requests
{
    /// <summary>
    /// Responsible to trigger console
    /// </summary>
    public class ConsoleUmbraRequest : BaseUmbraRequest
    {
        public override void Dispatch(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation context)
        {
            this.Title = "Console";
            this.Content = "<div class=\"umbra_console\">C</div>";
            this.AddScript("plugins/umbra.console");
            this.ViewTypeToken = typeof(ConsoleUmbraRequest).FullName;
        }
    }
}
