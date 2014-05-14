using BurnSystems.Test;
using DatenMeister;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.Tests.DataProvider
{
    [TestFixture]
    public class XmlTests
    {
        [Test]
        public void TestReadingSimpleListWithIdGet()
        {
            var xmlProvider = new XmlDataProvider();
            var xmlExtent = xmlProvider.Load("data/xml/simplelistwithid.xml", new XmlSettings());
            Assert.That(xmlExtent, Is.Not.Null);

            var xmlRootElement = xmlExtent.Elements();

            Assert.That(xmlRootElement, Is.Not.Null);

            var items = xmlRootElement.ToList();
            Assert.That(items.Count, Is.EqualTo(6));

            var item0 = items[0].AsIObject();
            var value = item0.get("").AsSingle();
            Assert.That(value, Is.EqualTo("This is content"));
            Assert.That(item0.get("Nix").AsSingle(), Is.EqualTo(ObjectHelper.NotSet));
        }

        [Test]
        public void TestReadingSimpleListWithIdGetAll()
        {
            var xmlProvider = new XmlDataProvider();
            var xmlExtent = xmlProvider.Load("data/xml/simplelistwithid.xml", new XmlSettings());

            // Gets the first object '/list/item[0]'
            var firstElement = xmlExtent.Elements().AsIObject();
            Assert.That(firstElement != null);
            var allProperties = firstElement.getAll();
            Assert.That(allProperties.Any(x => x.PropertyName == string.Empty), Is.True);
            Assert.That(allProperties.Any(x => x.PropertyName == "id"), Is.True);
            Assert.That(allProperties.First(x => x.PropertyName == string.Empty).PropertyName, Is.EqualTo(String.Empty));
            Assert.That(allProperties.First(x => x.PropertyName == string.Empty).Value, Is.EqualTo("This is content"));
        }

        [Test]
        public void TestReadingSimpleListWithIdSet()
        {
            var xmlProvider = new XmlDataProvider();
            var xmlExtent = xmlProvider.Load("data/xml/simplelistwithid.xml", new XmlSettings());

            // Gets the first object '/list/item[0]'
            var firstElement = xmlExtent.Elements().AsIObject();

            Assert.That(xmlExtent.XmlDocument.Element("list").Elements("item").First().Value, Is.EqualTo("This is content"));
            firstElement.set(string.Empty, "Test");
            Assert.That(firstElement.get(string.Empty).AsSingle(), Is.EqualTo("Test"));
            Assert.That(xmlExtent.XmlDocument.Element("list").Elements("item").First().Value, Is.EqualTo("Test"));
        }

        [Test]
        public void TestGetGetAllAndSetByAttributes()
        {
            var xmlProvider = new XmlDataProvider();
            var xmlExtent = xmlProvider.Load("data/xml/simplelistinattribute.xml", new XmlSettings());
            Assert.That(xmlExtent, Is.Not.Null);

            // Gets the first object '/list/item[0]'
            var firstElement = xmlExtent.Elements().AsIObject();
            Assert.That(firstElement, Is.Not.Null);

            Assert.That(firstElement.get(string.Empty).AsSingle(), Is.EqualTo("One"));
            Assert.That(firstElement.get("letter").AsSingle(), Is.EqualTo("a"));

            var pairs = firstElement.getAll();
            Assert.That(pairs.Any(x => x.PropertyName == string.Empty), Is.True);
            Assert.That(pairs.Any(x => x.PropertyName == "id"), Is.True);
            Assert.That(pairs.Any(x => x.PropertyName == "letter"), Is.True);
            Assert.That(pairs.Any(x => x.PropertyName == "nix"), Is.False);

            // Gets second item '/list/item[1]'
            var secondElement = xmlExtent.Elements().AsEnumeration().ElementAt(1).AsIObject();
            Assert.That(secondElement, Is.Not.Null);

            Assert.That(secondElement.get(string.Empty).AsSingle(), Is.EqualTo("Two"));
            Assert.That(secondElement.get("letter").AsSingle(), Is.EqualTo("b"));

            // Sets the content of second item
            secondElement.set("letter", "to be");

            Assert.That(secondElement.get("letter").AsSingle(), Is.EqualTo("to be"));
            Assert.That(xmlExtent.XmlDocument.Element("list").Elements("item").ElementAt(1).Attribute("letter").Value,
                Is.EqualTo("to be"));
        }

        /// <summary>
        /// Tests the id concept according to issue #11
        /// https://github.com/mbrenn/datenmeister/issues/11
        /// </summary>
        [Test]
        public void TestIdConcept()
        {
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element />" + // No element, should throw an exception
                "</root>");

            var xmlExtent = new XmlExtent(document, "test");
            Assert.That(xmlExtent.Elements().ElementAt(0).AsIObject().Id, Is.EqualTo("e1"));
            Assert.That(xmlExtent.Elements().ElementAt(1).AsIObject().Id, Is.EqualTo("e2"));
            Assert.Throws<InvalidOperationException>(() => { xmlExtent.Elements().ElementAt(2); return; });

            var newElement = xmlExtent.CreateObject(null);
            Assert.That(newElement.Id, Is.Not.Null.Or.Empty);

            newElement.set("id", "e4");

            Assert.That(newElement.Id, Is.EqualTo("e4"));
        }

        /// <summary>
        /// Tests the reference concet
        /// </summary>
        [Test]
        public void TestReferenceWithFullPath()
        {
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" reference-ref=\"test:///#e1\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = new DatenMeisterPool();
            pool.Add(xmlExtent, null);

            var value = pool.ResolveByPath("test:///#e4") as IObject;
            Assert.That(value, Is.Not.Null);

            var refValue = value.get("reference");
            var refValueAsIObject = refValue.AsSingle().AsIObject();
            Assert.That(refValueAsIObject, Is.Not.Null);

            Assert.That(refValueAsIObject.Id, Is.EqualTo("e1"));
        }

        [Test]
        public void TestReferenceWithPartialPath()
        {
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" reference-ref=\"#e1\" />" + 
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = new DatenMeisterPool();
            pool.Add(xmlExtent, null);

            var value = pool.ResolveByPath("test:///#e4") as IObject;
            Assert.That(value, Is.Not.Null);

            var refValue = value.get("reference");
            var refValueAsIObject = refValue.AsSingle().AsIObject();
            Assert.That(refValueAsIObject, Is.Not.Null);

            Assert.That(refValueAsIObject.Id, Is.EqualTo("e1"));
        }

        [Test]
        public void TestSetReference()
        {
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" />" + // No element, should throw an exception
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = new DatenMeisterPool();
            pool.Add(xmlExtent, null);

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;
            var valueE1 = pool.ResolveByPath("test:///#e1") as IObject;
            var valueE2 = pool.ResolveByPath("test:///#e2") as IObject;
            Assert.That(valueE4, Is.Not.Null);

            valueE4.set("reference", valueE1);

            var element = document.Root.Elements("element")
                .Where(x => (x.Attribute("id") ?? new XAttribute("id", string.Empty)).Value == "e4")
                .FirstOrDefault();
            Assert.That(element, Is.Not.Null);
            Assert.That(element.Attribute("reference-ref"), Is.Not.Null);
            Assert.That(element.Attribute("reference-ref").Value, Is.EqualTo("#e1"));

            valueE4.set("reference", valueE2);

            var element2 = document.Root.Elements("element")
                .Where(x => (x.Attribute("id") ?? new XAttribute("id", string.Empty)).Value == "e4")
                .FirstOrDefault();

            Assert.That(element2, Is.Not.Null);
            Assert.That(element2.Attribute("reference-ref"), Is.Not.Null);
            Assert.That(element2.Attribute("reference-ref").Value, Is.EqualTo("#e2"));

            var retrievedValue = valueE4.get("reference").AsIObject();
            Assert.That(retrievedValue, Is.Not.Null);
            Assert.That(retrievedValue.Id, Is.EqualTo("e2"));
        }
    }
}
