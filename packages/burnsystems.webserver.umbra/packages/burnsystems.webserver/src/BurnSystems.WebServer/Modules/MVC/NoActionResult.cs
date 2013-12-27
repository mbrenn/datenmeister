using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    /// <summary>
    /// This action result is returned, when nothing has to be done and 
    /// everything required has been done by during execution of webmethod
    /// </summary>
    public class NoActionResult : IActionResult
    {
        public void Execute(System.Net.HttpListenerContext listenerContext, ObjectActivation.IActivates container)
        {
        }
    }
}
