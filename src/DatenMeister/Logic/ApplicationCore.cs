using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Transformations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Stores the data that is used for the application specific data
    /// </summary>
    public class ApplicationCore : IDisposable
    {
        /// <summary>
        /// Stores the logger 
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(ApplicationCore));

        /// <summary>
        /// Defines the uri for the application core
        /// </summary>
        public const string ApplicationDataUri = "datenmeister:///applications";

        /// <summary>
        /// Stores the application data in an extent
        /// </summary>
        private IURIExtent applicationData;

        public IURIExtent ApplicationData
        {
            get { return this.applicationData; }
        }

        /// <summary>
        /// Gets the storage path for a certain type of data
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetApplicationStoragePathFor(string type)
        {
            Ensure.That(this.Settings != null, "this.Settings = null");
            Ensure.That(!string.IsNullOrEmpty(this.Settings.ApplicationName), "No application name is given");

            // Gets and creates the directory
            var directoryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                string.Format(
                    "Depon.Net/{0}",
                    this.Settings.ApplicationName));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Gets the filename
            var filename = type + ".xml";
            var filePath = Path.Combine(directoryPath, filename);
            return filePath;
        }

        /// <summary>
        /// Gets or sets the name of the project
        /// </summary>
        public IDatenMeisterSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or serts the xml settings
        /// </summary>
        public XmlSettings XmlSettings
        {
            get;
            private set;
        }

        /// <summary>
        /// Defines the object that is used to show 
        /// </summary>
        public IObject ViewRecentObjects
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the project
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        public ApplicationCore(IDatenMeisterSettings settings)
        {
            this.Settings = settings;
            this.XmlSettings = new XmlSettings();
            this.XmlSettings.SkipRootNode = true;
            this.XmlSettings.Mapping.Add(DatenMeister.Entities.AsObject.DM.Types.RecentProject);

            this.LoadApplicationData();
        }

        /// <summary>
        /// Tries to load an extent from the given file. 
        /// The path of the file will be retrieved by GetStoragePathFor, which is within
        /// the LocalAppData of the user
        /// </summary>
        /// <param name="name">Name of the new extent</param>
        /// <param name="fileName">Used filename to load the extent</param>
        /// <param name="extentUri">Uri of the extent to be used</param>
        /// <param name="defaultAction">Action being called, when file is not existing.
        /// The action can be used to precreate the necessary nodes or to perform the mapping</param>
        /// <returns>Created or loaded Extent</returns>
        public IURIExtent LoadOrCreateByDefault(string name, string fileName, string extentUri, ExtentType extentType, Action<XmlExtent> defaultAction)
        {
            var filePath = this.GetApplicationStoragePathFor(name);
            IURIExtent createdPool = null;
            if (File.Exists(filePath))
            {
                try
                {
                    // File exists, we can directly load it
                    var dataProvider = new XmlDataProvider();

                    createdPool = dataProvider.Load(filePath, extentUri, this.XmlSettings);
                }
                catch (Exception exc)
                {
                    logger.Fail("Failure during loading of " + name + ": " + exc.Message);
                }
            }

            if (createdPool == null)
            {
                // File does not exist, we have to load it from 
                createdPool = XmlExtent.Create(this.XmlSettings, name, extentUri);
                if (defaultAction != null)
                {
                    defaultAction(createdPool as XmlExtent);
                }
            }

            var pool = Global.Application.Get<IPool>();
            pool.Add(createdPool, filePath, name, extentType);

            return createdPool;
        }

        /// <summary>
        /// Saves the extent as an xml file
        /// </summary>
        /// <param name="extentUri">Uri of the extent to be saved</param>
        public void SaveExtentByUri(string extentUri)
        {
            // Get pool entry            
            var pool = Global.Application.Get<IPool>();
            var instance = pool.GetInstance(extentUri);
            Ensure.That(instance != null, "The extent with Uri has not been found: " + extentUri);

            // Save the data
            var dataProvider = new XmlDataProvider();
            var extent = instance.Extent;
            if (!(extent is XmlExtent))
            {
                // Extent is not an XmlExtent... we need to copy all the stuff
                var xmlExtent = new XmlExtent(new XDocument(new XElement("root")), extentUri);
                ExtentCopier.Copy(extent, xmlExtent);

                extent = xmlExtent;
            }

            Ensure.That(extent is XmlExtent, "The given extent is not an XmlExtent");
            dataProvider.Save(extent as XmlExtent, instance.StoragePath, this.XmlSettings);
        }

        #region Storing and loading of application data

        /// <summary>
        /// Saves the application data
        /// </summary>
        public void LoadApplicationData()
        {
            this.applicationData = this.LoadOrCreateByDefault(
                "applicationdata",
                "Application Data",
                ApplicationDataUri,
                ExtentType.ApplicationData,
                null);
        }

        /// <summary>
        /// Saves the application data
        /// </summary>
        public void SaveApplicationData()
        {
            this.SaveExtentByUri(ApplicationDataUri);
        }

        #endregion

        /// <summary>
        /// Stores the application data into the file
        /// </summary>
        public void Dispose()
        {
            this.SaveApplicationData();
            GC.SuppressFinalize(this);
        }
    }
}
