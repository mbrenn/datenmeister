using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.PostVariables
{
    public class WebFile
    {
        /// <summary>
        /// Initializes a new instance of the WebFile class. 
        /// </summary>
        /// <param name="filename">Name of file</param>
        /// <param name="content">Content of file</param>
        public WebFile(string filename, byte[] content)
        {
            this.Filename = filename;
            this.Content = content;
        }

        /// <summary>
        /// Gets or sets the filename 
        /// </summary>
        public string Filename
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content of file
        /// </summary>
        public byte[] Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether a file was uploaded.
        /// </summary>
        public bool IsFileUploaded
        {
            get { return this.Content.Length > 0; }
        }
    }
}
