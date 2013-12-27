using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Collections.Specialized;
using BurnSystems.Test;

namespace BurnSystems.Collections
{
    /// <summary>
    /// This class transforms a list into another view dynamically. 
    /// It implements the interface IList, INotifyPropertyChanged and INotifyCollectionChanged correctly.
    /// 
    /// It is necessary to call Detach if the transformation is not required any more. 
    /// This list is read-only because the original list has to be modified
    /// </summary>
    /// <typeparam name="T">Type of the elements of the list that is the source type</typeparam>
    /// <typeparam name="Q">Type of the elements of the list, that shall be delivered</typeparam>    
	public class ListTransformView<T, Q> : IList<Q>, IList, INotifyPropertyChanged //, INotifyCollectionChanged
    {
        /// <summary>
        /// Stores the synchronization root
        /// </summary>
        private object syncRoot = new object();

        /// <summary>
        /// Stores the selector
        /// </summary>
        private Func<T, Q> selector;

        /// <summary>
        /// Stores the list
        /// </summary>
        private IList<T> list;

        /// <summary>
        /// Initializes a new instance of the ListTransformView
        /// </summary>
        /// <param name="list">List to be transformed</param>
        /// <param name="selector">Selector to be used for transformation</param>
        public ListTransformView(IList<T> list ,Func<T,Q> selector)
        {
            var notifyPropertyChanged = list as INotifyPropertyChanged;
            var notifyCollectionChanged = list as INotifyCollectionChanged;
            Ensure.IsTrue(notifyCollectionChanged != null);
            Ensure.IsTrue(notifyPropertyChanged != null);

            notifyPropertyChanged.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
            notifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);

            this.list = list;
            this.selector = selector;
        }

        /// <summary>
        /// This method detaches the events from the original list
        /// </summary>
        public void Detach()
        {
            var notifyPropertyChanged = this.list as INotifyPropertyChanged;
            var notifyCollectionChanged = this.list as INotifyCollectionChanged;
            notifyPropertyChanged.PropertyChanged -= new PropertyChangedEventHandler(OnPropertyChanged);
            notifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(OnCollectionChanged);
        }
        
		
        /// <summary>
        /// Called, if a property has been changed
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of event</param>
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
			return; 
			
			/*
            var ev = this.CollectionChanged;
            if (ev != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        ev(
                            this,
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Add,
                                e.NewItems
                                    .OfType<T>()
                                    .Select(x => this.selector(x))
                                    .ToList(),
                                e.NewStartingIndex));
                        break;
                    case NotifyCollectionChangedAction.Move:
                        ev(
                            this,
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Move,
                                e.NewItems
                                    .OfType<T>()
                                    .Select(x => this.selector(x))
                                    .ToList(),
                                e.NewStartingIndex,
                                e.OldStartingIndex));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        ev(
                            this,
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Remove,
                                e.OldItems
                                    .OfType<T>()
                                    .Select(x => this.selector(x))
                                    .ToList(),
                                e.OldStartingIndex));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        ev(
                            this,
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Replace,
                                e.OldItems
                                    .OfType<T>()
                                    .Select(x => this.selector(x))
                                    .ToList(),
                                e.NewItems
                                    .OfType<T>()
                                    .Select(x => this.selector(x))
                                    .ToList()));
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        ev(
                            this,
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Replace));
                        break;
                    default:
                        break;
                }
            }
            */
        }

        /// <summary>
        /// Called, if a property has been changed
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of event</param>
        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var ev = this.PropertyChanged;
            if (ev != null)
            {
                ev(sender, e);
            }
        }

        /// <summary>
        /// Gets the index of a specific item
        /// </summary>
        /// <param name="item">Item to be queried</param>
        /// <returns>Index of the item</returns>
        public int IndexOf(Q item)
        {
            var found = -1;

            foreach (var element in this.list.Select(x => this.selector(x)))
            {
                found++;
                if (element == null && item == null)
                {
                    return found;
                }

                if (element == null)
                {
                    continue;
                }

                if (element.Equals(item))
                {
                    return found;
                }
            }

            return -1;
        }

        /// <summary>
        /// This method is not implemented
        /// </summary>
        /// <param name="index">Index of the element</param>
        /// <param name="item">Element to be added</param>
        public void Insert(int index, Q item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented
        /// </summary>
        /// <param name="index">Item to be removed</param>
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets an item. 
        /// The setting of items is not implemented
        /// </summary>
        /// <param name="index">Index of the element</param>
        /// <returns>Element to be retrieved</returns>
        public Q this[int index]
        {
            get
            {
                return this.selector(this.list[index]);
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This method is not implemented
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(Q item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if an element is contained
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>True, if item is in list</returns>
        public bool Contains(Q item)
        {
            return this.IndexOf(item) != -1;
        }

        /// <summary>
        /// Copies the elements to an array
        /// </summary>
        /// <param name="array">Array to be used</param>
        /// <param name="arrayIndex">Position where the data shall be copied</param>
        public void CopyTo(Q[] array, int arrayIndex)
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

                if (pos >= array.Length)
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
            get { return this.list.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the list is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Method is not implemented
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <returns>true, if item has been removed</returns>
        public bool Remove(Q item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>Enumerator for the instance</returns>
        public IEnumerator<Q> GetEnumerator()
        {
            foreach (var item in this.list)
            {
                yield return this.selector(item);
            }
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>Enumerator for the instance</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in this.list)
            {
                yield return this.selector(item);
            }
        }

        /// <summary>
        /// This method is not implemented
        /// </summary>
        /// <param name="value">Value to be added</param>
        /// <returns>Position of new item</returns>
        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the item is inclued
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>true, if item is included</returns>
        public bool Contains(object value)
        {
            return this.IndexOf(value) != -1;
        }

        /// <summary>
        /// Gets the index of the item within the array
        /// </summary>
        /// <param name="value">Item whose index is required</param>
        /// <returns>Index of item or -1 if not existing</returns>
        public int IndexOf(object value)
        {
            var realValue = value is Q;
            if (!realValue && value != null)
            {
                // Not null, but not of type value
                return -1;
            }

            return this.IndexOf((Q) value);
        }

        /// <summary>
        /// Method not implemented
        /// </summary>
        /// <param name="index">Index of the new element</param>
        /// <param name="value">Value of the element</param>
        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// True, if this has a fixed size
        /// </summary>
        public bool IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="value">Item to be removed</param>
        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets a specific item. The setting of items is not implemented
        /// </summary>
        /// <param name="index">Index of the item</param>
        /// <returns>Requested object</returns>
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Copies the elements to an array
        /// </summary>
        /// <param name="array">Array to be filled</param>
        /// <param name="index">Index within the array</param>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var pos = index;
            foreach (var element in this)
            {
                array.SetValue(element, pos);
                pos++;

                if (pos >= array.Length)
                {
                    throw new ArgumentException();
                }
            }
        }

        /// <summary>
        /// True, if the instance is synchronized.
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the synchronisation root
        /// </summary>
        public object SyncRoot
        {
            get { return this.syncRoot; }
        }

        /// <summary>
        /// This event is called, when a property has been changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// This event is called, when a collection has been changed.
        /// This method is not implemented in Mono
        /// </summary>
        //public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
