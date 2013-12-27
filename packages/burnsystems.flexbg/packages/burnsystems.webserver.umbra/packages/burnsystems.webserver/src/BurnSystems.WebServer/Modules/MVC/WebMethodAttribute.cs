using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    /// <summary>
    /// Used to define a webmethod
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class WebMethodAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the webmethod, if name of webmethod is different 
        /// to the name of the Method
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
