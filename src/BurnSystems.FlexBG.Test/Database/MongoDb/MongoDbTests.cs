using BurnSystems.FlexBG.Modules.Database.MongoDb;
using BurnSystems.ObjectActivation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Test.Database.MongoDb
{
    [TestFixture]
    public class MongoDbTests
    {
        public static IActivates Init()
        {
            var activationContainer = new ActivationContainer("Test");
            var settings = new MongoDbSettings()
            {
                ConnectionString = "mongodb://192.168.1.126/",
                Database = "test"
            };

            activationContainer.Bind<MongoDbSettings>().ToConstant ( settings);
            activationContainer.Bind<MongoDbConnection>().To<MongoDbConnection>();

            return activationContainer;
        }

        [Test]
        public void TestDatabaseRetrieval()
        {
            var container = Init();
            var dbConnection = container.Get<MongoDbConnection>();

            Assert.That(dbConnection, Is.Not.Null);

            var database = dbConnection.Database;
            Assert.That(database, Is.Not.Null);

            var stats = database.GetStats();
            Assert.That(stats, Is.Not.Null);
        }
    }
}
