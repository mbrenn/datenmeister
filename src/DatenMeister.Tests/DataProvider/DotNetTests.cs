using DatenMeister.DataProvider.DotNet;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.DataProvider
{
    [TestFixture]
    public class DotNetTests
    {
        [Test]
        public void TestGettingSetting()
        {
            var extent = new DotNetExtent("test:///");
            var value = new DotNetObject(extent, new TestClass(), Guid.Empty.ToString());

            var properties = value.getAll();
            Assert.That(properties.Any(x => x.PropertyName == "TextValue"));
            Assert.That(properties.Any(x => x.PropertyName == "NumberValue"));

            value.set("TextValue", "Dies ist ein Test");
            value.set("NumberValue", 123L);

            Assert.Throws<ArgumentException>(() =>
                {
                    value.set("XYZ", "T");
                });

            var textValue = value.get("TextValue");
            var numberValue = value.get("NumberValue");

            Assert.That(textValue, Is.EqualTo("Dies ist ein Test"));
            Assert.That(numberValue, Is.EqualTo(123L));

            properties = value.getAll();
            Assert.That(properties.Any(x => x.PropertyName == "TextValue"));
            Assert.That(properties.Any(x => x.PropertyName == "NumberValue"));

            Assert.That(properties.Any(x => x.Value.ToString() == "Dies ist ein Test"));
            Assert.That(properties.Any(x => x.Value.ToString() == "123"));
        }

        public class TestClass
        {
            public string TextValue
            {
                get;
                set;
            }

            public long NumberValue
            {
                get;
                set;
            }
        }
    }
}
