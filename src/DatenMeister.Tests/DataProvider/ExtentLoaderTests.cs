using DatenMeister.DataProvider;
using DatenMeister.DataProvider.CSV;
using DatenMeister.DataProvider.Generic;
using DatenMeister.Entities.AsObject.DM;
using DatenMeister.Pool;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.DataProvider
{
    [TestFixture]
    public class ExtentLoaderTests
    {
        [Test]
        public void TestLoader()
        {
            var extent = new GenericExtent("dm:///loader");
            var csvLoader = new GenericObject();

            CSVExtentLoadInfo.setExtentType(csvLoader, "DatenMeister.CSV");
            CSVExtentLoadInfo.setExtentUri(csvLoader, "dm:///csv");
            CSVExtentLoadInfo.setFilePath(csvLoader, "data/csv/withoutheader.txt");
            extent.Elements().add(csvLoader);

            var pool = DatenMeisterPool.CreateDecoupled();
            var loader = new ExtentLoader();
            loader.SyncExtents(extent.Elements(), pool);

            Assert.That(pool.ExtentContainer.Count(), Is.EqualTo(1));
            var csvExtent = pool.ExtentContainer.First().Extent as CSVExtent;
            Assert.That(csvExtent != null);

            CSVTests.TestContentOfCSVFileWithoutheader(csvExtent);
        }
    }
}
