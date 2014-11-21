using DatenMeister.Entities.DM;
using DatenMeister.Pool;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Manages the currently loaded workbench. It includes an abstraction layer for the IPool and automatically loads and disposes the objects
    /// </summary>
    public class WorkbenchManager
    {
        /// <summary>
        /// Stores the workbench instance
        /// </summary>
        private Workbench workbench;

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
        public static Workbench Get()
        {
            return Injection.Application.Get<Workbench>();
        }

        /// <summary>
        /// Initializes a new instance of the WorkbenchManager
        /// </summary>
        /// <param name="pool"></param>
        [Inject]
        public WorkbenchManager([Optional] IPool pool)
        {
            this.Pool = pool;
        }

        /// <summary>
        /// Creates a new and empty workbench
        /// </summary>
        public void CreateNewWorkbench()
        {
            this.Pool = DatenMeisterPool.Create();
        }

        public void AddExtent(IURIExtent extent, ExtentType type)
        {
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
        }
    }
}
