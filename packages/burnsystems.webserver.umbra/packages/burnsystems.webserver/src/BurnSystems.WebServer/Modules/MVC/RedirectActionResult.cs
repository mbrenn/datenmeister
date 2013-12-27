using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    public class RedirectActionResult : IActionResult
    {
        private string redirectUrl;

        public RedirectActionResult(string redirectUrl)
        {
            this.redirectUrl = redirectUrl;
        }

        public void Execute(System.Net.HttpListenerContext listenerContext, ObjectActivation.IActivates container)
        {
            listenerContext.Response.Redirect(this.redirectUrl);
        }
    }
}
