//-----------------------------------------------------------------------
// <copyright file="XmlSetList.cs" company="Martin Brenn">
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
    using System.Linq;
    using System.Xml.Linq;
    using BurnSystems.Interfaces;

    /// <summary>
    /// This implements the IList interface on an XContainer element. 
    /// All addings, deletions, changes on this container will be directly done
    /// on the element
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    public class XmlSetList<T> : IList<T>
    {
        /// <summary>
        /// Stores the converter
        /// </summary>
        private IXElementConverter<T> converter;

        /// <summary>
        /// Container node being associated the list
        /// </summary>
        private IEnumerable<XContainer> containers;

        /// <summary>
        /// Initializes a new instance of the XmlList class.
        /// </summary>
        /// <param name="containers">Xml-Container storing the elements</param>
        /// <param name="converter">Converter to be used to convert items to xml and vice versa</param>
        public XmlSetList(IEnumerable<XContainer> containers, IXElementConverter<T> converter)
        {
            this.containers = containers;
            this.converter = converter;
        }

        /// <summary>
        /// Initializes a new instance of the XmlList class.
        /// </summary>
        /// <param name="elements">Xml-Container storing the elements</param>
        /// <param name="converter">Converter to be used to convert items to xml and vice versa</param>
        public XmlSetList(IEnumerable<XElement> elements, IXElementConverter<T> converter)
        {
            this.containers = elements.Cast<XContainer>();
            this.converter = converter;
        }

        /// <summary>
        /// Gets the index of the item
        /// </summary>
        /// <param name="item">Item to be requested</param>
        /// <returns>Index of the item</returns>
        public int IndexOf(T item)
        {
            var pos = 0;

            foreach (var entity in
                this.containers.Elements()
                    .Select(x => converter.Convert(x)))
            {
                if ((item == null && entity == null)
                    || (entity != null && entity.Equals(item)))
                {
                    return pos;
                }

                pos++;
            }

            return -1;
        }

        /// <summary>
        /// Inserts a new item to the xmllist
        /// </summary>
        /// <param name="index">Index of the item to be added</param>
        /// <param name="item">Item to be added</param>
        public void Insert(int index, T item)
        {
            var element = this.containers.Elements()
                .ElementAtOrDefault(index - 1);

            if (element == null)
            {
                this.containers.Last().Add(converter.Convert(item));
            }
            else
            {
                element.AddAfterSelf(converter.Convert(item));
            }
        }

        /// <summary>
        /// Removes an item at a certain position
        /// </summary>
        /// <param name="index">Index of the item</param>
        public void RemoveAt(int index)
        {
            var element = this.containers.Elements().ElementAtOrDefault(index);
            if (element != null)
            {
                element.Remove();
            }
        }

        /// <summary>
        /// Gets or sets an element at a certain position
        /// </summary>
        /// <param name="index">Index of the item</param>
        /// <returns>Item at position</returns>
        public T this[int index]
        {
            get
            {
                var element = this.containers.Elements().ElementAt(index);
                return converter.Convert(element);
            }
            set
            {
                var element = this.containers.Elements().ElementAtOrDefault(index);
                if (element != null)
                {
                    element.ReplaceWith(converter.Convert(value));
                }
                else
                {
                    this.containers.Last().Add(converter.Convert(value));
                }
            }
        }

        /// <summary>
        /// Adds an item
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            this.containers.Last().Add(converter.Convert(item));
        }

        /// <summary>
        /// Clears the complete list
        /// </summary>
        public void Clear()
        {
            foreach (var container in this.containers)
            {
                container.RemoveNodes();
            }
        }

        /// <summary>
        /// Checks whether the xml list contains a specific item
        /// </summary>
        /// <param name="item">Item which shall be checked</param>
        /// <returns>true, if item is included</returns>
        public bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        /// <summary>
        /// Copies the complete list to an array
        /// </summary>
        /// <param name="array">Array to be added</param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var pos = arrayIndex;
            foreach (var element in this)
            {
                array[pos] = element;
                pos++;

                if (pos > array.Length)
                {
                    throw new ArgumentException();
                }
            }
        }

        /// <summary>
        /// Gets the number of elements
        /// </summary>
        public int Count
        {
            get
            {
                var count = this.containers.Elements().Count();
                return count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the list is read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes a specific item from xml list
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <returns>True, if item has been removed</returns>
        public bool Remove(T item)
        {
            var position = this.IndexOf(item);
            if (position != -1)
            {
                this.RemoveAt(position);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets an enumerator for all elements. 
        /// Elements being null are skipped
        /// </summary>
        /// <returns>Enumerator for the list</returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in this.containers.Elements()
                .Select(x => converter.Convert(x)))
            {
                yield return item;
            }
        }

        /// <summary>
        /// Gets an enumerator for all elements. 
        /// Elements being null are skipped
        /// </summary>
        /// <returns>Enumerator for the list</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Creates an xml-List whose values matches to a specific attribute of all subnodes
        /// </summary>
        /// <param name="container">Container element containing values of all subnodes</param>
        /// <param name="nodeName">Name of the node containing the information</param>
        /// <param name="attributeName">Name of the attribute that is requested</param>
        /// <returns>List of information</returns>
        public static IList<T> GetListForAttributes(IEnumerable<XContainer> container, string nodeName, string attributeName)
        {
            var converter = new XmlList<T>.AttributeEntityConverter<T>(nodeName, attributeName);

            return new XmlSetList<T>(
                container,
                converter);
        }

        /// <summary>
        /// Creates an xml-List whose values matches to a specific attribute of all subnodes
        /// </summary>
        /// <param name="container">Container element containing values of all subnodes</param>
        /// <param name="nodeName">Name of the node containing the information</param>
        /// <param name="attributeName">Name of the attribute that is requested</param>
        /// <returns>List of information</returns>
        public static IList<T> GetListForAttributes(IEnumerable<XElement> container, string nodeName, string attributeName)
        {
            return GetListForAttributes(container.Cast<XContainer>(), nodeName, attributeName);
        }

        /// <summary>
        /// Creates an xml-List whose values matches to a specific element of all subnodes
        /// </summary>
        /// <param name="container">Container element containing values of all subnodes</param>
        /// <param name="nodeName">Name of the node containing the information</param>
        /// <returns>List of information</returns>
        public static IList<T> GetListForElements(IEnumerable<XContainer> container, string nodeName)
        {
            var converter = new XmlList<T>.ElementEntityConverter<T>(nodeName);

            return new XmlSetList<T>(
                container,
                converter);
        }

        /// <summary>
        /// Creates an xml-List whose values matches to a specific element of all subnodes
        /// </summary>
        /// <param name="container">Container element containing values of all subnodes</param>
        /// <param name="nodeName">Name of the node containing the information</param>
        /// <returns>List of information</returns>
        public static IList<T> GetListForElements(IEnumerable<XElement> container, string nodeName)
        {
            return GetListForElements(container.Cast<XContainer>(), nodeName);
        }
    }
}