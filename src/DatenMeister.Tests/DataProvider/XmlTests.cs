using BurnSystems.Test;
using DatenMeister;
using DatenMeister.DataProvider;
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

            var newElement = Factory.GetFor(xmlExtent).CreateInExtent(xmlExtent);
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
            pool.DoDefaultBinding();
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
            pool.DoDefaultBinding();
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
                    "<element id=\"e4\" />" + 
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = new DatenMeisterPool();
            pool.DoDefaultBinding();
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

        [Test]
        public void TestReflectiveSequence()
        {
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = new DatenMeisterPool();
            pool.DoDefaultBinding();
            pool.Add(xmlExtent, null);

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;
            var valueE1 = pool.ResolveByPath("test:///#e1") as IObject;
            var valueE2 = pool.ResolveByPath("test:///#e2") as IObject;

            // Ok, first thing: Add the stuff as references
            var asSequence = valueE1.get("subelements").AsReflectiveSequence();
            Assert.That(asSequence, Is.Not.Null);
            Assert.That(asSequence.size(), Is.EqualTo(0));
            asSequence.add(valueE2);

            // Check, if we get it back
            Assert.That(asSequence.size(), Is.EqualTo(1));
            Assert.That(asSequence.get(0).AsSingle().AsIObject().Id, Is.EqualTo("e2"));
            
            // Do a clear
            asSequence.clear();
            Assert.That(asSequence.size(), Is.EqualTo(0));

            // Add several instances
            asSequence.add(valueE2);
            asSequence.add(valueE4);
            asSequence.add(valueE1);

            Assert.That(asSequence.size(), Is.EqualTo(3));
            Assert.That(asSequence.get(0).AsSingle().AsIObject().Id, Is.EqualTo("e2"));
            Assert.That(asSequence.get(1).AsSingle().AsIObject().Id, Is.EqualTo("e4"));
            Assert.That(asSequence.get(2).AsSingle().AsIObject().Id, Is.EqualTo("e1"));

            // Now remove second one
            asSequence.remove(1);
            Assert.That(asSequence.size(), Is.EqualTo(2));
            Assert.That(asSequence.get(0).AsSingle().AsIObject().Id, Is.EqualTo("e2"));
            Assert.That(asSequence.get(1).AsSingle().AsIObject().Id, Is.EqualTo("e1"));
            
            // Re-add it
            asSequence.add(1, valueE4);
            Assert.That(asSequence.size(), Is.EqualTo(3));
            Assert.That(asSequence.get(0).AsSingle().AsIObject().Id, Is.EqualTo("e2"));
            Assert.That(asSequence.get(1).AsSingle().AsIObject().Id, Is.EqualTo("e4"));
            Assert.That(asSequence.get(2).AsSingle().AsIObject().Id, Is.EqualTo("e1"));

            // Remove a specific one
            asSequence.remove(valueE2);
            Assert.That(asSequence.size(), Is.EqualTo(2));
            Assert.That(asSequence.get(0).AsSingle().AsIObject().Id, Is.EqualTo("e4"));
            Assert.That(asSequence.get(1).AsSingle().AsIObject().Id, Is.EqualTo("e1"));

            // Replace one
            asSequence.set(1, valueE2);
            Assert.That(asSequence.size(), Is.EqualTo(2));
            Assert.That(asSequence.get(0).AsSingle().AsIObject().Id, Is.EqualTo("e4"));
            Assert.That(asSequence.get(1).AsSingle().AsIObject().Id, Is.EqualTo("e2"));
        }

        [Test]
        public void TestFactory()
        {
            var document = XDocument.Parse(
                "<root>" +
                    "<tasks />" +
                    "<persons />" +
                "</root>");

            var typeTask = new GenericObject();
            var typePerson = new GenericObject();
            var xmlExtent = new XmlExtent(document, "test:///");
            xmlExtent.Settings.Mapping.Add("task", typeTask, x => x.Elements("root").Elements("tasks").FirstOrDefault());
            xmlExtent.Settings.Mapping.Add("person", typePerson, x => x.Elements("root").Elements("persons").FirstOrDefault());
            var pool = new DatenMeisterPool();
            pool.DoDefaultBinding();
            pool.Add(xmlExtent, null);

            var factory = Factory.GetFor(xmlExtent);
            var person1 = factory.CreateInExtent(xmlExtent, typePerson);
            var person2 = factory.CreateInExtent(xmlExtent, typePerson);

            var task1 = factory.CreateInExtent(xmlExtent, typeTask);

            person1.set("name", "Brenn");
            person1.set("prename", "Martin");

            person2.set("name", "Brenner");
            person2.set("prename", "Martina");

            task1.set("name", "Toller Task");

            // Now do the checks
            Assert.That(
                document.Elements("root").Elements("tasks").Elements("task").Count(), 
                Is.EqualTo(1));
            Assert.That(
                document.Elements("root").Elements("persons").Elements("person").Count(), 
                Is.EqualTo(2));

            Assert.That(
                document.Elements("root").Elements("persons").Elements("person").First().Attribute("name").Value, 
                Is.EqualTo("Brenn"));
            Assert.That(
                document.Elements("root").Elements("persons").Elements("person").ElementAt(1).Attribute("name").Value, 
                Is.EqualTo("Brenner"));

            Assert.That(
                document.Elements("root").Elements("tasks").Elements("task").First().Attribute("name").Value,
                Is.EqualTo("Toller Task"));
        }

        [Test]
        public void TestAutoGeneratedCode()
        {
            var document = XDocument.Parse(
                "<root>" +
                    "<comments />" +
                    "<textfields />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            
            var typeExtent = DatenMeister.Entities.AsObject.FieldInfo.Types.Init();

            xmlExtent.Settings.Mapping.Add(
                "comment", 
                DatenMeister.Entities.AsObject.FieldInfo.Types.Comment, 
                x => x.Elements("root").Elements("comments").FirstOrDefault());

            xmlExtent.Settings.Mapping.Add(
                "textfield", 
                DatenMeister.Entities.AsObject.FieldInfo.Types.TextField, 
                x => x.Elements("root").Elements("textfields").FirstOrDefault());
            var pool = new DatenMeisterPool();
            pool.DoDefaultBinding();
            pool.Add(xmlExtent, null);
            var factory = new FactoryProvider().CreateFor(xmlExtent);

            var comment = factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.Comment);
            var commentObj = new DatenMeister.Entities.AsObject.FieldInfo.Comment(comment);
            commentObj.setName("Description");
            commentObj.setBinding("Binding");

            Assert.That(commentObj.getName(), Is.EqualTo("Description"));
            Assert.That(commentObj.getBinding(), Is.EqualTo("Binding"));
        }
    }
}
