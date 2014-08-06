using System;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System.IO;
using DatenMeister.DataProvider.Xml;


namespace DatenMeister.Logic
{
    /// <summary>
    /// Implements all necessary methods that helps to work with the datapool.
    /// Especially creation and deletion of extent is offered via methods.
    /// The method should be created by ObjectActivation framework
    /// </summary>
    public class ExtentPoolLogic
    {
        /// <summary>
        /// Gets or sets the xml data provider.
        /// </summary>
        /// <value>
        /// The xml data provider.
        /// </value>
        [Inject]
        public XmlDataProvider XmlDataProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Datenmeister pool being used to work on.
        /// </summary>
        /// <value>
        /// The pool.
        /// </value>
        [Inject(IsMandatory = true)]
        public DatenMeisterPool Pool
        {
            get;
            set;
        }

        public ExtentPoolLogic()
        {
        }

        /// <summary>
        /// Creates an empty Extent with a certain name and filename and attaches it to the datenmeisterpool.
        /// </summary>
        /// <param name='name'>
        /// Name of the pool to be added
        /// </param>
        /// <param name='url>
        /// Url of the element to be added
        /// </param>
        /// <param name='filename'>
        /// Name of the file, which shall be stored into the database
        /// </param>
        public ExtentInstance CreateEmpty(string name, string url, string filename, ExtentType extentType)
        {
            Ensure.That(this.Pool != null);

            // Checks, if filename is sanity. 
            if (Path.GetFileNameWithoutExtension(filename) != filename)
            {
                throw new ArgumentException("Filename contains directory information or file extension");
            }

            // Check, if filename contains some invalid characters. 
            // It can be seen as a security check
            if (Path.IsPathRooted(filename) || filename.Contains(Path.AltDirectorySeparatorChar.ToString())
                || filename.Contains(Path.DirectorySeparatorChar.ToString()) || filename.Contains("."))
            {
                throw new ArgumentException("Filename contains invalid characters");
            }

            // Ok, we got it
            var path = Path.Combine("data", filename);
            var extent = this.XmlDataProvider.CreateEmpty(path, url, name, extentType);

            return extent;
        }
    }
}

