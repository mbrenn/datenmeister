using DatenMeister.DataProvider;
using DatenMeister.DataProvider.CSV;
using DatenMeister.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.DataProvider
{
    [TestFixture]
    public class CSVTests
    {
        [Test]
        public void TestImportingWithoutHeader()
        {
            var settings = new CSVSettings()
            {
                HasHeader = false,
                Separator = ","
            };

            var provider = new CSVDataProvider();
            var extent = provider.Load("data/csv/withoutheader.txt", settings);
            Assert.That(extent, Is.Not.Null);
            Assert.That(extent.HeaderNames.Count, Is.EqualTo(0));

            var elements = extent.Elements().Select(x => x as IObject).ToList();
            Assert.That(elements.Count, Is.EqualTo(4));

            Assert.That(elements[0].get("Column 0").ToString(), Is.EqualTo("1"));
            Assert.That(elements[0].get("Column 1").ToString(), Is.EqualTo("2"));
            Assert.That(elements[1].get("Column 2").ToString(), Is.EqualTo("three"));
            Assert.That(elements[2].get("Column 2").ToString(), Is.EqualTo("example3"));

            Assert.That(elements[0].isSet("Column 0"), Is.True);
            Assert.That(elements[0].isSet("Column 5"), Is.False);
            Assert.That(elements[0].isSet("Unknown"), Is.False);

            var column1 = elements[0].getAll().Where(x => x.PropertyName == "Column 0").FirstOrDefault();
            Assert.That(column1 != null);
            Assert.That(column1.Value.ToString() == "1");

            var column2 = elements[3].getAll().Where(x => x.PropertyName == "Column 0").FirstOrDefault();
            Assert.That(column2 != null);
            Assert.That(column2.Value.ToString() == "cat");
        }

        [Test]
        public void TestImportingWithHeader()
        {
            var settings = new CSVSettings()
            {
                HasHeader = true,
                Separator = ","
            };

            var provider = new CSVDataProvider();
            var extent = provider.Load("data/csv/withheader.txt", settings);
            Assert.That(extent, Is.Not.Null);
            Assert.That(extent.HeaderNames.Count, Is.EqualTo(3));
            Assert.That(extent.HeaderNames[0], Is.EqualTo("Title1"));
            Assert.That(extent.HeaderNames[1], Is.EqualTo("Title2"));
            Assert.That(extent.HeaderNames[2], Is.EqualTo("Title3"));

            var elements = extent.Elements().Select(x => x as IObject).ToList();
            Assert.That(elements.Count, Is.EqualTo(4));

            Assert.That(elements[0].get("Column 0").ToString(), Is.EqualTo("1"));
            Assert.That(elements[0].get("Column 1").ToString(), Is.EqualTo("2"));
            Assert.That(elements[1].get("Column 2").ToString(), Is.EqualTo("three"));
            Assert.That(elements[2].get("Column 2").ToString(), Is.EqualTo("example3"));

            Assert.That(elements[0].get("Title1").ToString(), Is.EqualTo("1"));
            Assert.That(elements[0].get("Title2").ToString(), Is.EqualTo("2"));
            Assert.That(elements[1].get("Title3").ToString(), Is.EqualTo("three"));
            Assert.That(elements[2].get("Title3").ToString(), Is.EqualTo("example3"));

            Assert.That(elements[0].isSet("Column 0"), Is.True);
            Assert.That(elements[0].isSet("Column 5"), Is.False);
            Assert.That(elements[0].isSet("Unknown"), Is.False);
            Assert.That(elements[0].isSet("Title1"), Is.True);
            Assert.That(elements[0].isSet("Title2"), Is.True);
            Assert.That(elements[0].isSet("Title3"), Is.True);
            Assert.That(elements[0].isSet("Title4"), Is.False);

            var column1 = elements[0].getAll().Where(x => x.PropertyName == "Title1").FirstOrDefault();
            Assert.That(column1 != null);
            Assert.That(column1.Value.ToString() == "1");

            var column2 = elements[3].getAll().Where(x => x.PropertyName == "Title3").FirstOrDefault();
            Assert.That(column2 != null);
            Assert.That(column2.Value.ToString() == "rat");
        }

        [Test]
        public void TestSettingUnsetting()
        {
            var settings = new CSVSettings()
            {
                HasHeader = true,
                Separator = ","
            };

            var provider = new CSVDataProvider();
            var extent = provider.Load("data/csv/withheader.txt", settings);
            var elements = extent.Elements().Select(x => x as IObject).ToList();

            elements[0].set("Column 0", "Eins");
            Assert.That(elements[0].get("Title1").ToString(), Is.EqualTo("Eins"));
            Assert.That(elements[1].get("Title2").ToString(), Is.EqualTo("two"));

            elements[1].unset("Column 1");
            Assert.That(elements[1].get("Title2").ToString(), Is.EqualTo(string.Empty));
            Assert.That(elements[1].get("Column 1").ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void TestDelete()
        {
            var settings = new CSVSettings()
            {
                HasHeader = true,
                Separator = ","
            };

            var provider = new CSVDataProvider();
            var extent = provider.Load("data/csv/withheader.txt", settings);
            var elements = extent.Elements().Select(x => x as IObject).ToList();

            elements[0].set("Column 0", "Eins");
            Assert.That(elements[0].get("Title1").ToString(), Is.EqualTo("Eins"));
            Assert.That(elements[1].get("Title2").ToString(), Is.EqualTo("two"));

            elements[0].delete();
            elements = extent.Elements().Select(x => x as IObject).ToList();
            Assert.That(elements[0].get("Title1").ToString(), Is.EqualTo("one"));
        }

        [Test]
        public void TestAdd()
        {
            ApplicationCore.PerformBinding();
            var pool = DatenMeisterPool.Create();

            var settings = new CSVSettings()
            {
                HasHeader = true,
                Separator = ","
            };

            var provider = new CSVDataProvider();
            var extent = provider.Load("data/csv/withheader.txt", settings);
            var elements = extent.Elements().Select(x => x as IObject).ToList();

            elements[0].set("Column 0", "Eins");
            Assert.That(elements[0].get("Title1").ToString(), Is.EqualTo("Eins"));
            Assert.That(elements[1].get("Title2").ToString(), Is.EqualTo("two"));
            Assert.That(elements.Count, Is.EqualTo(4));

            // Creates one object
            var factory = Factory.GetFor(extent);
            var newElement = factory.CreateInExtent(extent);
            elements = extent.Elements().Select(x => x as IObject).ToList();
            Assert.That(elements.Count, Is.EqualTo(5));

            // Creates another object
            newElement = factory.CreateInExtent(extent);
            newElement.set("Title1", "Fünf");

            elements = extent.Elements().Select(x => x as IObject).ToList();
            Assert.That(elements.Count, Is.EqualTo(6));
            Assert.That(elements.Last().get("Title1"), Is.EqualTo("Fünf"));
        }

        [Test]
        public void TestSavingHeaderHeader()
        {
            var settings = new CSVSettings()
            {
                HasHeader = true,
                Separator = ","
            };

            var provider = new CSVDataProvider();
            var extent = provider.Load("data/csv/withheader.txt", settings);

            provider.Save(extent, "test_y_y.txt", settings);

            var extent2 = provider.Load("test_y_y.txt", settings);
            Assert.That(extent2.Elements().Count(), Is.EqualTo(extent.Elements().Count()));
        }

        [Test]
        public void TestSavingHeaderNoHeader()
        {
            var settings = new CSVSettings()
            {
                HasHeader = true,
                Separator = ","
            };

            var provider = new CSVDataProvider();
            var extent = provider.Load("data/csv/withheader.txt", settings);

            var newSettings = new CSVSettings()
            {
                HasHeader = false,
                Separator = ","
            };

            provider.Save(extent, "test_y_n.txt", newSettings);

            var extent2 = provider.Load("test_y_n.txt", newSettings);
            Assert.That(extent2.Elements().Count(), Is.EqualTo(extent.Elements().Count()));
        }

        [Test]
        public void TestSavingNoHeaderNoHeader()
        {
            var settings = new CSVSettings()
            {
                HasHeader = false,
                Separator = ","
            };

            var provider = new CSVDataProvider();
            var extent = provider.Load("data/csv/withoutheader.txt", settings);

            var newSettings = new CSVSettings()
            {
                HasHeader = false,
                Separator = ","
            };

            provider.Save(extent, "test_n_n.txt", newSettings);

            var extent2 = provider.Load("test_n_n.txt", newSettings);
            Assert.That(extent2.Elements().Count(), Is.EqualTo(extent.Elements().Count()));
        }

        [Test]
        public void TestSavingNoHeaderHeader()
        {
            var settings = new CSVSettings()
            {
                HasHeader = false,
                Separator = ","
            };

            var provider = new CSVDataProvider();
            var extent = provider.Load("data/csv/withoutheader.txt", settings);

            var newSettings = new CSVSettings()
            {
                HasHeader = true,
                Separator = ","
            };

            provider.Save(extent, "test_n_y.txt", newSettings);

            var extent2 = provider.Load("test_n_y.txt", newSettings);
            Assert.That(extent2.Elements().Count(), Is.EqualTo(extent.Elements().Count()));
        }

        [Test]
        public void TestSetExtentToObject()
        {
            var settings = new CSVSettings()
            {
                HasHeader = true,
                Separator = ","
            };

            var provider = new CSVDataProvider();
            var extent = provider.Load("data/csv/withheader.txt", settings);
            var elements = extent.Elements().Select(x => x as IObject).ToList();

            Assert.That(elements.Count, Is.GreaterThan(0));
            Assert.That(elements[0].Extent, Is.EqualTo(extent));
        }
    }
}
