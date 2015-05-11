using DatenMeister.DataProvider.Generic;
using DatenMeister.Logic;
using DatenMeister.Pool;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.PoolLogic
{
    [TestFixture]
    public class DecouplingTests
    {
        [Test]
        public void TestDecoupledUmlInit()
        {
            Injection.Reset();

            // Creates the datapool with UML information
            var dataPool = DatenMeisterPool.CreateDecoupled();
            var metaTypeExtent = new GenericExtent("datenmeister:///datenmeister/metatypes/");
            DatenMeister.Entities.AsObject.Uml.Types.Init(metaTypeExtent);
            dataPool.Add(metaTypeExtent, null, ExtentType.MetaType);
        }
    }
}
