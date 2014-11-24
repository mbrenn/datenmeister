using BurnSystems.Logging;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.DataProvider.Wrapper.EventOnChange;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic.MethodProvider;
using DatenMeister.Logic.TypeResolver;
using DatenMeister.Pool;
using Ninject;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Stores and manages the data for a user-driven application, which handles the 
    /// extents and the workbench
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
        /// This event is thrown, when the viewset is finalized
        /// </summary>
        public event EventHandler ViewSetFinalized;

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
                OnlyUseAssignedNodes = true
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

            // Initializes the Method provider
            Injection.Application.Bind<IMethodProvider>().To<SimpleMethodProvider>().InSingletonScope();

            // Initializes the workbench
            Injection.Application.Bind<WorkbenchManager>().To<WorkbenchManager>();
            Injection.Application.Bind<WorkbenchContainer>().To<WorkbenchContainer>().InSingletonScope();

            if (!onlyBootStrap)
            {
                // Initializes the global dot net extent
                var globalDotNetExtent = new GlobalDotNetExtent();
                Injection.Application.Bind<GlobalDotNetExtent>().ToConstant(globalDotNetExtent);
                DatenMeister.Entities.AsObject.FieldInfo.Types.AssignTypeMapping(globalDotNetExtent);
                DatenMeister.Entities.AsObject.DM.Types.AssignTypeMapping(globalDotNetExtent);
            }
        }

        public void PerformInitializationOfViewSet()
        {            
            PerformBinding();

            // Creates the workbench
            var workBenchManager = WorkbenchManager.Get();
            workBenchManager.CreateNewWorkbench();

            // Initializes the database itself and adds the metatype to the workbench
            this.MetaTypeExtent.ReleaseFromPool();
            workBenchManager.AddExtent(this.MetaTypeExtent,
                new ExtentParam("MetaTypes", ExtentType.MetaType)
                {
                    IsPrepopulated = true
                });

            // Adds the extent for the extents
            var poolExtent = new DatenMeisterPoolExtent(workBenchManager.Pool as DatenMeisterPool);
            workBenchManager.AddExtent(
                poolExtent, 
                new ExtentParam(DatenMeisterPoolExtent.DefaultName, ExtentType.Extents)
                    .AsPrepopulated());
            // Loads the application data from file
            this.LoadApplicationDataIfNotLoaded();

            // Call the private settings that the viewset needs to be initialized
            this.privateSettings.InitializeViewSet(this);

            // After the viewset is initialized, replace the view extents by wrapped
            // EventOnChange Extent. 
            // So, view can be updated, when the content of the extent changed
            foreach (var instance in workBenchManager.Pool.ExtentContainer.Where(x => x.Info.extentType == ExtentType.View))
            {
                // Just replace the extent
                instance.Extent = new EventOnChangeExtent(instance.Extent);
            }

            this.AddDefaultQueries();

            this.OnViewSetInitialized();
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

        /// <summary>
        /// Calls the ViewSetInitialized event
        /// </summary>
        private void OnViewSetFinalized()
        {
            var ev = this.ViewSetFinalized;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }

        public void PerformInitializeFromScratch()
        {
            this.privateSettings.FinalizeExtents(this, false);

            // Now, call the event that the initialization has been redone
            this.OnViewSetFinalized();
        }

        public void PerformInitializeAfterLoading()
        {
            this.privateSettings.FinalizeExtents(this, true);

            // Now, call the event that the initialization has been redone
            this.OnViewSetFinalized();
        }

        public void PerformInitializeExampleData()
        {
            this.privateSettings.InitializeForExampleData(this);
        }

        /// <summary>
        /// Calls the StoreViewSet method in the settings
        /// </summary>
        public void StoreWorkbench(string path)
        {
            // Saves the complete workbench at the given path
            WorkbenchManager.Get().SaveWorkbench(path);

            // and afterwards store the viewset
            //this.privateSettings.StoreViewSet(this);
        }

        /// <summary>
        /// Loads the workbench by giving a path
        /// </summary>
        /// <param name="path">Path to be loaded</param>
        public void LoadWorkbench(string path)
        {
            this.PerformInitializationOfViewSet();
            WorkbenchManager.Get().LoadWorkbench(path);

            this.privateSettings.FinalizeExtents(this, true);
        }

        /// <summary>
        /// Saves the complete application setting
        /// </summary>
        public void Stop()
        {
            // Stores the settings
            this.SaveApplicationData();
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
            var instance = pool.GetContainer(extentUri);
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
                instance.Info.storagePath);
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
        /// Loads the application data, if the data is not already loaded
        /// </summary>
        public void LoadApplicationDataIfNotLoaded()
        {
            // Stores the name of the applicationdata
            var name = "applicationdata";
            if (!this.isApplicationDataLoaded)
            {
                var filePath = this.GetApplicationStoragePathFor(name);
                var workBenchManager = WorkbenchManager.Get();
                var info = workBenchManager.LoadOrCreateExtent(
                     filePath,
                     ApplicationDataUri,
                     new ExtentParam(name, ExtentType.ApplicationData, filePath)
                     {
                         IsPrepopulated = true
                     },
                     () => { return XmlExtent.Create(XmlSettings.Empty, name, ApplicationDataUri); },
                     null);
                info.isPrepopulated = true;
                this.applicationData = workBenchManager.Pool.GetExtent(info);

                this.isApplicationDataLoaded = true;
            }
            else
            {
                var pool = PoolResolver.GetDefaultPool();
                this.applicationData.ReleaseFromPool();

                // Adds the application data to the workbench
                var info = WorkbenchManager.Get().AddExtent(
                    this.applicationData,
                    new ExtentParam(
                        name,
                        ExtentType.ApplicationData,
                        this.GetApplicationStoragePathFor(name)));
                info.isPrepopulated = true;
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
