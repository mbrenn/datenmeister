using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.Collections;
using NUnit.Framework;

namespace BurnSystems.UnitTests.Collections
{
    /// <summary>
    /// Implements some tests for the autodictionary
    /// </summary>
    [TestFixture]
    public class AutoDictionaryTests
    {
        [Test]
        public void TestAutoInsert()
        {
            var dictionary = new AutoDictionary<Item>();

            var itemA_1 = new Item("A", "1");
            var itemB_2 = new Item("B", "2");
            var itemB_3 = new Item("B", "3");
            var itemC_2 = new Item("C", "2");

            dictionary.Add(itemA_1);
            dictionary.Add(itemB_2);
            dictionary.Add(itemB_3);
            dictionary.Add(itemC_2);
			
            Assert.That(dictionary.Count, Is.EqualTo(3));
            Assert.That(dictionary["A"].Value, Is.EqualTo("1"));
            Assert.That(dictionary["B"].Value, Is.EqualTo("3"));
            Assert.That(dictionary["C"].Value, Is.EqualTo("2"));
        }

        /// <summary>
        /// Defines the item which is used to test the autodictionary
        /// </summary>
        public class Item : IHasKey
        {
            public Item(string key, string value)
            {
                this.Key = key;
                this.Value = value;
            }

            /// <summary>
            /// Gets or sets the key
            /// </summary>
            public string Key
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the value
            /// </summary>
            public string Value
            {
                get;
                set;
            }
        }
    }
}
