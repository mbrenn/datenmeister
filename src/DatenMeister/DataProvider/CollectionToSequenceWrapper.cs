using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    public class CollectionToSequenceWrapper : IReflectiveSequence
    {
        private IReflectiveCollection collection;

        public CollectionToSequenceWrapper ( IReflectiveCollection collection)
        {
            this.collection = collection;
        }

        public IURIExtent Extent
        {
            get { return this.collection.Extent; }
        }

        public bool add(object value)
        {
            return this.collection.add(value);
        }

        public bool addAll(IReflectiveSequence value)
        {
            return this.collection.addAll(value);
        }

        public void clear()
        {
            this.collection.clear();
        }

        public bool remove(object value)
        {
            return this.collection.remove(value);
        }

        public int size()
        {
            return this.collection.size();
        }

        public void Add(object item)
        {
            this.collection.Add(item);
        }

        public void Clear()
        {
            this.collection.Clear();
        }

        public bool Contains(object item)
        {
            return this.collection.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            this.collection.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.collection.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.collection.IsReadOnly; }
        }

        public bool Remove(object item)
        {
            return this.collection.Remove(item);
        }

        public IEnumerator<object> GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }
        
        //

        public void add(int index, object value)
        {
            throw new NotImplementedException();
        }

        public object get(int index)
        {
            throw new NotImplementedException();
        }

        public object remove(int index)
        {
            throw new NotImplementedException();
        }

        public object set(int index, object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
