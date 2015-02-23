using DatenMeister.DataProvider;
using DatenMeister.DataProvider.Generic;
using DatenMeister.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Tests.DataProvider
{
    [TestFixture]
    public class GenericTests
    {
        /// <summary>
        /// Performs some tests on the GenericObject by using 'AsSingle()'
        /// </summary>
        [Test]
        public void TestAsSingle()
        {
            var genericObject = new GenericObject();
            genericObject.set("name", "Brenn");
            genericObject.set("prename", "Martin");

            Assert.That(genericObject.getAsSingle("name"), Is.EqualTo("Brenn"));
            Assert.That(genericObject.getAsSingle("prename"), Is.EqualTo("Martin"));
            Assert.That(genericObject.getAsSingle("notset"), Is.EqualTo(ObjectHelper.NotSet));

            Assert.That(genericObject.isSet("name"), Is.True);
            Assert.That(genericObject.isSet("notset"), Is.False);

            Assert.That(genericObject.unset("name"), Is.True);
            Assert.That(genericObject.unset("notset"), Is.False);
            Assert.That(genericObject.unset("name"), Is.False);

            Assert.That(genericObject.getAsSingle("name"), Is.EqualTo(ObjectHelper.NotSet));
        }

        /// <summary>
        /// Performs some tests on the GenericObject by using 'AsReflectiveSequence()'.
        /// This test does not validate full sequence
        /// </summary>
        [Test]
        public void TestAsReflectiveSequence()
        {
            var genericObject = new GenericObject();
            genericObject.set("name", "Brenn");
            genericObject.set("prename", "Martin");

            var children = genericObject.getAsReflectiveSequence("children");
            children.add("Child 1");
            children.add("Child 2");

            var childrenTest = genericObject.getAsReflectiveSequence("children");
            foreach (var child in childrenTest)
            {
                Assert.That(child.ToString(), Is.EqualTo("Child 1").Or.EqualTo("Child 2"));
            }

            var childrenTest2 = genericObject.getAsReflectiveSequence("children");
            foreach (var child in childrenTest2)
            {
                Assert.That(child.ToString(), Is.EqualTo("Child 1").Or.EqualTo("Child 2"));
            }

            var childrenTest3 = genericObject.getAsReflectiveSequence("children");
            foreach (var child in childrenTest3)
            {
                Assert.That(child.AsSingle().ToString(), Is.EqualTo("Child 1").Or.EqualTo("Child 2"));
            }

            children.remove("Child 2");
            foreach (var child in (childrenTest))
            {
                Assert.That(child.ToString(), Is.EqualTo("Child 1"));
            }
        }

        [Test]
        public void TestGetTypeOfEnumerationByType()
        {
            Assert.That(
                ObjectConversion.GetTypeOfEnumerableByType(typeof(string)),
                Is.Null);

            Assert.That(
                ObjectConversion.GetTypeOfEnumerableByType(typeof(int)),
                Is.Null);

            Assert.That(
                ObjectConversion.GetTypeOfEnumerableByType(typeof(ConsoleColor)),
                Is.Null);

            Assert.That(
                ObjectConversion.GetTypeOfEnumerableByType(typeof(TimeZone)),
                Is.Null);

            Assert.That(
                ObjectConversion.GetTypeOfEnumerableByType(typeof(List<int>)),
                Is.EqualTo(typeof(int)));
            Assert.That(
                ObjectConversion.GetTypeOfEnumerableByType(typeof(List<string>)),
                Is.EqualTo(typeof(string)));
            Assert.That(
                ObjectConversion.GetTypeOfEnumerableByType(typeof(List<TimeZone>)),
                Is.EqualTo(typeof(TimeZone)));

            Assert.That(
                ObjectConversion.GetTypeOfEnumerableByType(typeof(System.Collections.Generic.LinkedList<double>)),
                Is.EqualTo(typeof(double)));
        }
    }
}
