using DatenMeister.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests
{
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void TestAsString()
        {
            Assert.That(Extensions.AsSingle(null), Is.EqualTo(ObjectHelper.Null));
            Assert.That(Extensions.AsSingle("TEST"), Is.EqualTo("TEST"));
        }
    }
}
