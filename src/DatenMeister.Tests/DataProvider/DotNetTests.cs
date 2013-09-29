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
            var value = new DotNetObject(new TestClass(), Guid.Empty.ToString());

            var properties = value.GetAll();
            Assert.That(properties.Any(x => x.PropertyName == "TextValue"));
            Assert.That(properties.Any(x => x.PropertyName == "NumberValue"));

            value.Set("TextValue", "Dies ist ein Test");
            value.Set("NumberValue", 123L);

            Assert.Throws<ArgumentException>(() =>
                {
                    value.Set("XYZ", "T");
                });

            var textValue = value.Get("TextValue");
            var numberValue = value.Get("NumberValue");

            Assert.That(textValue, Is.EqualTo("Dies ist ein Test"));
            Assert.That(numberValue, Is.EqualTo(123L));

            properties = value.GetAll();
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
