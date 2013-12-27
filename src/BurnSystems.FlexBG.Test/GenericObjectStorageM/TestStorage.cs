using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Modules.GenericObjectStorageM;
using BurnSystems.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Test.GenericObjectStorageM
{
    [TestFixture]
    public class TestStorage
    {
        public static void Init()
        {
            Log.TheLog.Reset();
            Log.TheLog.AddLogProvider(new DebugProvider());
            Log.TheLog.FilterLevel = LogLevel.Verbose;

            SerializedFile.ClearCompleteDataDirectory();
        }

        [Test]
        public void TestAdding()
        {
            Init();

            var storage = new GenericObjectStorage();
            storage.Start();
            storage.Set<Data>(new Data("Test"));

            var data = storage.Get<Data>();
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Value, Is.EqualTo("Test"));

            storage.Shutdown();
        }

        [Test]
        public void TestAddingWithPath()
        {
            Init();

            var storage = new GenericObjectStorage();
            storage.Start();

            storage.Set<Data>("/", new Data("Test"));
            storage.Set<Data>("/a", new Data("Test-A"));
            storage.Set<Data>("/b", new Data("Test-B"));

            var data = storage.Get<Data>("/");
            Assert.That(data.Value, Is.EqualTo("Test"));
            var data1 = storage.Get<Data>("/a");
            Assert.That(data1.Value, Is.EqualTo("Test-A"));
            var data2 = storage.Get<Data>("/b");
            Assert.That(data2.Value, Is.EqualTo("Test-B"));
            var data3 = storage.Get<Data>("/c");
            Assert.That(data3, Is.Null);

            storage.Shutdown();
        }

        [Test]
        public void TestAddingWithType()
        {
            Init();

            var storage = new GenericObjectStorage();
            storage.Start();

            storage.Set<Data>("/", new Data("Test"));
            storage.Set<OtherData>("/", new OtherData("OtherTest"));

            var data = storage.Get<Data>("/");
            Assert.That(data.Value, Is.EqualTo("Test"));
            var data1 = storage.Get<OtherData>("/");
            Assert.That(data1.Value, Is.EqualTo("OtherTest"));

            storage.Shutdown();
        }

        [Test]
        public void TestAddingWithTypeAndPersistant()
        {
            Init();
            
            var storage = new GenericObjectStorage();
            storage.Start();

            storage.Set<Data>("/", new Data("Test"));
            storage.Set<OtherData>("/", new OtherData("OtherTest"));

            var data = storage.Get<Data>("/");
            Assert.That(data.Value, Is.EqualTo("Test"));
            var data1 = storage.Get<OtherData>("/");
            Assert.That(data1.Value, Is.EqualTo("OtherTest"));

            storage.Shutdown();

            var storage2 = new GenericObjectStorage();
            storage2.Start();

            var data3 = storage2.Get<Data>("/");
            Assert.That(data3.Value, Is.EqualTo("Test"));
            var data4 = storage2.Get<OtherData>("/");
            Assert.That(data4.Value, Is.EqualTo("OtherTest"));
            storage.Shutdown();
        }

        [Serializable]
        public class Data
        {
            public string Value
            {
                get;
                set;
            }

            public Data(string value)
            {
                this.Value = value;
            }
        }

        [Serializable]
        public class OtherData
        {
            public string Value
            {
                get;
                set;
            }

            public OtherData(string value)
            {
                this.Value = value;
            }
        }
    }
}
