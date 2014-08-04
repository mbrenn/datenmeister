using BurnSystems.ObjectActivation;
using DatenMeister.Transformations;
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

            var resolvedFiltered = pool.ResolveByPath(database.ProjectExtent.ContextURI() + "?type=Task") as IReflectiveCollection;
            Assert.That(resolvedFiltered != null);
            Assert.That(resolvedFiltered.Count(), Is.EqualTo(TestDatabase.TotalTasks));
        }

        [Test]
        public void FilterByPropertyBoolean()
        {
            Global.Reset();
            var db = new TestDatabase();
            var pool = db.Init();
            var filtered = new FilterByPropertyTransformation(
                db.ProjectExtent.Elements(),
                "isFemale",
                true);

            Assert.That(filtered.getAll().Count(), Is.EqualTo(1));
            Assert.That(db.ProjectExtent.Elements().Count(), Is.EqualTo(3));
        }
    }
}
