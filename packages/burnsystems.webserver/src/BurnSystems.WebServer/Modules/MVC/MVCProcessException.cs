using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    /// <summary>
    /// This exception is caught by MVC Controller and a json is created
    /// </summary>
    public class MVCProcessException : Exception
    {
        /// <summary>
        /// Gets or sets the code of the exception
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the MVCException 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public MVCProcessException(string code, string message)
            : base(message)
        {
            this.Code = code;
        }

        /// <summary>
        /// Initializes a new instance of the MVCException 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public MVCProcessException(string code, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Code = code;
        }
    }
}
