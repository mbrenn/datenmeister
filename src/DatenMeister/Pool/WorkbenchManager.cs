using BurnSystems.Logging;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Entities.DM;
using DatenMeister.Logic;
using DatenMeister.Pool;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.Pool
{
    /// <summary>
    /// Manages the currently loaded workbench. It includes an abstraction layer for the IPool and automatically loads and disposes the objects.
    /// </summary>
    public class WorkbenchManager
    {
        /// <summary>
        /// Defines the logger to be used
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(WorkbenchManager));

        /// <summary>
        /// Stores the workbench instance
        /// </summary>
        public WorkbenchContainer WorkbenchContainer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the pool
        /// </summary>
        public IPool Pool
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the workbench from the injection
        /// </summary>
        /// <returns></returns>
        public static WorkbenchManager Get()
        {
            return Injection.Application.Get<WorkbenchManager>();
        }

        /// <summary>
        /// Initializes a new instance of the WorkbenchManager
        /// </summary>
        /// <param name="pool"></param>
        [Inject]
        public WorkbenchManager(WorkbenchContainer container, [Optional] IPool pool)
        {
            this.WorkbenchContainer = container;
            this.Pool = pool;
        }

        /// <summary>
        /// Creates a new and empty workbench
        /// </summary>
        public void CreateNewWorkbench()
        {
            this.Pool = DatenMeisterPool.Create();
            this.WorkbenchContainer.Workbench = new Workbench();
        }

        /// <summary>
        /// Adds an extent to the workbench. 
        /// The information will 
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="info"></param>
        public ExtentInfo AddExtent(IURIExtent extent, ExtentParam info)
        {
            var extentInfo = this.Pool.Add(extent, info.StoragePath, info.Name, info.ExtentType);
            extentInfo.isPrepopulated = info.IsPrepopulated;
            extentInfo.dataProviderSettings = info.DataProviderSettings;

            this.WorkbenchContainer.Workbench.instances.Add(extentInfo);
            return extentInfo;
        }

        /// <summary>
        /// Loads an extent or creates an empty one, if there is no extent to get loaded
        /// </summary>
        /// <param name="storagePath">The path, which is used to load the extent</param>
        /// <param name="uri">The URI being used to load the extent</param>
        /// <param name="info">Information for the extent</param>
        /// <param name="funcWhenNotLoaded">This function will be called, when the loading of the extent could not be done</param>
        /// <param name="actionWhenLoaded">This action will be executed, when the loading was successful</param>
        /// <returns>The created extent information</returns>
        public ExtentInfo LoadOrCreateExtent(string storagePath, string uri, ExtentParam info, Func<IURIExtent> funcWhenNotLoaded, Action<IURIExtent> actionWhenLoaded)
        {
            logger.Message("Loading" + info.Name);

            IURIExtent createdExtent = null;
            var xmlSettings = info.DataProviderSettings as XmlSettings;

            if (File.Exists(storagePath))
            {
                try
                {
                    // File exists, we can directly load it
                    var dataProvider = new XmlDataProvider();
                    createdExtent = dataProvider.Load(storagePath, uri, xmlSettings);

                    if (actionWhenLoaded != null)
                    {
                        actionWhenLoaded(createdExtent);
                    }
                }
                catch (Exception exc)
                {
                    logger.Fail("Failure during loading of " + info.Name + ": " + exc.Message);
                }
            }

            if (createdExtent == null)
            {
                // File does not exist, we have to load it from 
                if (funcWhenNotLoaded != null)
                {
                    createdExtent = funcWhenNotLoaded();
                }
            }

            // Everything is loaded or created, now add the extent
            return this.AddExtent(createdExtent, info);
        }

        /// <summary>
        /// Loads the workbench at the given path
        /// </summary>
        /// <param name="path">Path, where workbench will be loaded</param>
        public void LoadWorkbench(string path)
        {
        }

        /// <summary>
        /// Saves the current workbench and all associated extents.         
        /// </summary>
        /// <param name="path">Path, where workbench will be stored. If path is null, the last path will be used</param>
        public void SaveWorkbench(string path = null)
        {
            if (path == null)
            {
                path = this.WorkbenchContainer.Workbench.path;
            }

            // Checks, if the path is still null, then no real path is given.
            if (path == null)
            {
                throw new ArgumentException("path is null and now default path is given");
            }

            var dotNetObject = Injection.Application.Get<GlobalDotNetExtent>().CreateObject(this.WorkbenchContainer.Workbench);

            var xDocument = new XDocument (new XElement("workbench") );
            var xmlExtent = new XmlExtent(xDocument, "datenmeister:///application/workbench");
            var xmlObject = new XmlObject(xmlExtent, xDocument.Root);
            var objCopier = new ObjectCopier(xmlExtent);
            objCopier.CopyElement(dotNetObject, xmlObject);

            // Stores the object into the file
            var provider = new XmlDataProvider();
            provider.Save(xmlExtent, path, XmlSettings.Empty);
        }
    }
}
