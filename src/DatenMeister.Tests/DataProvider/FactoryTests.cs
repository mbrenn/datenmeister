using DatenMeister.DataProvider;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.DataProvider
{
    /// <summary>
    /// Defines the factory tests
    /// </summary>
    [TestFixture]
    public class FactoryTests
    {
        [Test]
        public void TestXmlTests()
        {
            var extent = XmlTests.CreateTestExtent();
            var factory = Factory.GetFor(extent);
            var obj = factory.CreateInExtent(extent, XmlTests.TypePerson);
            Assert.That(obj, Is.Not.Null);
        }
    }
}
