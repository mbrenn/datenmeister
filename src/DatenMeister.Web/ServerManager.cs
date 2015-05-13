using DatenMeister.DataProvider.CSV;
using DatenMeister.DataProvider.Generic;
using DatenMeister.Logic;
using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DatenMeister.Web
{
    /// <summary>
    /// Stores the server manager
    /// </summary>
    public class ServerManager : IServerManager
    {
        /// <summary>
        /// Stores the uri of usermanagement
        /// </summary>
        public const string UriUserManagement = "dm:///usermanagement";

        /// <summary>
        /// Stores the server pool
        /// </summary>
        private DatenMeisterPool serverPool;

        /// <summary>
        /// Stores the server pool
        /// </summary>
        private DatenMeisterPool dataPool;

        public void Init()
        {
            this.serverPool = DatenMeisterPool.CreateDecoupled();

            // Load the users
            var provider = new CSVDataProvider();
            var userPath =  Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data/users.txt");
            var loadedCSVExtent = provider.Load(
                UriUserManagement,
                userPath,
                new CSVSettings()
                {
                    HasHeader = true,
                    Separator = ","
                });

            this.serverPool.Add(loadedCSVExtent, null, ExtentType.Data);

            // Creates the datapool with UML information
            this.dataPool = DatenMeisterPool.CreateDecoupled();
            var metaTypeExtent = new GenericExtent("datenmeister:///datenmeister/metatypes/");
            DatenMeister.Entities.AsObject.Uml.Types.Init(metaTypeExtent, new GenericFactory(metaTypeExtent));
            this.dataPool.Add(metaTypeExtent, null, ExtentType.MetaType);
        }

        /// <summary>
        /// Gets the server pool
        /// </summary>
        /// <returns>Gets the pool being used by a server</returns>
        public IPool GetServerPool()
        {
            return this.serverPool;
        }

        /// <summary>
        /// Gets the pool, reflecting the data itself
        /// </summary>
        /// <returns>The pool being used for data</returns>
        public IPool GetDataPool()
        {
            return this.dataPool;
        }
    }
}
