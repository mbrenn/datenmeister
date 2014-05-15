using DatenMeister.DataProvider;
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

            Assert.That(genericObject.get("name").AsSingle(), Is.EqualTo("Brenn"));
            Assert.That(genericObject.get("prename").AsSingle(), Is.EqualTo("Martin"));
            Assert.That(genericObject.get("notset").AsSingle(), Is.EqualTo(ObjectHelper.NotSet));

            Assert.That(genericObject.isSet("name"), Is.True);
            Assert.That(genericObject.isSet("notset"), Is.False);

            Assert.That(genericObject.unset("name"), Is.True);
            Assert.That(genericObject.unset("notset"), Is.False);
            Assert.That(genericObject.unset("name"), Is.False);

            Assert.That(genericObject.get("name").AsSingle(), Is.EqualTo(ObjectHelper.NotSet));
        }
    }
}
