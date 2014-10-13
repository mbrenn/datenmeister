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
        [Test]
        public void TestTypes()
        {
            var core = new ApplicationCore();
            core.Start<DummyDatenMeisterSettings>();
            var  metaTypeExtent = new GenericExtent("datenmeister:///datenmeister/metatypes/");
            DatenMeister.Entities.AsObject.Uml.Types.Init(metaTypeExtent);
        }

    }
}
