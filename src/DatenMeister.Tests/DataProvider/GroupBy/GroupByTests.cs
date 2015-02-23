using DatenMeister.Transformations.GroupBy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.DataProvider.GroupBy
{
    /// <summary>
    /// Contains some tests for the group by
    /// </summary>
    [TestFixture]
    public class GroupByTests
    {
        [Test]
        public void TestByType()
        {
            var testExtent = XmlTests.CreateTestExtent();

            var groupBy = new GroupByTypeTransformation(testExtent.Elements());
            var elements = groupBy.getAll();

            Assert.That(elements.Count(), Is.EqualTo(2));
            foreach (var element in elements)
            {   
                var elementAsObject = element as GroupByObject;
                Assert.That(elementAsObject, Is.Not.Null);
                var key = elementAsObject.key;
                var value = elementAsObject.values; 

                // Checking, if request by normal request is also working
                var key2 = elementAsObject.getAsSingle("key");
                var values2 = elementAsObject.getAsReflectiveSequence("values");
                Assert.That(key, Is.EqualTo(key2));
                Assert.That(value, Is.EqualTo(values2));

                // Do the content test
                if (key == XmlTests.TypePerson)
                {
                    Assert.That(value.Count(), Is.EqualTo(2));
                    var personBrenn = value.Where(x => x.AsIObject().getAsSingle("name").ToString() == "Brenn").FirstOrDefault();
                    Assert.That(personBrenn, Is.Not.Null);
                }
                else if (key == XmlTests.TypeTask)
                {
                    Assert.That(value.Count(), Is.EqualTo(1));
                }
                else
                {
                    Assert.Fail("key is not of type person and task");
                }
            }
        }
    }
}
