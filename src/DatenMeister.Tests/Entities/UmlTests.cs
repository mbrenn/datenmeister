using DatenMeister.DataProvider;
using DatenMeister.Logic;
using DatenMeister.Logic.Settings;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.Entities
{
    [TestFixture]
    public class UmlTests
    {
        /// <summary>
        /// Tests the UML types
        /// </summary>
        [Test]
        public void TestTypes()
        {
            var core = new ApplicationCore();
            core.Start<DummyDatenMeisterSettings>();
            var metaTypeExtent = new GenericExtent("datenmeister:///datenmeister/metatypes/");
            DatenMeister.Entities.AsObject.Uml.Types.Init(metaTypeExtent);

            Assert.That(DatenMeister.Entities.AsObject.Uml.Types.Type, Is.Not.Null);
            Assert.That(DatenMeister.Entities.AsObject.Uml.Types.NamedElement, Is.Not.Null);

            // Checks the names
            Assert.That(
                DatenMeister.Entities.AsObject.Uml.Types.Type.get("name").AsSingle().ToString(),
                Is.EqualTo("Type"));
            Assert.That(
                DatenMeister.Entities.AsObject.Uml.Types.NamedElement.get("name").AsSingle().ToString(),
                Is.EqualTo("NamedElement"));
            Assert.That(
                DatenMeister.Entities.AsObject.Uml.Types.Property.get("name").AsSingle().ToString(),
                Is.EqualTo("Property"));

            // Checks the types
            Assert.That(
                (DatenMeister.Entities.AsObject.Uml.Types.Type as IElement).getMetaClass(),
                Is.EqualTo(DatenMeister.Entities.AsObject.Uml.Types.Type));
            Assert.That(
                (DatenMeister.Entities.AsObject.Uml.Types.NamedElement as IElement).getMetaClass(),
                Is.EqualTo(DatenMeister.Entities.AsObject.Uml.Types.Type));
            Assert.That(
                (DatenMeister.Entities.AsObject.Uml.Types.Property as IElement).getMetaClass(),
                Is.EqualTo(DatenMeister.Entities.AsObject.Uml.Types.Type));
        }

    }
}
