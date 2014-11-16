using DatenMeister.Entities.UML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Data.FileSystem
{
    public abstract class FileSystemObject : NamedElement
    {
        /// <summary>
        /// Stores the file system information, which is used to get the properties
        /// </summary>
        protected abstract System.IO.FileSystemInfo FileSystemInfo
        {
            get;
        }

        /// <summary>
        /// Gets the id of the object
        /// </summary>
        public virtual string id
        {
            get { return this.relativePath; }
        }

        public new virtual string name
        {
            get { return this.FileSystemInfo.Name; }
        }

        public virtual string relativePath
        {
            get;
            set;
        }

        public virtual string extension
        {
            get { return Path.GetExtension(this.FileSystemInfo.Name); }
        }
    }
}
