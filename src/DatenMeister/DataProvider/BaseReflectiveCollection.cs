﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Implements a base reflective sequence, where all IEnumeration methods 
    /// are mapped to the IReflectiveCollection methods. Those methods, which 
    /// require a certain order, throw an InvalidOperationException
    /// </summary>
    public abstract class BaseReflectiveCollection : IReflectiveCollection
    {
        public abstract bool add(object value);

        public virtual bool addAll(IReflectiveSequence elements)
        {
            var result = false;
            foreach (var element in elements)
            {
                result |= this.add(element);
            }

            return result;
        }

        public abstract void clear();

        public abstract object remove(object value);

        public abstract int size();

        public abstract IEnumerable<object> getAll();

        void ICollection<object>.Add(object item)
        {
            this.add(item);
        }

        void ICollection<object>.Clear()
        {
            this.clear();
        }

        bool ICollection<object>.Contains(object item)
        {
            return this.Any(x => x == item);
        }

        void ICollection<object>.CopyTo(object[] array, int arrayIndex)
        {
            foreach (var item in array)
            {
                this.add(item);
            }
        }

        int ICollection<object>.Count
        {
            get { return this.size(); }
        }

        bool ICollection<object>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<object>.Remove(object item)
        {
            return this.remove(item) != null;
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return this.getAll().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.getAll().GetEnumerator();
        }
    }
}
