using DatenMeister.DataProvider;
using DatenMeister.Logic;
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
            var db = new TestDatabase();
            var pool = db.Init();
            var filtered =
                db.ProjectExtent.Elements().FilterByProperty("isFemale", true);

            Assert.That(filtered.Count(), Is.EqualTo(1));
            Assert.That(db.ProjectExtent.Elements().Count(), Is.EqualTo(3));

            var filtered2 =
                db.ProjectExtent.Elements().FilterByProperty("isFemale", false);
            Assert.That(filtered2.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FilterByExtentTypeTest()
        {
            var database = new TestDatabase();
            var pool = database.Init();

            var filtered = database.ProjectExtent.Elements().FilterByExtentType(ExtentType.Query);
            Assert.That(filtered.Count(), Is.EqualTo(0));

            filtered = database.ProjectExtent.Elements().FilterByExtentType(ExtentType.Data);
            Assert.That(filtered.Count(), Is.EqualTo(TestDatabase.TotalElements));
        }

        [Test]
        public void TestMakeUnique()
        {
            var extent = new GenericExtent("dm:///test");
            var obj1 = new GenericObject();
            var obj2 = new GenericObject(); 
            var obj3 = new GenericObject();

            extent.Elements().Add(obj1);
            extent.Elements().Add(obj1);
            extent.Elements().Add(obj2);
            extent.Elements().Add(obj3);
            extent.Elements().Add(obj3);

            Assert.That(extent.Elements().Count(), Is.EqualTo(5));

            Assert.That(extent.Elements().MakeUnique().Count(), Is.EqualTo(3));
        }
    }
}
