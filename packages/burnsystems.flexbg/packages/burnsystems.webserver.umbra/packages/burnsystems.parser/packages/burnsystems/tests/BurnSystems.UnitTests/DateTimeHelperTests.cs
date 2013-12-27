using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace BurnSystems.UnitTests
{
    /// <summary>
    /// Test class for DateTimeHelper
    /// </summary>
    [TestFixture]
    public class DateTimeHelperTests
    {
        [Test]
        public void TestGoBackToHour()
        {
            var time = new DateTime(8, 9, 10, 23, 10, 10); // 10.09.08 23:10:10
            var timeBack10 = time.GoBackToHour(10);
            var timeBack23 = time.GoBackToHour(23);

            Assert.That(timeBack10, Is.EqualTo(new DateTime(8, 9, 10, 10, 0, 0)));
            Assert.That(timeBack23, Is.EqualTo(new DateTime(8, 9, 10, 23, 0, 0)));

            time = new DateTime(8, 9, 10, 11, 10, 10); // 10.09.08 23:10:10
            timeBack10 = time.GoBackToHour(10);
            timeBack23 = time.GoBackToHour(23);

            Assert.That(timeBack10, Is.EqualTo(new DateTime(8, 9, 10, 10, 0, 0)));
            Assert.That(timeBack23, Is.EqualTo(new DateTime(8, 9, 09, 23, 0, 0)));
        }
    }
}
