using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.Common
{
    /// <summary>
    /// Implements a base reflective sequence, where all IEnumeration methods 
    /// are mapped to the IReflectiveCollection methods. Those methods, which 
    /// require a certain order, throw an InvalidOperationException
    /// </summary>
    public abstract class BaseReflectiveCollection : IReflectiveCollection
    {
        public BaseReflectiveCollection(IURIExtent extent)
        {
            this.Extent = extent;
        }

        public IURIExtent Extent
        {
            get;
            private set;
        }

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

        public abstract bool remove(object value);

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
            var elements = this.getAll();

            foreach (var item in elements)
            {
                array[arrayIndex] = item;
                arrayIndex++;
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
            return this.remove(item);
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
