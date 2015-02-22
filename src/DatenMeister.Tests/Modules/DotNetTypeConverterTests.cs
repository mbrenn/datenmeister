using DatenMeister.DataProvider;
using DatenMeister.DataProvider.Generic;
using DatenMeister.Entities.AsObject.Uml;
using DatenMeister.Logic.TypeConverter;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class DotNetTypeConverterTests
    {
        [Test]
        public void TestDotNetTypeConverter()
        {
            Injection.Reset();
            Injection.Application.Bind<IFactoryProvider>().To<FactoryProvider>();

            var metaTypes = new GenericExtent("datenmeister:///test/types");
            var genericExtent = new GenericExtent("datenmeister:///test/types");
            var factoryProvider = new FactoryProvider();
            DatenMeister.Entities.AsObject.Uml.Types.Init(metaTypes);

            var dotNetTypeConverter = new DotNetTypeConverter();
            dotNetTypeConverter.FactoryProvider = factoryProvider;

            var createdType = dotNetTypeConverter.Convert(genericExtent, typeof(TestDatabase.Person));
            Assert.That(createdType.get("name").AsSingle().ToString(), Is.EqualTo("DatenMeister.Tests.TestDatabase+Person"));

            var attributes = createdType.get("ownedAttribute").AsEnumeration().ToList();
            Assert.That(attributes.Any(x => x.AsIObject().get("name").AsSingle().ToString() == "FirstName"));
            Assert.That(attributes.Any(x => x.AsIObject().get("name").AsSingle().ToString() == "LastName"));
            Assert.That(attributes.Any(x => x.AsIObject().get("name").AsSingle().ToString() == "Age"));
            Assert.That(!attributes.Any(x => x.AsIObject().get("name").AsSingle().ToString() == "PrivateVariable"));
            Assert.That(!attributes.Any(x => x.AsIObject().get("name").AsSingle().ToString() == "StaticVariable"));
        }
    }
}
