using DatenMeister.DataProvider;
using DatenMeister.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests
{
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void TestAsString()
        {
            Assert.That(Extensions.AsSingle(null), Is.EqualTo(ObjectHelper.Null));
            Assert.That(Extensions.AsSingle("TEST"), Is.EqualTo("TEST"));
        }

        [Test]
        public void TestCommonValueNotSet()
        {
            var value1 = new GenericObject();
            var value2 = new GenericObject();

            var values = new IObject[] { value1, value2 };

            var commonValue = ObjectHelper.GetCommonValue(values, "test");
            Assert.That(commonValue, Is.EqualTo(ObjectHelper.NotSet));

            value1.set("test", "Hallo");
            commonValue = ObjectHelper.GetCommonValue(values, "test");
            Assert.That(commonValue, Is.EqualTo(ObjectHelper.Different));

            value2.set("test", "Hallo");
            commonValue = ObjectHelper.GetCommonValue(values, "test");
            Assert.That(commonValue, Is.EqualTo("Hallo"));

            value2.set("test", "Hallo Martin");
            commonValue = ObjectHelper.GetCommonValue(values, "test");
            Assert.That(commonValue, Is.EqualTo(ObjectHelper.Different));
        }
    }
}
