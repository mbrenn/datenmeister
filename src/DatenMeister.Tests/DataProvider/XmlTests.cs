﻿using BurnSystems.Test;
using DatenMeister;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Assert.That(item0.get("Nix"), Is.EqualTo(ObjectHelper.NotSet));
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
    }
}
