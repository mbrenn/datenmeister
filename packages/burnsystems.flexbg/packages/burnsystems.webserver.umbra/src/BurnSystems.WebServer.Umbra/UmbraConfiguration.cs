using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.WebServer.Dispatcher;

namespace BurnSystems.WebServer.Umbra
{
    /// <summary>
    /// Configuration for umbra
    /// </summary>
    public class UmbraConfiguration
    {
        /// <summary>
        /// Gets or sets the webpath, where umbra shall be located. 
        /// </summary>
        public string WebPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default configuration
        /// </summary>
        public static UmbraConfiguration Default
        {
            get
            {
                var result = new UmbraConfiguration();
                result.WebPath = "/umbra/";
                return result;
            }
        }
    }
}
