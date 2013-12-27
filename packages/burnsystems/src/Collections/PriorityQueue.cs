//-----------------------------------------------------------------------
// <copyright file="PriorityQueue.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements a priority queue, in which the element with 
    /// the smallest value is at first position
    /// </summary>
    /// <typeparam name="T">Type of elements</typeparam>
    public class PriorityQueue<T> : IEnumerable<T>
    {
        /// <summary>
        /// Storage for elements
        /// </summary>
        private List<T> elements = new List<T>();

        /// <summary>
        /// Comparer for elements
        /// </summary>
        private Comparison<T> comparer;

        /// <summary>
        /// Initializes a new instance of the PriorityQueue class.
        /// The empty constructor is only valid, if <c>T</c> is inherited
        /// of IComparable
        /// </summary>
        public PriorityQueue()
        {
            this.comparer =
                (x, y) =>
                    ((IComparable)x).CompareTo((IComparable)y);
        }

        /// <summary>
        /// Initializes a new instance of the PriorityQueue class.
        /// </summary>
        /// <param name="comparer">Comparer to be used</param>
        public PriorityQueue(Comparison<T> comparer)
        {
            this.comparer = comparer;
        }

        /// <summary>
        /// Gets the number of elements
        /// </summary>
        public int Count
        {
            get
            {
                return this.elements.Count;
            }
        }

        /// <summary>
        /// Adds a new item to priorityqueue
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            bool exists;
            var position = this.GetPosition(item, out exists);

            this.elements.Insert(position, item);
        }

        /// <summary>
        /// Removes the item from priority queue
        /// </summary>
        /// <param name="item">Item to be removed</param>
        public void Remove(T item)
        {
            bool exists;
            var position = this.GetPosition(item, out exists);

            if (exists)
            {
                this.elements.RemoveAt(position);
            }
        }

        /// <summary>
        /// Checks, if the item exists
        /// </summary>
        /// <param name="item">Item to be checked.</param>
        /// <returns>True, if item exists in list</returns>
        public bool Exists(T item)
        {
            bool exists;

            this.GetPosition(item, out exists);
            return exists;
        }

        /// <summary>
        /// Gets the first entry
        /// </summary>
        /// <returns>First entry to be added</returns>
        public T Peek()
        {
            if (this.Count == 0)
            {
                return default(T);
            }

            return this.elements[0];
        }

        /// <summary>
        /// Gets the first object and removes it from queue
        /// </summary>
        /// <returns>Object to be popped.</returns>
        public T Pop()
        {
            if (this.Count == 0)
            {
                return default(T);
            }

            // Not fast, but beautiful
            var result = this.elements[0];
            this.elements.RemoveAt(0);
            return result;
        }

        /// <summary>
        /// Clears the eventlist
        /// </summary>
        public void Clear()
        {
            this.elements.Clear();
        }

        /// <summary>
        /// Gets all elements an array. 
        /// </summary>
        /// <returns>Array of elements</returns>
        public T[] GetElements()
        {
            return this.elements.ToArray();
        }

        /// <summary>
        /// Gets an enumerator for the class.
        /// </summary>
        /// <returns>Enumerator enumerating all elements</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        /// <summary>
        /// Resorts the priorityqueue
        /// </summary>
        public void Resort()
        {
            // Resort in inverse order because the smallest objects are at the end
            this.elements.Sort((x, y) => this.comparer(y, x));
        }

        /// <summary>
        /// Gets an enumerator for the class.
        /// </summary>
        /// <returns>Enumerator enumerating all elements</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        /// <summary>
        /// Gets the position of the entry.
        /// </summary>
        /// <param name="item">Entry, whose position should be queried.</param>
        /// <param name="exists">true, if the element was found</param>
        /// <returns>Position of entry, which be the one of the entry. </returns>
        private int GetPosition(T item, out bool exists)
        {
            if (this.Count == 0)
            {
                exists = false;
                return 0;
            }

            var left = 0;
            var right = this.Count; // Exclusive border

            var current = (left + right) / 2;
            while (left < right && current < this.Count)
            {
                var compareValue = this.comparer(item, this.elements[current]);

                if (compareValue >= 0)
                {
                    right = current;
                }
                else
                {
                    left = current + 1;
                }

                current = (left + right) / 2;
            }

            // Found ?! 
            if (current >= this.Count)
            {
                exists = false;
                return current;
            }
            else
            {
                exists = this.elements[current].Equals(item);
                return current;
            }
        }
    }
}
