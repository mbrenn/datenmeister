using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Transformations;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.WPF.Helper
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
        /// Gets the storage path for the application date
        /// </summary>
        public string StoragePath
        {
            get
            {
                Ensure.That(this.Settings != null, "this.Settings = null");
                Ensure.That(!string.IsNullOrEmpty(this.Settings.ApplicationName), "No application name is given");

                // Gets and creates the directory
                var directoryPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Depon.Net/DatenMeister/AppData");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Gets the filename
                var filename = this.Settings.ApplicationName + ".xml";
                var filePath = Path.Combine(directoryPath, filename);
                return filePath;
            }
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
        /// Saves the application data
        /// </summary>
        public void LoadApplicationData()
        {
            var filePath = this.StoragePath;
            if (File.Exists(filePath))
            {
                try
                {
                    // File exists, we can directly load it
                    var dataProvider = new XmlDataProvider();

                    this.applicationData = dataProvider.Load(filePath, ApplicationDataUri, this.XmlSettings);
                }
                catch (Exception exc)
                {
                    logger.Fail("Failure during loading of ApplicatonSettings" + exc.Message);
                }
            }

            if (this.applicationData == null)
            {
                // File does not exist, we have to load it from 
                this.applicationData = XmlExtent.Create(this.XmlSettings, "applicationdata", ApplicationDataUri);
            }

            var pool = Global.Application.Get<IPool>();
            pool.Add(this.applicationData, filePath, "Application data");
        }

        /// <summary>
        /// Saves the application data
        /// </summary>
        public void SaveApplicationData()
        {
            // Save the data
            var dataProvider = new XmlDataProvider();

            var filePath = this.StoragePath;
            dataProvider.Save(this.applicationData as XmlExtent, filePath, this.XmlSettings);
        }

        /// <summary>
        /// Stores the application data into the file
        /// </summary>
        public void Dispose()
        {
            this.SaveApplicationData();
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Creates the window for the DatenMeister. 
        /// </summary>
        /// <param name="core">Application of the window</param>
        /// <returns>Returned window</returns>
        public IDatenMeisterWindow CreateWindow()
        {
            var wnd = new DatenMeisterWindow(this);

            // Just sets the title and shows the Window
            wnd.Show();
            wnd.RefreshTabs();

            return wnd;
        }
    }
}
