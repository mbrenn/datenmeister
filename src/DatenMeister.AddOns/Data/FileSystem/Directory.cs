using DatenMeister.Entities.UML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Data.FileSystem
{
    public class Directory : FileSystemObject
    {
        /// <summary>
        /// Stores the information for the directory
        /// </summary>
        private System.IO.DirectoryInfo directoryInfo;

        /// <summary>
        /// Initializes a new instance of the directoryInfo instance
        /// </summary>
        /// <param name="directoryInfo">The directory information being used</param>
        public Directory(System.IO.DirectoryInfo directoryInfo, string relativePath)
        {
            this.directoryInfo = directoryInfo;
            this.relativePath = relativePath;
        }

        /// <summary>
        /// Creates an enumeration of file system objects, specfically the directories
        /// and the files on a certain location
        /// </summary>
        /// <param name="rootPath">Path to be queried</param>
        /// <param name="relativePath">Contains the relative path of the directory. 
        /// Is always closed with a slash</param>
        /// <returns>Enumeration of filesystem objects</returns>
        public static IEnumerable<FileSystemObject> CreateOnDirectory(string rootPath, string relativePath = "")
        {
            if (!System.IO.Directory.Exists(rootPath))
            {
                throw new InvalidOperationException("The path does not exist");
            }

            var directoryInfo = new System.IO.DirectoryInfo(rootPath);
            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                yield return new Directory(
                    subDirectoryInfo,
                    string.Format("{0}/{1}", relativePath, subDirectoryInfo.Name));
            }

            // Returns the files
            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                yield return new File(
                    fileInfo,
                    string.Format("{0}/{1}", relativePath, fileInfo.Name));
            }
        }

        /// <summary>
        /// Gets the FileSystemInfo for the the base clas
        /// </summary>
        protected override System.IO.FileSystemInfo FileSystemInfo
        {
            get { return this.directoryInfo; }
        }
    }
}
