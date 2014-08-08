using System;
using System.Linq;
using NUnit.Framework;
using DatenMeister.DataProvider.Xml;
using System.IO;
using DatenMeister.Logic;
using DatenMeister.Pool;
using DatenMeister.DataProvider;

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

            var pool = DatenMeisterPool.Create();

            var xmlDataProvider = new XmlDataProvider();
            var extent1 = xmlDataProvider.CreateEmpty(
                "data/empty1.xml",
                "http://test",
                "MyName",
                 ExtentType.Data
            );
            var extent2 = xmlDataProvider.CreateEmpty(
                "data/empty2.xml",
                "http://test2",
                "MyName2",
                 ExtentType.Data
            );

            pool.Add(extent1);
            pool.Add(extent2);

            var poolProvider = new DatenMeisterPoolProvider();

            // Saving is now done
            poolProvider.Save(pool, "data/pools.xml");

            // Try to read
            var poolProviderLoad = new DatenMeisterPoolProvider();
            var loadPool = DatenMeisterPool.Create();
            poolProviderLoad.Load(loadPool, "data/pools.xml", ExtentType.Extents);

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

            var pool = DatenMeisterPool.Create();

            var xmlDataProvider = new XmlDataProvider();
            var extent1 = xmlDataProvider.CreateEmpty(
                "data/empty1.xml",
                "http://test",
                "MyName",
                 ExtentType.Data
            );

            var obj = Factory.GetFor(extent1.Extent).CreateInExtent(extent1.Extent);
            obj.set("id", "id_test");

            pool.Add(extent1);

            var poolResolver = PoolResolver.GetDefault(pool);
            var resolved = poolResolver.Resolve("http://test");
            Assert.That(resolved, Is.EqualTo(extent1.Extent));

            var resolved2 = poolResolver.Resolve("http://test#id_test");
            Assert.That(resolved2, Is.TypeOf<XmlObject>());
            Assert.That((resolved2 as XmlObject).Id, Is.EqualTo("id_test"));
        }

        /// <summary>
        /// Test for issue #5
        /// </summary>
        [Test]
        public void CreateResolvePath()
        {
            PrepareDirectory();

            var pool = DatenMeisterPool.Create();

            var xmlDataProvider = new XmlDataProvider();
            var extent1 = xmlDataProvider.CreateEmpty(
                "data/empty1.xml",
                "http://test",
                "MyName",
                 ExtentType.Data
            );

            var obj = Factory.GetFor(extent1.Extent).CreateInExtent(extent1.Extent);
            obj.set("id", "id_test");

            pool.Add(extent1);

            var poolResolver = PoolResolver.GetDefault(pool);
            var path1 = poolResolver.GetResolvePath(obj);
            Assert.That(path1, Is.Not.Empty);
            Assert.That(path1, Is.Not.Null);

            var resolved2 = poolResolver.Resolve("http://test#id_test") as IObject;
            Assert.That(resolved2, Is.Not.Null);
            var path2 = poolResolver.GetResolvePath(resolved2);

            Assert.That(path1, Is.EqualTo(path2));
        }

        [Test]
        public void TestAddTwoExtentsWithSameUrl()
        {
            PrepareDirectory();

            var pool = DatenMeisterPool.Create();

            var xmlDataProvider = new XmlDataProvider();
            var extent1 = xmlDataProvider.CreateEmpty(
                "data/empty1.xml",
                "http://test",
                "MyName",
                 ExtentType.Data
            );

            var extent2 = xmlDataProvider.CreateEmpty(
                "data/empty2.xml",
                "http://test",
                "MyName",
                 ExtentType.Data
            );

            var extent3 = xmlDataProvider.CreateEmpty(
                "data/empty3.xml",
                "http://test",
                "MyName",
                ExtentType.Data
            );


            var extent4 = xmlDataProvider.CreateEmpty(
                "data/empty4.xml",
                "http://test",
                "MyName",
                 ExtentType.Data
            );

            var countBefore = pool.Extents.Count();
            pool.Add(extent1);
            pool.Add(extent2);

            // API Method 1
            var extents = pool.Extents;
            Assert.That(extents.Count(), Is.EqualTo(countBefore + 1));
            Assert.That(extents.Any(x => x == extent2.Extent), Is.True);

            pool.Add(extent3.Extent, null, ExtentType.Data);

            // API Method 2
            extents = pool.Extents;
            Assert.That(extents.Count(), Is.EqualTo(countBefore + 1));
            Assert.That(extents.Any(x => x == extent3.Extent), Is.True);

            pool.Add(extent4.Extent, null, "Name", ExtentType.Data);

            // API Method 3
            extents = pool.Extents;
            Assert.That(extents.Count(), Is.EqualTo(countBefore + 1));
            Assert.That(extents.Any(x => x == extent4.Extent), Is.True);
        }
    }
}
