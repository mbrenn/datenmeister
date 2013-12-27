using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView
{
    /// <summary>
    /// Some extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns the json string 
        /// </summary>
        /// <param name="information">Context information to be used</param>
        /// <param name="container">Container for request</param>
        /// <param name="item">Object to be returned</param>
        public static void Json(this ContextDispatchInformation information, IActivates container, object item)
        {
            var result = new JsonActionResult(item);
            result.Execute(information.Context, container);
        }
    }
}
