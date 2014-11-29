using DatenMeister.DataProvider;
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
        public void TestCopyWithoutReference()
        {
            ApplicationCore.PerformBinding();
            var pool = DatenMeisterPool.Create();

            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" text=\"xyz\"/>" +
                    "<element id=\"e2\" />" +
                "</root>");

            var copyExtent = CreateCopiedExtent(document);

            Assert.That(copyExtent.Elements().ElementAt(0).AsIObject().Id, Is.EqualTo("e1"));
            Assert.That(copyExtent.Elements().ElementAt(0).AsIObject().get("text").AsSingle().ToString(), Is.EqualTo("xyz"));
            Assert.That(copyExtent.Elements().ElementAt(1).AsIObject().Id, Is.EqualTo("e2"));
        }

        [Test]
        public void TestCopyIncludingEnums()
        {
            ApplicationCore.PerformBinding();
            var pool = DatenMeisterPool.Create();

            var sourceExtent = new GenericExtent("datenmeister:///source");
            var targetExtent = new GenericExtent("datenmeister:///target");

            var sourceElement = new GenericElement();
            sourceElement.set("test", ConsoleColor.DarkBlue);
            sourceExtent.Elements().add(sourceElement);

            ExtentCopier.Copy(sourceExtent, targetExtent);

            var targetElement = targetExtent.Elements().FirstOrDefault() as IObject;
            Assert.That(targetElement, Is.Not.Null);

            var enumTest = targetElement.get("test").AsSingle();
            Assert.That(enumTest, Is.Not.Null);
            Assert.That(enumTest, Is.Not.EqualTo(ObjectHelper.NotSet));
            Assert.That(ObjectConversion.IsEnum(enumTest), Is.True);
            Assert.That((ConsoleColor)enumTest, Is.EqualTo(ConsoleColor.DarkBlue));
        }

        [Test]
        public void TestCopyIncludingStrings()
        {
            ApplicationCore.PerformBinding();
            var pool = DatenMeisterPool.Create();

            var sourceExtent = new GenericExtent("datenmeister:///source");
            var targetExtent = new GenericExtent("datenmeister:///target");

            var sourceElement = new GenericElement();
            sourceElement.set("test", "ABC");
            sourceExtent.Elements().add(sourceElement);

            ExtentCopier.Copy(sourceExtent, targetExtent);

            var targetElement = targetExtent.Elements().FirstOrDefault() as IObject;
            Assert.That(targetElement, Is.Not.Null);

            var enumTest = targetElement.get("test").AsSingle();
            Assert.That(enumTest, Is.Not.Null);
            Assert.That(enumTest, Is.Not.EqualTo(ObjectHelper.NotSet));
            Assert.That(enumTest, Is.TypeOf<string>());
            Assert.That(enumTest, Is.EqualTo("ABC"));
        }

        [Test]
        public void TestCopyWithReference()
        {
            ApplicationCore.PerformBinding();
            var pool = DatenMeisterPool.Create();

            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" reference-ref=\"#e1\" />" +
                "</root>"); 
            
            var copyExtent = CreateCopiedExtent(document);

            copyExtent.XmlDocument.Save("c:\\Users\\Martin\\test.xml");

            var value = PoolResolver.ResolveInExtent("test://copy/#e4", copyExtent) as IObject;
            Assert.That(value, Is.Not.Null);

            var refValue = value.get("reference");
            var refValueAsIObject = refValue.AsSingle().AsIObject();
            Assert.That(refValueAsIObject, Is.Not.Null);

            Assert.That(refValueAsIObject.Id, Is.EqualTo("e1"));
        }

        /// <summary>
        /// Helper method, which gets an XDocument, stores it into an XmlExtent and
        /// copies the XmlExtent. The XmlExtent containing the copy is returned
        /// </summary>
        /// <param name="document">Document to be copied</param>
        /// <returns>Copied XmlExtent</returns>
        private static XmlExtent CreateCopiedExtent(XDocument document)
        {
            ApplicationCore.PerformBinding();
            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();

            pool.Add(xmlExtent, null, ExtentType.Data);

            var copyExtent = new XmlExtent(
                XDocument.Parse("<root></root>"),
                "test://copy/");
            pool.Add(copyExtent, null, ExtentType.Data);

            ExtentCopier.Copy(xmlExtent, copyExtent);
            return copyExtent;
        }
    }
}
