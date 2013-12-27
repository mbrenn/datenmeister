using BurnSystems.ObjectActivation;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.Database.MongoDb
{
    /// <summary>
    /// Defines the database connection
    /// </summary>
    public class MongoDbConnection
    {
        /// <summary>
        /// Stores the client
        /// </summary>
        private MongoClient client;

        /// <summary>
        /// Stores the database
        /// </summary>
        private MongoDatabase database;

        [Inject(IsMandatory = true)]
        public MongoDbSettings Settings
        {
            get;
            set;
        }
        
        /// <summary>
        /// Retrieves the client
        /// </summary>
        public MongoClient Client
        {
            get
            {
                if (this.client == null)
                {
                    this.client = new MongoClient(this.Settings.ConnectionString);
                }

                return this.client;
            }
        }

        public MongoDatabase Database
        {
            get
            {
                if (this.database == null)
                {
                    var client = this.Client;
                    var server = client.GetServer();
                    this.database = server.GetDatabase(this.Settings.Database);
                }

                return this.database;
            }
        }

    }
}
