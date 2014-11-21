using DatenMeister.DataProvider.Wrapper;
using DatenMeister.DataProvider.Wrapper.EventOnChange;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
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
    public class WrapperTests
    {

        [Test]
        public void TestWrapperExtent()
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
            pool.Add(
                new WrapperExtent<WrapperReflectiveSequence, WrapperElement, WrapperUnspecified>(xmlExtent),
                null,
                ExtentType.Data);

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
        public void TestEventOnChangeExtent()
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
            var onChangeExtent = new EventOnChangeExtent(xmlExtent);
            var ev = 0;
            onChangeExtent.ChangeInExtent += (x, y) => { 
                ev++; 
            };
            pool.Add(onChangeExtent, null, ExtentType.Data);

            Assert.That(ev, Is.EqualTo(0));

            var valueE4 = pool.ResolveByPath("test:///#e4") as IObject;
            var sequence = valueE4.get("value").AsReflectiveSequence();
            sequence.add("Value 1");
            sequence.add("Value 2");
            sequence.add("Value 3");

            Assert.That(ev, Is.GreaterThan(0));
            ev = 0;

            // Checks, if setting had been successful
            sequence = valueE4.get("value").AsReflectiveSequence();
            Assert.That(sequence.size(), Is.EqualTo(3));
            var value1 = sequence.get(0);
            var value2 = sequence.get(1);
            var value3 = sequence.get(2);

            Assert.That(value1, Is.EqualTo("Value 1"));
            Assert.That(value2, Is.EqualTo("Value 2"));
            Assert.That(value3, Is.EqualTo("Value 3"));
            Assert.That(ev, Is.EqualTo(0));

            valueE4.set("test", "TEST2");
            Assert.That(ev, Is.GreaterThan(0));
        }
    }
}
