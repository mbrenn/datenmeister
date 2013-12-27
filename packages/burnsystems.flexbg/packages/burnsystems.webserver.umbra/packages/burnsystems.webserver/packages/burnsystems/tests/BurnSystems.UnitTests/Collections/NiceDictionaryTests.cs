using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.Collections;

namespace BurnSystems.UnitTests.Collections
{
    /// <summary>
    /// Implements some tests for the nice dictionary
    /// </summary>
    [TestFixture]
    public class NiceDictionaryTests
    {
        [Test]
        public void TestEntries()
        {
            var niceDictionary = new NiceDictionary<int, int>();
            niceDictionary[2] = 4;
            niceDictionary[5] = 25;

            Assert.That(niceDictionary[2], Is.EqualTo(4));
            Assert.That(niceDictionary[5], Is.EqualTo(25));
            Assert.That(niceDictionary[3], Is.EqualTo(0));

            var niceDictionary2 = new NiceDictionary<string, string>();
            niceDictionary2["four"] = "4";
            niceDictionary2["five"] = "5";

            Assert.That(niceDictionary2["four"], Is.EqualTo("4"));
            Assert.That(niceDictionary2["five"], Is.EqualTo("5"));
            Assert.That(niceDictionary2["six"], Is.Null);
        }
    }
}
