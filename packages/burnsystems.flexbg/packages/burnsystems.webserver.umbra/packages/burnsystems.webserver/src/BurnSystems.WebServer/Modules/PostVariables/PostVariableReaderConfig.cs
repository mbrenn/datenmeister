using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.PostVariables
{
    public class PostVariableReaderConfig
    {
        /// <summary>
        /// Gets or sets the maximum post length
        /// </summary>
        public int MaxPostLength
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes an instance of the PostVariableReaderConfig class
        /// </summary>
        public PostVariableReaderConfig()
        {
            this.MaxPostLength = 16 * 1024 * 1024;
        }
    }
}
