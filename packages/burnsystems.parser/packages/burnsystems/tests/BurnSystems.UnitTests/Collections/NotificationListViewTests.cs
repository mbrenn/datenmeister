using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.Collections;
using System.ComponentModel;
using System.Collections.Specialized;

namespace BurnSystems.UnitTests.Collections
{
	/*
    /// <summary>
    /// This class contains some tests for the NotificationListView class
    /// </summary>
    [TestFixture]
    public class NotificationListViewTests
    {
        [Test]
        public void TestBasicFunctions()
        {
            var list = new NotificationListView<string>();
            list.Add("A");
            Assert.That(list.Contains("A"), Is.True);

            list.Add("B");
            list.Add("C");

            Assert.That(list.IndexOf("C"), Is.EqualTo(2));

            list.Remove("B");
            Assert.That(list.Contains("B"), Is.False);
            Assert.That(list.IndexOf("C"), Is.EqualTo(1));

            list.RemoveAt(1);
            Assert.That(list.Count, Is.EqualTo(1));
            list.Add("D");
            list.Add("E");

            list.Insert(1, "B");
            Assert.That(list.Count, Is.EqualTo(4));

            var array = new string[4];
            list.CopyTo(array, 0);

            Assert.That(array[0], Is.EqualTo("A"));
            Assert.That(array[1], Is.EqualTo("B"));
            Assert.That(array[2], Is.EqualTo("D"));
            Assert.That(array[3], Is.EqualTo("E"));
        }

        [Test]
        public void TestPropertyChanges()
        {
            var list = new NotificationListView<string>();
            var last = new LastPropertyChanges();
            list.PropertyChanged += (x, y) => last.PropertyNames.Add(y.PropertyName);

            list.Add("A");
            Assert.That(last.PropertyNames, Contains.Item("Item[]"));
            Assert.That(last.PropertyNames, Contains.Item("Count"));
            last.PropertyNames.Clear();

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(last.PropertyNames.Count, Is.EqualTo(0));

            list.Add("B");
            last.PropertyNames.Clear();

            list.Remove("B");
            Assert.That(last.PropertyNames, Contains.Item("Item[]"));
            Assert.That(last.PropertyNames, Contains.Item("Count"));
            last.PropertyNames.Clear();

            list[0] = "C";
            Assert.That(last.PropertyNames, Contains.Item("Item[]"));
            Assert.That(last.PropertyNames.Contains("Count"), Is.False);

            list.Add("D");
            last.PropertyNames.Clear();

            list.Insert(1, "C");
            Assert.That(last.PropertyNames, Contains.Item("Item[]"));
            Assert.That(last.PropertyNames, Contains.Item("Count"));
            last.PropertyNames.Clear();

            list.RemoveAt(0);
            Assert.That(last.PropertyNames, Contains.Item("Item[]"));
            Assert.That(last.PropertyNames, Contains.Item("Count"));
            last.PropertyNames.Clear();

            list.Clear();
            Assert.That(last.PropertyNames, Contains.Item("Item[]"));
            Assert.That(last.PropertyNames, Contains.Item("Count"));
            last.PropertyNames.Clear();
        }

        [Test]
        public void TestCollectionChanges()
        {
            var list = new NotificationListView<string>();
            var last = new LastCollectionChanges();
            list.CollectionChanged += (x, y) => last.Arguments.Add(y);

            list.Add("A");
            var added = last.Arguments.Where(z => z.Action == NotifyCollectionChangedAction.Add).FirstOrDefault();
            Assert.That(last.Arguments.Count, Is.EqualTo(1));
            Assert.That(added, Is.Not.Null);
            Assert.That(added.Action == NotifyCollectionChangedAction.Add, Is.True);
            Assert.That(added.NewStartingIndex == 0, Is.True);
            Assert.That(added.NewItems.Count, Is.EqualTo(1));
            Assert.That(added.NewItems[0], Is.EqualTo("A"));

            last.Arguments.Clear();

            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(last.Arguments.Count, Is.EqualTo(0));

            list.Add("B");
            last.Arguments.Clear();

            list.Remove("B");
            Assert.That(last.Arguments.Count, Is.EqualTo(1));
            var deleted = last.Arguments.Where(z => z.Action == NotifyCollectionChangedAction.Remove).FirstOrDefault();
            Assert.That(deleted, Is.Not.Null);
            Assert.That(deleted.OldStartingIndex == 1, Is.True);
            Assert.That(deleted.OldItems[0], Is.EqualTo("B"));
            
            list.Add("B");
            last.Arguments.Clear();

            list[1] = "C";
            Assert.That(last.Arguments.Count, Is.EqualTo(1));
            var replaced = last.Arguments.Where(z => z.Action == NotifyCollectionChangedAction.Replace).FirstOrDefault();
            Assert.That(replaced.NewStartingIndex == 1, Is.True );
            Assert.That(replaced.OldStartingIndex == 1, Is.True);
            Assert.That(replaced.OldItems[0], Is.EqualTo("B"));
            Assert.That(replaced.NewItems[0], Is.EqualTo("C"));

            list.Add("D");
            last.Arguments.Clear();

            list.Insert(1, "E");
            Assert.That(last.Arguments.Count, Is.EqualTo(1));
            var inserted = last.Arguments.Where(z => z.Action == NotifyCollectionChangedAction.Add).FirstOrDefault();
            Assert.That(inserted.Action == NotifyCollectionChangedAction.Add, Is.True);
            Assert.That(inserted.NewStartingIndex == 1, Is.True);
            Assert.That(inserted.NewItems.Count, Is.EqualTo(1));
            Assert.That(inserted.NewItems[0], Is.EqualTo("E"));
            last.Arguments.Clear();

            list.RemoveAt(1);
            Assert.That(last.Arguments.Count, Is.EqualTo(1));
            deleted = last.Arguments.Where(z => z.Action == NotifyCollectionChangedAction.Remove).FirstOrDefault();
            Assert.That(deleted, Is.Not.Null);
            Assert.That(deleted.OldStartingIndex == 1, Is.True);
            Assert.That(deleted.OldItems[0], Is.EqualTo("E"));
            last.Arguments.Clear();

            list.Clear();
            Assert.That(last.Arguments.Count, Is.EqualTo(1));
            var resetted = last.Arguments.Where(z => z.Action == NotifyCollectionChangedAction.Reset).FirstOrDefault();
            Assert.That(resetted, Is.Not.Null);
            last.Arguments.Clear();
        }

        /// <summary>
        /// Stores the last call
        /// </summary>
        private class LastPropertyChanges
        {
            /// <summary>
            /// Gets or sets the name of the property
            /// </summary>
            public List<string> PropertyNames = new List<string>();
        }

		/*
        /// <summary>
        /// Stores the last information about collection changes
        /// </summary>
        private class LastCollectionChanges
        {
            /// <summary>
            /// Gets or sets the arguments 
            /// </summary>
            public List<NotifyCollectionChangedEventArgs> Arguments = new List<NotifyCollectionChangedEventArgs>();
        }
		
    }*/
}
