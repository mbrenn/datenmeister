using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Tests.PoolLogic
{
    [TestFixture]
    public class FilterTests
    {
        [Test]
        public void FilterByTypeTest()
        {
            var database = new TestDatabase();
            var pool = database.Init();
         
            var resolved = pool.ResolveByPath(database.ProjectExtent.ContextURI()) as IURIExtent;
            Assert.That(resolved != null);
            Assert.That(resolved.Elements().Count(), Is.EqualTo(TestDatabase.TotalElements));

            var resolvedFiltered = pool.ResolveByPath(database.ProjectExtent.ContextURI() + "?type=Task") as IURIExtent;
            Assert.That(resolvedFiltered != null);
            Assert.That(resolvedFiltered.Elements().Count(), Is.EqualTo(TestDatabase.TotalTasks));
        }
    }
}
