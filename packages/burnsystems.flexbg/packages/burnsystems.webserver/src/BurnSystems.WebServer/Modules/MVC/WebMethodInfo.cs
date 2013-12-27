using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BurnSystems.WebServer.Modules.MVC
{
    internal class WebMethodInfo
    {
        /// <summary>
        /// Gets or sets the name of the webinfo
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the method info being used
        /// </summary>
        public MethodInfo MethodInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HTTP-Method, where this method shall be called.
        /// If null, for all methods
        /// </summary>
        public string IfMethodIs
        {
            get;
            set;
        }
    }
}
