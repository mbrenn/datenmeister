using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems;
using System.Xml.Linq;

namespace BurnSystems.UnitTests
{
    /// <summary>
    /// Contains some test checking the XmlHelper-class
    /// </summary>
    [TestFixture]
    public class XmlHelperTests
    {
        [Test]
        public void GetExistingElementViaGetOrCreate()
        {
            var xDocument = new XDocument(
                new XElement(
                    "tests",
                    new XElement(
                        "existing",
                        new XAttribute(
                            "Wert",
                            "12"))));

            var xmlTests = xDocument.Element("tests");
            var xmlExisting = xmlTests.Element("existing");

            var xmlExistingCheck = xDocument.Elements("tests").GetOrCreateLastElement("existing");

            Assert.That(xmlExistingCheck, Is.SameAs(xmlExisting));
        }

        [Test]
        public void GetExistingAttributeViaGetOrCreate()
        {
            var xDocument = new XDocument(
                new XElement(
                    "tests",
                    new XElement(
                        "existing",
                        new XAttribute(
                            "Wert",
                            "12"))));

            var xmlTests = xDocument.Element("tests");
            var xmlExisting = xmlTests.Element("existing");
            var xmlAttribute = xmlExisting.Attribute("Wert");

            var xmlAttributeCheck = xDocument.Elements("tests").Elements("existing").GetOrCreateLastAttribute("Wert");

            Assert.That(xmlAttributeCheck, Is.SameAs(xmlAttribute));
        }

        [Test]
        public void CreateAttributeViaGetOrCreate()
        {
            var xDocument = new XDocument(
                new XElement(
                    "tests",
                    new XElement(
                        "existing",
                        new XAttribute(
                            "Wert",
                            "12"))));

            var xmlTests = xDocument.Element("tests");
            var xmlExisting = xmlTests.Element("existing");
            var xmlAttribute = xmlExisting.Attribute("Wert");

            var xmlAttributeCheck = xDocument.Elements("tests").Elements("existing").GetOrCreateLastAttribute("Value", "test");
            Assert.That(xmlAttributeCheck, Is.Not.Null);
            Assert.That(xmlAttributeCheck, Is.Not.SameAs(xmlAttribute));

            var xmlAttributeCorrect = xmlExisting.Attribute("Value");

            Assert.That(xmlAttributeCorrect, Is.SameAs(xmlAttributeCheck));
            Assert.That(xmlAttributeCorrect.Value, Is.EqualTo("test"));
            Assert.That(xmlAttributeCheck.Value, Is.EqualTo("test"));
        }

        [Test]
        public void CreateElementViaGetOrCreate()
        {
            var xDocument = new XDocument(
                new XElement(
                    "tests",
                    new XElement(
                        "existing",
                        new XAttribute(
                            "Wert",
                            "12"))));

            var xmlTests = xDocument.Element("tests");
            var xmlExisting = xmlTests.Element("existing");

            var xmlElementCheck = xDocument.Elements("tests").GetOrCreateLastElement("notexisting");
            Assert.That(xmlElementCheck, Is.Not.Null);
            Assert.That(xmlElementCheck, Is.Not.SameAs(xmlExisting));

            var xmlElementCorrect = xmlTests.Element("notexisting");

            Assert.That(xmlElementCorrect, Is.SameAs(xmlElementCheck));
        }

        [Test]
        public void GetLastElementViaGetOrCreate()
        {
            var xDocument = new XDocument(
                new XElement(
                    "tests",
                    new XElement(
                        "existing",
                        new XAttribute(
                            "Wert",
                            "12")),
                    new XElement(
                        "existing",
                        new XAttribute(
                            "Wert",
                            "25"))));

            var xmlTests = xDocument.Element("tests");
            var xmlExisting = xmlTests.Elements("existing").Last();

            var xmlElementCheck = xDocument.Elements("tests").GetOrCreateLastElement("existing");
            Assert.That(xmlElementCheck, Is.Not.Null);
            Assert.That(xmlElementCheck, Is.SameAs(xmlExisting));
            Assert.That(xmlElementCheck.Attribute("Wert").Value, Is.EqualTo("25"));
        }

        [Test]
        public void GetLastAttributeViaGetOrCreate()
        {
            var xDocument = new XDocument(
                new XElement(
                    "tests",
                    new XElement(
                        "existing",
                        new XAttribute(
                            "Wert",
                            "12")),
                    new XElement(
                        "existing",
                        new XAttribute(
                            "Wert",
                            "25"))));

            var xmlTests = xDocument.Element("tests");
            var xmlExisting = xmlTests.Elements("existing").Last();
            var xmlAttribute = xmlExisting.Attribute("Wert");

            var xmlAttributeCheck = xDocument.Elements("tests").Elements("existing").GetOrCreateLastAttribute("Wert");
            Assert.That(xmlAttributeCheck, Is.Not.Null);
            Assert.That(xmlAttributeCheck, Is.SameAs(xmlAttribute));
            Assert.That(xmlAttributeCheck.Value, Is.EqualTo("25"));
        }
    }
}
