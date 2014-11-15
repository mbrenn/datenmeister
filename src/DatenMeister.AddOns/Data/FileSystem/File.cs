using DatenMeister.Entities.UML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Data.FileSystem
{
    public class File : FileSystemObject
    {
        /// <summary>
        /// Stores the file information
        /// </summary>
        private System.IO.FileInfo fileInfo;

        /// <summary>
        /// Initializes a new instance of the File class.
        /// </summary>
        /// <param name="fileInfo">The file information to be used</param>
        public File(System.IO.FileInfo fileInfo, string relativePath)
        {
            this.fileInfo = fileInfo;
            this.relativePath = relativePath;
        }

        /// <summary>
        /// Gets the length of the file
        /// </summary>
        public virtual long length
        {
            get { return this.fileInfo.Length; }
        }

        protected override System.IO.FileSystemInfo FileSystemInfo
        {
            get { return this.fileInfo; }
        }
    }
}
