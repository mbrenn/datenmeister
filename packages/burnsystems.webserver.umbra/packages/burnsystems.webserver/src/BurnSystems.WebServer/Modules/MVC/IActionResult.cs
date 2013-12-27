using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    /// <summary>
    /// Defines the result of an MVC-Controller action, which is executed
    /// deferred. 
    /// </summary>
    public interface IActionResult
    {
        /// <summary>
        /// Executes the internal information and performs the necessary actions, so webserver can receive the
        /// required information
        /// </summary>
        /// <param name="listenerContext">Listener context to be used for an answer</param>
        void Execute(HttpListenerContext listenerContext, IActivates container); 
    }
}
