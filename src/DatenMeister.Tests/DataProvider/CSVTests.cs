using DatenMeister.DataProvider.CSV;
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

            Assert.That(elements[0].Get("Column 0").ToString(), Is.EqualTo("1"));
            Assert.That(elements[0].Get("Column 1").ToString(), Is.EqualTo("2"));
            Assert.That(elements[1].Get("Column 2").ToString(), Is.EqualTo("three"));
            Assert.That(elements[2].Get("Column 2").ToString(), Is.EqualTo("example3"));

            Assert.That(elements[0].IsSet("Column 0"), Is.True);
            Assert.That(elements[0].IsSet("Column 5"), Is.False);
            Assert.That(elements[0].IsSet("Unknown"), Is.False);

            var column1 = elements[0].GetAll().Where(x => x.First == "Column 0").FirstOrDefault();
            Assert.That(column1 != null);
            Assert.That(column1.Second.ToString() == "1");

            var column2 = elements[3].GetAll().Where(x => x.First == "Column 0").FirstOrDefault();
            Assert.That(column2 != null);
            Assert.That(column2.Second.ToString() == "cat");
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

            Assert.That(elements[0].Get("Column 0").ToString(), Is.EqualTo("1"));
            Assert.That(elements[0].Get("Column 1").ToString(), Is.EqualTo("2"));
            Assert.That(elements[1].Get("Column 2").ToString(), Is.EqualTo("three"));
            Assert.That(elements[2].Get("Column 2").ToString(), Is.EqualTo("example3"));

            Assert.That(elements[0].Get("Title1").ToString(), Is.EqualTo("1"));
            Assert.That(elements[0].Get("Title2").ToString(), Is.EqualTo("2"));
            Assert.That(elements[1].Get("Title3").ToString(), Is.EqualTo("three"));
            Assert.That(elements[2].Get("Title3").ToString(), Is.EqualTo("example3"));

            Assert.That(elements[0].IsSet("Column 0"), Is.True);
            Assert.That(elements[0].IsSet("Column 5"), Is.False);
            Assert.That(elements[0].IsSet("Unknown"), Is.False);
            Assert.That(elements[0].IsSet("Title1"), Is.True);
            Assert.That(elements[0].IsSet("Title2"), Is.True);
            Assert.That(elements[0].IsSet("Title3"), Is.True);
            Assert.That(elements[0].IsSet("Title4"), Is.False);

            var column1 = elements[0].GetAll().Where(x => x.First == "Title1").FirstOrDefault();
            Assert.That(column1 != null);
            Assert.That(column1.Second.ToString() == "1");

            var column2 = elements[3].GetAll().Where(x => x.First == "Title3").FirstOrDefault();
            Assert.That(column2 != null);
            Assert.That(column2.Second.ToString() == "rat");
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

            elements[0].Set("Column 0", "Eins");
            Assert.That(elements[0].Get("Title1").ToString(), Is.EqualTo("Eins"));
            Assert.That(elements[1].Get("Title2").ToString(), Is.EqualTo("two"));

            elements[1].Unset("Column 1");
            Assert.That(elements[1].Get("Title2").ToString(), Is.EqualTo(string.Empty));
            Assert.That(elements[1].Get("Column 1").ToString(), Is.EqualTo(string.Empty));
        }
    }
}
