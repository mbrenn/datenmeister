using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Data.FileSystem
{
    public class FileSystemExtent : IURIExtent
    {
        /// <summary>
        /// Gets the root path for the elements
        /// </summary>
        public string RootPath
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the uri
        /// </summary>
        private string uri;

        /// <summary>
        /// Initializes a new instance of the FileSystemExtent
        /// </summary>
        /// <param name="uri">Uri to be set</param>
        /// <param name="rootPath">Root Path</param>
        public FileSystemExtent(string uri, string rootPath)
        {
            this.uri = uri;
            this.RootPath = rootPath;
        }

        public string ContextURI()
        {
            return this.uri;
        }

        public IReflectiveSequence Elements()
        {
            throw new NotImplementedException();
        }

        public IPool Pool
        {
            get;
            set;
        }

        public bool IsDirty
        {
            get;
            set;
        }
    }
}
