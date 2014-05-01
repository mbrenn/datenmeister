using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
using DatenMeister.Pool;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatenMeister.Tests.PoolLogic
{
    [TestFixture]
    public class CopyExtentsTests
    {
        [Test]
        public void TestCopy()
        {
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" reference-ref=\"#e1\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = new DatenMeisterPool();
            var secondDataPool = new DatenMeisterPool();
            pool.Add(xmlExtent, null);

            var copyExtent = new XmlExtent(
                XDocument.Parse("<root></root>"),
                "test://copy/");
            secondDataPool.Add(copyExtent, null);

            ExtentCopier.Copy(xmlExtent, copyExtent);

            Assert.That(copyExtent.Elements().ElementAt(0).AsIObject().Id, Is.EqualTo("e1"));
            Assert.That(copyExtent.Elements().ElementAt(1).AsIObject().Id, Is.EqualTo("e2"));

            var value = PoolResolver.ResolveInExtent ("test:///#e4", copyExtent) as IObject;
            Assert.That(value, Is.Not.Null);

            var refValue = value.get("reference");
            var refValueAsIObject = refValue.AsSingle().AsIObject();
            Assert.That(refValueAsIObject, Is.Not.Null);

            Assert.That(refValueAsIObject.Id, Is.EqualTo("e1"));

        }
    }
}
