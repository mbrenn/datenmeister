using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.Database.MongoDb
{
    /// <summary>
    /// Defines the connection string for mongo db
    /// </summary>
    public class MongoDbSettings
    {
        /// <summary>
        /// Defines the connection string
        /// </summary>
        public string ConnectionString
        {
            get;
            set;
        }

        public string Database
        {
            get;
            set;
        }
    }
}
