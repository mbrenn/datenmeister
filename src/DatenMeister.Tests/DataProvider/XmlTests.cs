using BurnSystems.Test;
using DatenMeister;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.DataProvider.Generic;
using DatenMeister.DataProvider.Wrapper;
using DatenMeister.DataProvider.Wrapper.EventOnChange;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Entities.FieldInfos;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
using DatenMeister.Pool;
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
            var xmlExtent = xmlProvider.Load("data/xml/simplelistwithid.xml");
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
            var xmlExtent = xmlProvider.Load("data/xml/simplelistwithid.xml");

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
            var xmlExtent = xmlProvider.Load("data/xml/simplelistwithid.xml");

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
            var xmlExtent = xmlProvider.Load("data/xml/simplelistinattribute.xml");
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
            ApplicationCore.PerformBinding();
            var pool = DatenMeisterPool.Create();

            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element />" + // No element, should throw an exception
                "</root>");

            var xmlExtent = new XmlExtent(document, "test");
            Assert.That(xmlExtent.Elements().ElementAt(0).AsIObject().Id, Is.EqualTo("e1"));
            Assert.That(xmlExtent.Elements().ElementAt(1).AsIObject().Id, Is.EqualTo("e2"));
            Assert.That(xmlExtent.Elements().ElementAt(2).AsIObject().Id, Is.Null.Or.Empty);

            var newElement = Factory.GetFor(xmlExtent).CreateInExtent(xmlExtent);
            Assert.That(newElement.Id, Is.Not.Null.Or.Empty);

            newElement.set("id", "e4");
            Assert.That(newElement.isSet("id"), Is.True);

            Assert.That(newElement.Id, Is.EqualTo("e4"));
        }

        /// <summary>
        /// Tests the reference concet
        /// </summary>
        [Test]
        public void TestReferenceWithFullPath()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" reference-ref=\"test:///#e1\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

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
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" reference-ref=\"#e1\" />" + 
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

            var value = pool.ResolveByPath("test:///#e4") as IObject;
            Assert.That(value, Is.Not.Null);

            var refValue = value.get("reference");
            var refValueAsIObject = refValue.AsSingle().AsIObject();
            Assert.That(refValueAsIObject, Is.Not.Null);

            Assert.That(refValueAsIObject.Id, Is.EqualTo("e1"));
        }

        [Test]
        public void TestReferenceAndRemove()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" reference-ref=\"#e1\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);
            
            var valueE1 = pool.ResolveByPath("test:///#e1") as IObject;
            valueE1.delete();

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;
            Assert.That(valueE4, Is.Not.Null);

            var refValue = valueE4.get("reference");
            var refValueAsIObject = refValue.AsSingle();
            Assert.That(refValueAsIObject, Is.EqualTo(ObjectHelper.Null));
        }

        [Test]
        public void TestSetReference()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" />" + 
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

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
        public void TestGetReflectiveCollectionWithReferences()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" reference-ref=\"#e1 #e2\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;
            Assert.That(valueE4, Is.Not.Null);

            var refValue = valueE4.get("reference").AsReflectiveCollection();
            Assert.That(refValue, Is.Not.Null);
            Assert.That(refValue.Count, Is.EqualTo(2));

            Assert.That(refValue.Any(x => x.AsIObject().Id == "e1"), Is.True);
            Assert.That(refValue.Any(x => x.AsIObject().Id == "e2"), Is.True);
        }

        [Test]
        public void TestGetAndSetReflectiveCollectionWithReferences()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;
            Assert.That(valueE4, Is.Not.Null);

            var refValue = valueE4.get("reference").AsReflectiveCollection();
            Assert.That(refValue, Is.Not.Null);
            Assert.That(refValue.Count, Is.EqualTo(0));
            refValue.add(pool.ResolveByPath("test:///#e1") as IObject);
            Assert.That(refValue.Count, Is.EqualTo(1));
            refValue.add(pool.ResolveByPath("test:///#e2") as IObject);
            Assert.That(refValue.Count, Is.EqualTo(2));

            Assert.That(refValue.Any(x => x.AsIObject().Id == "e1"), Is.True);
            Assert.That(refValue.Any(x => x.AsIObject().Id == "e2"), Is.True);

            var xmlAttribute = (valueE4 as XmlObject).Node.Attribute("reference-ref");
            Assert.That(xmlAttribute, Is.Not.Null);
            Assert.That(xmlAttribute.Value, Is.EqualTo("#e1 #e2"));
        }

        [Test]
        public void TestReflectiveSequence()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

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
        public void TestReflectiveSequenceWithStrings()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;
            var sequence = valueE4.get("value").AsReflectiveSequence();
            sequence.add("Value 1");
            sequence.add("Value 2");
            sequence.add("Value 3");

            // Checks, if setting had been successful
            sequence = valueE4.get("value").AsReflectiveSequence();
            Assert.That(sequence.size(), Is.EqualTo(3));
            var value1 = sequence.get(0);
            var value2 = sequence.get(1);
            var value3 = sequence.get(2);

            Assert.That(value1, Is.EqualTo("Value 1"));
            Assert.That(value2, Is.EqualTo("Value 2"));
            Assert.That(value3, Is.EqualTo("Value 3"));
        }

        [Test]
        public void TestEmbeddedElementsAsSingleWithSubElement()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;
            var newElement = CreateUser("Brenn", "Martin", "Teststraße 1");

            valueE4.set("user", newElement);

            // Check, if we get all the information
            var retrievedUser = valueE4.get("user").AsSingle().AsIObject();
            Assert.That(retrievedUser.get("name").AsSingle().ToString(), Is.EqualTo("Brenn"));

            var retrievedAddress = retrievedUser.get("address").AsSingle().AsIObject();
            Assert.That(retrievedAddress.get("street").AsSingle().ToString(), Is.EqualTo("Teststraße 1"));
        }

        [Test]
        public void TestEmbeddedElementsAsReflectiveCollectionWithSubElement()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;
            var newUser1 = CreateUser("Brenn", "Martin", "Teststraße 1");
            var newUser2 = CreateUser("Brenner", "Martina", "Teststraße 2");
            var newUser3 = CreateUser("Brennus", "Martinus", "Teststraße 3");
            var newUser4 = CreateUser("Brennas", "Martinas", "Teststraße 4");

            var users = valueE4.get("user").AsReflectiveCollection();
            users.add(newUser1);
            users.add(newUser2);
            users.add(newUser3);

            users = valueE4.get("user").AsReflectiveCollection();
            users.add(newUser4);

            // Check, if we get all the information
            var retrievedUsers = valueE4.get("user").AsReflectiveCollection();
            Assert.That(retrievedUsers.Count, Is.EqualTo(4));
            Assert.That(retrievedUsers.ElementAt(0).AsIObject().get("name").AsSingle().ToString(), Is.EqualTo("Brenn"));
            Assert.That(retrievedUsers.ElementAt(1).AsIObject().get("name").AsSingle().ToString(), Is.EqualTo("Brenner"));
            Assert.That(retrievedUsers.ElementAt(3).AsIObject().get("name").AsSingle().ToString(), Is.EqualTo("Brennas"));

            var retrievedAddress = retrievedUsers.ElementAt(0).AsIObject().get("address").AsSingle().AsIObject();
            Assert.That(retrievedAddress.get("street").AsSingle().ToString(), Is.EqualTo("Teststraße 1"));

            retrievedAddress = retrievedUsers.ElementAt(3).AsIObject().get("address").AsSingle().AsIObject();
            Assert.That(retrievedAddress.get("street").AsSingle().ToString(), Is.EqualTo("Teststraße 4"));

            var n = 0;
            foreach (var user in retrievedUsers)
            {
                n++;
                var name = user.AsIObject().get("name").AsSingle().ToString();
                Assert.That(name,
                    Is.EqualTo("Brenn").Or.EqualTo("Brenner").Or.EqualTo("Brennus").Or.EqualTo("Brennas"));
            }

            Assert.That(n, Is.EqualTo(4));
        }

        [Test]
        public void TestEmbeddedElementsAsSingle()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<element id=\"e1\" />" +
                    "<element id=\"e2\" />" +
                    "<element id=\"e4\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;

            var newElement = new GenericElement();
            newElement.set("name", "Brenn");
            newElement.set("firstname", "Martin");

            valueE4.set("user", newElement);

            // Check, if we get all the information
            var retrievedElement = valueE4.get("user").AsSingle().AsIObject();
            Assert.That(retrievedElement.get("name").AsSingle().ToString(), Is.EqualTo("Brenn"));
        }

        [Test]
        public void TestGetAndSetOfDateTime()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                "<element id=\"e1\" />" +
                "<element id=\"e2\" />" +
                "<element id=\"e4\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;

            valueE4.set("bday", new DateTime(1981, 11, 16, 11, 42, 00));
            
            // Check, if we get all the information
            var retrievedBDay = ObjectConversion.ToDateTime(valueE4.get("bday").AsSingle());
            Assert.That(retrievedBDay, Is.EqualTo(new DateTime(1981, 11, 16, 11, 42, 00)));
        }

        [Test]
        public void TestRetrieveInformationFromRootNode()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root property=\"value\">" +
                "<element id=\"e1\" />" +
                "<element id=\"e2\" />" +
                "<element id=\"e4\" />" +
                "</root>");

            var xmlExtent = new XmlExtent(
                document, 
                "test:///",
                new XmlSettings()
                {
                    UseRootNode = true
                });

            var valueAsIObject = xmlExtent.Elements().First().AsIObject();
            Assert.That(valueAsIObject, Is.Not.Null);

            var propertyValue = valueAsIObject.get("property").AsSingle().ToString();
            Assert.That(propertyValue, Is.EqualTo("value"));
        }

        [Test]
        public void TestFactory()
        {
            var xmlExtent = CreateTestExtent();

            var xmldocument = xmlExtent.XmlDocument;
            // Now do the checks
            Assert.That(
                xmldocument.Elements("root").Elements("tasks").Elements("task").Count(), 
                Is.EqualTo(1));
            Assert.That(
                xmldocument.Elements("root").Elements("persons").Elements("person").Count(), 
                Is.EqualTo(2));

            Assert.That(
                xmldocument.Elements("root").Elements("persons").Elements("person").First().Attribute("name").Value, 
                Is.EqualTo("Brenn"));
            Assert.That(
                xmldocument.Elements("root").Elements("persons").Elements("person").ElementAt(1).Attribute("name").Value, 
                Is.EqualTo("Brenner"));

            Assert.That(
                xmldocument.Elements("root").Elements("tasks").Elements("task").First().Attribute("name").Value,
                Is.EqualTo("Toller Task"));
        }

        [Test]
        public void TestTypedElementsAsSubElements()
        {
            var extent = CreateRawTestExtent();
            var factory = Factory.GetFor(extent);

            var person1 = new GenericElement(null, null, TypePerson, null);
            person1.set("name", "Brenn");
            var task1 = new GenericElement(null, null, TypeTask, null);
            task1.set("name", "nice task");

            var container = factory.create(null);
            extent.Elements().add(container);

            container.set("person", person1);
            container.set("task", task1);

            // Now do the check

            Assert.That(extent.Elements().Count, Is.EqualTo(1));
            var container2 = extent.Elements().FirstOrDefault().AsSingle().AsIObject();

            Assert.That(container2, Is.Not.Null);
            var person2 = container2.get("person").AsSingle().AsIObject() as IElement;
            var task2 = container2.get("task").AsSingle().AsIObject() as IElement;

            Assert.That(person2, Is.Not.Null);
            Assert.That(task2, Is.Not.Null);

            Assert.That(person2.getMetaClass(), Is.EqualTo(TypePerson));
            Assert.That(task2.getMetaClass(), Is.EqualTo(TypeTask));
        }

        [Test]
        public void TestNoPropertiesForNotSetItemsIssue99()
        {
            var extent = CreateRawTestExtent();
            var factory = new XmlFactory(extent);

            var createdObject = factory.create(TypePerson) as XmlObject;
            Assert.That(createdObject, Is.Not.Null);
            extent.Elements().add(createdObject);

            createdObject.set("valueNotSet", ObjectHelper.NotSet);
            var xmlNode = createdObject.Node;
            Assert.That(xmlNode.Attributes().Any(x=>x.Name == "value123"), Is.False);
            Assert.That(xmlNode.Elements().Any(x => x.Name == "value123"), Is.False);
        }

        [Test]
        public void TestSettingOfNull()
        {
            var extent = CreateRawTestExtent();
            var factory = new XmlFactory(extent);

            var createdObject = factory.create(TypePerson) as XmlObject;
            Assert.That(createdObject, Is.Not.Null);
            extent.Elements().add(createdObject);

            createdObject.set("valueSet", ObjectHelper.Null);
            var xmlNode = createdObject.Node;
            Assert.That(xmlNode.Attributes().Any(x => x.Name == "valueSet"), Is.True);
            Assert.That(xmlNode.Elements().Any(x => x.Name == "valueSet"), Is.True);
        }

        [Test]
        public void TestSetAndGetAllOfTypedElement()
        {
            var extent = CreateRawTestExtent();
            var factory = new XmlFactory(extent);

            var createdObject = factory.create(TypePerson);
            createdObject.set("name", "yes");

            var all = createdObject.getAll();
            Assert.That(all.Count(), Is.EqualTo(2));
        }

        [Test]
        public void TestSettingOfEnumerationsWithinObjects()
        {
            var viewTypes = ViewHelper.ViewTypes;
            var extent = CreateRawTestExtent();
            var factory = new XmlFactory(extent);

            var createdObject = factory.create(null);

            // Creates the referenced thing
            var table = factory.create(null);
            var asObjectTasks = new DatenMeister.Entities.AsObject.FieldInfo.TableView(table);
            var taskColumns = new DotNetSequence(
                ViewHelper.ViewTypes,
                new TextField("Name", "name"),
                new TextField("Start", "startdate"),
                new TextField("Ende", "enddate"),
                new TextField("Finished", "finished"),
                new TextField("Assigned", "assignedPerson"),
                new TextField("Predecessors", "predecessors"));
            asObjectTasks.setFieldInfos(taskColumns);
            table.set("name", "My Table");
            extent.Elements().add(table);
            
            // Creates the detail view
            var detail = factory.create(null);
            detail.set("name", "Person (Detail)");

            var taskDetailColumns = new DotNetSequence(
                viewTypes,
                new TextField("Name", "name"),
                new TextField("Start", "startdate"),
                new MultiReferenceField("Predecessors", "predecessors", "uri?type=Task", "name")
                {
                    tableViewInfo = table
                });
            detail.set("fieldInfos", taskDetailColumns);

            // Now is doing all the checks
            var value = detail.get("fieldInfos").AsReflectiveCollection();
            Assert.That(value, Is.Not.Null);
            Assert.That(value.size(), Is.EqualTo(3));

            // Gets the multireference
            var multiReference = value.ElementAt(2).AsIObject();
            Assert.That(multiReference, Is.Not.Null);
            var referencedTableObject = multiReference.get("tableViewInfo").AsIObject();
            Assert.That(referencedTableObject, Is.Not.Null);
            Assert.That(referencedTableObject.get("name").AsSingle().ToString(), Is.EqualTo("My Table"));
            var tableColumns = referencedTableObject.get("fieldInfos").AsReflectiveCollection();
            Assert.That(tableColumns, Is.Not.Null);
            Assert.That(tableColumns.size(), Is.EqualTo(6));
            Assert.That(tableColumns.ElementAt(2).AsIObject().get("name").AsSingle(), Is.EqualTo("Ende"));
            Assert.That(tableColumns.ElementAt(5).AsIObject().get("name").AsSingle(), Is.EqualTo("Predecessors"));
        }


        public static IObject TypePerson
        {
            get;
            set;
        }

        public static IObject TypeTask
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a simple extent, containing some types and some elements
        /// </summary>
        /// <returns>XmlExtent being created</returns>
        public static XmlExtent CreateRawTestExtent()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                "</root>");

            var typeExtent = new GenericExtent("test:///types");
            TypeTask = new GenericObject();
            TypeTask.set("name", "Task");
            TypePerson = new GenericObject();
            TypePerson.set("name", "Person");
            typeExtent.Elements().add(TypeTask);
            typeExtent.Elements().add(TypePerson);

            var xmlExtent = new XmlExtent(document, "test:///");
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);
            pool.Add(typeExtent, null, ExtentType.Type);

            return xmlExtent;
        }

        /// <summary>
        /// Creates a simple extent, containing some types and some elements
        /// </summary>
        /// <returns>XmlExtent being created</returns>
        public static XmlExtent CreateTestExtent(bool isEmpty = false)
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<tasks />" +
                    "<persons />" +
                "</root>");

            var typeExtent = new GenericExtent("test:///types");
            TypeTask = new GenericObject();
            TypeTask.set("name", "Task");
            TypePerson = new GenericObject();
            TypePerson.set("name", "Person");
            typeExtent.Elements().add(TypeTask);
            typeExtent.Elements().add(TypePerson);

            var xmlExtent = new XmlExtent(document, "test:///");
            xmlExtent.Settings.Mapping.Add("task", TypeTask, x => x.Elements("root").Elements("tasks").FirstOrDefault());
            xmlExtent.Settings.Mapping.Add("person", TypePerson, x => x.Elements("root").Elements("persons").FirstOrDefault());
            xmlExtent.Settings.OnlyUseAssignedNodes = isEmpty; // When an empty node has been requested, we assume untyped objects being stored at root node
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);
            pool.Add(typeExtent, null, ExtentType.Type);

            if (!isEmpty)
            {
                var factory = Factory.GetFor(xmlExtent);
                var person1 = factory.CreateInExtent(xmlExtent, TypePerson);
                var person2 = factory.CreateInExtent(xmlExtent, TypePerson);

                var task1 = factory.CreateInExtent(xmlExtent, TypeTask);

                person1.set("name", "Brenn");
                person1.set("prename", "Martin");

                person2.set("name", "Brenner");
                person2.set("prename", "Martina");

                task1.set("name", "Toller Task");
            }

            return xmlExtent;
        }

        [Test]
        public void TestAutoGeneratedCode()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<comments />" +
                    "<textfields />" +
                "</root>");

            var xmlExtent = new XmlExtent(document, "test:///");

            var umlExtent = new GenericExtent("datenmeister:///metatypes");
            DatenMeister.Entities.AsObject.Uml.Types.Init(umlExtent);
            var typeExtent = DatenMeister.Entities.AsObject.FieldInfo.Types.Init();

            xmlExtent.Settings.Mapping.Add(
                "comment", 
                DatenMeister.Entities.AsObject.FieldInfo.Types.Comment, 
                x => x.Elements("root").Elements("comments").FirstOrDefault());

            xmlExtent.Settings.Mapping.Add(
                "textfield", 
                DatenMeister.Entities.AsObject.FieldInfo.Types.TextField, 
                x => x.Elements("root").Elements("textfields").FirstOrDefault());
            var pool = DatenMeisterPool.Create();
            pool.Add(xmlExtent, null, ExtentType.Data);
            var factory = new FactoryProvider().CreateFor(xmlExtent);

            var comment = factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.Comment);
            var commentObj = new DatenMeister.Entities.AsObject.FieldInfo.Comment(comment);
            commentObj.setName("Description");
            commentObj.setBinding("Binding");

            Assert.That(commentObj.getName(), Is.EqualTo("Description"));
            Assert.That(commentObj.getBinding(), Is.EqualTo("Binding"));
        }

        /// <summary>
        /// Tests the autogenerated code for the enumerations. 
        /// </summary>
        [Test]
        public void TestAutoGeneratedCodeForEnums()
        {
            ApplicationCore.PerformBinding();
            var document = XDocument.Parse(
                "<root>" +
                    "<comments />" +
                    "<textfields />" +
                "</root>");
            var xmlExtent = new XmlExtent(document, "test:///");
            var factory = new FactoryProvider().CreateFor(xmlExtent);
            var xmlNode = factory.create(null);

            DatenMeister.Entities.AsObject.DM.ExtentInfo.setName(xmlNode, "ABC");
            var name = DatenMeister.Entities.AsObject.DM.ExtentInfo.getName(xmlNode);

            DatenMeister.Entities.AsObject.DM.ExtentInfo.setExtentType(xmlNode, ExtentType.Query);
            var type = DatenMeister.Entities.AsObject.DM.ExtentInfo.getExtentType(xmlNode);

            Assert.That(name, Is.EqualTo("ABC"));
            Assert.That(type, Is.EqualTo(ExtentType.Query));
        }

        /// <summary>
        /// Creates a user by given name and first name
        /// </summary>
        /// <returns></returns>
        private static GenericElement CreateUser(string name, string firstName, string street)
        {
            var newElement = new GenericElement();
            newElement.set("name", name);
            newElement.set("firstname", firstName);

            var newAddress = new GenericElement();
            newAddress.set("street", street);
            newAddress.set("zipcode", "12345");
            newAddress.set("town", "My Town");
            newElement.set("address", newAddress);
            return newElement;
        }

        [Test]
        public void TestSkipRootElements()
        {
            var document = XDocument.Parse(
                "<root>" +
                    "<comments />" +
                    "<textfields />" +
                    "<other p3:type=\"Task\" xmlns:p3=\"http://www.omg.org/spec/XMI/2.4.1\" />" +
                "</root>");

            var xmlSettings = new XmlSettings()
            {
                OnlyUseAssignedNodes = false
            };

            var extent = new XmlExtent(document, "no", xmlSettings);
            Assert.That(extent.Elements().Count, Is.EqualTo(3));

            xmlSettings = new XmlSettings()
            {
                OnlyUseAssignedNodes = true
            };

            extent = new XmlExtent(document, "no", xmlSettings);
            Assert.That(extent.Elements().Count, Is.EqualTo(1));
        }
    }
}
