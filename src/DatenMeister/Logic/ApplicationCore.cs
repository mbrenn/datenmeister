using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.DataProvider.Wrapper.EventOnChange;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic.TypeResolver;
using DatenMeister.Pool;
using DatenMeister.Transformations;
using Ninject;
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
    public partial class ApplicationCore
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

        /// <summary>
        /// Gets the application data
        /// </summary>
        public IURIExtent ApplicationData
        {
            get { return this.applicationData; }
        }

        /// <summary>
        /// Stores the settings in private scope
        /// </summary>
        private IDatenMeisterSettings privateSettings;

        /// <summary>
        /// Gets or sets the name of the project
        /// </summary>
        public IPublicDatenMeisterSettings Settings
        {
            get { return this.privateSettings; }
        }

        /// <summary>
        /// Gets or serts the xml settings
        /// </summary>
        private XmlSettings XmlSettings
        {
            get;
            set;
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
        /// Gets or sets the extent containing all the meta types
        /// </summary>
        public GenericExtent MetaTypeExtent
        {
            get;
            private set;
        }

        /// <summary>
        /// This event is thrown, when the viewset is initialized
        /// </summary>
        public event EventHandler ViewSetInitialized;

        /// <summary>
        /// Stores the value whether the application data is loaded
        /// </summary>
        private bool isApplicationDataLoaded = false;

        /// <summary>
        /// Initializes a new instance of the project
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        public ApplicationCore()
        {
            this.XmlSettings = new XmlSettings()
            {
                SkipRootNode = true
            };
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
        /// Starts the application. The created settings are afterwards available
        /// at this.Settings.
        /// </summary>
        /// <typeparam name="T">Type of the window</typeparam>
        public void Start<T>() where T : IDatenMeisterSettings, new()
        {
            PerformBinding(true /*Only Bootstrap binding*/);

            // Initialization of all meta types
            this.privateSettings = new T();
            Injection.Application.Bind<IPublicDatenMeisterSettings>().ToConstant(this.privateSettings);
            Injection.Application.Bind<IDatenMeisterSettings>().ToConstant(this.privateSettings);

            // Initializes the metatypes
            this.MetaTypeExtent = new GenericExtent("datenmeister:///datenmeister/metatypes/");
            DatenMeister.Entities.AsObject.Uml.Types.Init(this.MetaTypeExtent);
            DatenMeister.Entities.AsObject.FieldInfo.Types.Init(this.MetaTypeExtent);
            DatenMeister.Entities.AsObject.DM.Types.Init(this.MetaTypeExtent);
            PerformBinding();

            this.privateSettings.InitializeForBootUp(this);
            this.PerformInitializationOfViewSet();
        }

        /// <summary>
        /// Resets and performs the necessary binding
        /// </summary>
        public static void PerformBinding(bool onlyBootStrap = false)
        {
            // At the moment, reset the complete Binding
            Injection.Reset();

            // Initializes the default factory provider
            Injection.Application.Bind<IFactoryProvider>().To<FactoryProvider>();

            // Initializes the default resolver
            Injection.Application.Bind<IPoolResolver>().To<PoolResolver>();

            // Initializes the default type resolver
            Injection.Application.Bind<ITypeResolver>().To<TypeResolverImpl>().InSingletonScope();

            if (!onlyBootStrap)
            {
                // Initializes the global dot net extent
                var globalDotNetExtent = new GlobalDotNetExtent();
                Injection.Application.Bind<GlobalDotNetExtent>().ToConstant(globalDotNetExtent);
                DatenMeister.Entities.AsObject.FieldInfo.Types.AssignTypeMapping(globalDotNetExtent);
            }
        }

        public void PerformInitializationOfViewSet()
        {
            PerformBinding();
            var pool = DatenMeisterPool.Create();

            // Initializes the database itself
            this.MetaTypeExtent.ReleaseFromPool();

            // Adds the metatypes
            pool.Add(this.MetaTypeExtent, null, "MetaTypes", ExtentType.MetaType);

            this.LoadApplicationData();

            // Call the private settings that the viewset needs to be initialized
            this.privateSettings.InitializeViewSet(this);

            // After the viewset is initialized, replace the view extents by wrapped
            // EventOnChange Extent. 
            // So, view can be updated, when the content of the extent changed
            foreach (var instance in pool.Instances.Where(x => x.ExtentType == ExtentType.View))
            {
                // Just replace the extent
                instance.Extent = new EventOnChangeExtent(instance.Extent);
            }

            // Now, call the event that the initialization has been redone
            this.OnViewSetInitialized();

            this.AddDefaultViews();
        }

        /// <summary>
        /// Calls the ViewSetInitialized event
        /// </summary>
        private void OnViewSetInitialized()
        {
            var ev = this.ViewSetInitialized;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }

        public void PerformInitializeFromScratch()
        {
            this.privateSettings.InitializeFromScratch(this);
        }

        public void PerformInitializeAfterLoading()
        {
            this.privateSettings.InitializeAfterLoading(this);
        }

        public void PerformInitializeExampleData()
        {
            this.privateSettings.InitializeForExampleData(this);
        }

        /// <summary>
        /// Calls the StoreViewSet method in the settings
        /// </summary>
        public void StoreViewSet()
        {
            // Stores the settings
            this.SaveApplicationData();

            // and afterwards store the viewset
            this.privateSettings.StoreViewSet(this);
        }

        /// <summary>
        /// Tries to load an extent from the given file. 
        /// The path of the file will be retrieved by GetStoragePathFor, which is within
        /// the LocalAppData of the user
        /// </summary>
        /// <param name="name">Name of the new extent</param>
        /// <param name="fileName">Used filename to load the extent</param>
        /// <param name="extentUri">Uri of the extent to be used</param>
        /// <param name="defaultActionForCreation">Action being called, when file is not existing.
        /// The action can be used to precreate the necessary nodes or to perform the mapping</param>
        /// <param name="defaultActionForLoading">The action that will be performed
        /// after the </param>
        /// <returns>Created or loaded Extent</returns>
        public IURIExtent LoadOrCreateByDefault(
            string name,
            string extentUri,
            ExtentType extentType,
            Action<XmlExtent> defaultActionForCreation,
            Action<XmlExtent> defaultActionForLoading = null)
        {
            logger.Message("Loading" + name);
            var filePath = this.GetApplicationStoragePathFor(name);
            IURIExtent createdPool = null;
            var xmlSettings = this.GetXmlSettings(extentType);

            if (File.Exists(filePath))
            {
                try
                {
                    // File exists, we can directly load it
                    var dataProvider = new XmlDataProvider();
                    createdPool = dataProvider.Load(filePath, extentUri, xmlSettings);

                    if (defaultActionForLoading != null)
                    {
                        defaultActionForLoading(createdPool as XmlExtent);
                    }
                }
                catch (Exception exc)
                {
                    logger.Fail("Failure during loading of " + name + ": " + exc.Message);
                }
            }

            if (createdPool == null)
            {
                // File does not exist, we have to load it from 
                createdPool = XmlExtent.Create(xmlSettings, name, extentUri);
                if (defaultActionForCreation != null)
                {
                    defaultActionForCreation(createdPool as XmlExtent);
                }
            }

            var pool = PoolResolver.GetDefaultPool();
            pool.Add(createdPool, filePath, name, extentType);

            return createdPool;
        }

        /// <summary>
        /// Saves the extent as an xml file
        /// </summary>
        /// <param name="extentUri">Uri of the extent to be saved</param>
        public void SaveExtentByUri(string extentUri)
        {
            logger.Message("Saving: " + extentUri);

            // Get pool entry
            var pool = Injection.Application.Get<IPool>();
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
            dataProvider.Save(
                extent as XmlExtent,
                instance.StoragePath,
                this.GetXmlSettings(instance.ExtentType));
        }

        public XmlSettings GetXmlSettings(ExtentType type)
        {
            if (type == ExtentType.Data)
            {
                return this.XmlSettings;
            }

            return XmlSettings.Empty;
        }

        #region Storing and loading of application data

        /// <summary>
        /// Saves the application data
        /// </summary>
        public void LoadApplicationData()
        {
            // Stores the name of the applicationdata
            var name = "applicationdata";
            if (!this.isApplicationDataLoaded)
            {
                this.applicationData = this.LoadOrCreateByDefault(
                    name,
                    ApplicationDataUri,
                    ExtentType.ApplicationData,
                    null);
                this.isApplicationDataLoaded = true;
            }
            else
            {
                var pool = PoolResolver.GetDefaultPool();
                this.applicationData.ReleaseFromPool();
                pool.Add(
                    this.applicationData,
                    this.GetApplicationStoragePathFor(name),
                    name,
                    ExtentType.ApplicationData);
            }
        }

        /// <summary>
        /// Saves the application data
        /// </summary>
        public void SaveApplicationData()
        {
            this.SaveExtentByUri(ApplicationDataUri);
        }

        #endregion
    }
}
