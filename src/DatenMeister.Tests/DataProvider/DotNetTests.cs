using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
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
    public class DotNetTests
    {
        [Test]
        public void TestGettingSetting()
        {
            var extent = new DotNetExtent("test:///");
            var value = new DotNetObject(extent.Elements(), new TestClass(), Guid.Empty.ToString());

            var properties = value.getAll();
            Assert.That(properties.Any(x => x.PropertyName == "TextValue"));
            Assert.That(properties.Any(x => x.PropertyName == "NumberValue"));

            value.set("TextValue", "Dies ist ein Test");
            value.set("NumberValue", 123L);

            Assert.Throws<ArgumentException>(() =>
                {
                    value.set("XYZ", "T");
                });

            var textValue = value.get("TextValue").AsSingle();
            var numberValue = value.get("NumberValue").AsSingle();

            Assert.That(textValue, Is.EqualTo("Dies ist ein Test"));
            Assert.That(numberValue, Is.EqualTo(123L));

            properties = value.getAll();
            Assert.That(properties.Any(x => x.PropertyName == "TextValue"));
            Assert.That(properties.Any(x => x.PropertyName == "NumberValue"));

            Assert.That(properties.Any(x => x.Value.AsSingle().ToString() == "Dies ist ein Test"));
            Assert.That(properties.Any(x => x.Value.AsSingle().ToString() == "123"));
        }

        [Test]
        public void TestGlobalExtentObject()
        {
            ApplicationCore.PerformBinding();

            var extent = new GlobalDotNetExtent();
            var value = new TestClass();
            value.NumberValue = 2;
            value.TextValue = "Test";
            var valueObject = extent.CreateObject(value);

            Assert.That(valueObject.get("NumberValue").AsSingle(), Is.InstanceOf<long>());
            Assert.That((long)valueObject.get("NumberValue").AsSingle(), Is.EqualTo(2));

            Assert.That(valueObject.get("TextValue").AsSingle(), Is.InstanceOf<string>());
            Assert.That((string)valueObject.get("TextValue").AsSingle(), Is.EqualTo("Test"));

            valueObject.set("NumberValue", 5);
            valueObject.set("TextValue", "no Test");

            Assert.That(valueObject.get("NumberValue").AsSingle(), Is.InstanceOf<long>());
            Assert.That((long)valueObject.get("NumberValue").AsSingle(), Is.EqualTo(5));

            Assert.That(valueObject.get("TextValue").AsSingle(), Is.InstanceOf<string>());
            Assert.That((string)valueObject.get("TextValue").AsSingle(), Is.EqualTo("no Test"));
        }
        [Test]
        public void TestPropertiesGlobalExtentObject()
        {
            ApplicationCore.PerformBinding();

            var extent = new GlobalDotNetExtent();
            var value = new TestClass();
            value.NumberValue = 2;
            value.TextValue = "Test";
            var valueObject = extent.CreateObject(value) as IElement;
            Assert.That(valueObject, Is.Not.Null);

            var enumeration = valueObject.getMetaClass().get("ownedAttribute").AsEnumeration().ToList();
            Assert.That(enumeration.Count, Is.EqualTo(2));
            Assert.That(enumeration[0].AsIObject().get("name").AsSingle().ToString(), Is.EqualTo("TextValue"));
            Assert.That(enumeration[1].AsIObject().get("name").AsSingle().ToString(), Is.EqualTo("NumberValue"));
        }

        [Test]
        public void TestGlobalExtentReflectiveSequence()
        {
            ApplicationCore.PerformBinding();
            var extent = new GlobalDotNetExtent();

            var list = new List<TestClass>();
            list.Add(new TestClass() { NumberValue = 3, TextValue = "START" });
            var listObject = extent.CreateReflectiveSequence(list);

            var newElement = new TestClass();
            newElement.NumberValue = 1;
            newElement.TextValue = "TEXT";

            listObject.Add(newElement);

            // Element as typed
            var newElement2 = new GenericElement(type: extent.Mapping.FindByDotNetType(typeof(TestClass)).Type);
            newElement2.set("NumberValue", 2);;
            newElement2.set("TextValue", "More Text");
            listObject.Add(newElement2);

            // Element as non-typed
            var newElement3 = new GenericObject();
            newElement2.set("NumberValue", 2); ;
            newElement2.set("TextValue", "More Text");

            Assert.That(listObject.size(), Is.EqualTo(3));

            // Check first element
            var retrievedElement = listObject.ElementAt(0).AsIObject();
            Assert.That(retrievedElement, Is.Not.Null);

            Assert.That(retrievedElement.get("NumberValue").AsSingle(), Is.InstanceOf<long>());
            Assert.That((long)retrievedElement.get("NumberValue").AsSingle(), Is.EqualTo(3));

            Assert.That(retrievedElement.get("TextValue").AsSingle(), Is.InstanceOf<string>());
            Assert.That((string)retrievedElement.get("TextValue").AsSingle(), Is.EqualTo("START"));

            // Check second element
            retrievedElement = listObject.ElementAt(1).AsIObject();
            Assert.That(retrievedElement, Is.Not.Null);

            Assert.That(retrievedElement.get("NumberValue").AsSingle(), Is.InstanceOf<long>());
            Assert.That((long)retrievedElement.get("NumberValue").AsSingle(), Is.EqualTo(1));

            Assert.That(retrievedElement.get("TextValue").AsSingle(), Is.InstanceOf<string>());
            Assert.That((string)retrievedElement.get("TextValue").AsSingle(), Is.EqualTo("TEXT"));

            // Check third element
            retrievedElement = listObject.ElementAt(2).AsIObject();
            Assert.That(retrievedElement, Is.Not.Null);

            Assert.That(retrievedElement.get("NumberValue").AsSingle(), Is.InstanceOf<long>());
            Assert.That((long)retrievedElement.get("NumberValue").AsSingle(), Is.EqualTo(2));

            Assert.That(retrievedElement.get("TextValue").AsSingle(), Is.InstanceOf<string>());
            Assert.That((string)retrievedElement.get("TextValue").AsSingle(), Is.EqualTo("More Text"));

            listObject.RemoveAt(1);
            Assert.That(listObject.size(), Is.EqualTo(2));
        }

        public class TestClass
        {
            public string TextValue
            {
                get;
                set;
            }

            public long NumberValue
            {
                get;
                set;
            }
        }
    }
}
