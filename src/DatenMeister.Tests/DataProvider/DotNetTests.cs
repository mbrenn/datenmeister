using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.DataProvider.Generic;
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

            var textValue = value.getAsSingle("TextValue");
            var numberValue = value.getAsSingle("NumberValue");

            Assert.That(textValue, Is.EqualTo("Dies ist ein Test"));
            Assert.That(numberValue, Is.EqualTo(123L));

            properties = value.getAll();
            Assert.That(properties.Any(x => x.PropertyName == "TextValue"));
            Assert.That(properties.Any(x => x.PropertyName == "NumberValue"));

            Assert.That(properties.Any(x => x.Value.ToString() == "Dies ist ein Test"));
            Assert.That(properties.Any(x => x.Value.ToString() == "123"));
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

            Assert.That(valueObject.getAsSingle("NumberValue"), Is.InstanceOf<long>());
            Assert.That((long)valueObject.getAsSingle("NumberValue"), Is.EqualTo(2));

            Assert.That(valueObject.getAsSingle("TextValue"), Is.InstanceOf<string>());
            Assert.That((string)valueObject.getAsSingle("TextValue"), Is.EqualTo("Test"));

            valueObject.set("NumberValue", 5);
            valueObject.set("TextValue", "no Test");

            Assert.That(valueObject.getAsSingle("NumberValue"), Is.InstanceOf<long>());
            Assert.That((long)valueObject.getAsSingle("NumberValue"), Is.EqualTo(5));

            Assert.That(valueObject.getAsSingle("TextValue"), Is.InstanceOf<string>());
            Assert.That((string)valueObject.getAsSingle("TextValue"), Is.EqualTo("no Test"));
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

            var enumeration = valueObject.getMetaClass().getAsReflectiveSequence("ownedAttribute").ToList();
            Assert.That(enumeration.Count, Is.EqualTo(2));
            Assert.That(enumeration[0].AsIObject().getAsSingle("name").ToString(), Is.EqualTo("TextValue"));
            Assert.That(enumeration[1].AsIObject().getAsSingle("name").ToString(), Is.EqualTo("NumberValue"));
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
            newElement2.set("NumberValue", 2); ;
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

            Assert.That(retrievedElement.getAsSingle("NumberValue"), Is.InstanceOf<long>());
            Assert.That((long)retrievedElement.getAsSingle("NumberValue"), Is.EqualTo(3));

            Assert.That(retrievedElement.getAsSingle("TextValue"), Is.InstanceOf<string>());
            Assert.That((string)retrievedElement.getAsSingle("TextValue"), Is.EqualTo("START"));

            // Check second element
            retrievedElement = listObject.ElementAt(1).AsIObject();
            Assert.That(retrievedElement, Is.Not.Null);

            Assert.That(retrievedElement.getAsSingle("NumberValue"), Is.InstanceOf<long>());
            Assert.That((long)retrievedElement.getAsSingle("NumberValue"), Is.EqualTo(1));

            Assert.That(retrievedElement.getAsSingle("TextValue"), Is.InstanceOf<string>());
            Assert.That((string)retrievedElement.getAsSingle("TextValue"), Is.EqualTo("TEXT"));

            // Check third element
            retrievedElement = listObject.ElementAt(2).AsIObject();
            Assert.That(retrievedElement, Is.Not.Null);

            Assert.That(retrievedElement.getAsSingle("NumberValue"), Is.InstanceOf<long>());
            Assert.That((long)retrievedElement.getAsSingle("NumberValue"), Is.EqualTo(2));

            Assert.That(retrievedElement.getAsSingle("TextValue"), Is.InstanceOf<string>());
            Assert.That((string)retrievedElement.getAsSingle("TextValue"), Is.EqualTo("More Text"));

            listObject.RemoveAt(1);
            Assert.That(listObject.size(), Is.EqualTo(2));
        }

        [Test]
        public void TestListClasses()
        {
            var extent = new DotNetExtent("test:///");

            var netValue = new TestStringClass();
            netValue.Values.Add("A");
            netValue.Values.Add("B");

            var value = new DotNetObject(extent.Elements(), netValue, Guid.Empty.ToString());

            foreach (var x in value.getAsReflectiveSequence("Values"))
            {
                Assert.That(x, Is.TypeOf<string>());
                Assert.That(x.ToString(), Is.EqualTo("A").Or.EqualTo("B"));
            }

            foreach (var pair in value.getAll())
            {
                Assert.That(pair.PropertyName, Is.EqualTo("Values"));

                foreach (var x in (pair.Value as IReflectiveCollection))
                {
                    Assert.That(x, Is.TypeOf<string>());
                    Assert.That(x.ToString(), Is.EqualTo("A").Or.EqualTo("B"));
                }
            }
        }

        [Test]
        public void TestEmbeddedClasses()
        {
            var extent = new DotNetExtent("test:///");

            var netValue = new TestEmbeddedClass();
            netValue.TextValue = "Outer";
            var innerNetValue = new TestEmbeddedClass();
            innerNetValue.TextValue = "Inner";
            netValue.InnerValue = innerNetValue;

            var value = new DotNetObject(extent.Elements(), netValue, Guid.Empty.ToString());
            Assert.That(value.getAsSingle("TextValue"), Is.EqualTo("Outer"));

            var innerValue = value.getAsSingle("InnerValue").AsIObject();
            Assert.That(innerValue.getAsSingle("TextValue"), Is.EqualTo("Inner"));
            Assert.That(
                ObjectConversion.IsNull(
                    innerValue.getAsSingle("InnerValue")),
                Is.True);
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

        public class TestEmbeddedClass
        {
            public string TextValue
            {
                get;
                set;
            }

            public TestEmbeddedClass InnerValue
            {
                get;
                set;
            }
        }

        public class TestStringClass
        {
            public TestStringClass()
            {
                this.Values = new List<string>();
            }

            public List<string> Values
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Defines a test class that can be used to test the creation of instances
        /// </summary>
        public class TestListOfTestClasses
        {
            public string InnerValue
            {
                get;
                set;
            }

            public List<TestClass> Instances
            {
                get;
                set;
            }

            public TestListOfTestClasses()
            {
                this.Instances = new List<TestClass>();
            }
        }
    }
}
