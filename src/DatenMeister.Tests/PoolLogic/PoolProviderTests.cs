using System;
using System.Linq;
using NUnit.Framework;
using DatenMeister.DataProvider.Xml;
using System.IO;
using DatenMeister.Logic;
using DatenMeister.Pool;

namespace DatenMeister.Tests.PoolLogic
{
    [TestFixtureAttribute]
    public class PoolProviderTests
    {
        public static void PrepareDirectory()
        {
            if (!Directory.Exists("data"))
            {
                Directory.Delete("data", true);

            }

            Directory.CreateDirectory("data");
        }

        public PoolProviderTests()
        {
        }

        [Test]
        public void DoStoreAndLoad()
        {
            PrepareDirectory();

            var pool = new DatenMeisterPool();

            var xmlDataProvider = new XmlDataProvider();
            var extent1 = xmlDataProvider.CreateEmpty(
                "data/empty1.xml",
                "http://test",
                "MyName"
            );
            var extent2 = xmlDataProvider.CreateEmpty(
                "data/empty2.xml",
                "http://test2",
                "MyName2"
            );

            pool.Add(extent1);
            pool.Add(extent2);

            var poolProvider = new DatenMeisterPoolProvider();

            // Saving is now done
            poolProvider.Save(pool, "data/pools.xml");

            // Try to read
            var poolProviderLoad = new DatenMeisterPoolProvider();
            var loadPool = new DatenMeisterPool();
            poolProviderLoad.Load(loadPool, "data/pools.xml");

            var first = loadPool.Instances.Where(x => x.Name == "MyName").FirstOrDefault();
            var second = loadPool.Instances.Where(x => x.Name == "MyName2").FirstOrDefault();

            Assert.That(first, Is.Not.Null);
            Assert.That(second, Is.Not.Null);

            Assert.That(first.StoragePath == "data/empty1.xml");
            Assert.That(second.StoragePath == "data/empty2.xml");

            Assert.That(first.Extent, Is.Not.Null);
            Assert.That(second.Extent, Is.Not.Null);

            Assert.That(first.Extent, Is.TypeOf(typeof(XmlExtent)));
        }

        /// <summary>
        /// Test for issue #5
        /// </summary>
        [Test]
        public void RetrieveById()
        {
            PrepareDirectory();

            var pool = new DatenMeisterPool();

            var xmlDataProvider = new XmlDataProvider();
            var extent1 = xmlDataProvider.CreateEmpty(
                "data/empty1.xml",
                "http://test",
                "MyName"
            );

            var obj = extent1.Extent.CreateObject();
            obj.set("id", "id_test");

            pool.Add(extent1);

            var poolResolver = new PoolResolver(pool);
            var resolved = poolResolver.Resolve("http://test");
            Assert.That(resolved, Is.EqualTo(extent1.Extent));

            var resolved2 = poolResolver.Resolve("http://test#id_test");
            Assert.That(resolved2, Is.TypeOf<XmlObject>());
            Assert.That((resolved2 as XmlObject).Id, Is.EqualTo("id_test"));
        }

        [Test]
        public void TestAddTwoExtentsWithSameUrl()
        {
            PrepareDirectory();

            var pool = new DatenMeisterPool();

            var xmlDataProvider = new XmlDataProvider();
            var extent1 = xmlDataProvider.CreateEmpty(
                "data/empty1.xml",
                "http://test",
                "MyName"
            );

            PrepareDirectory();
            var extent2 = xmlDataProvider.CreateEmpty(
                "data/empty2.xml",
                "http://test",
                "MyName"
            );

            pool.Add(extent1);
            pool.Add(extent2);

            var extents = pool.Extents;
            Assert.That(extents.Count(), Is.EqualTo(1));
            Assert.That(extents.First(), Is.EqualTo(extent2.Extent));
        }
    }
}
