using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace BurnSystems.WebServer.Modules.MVC
{
    public class JsonActionResult : BaseActionResult, IActionResult, IValueActionResult
    {
        /// <summary>
        /// Gets or sets the object that shall be returned
        /// </summary>
        public object ReturnObject
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the JsonActionResult object
        /// </summary>
        /// <param name="returnObject">Object to be returned</param>
        public JsonActionResult(object returnObject)
        {
            this.ReturnObject = returnObject;
        }

        /// <summary>
        /// Executes the query 
        /// </summary>
        /// <param name="listenerContext">Listenercontext to be used for sending</param>
        public void Execute(System.Net.HttpListenerContext listenerContext, IActivates container)
        {
            var serializer = new JavaScriptSerializer();
            listenerContext.Response.ContentType = "application/json";

            this.SendResult(listenerContext, serializer.Serialize(this.ReturnObject));
        }
    }
}
