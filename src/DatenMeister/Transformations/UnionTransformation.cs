using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    public class UnionTransformation : IReflectiveCollection
    {
        /// <summary>
        /// Stores the collections
        /// </summary>
        private IEnumerable<IReflectiveCollection> collections;

        /// <summary>
        /// Initializes a new instance of the UnionTransformation class
        /// </summary>
        /// <param name="collections">Enumeration of collections</param>
        public UnionTransformation(IEnumerable<IReflectiveCollection> collections)
        {
            this.collections = collections;
        }
                
        public IURIExtent Extent
        {
            get { throw new NotImplementedException(); }
        }

        public bool add(object value)
        {
            throw new NotImplementedException();
        }

        public bool addAll(IReflectiveSequence value)
        {
            throw new NotImplementedException();
        }

        public void clear()
        {
            throw new NotImplementedException();
        }

        public bool remove(object value)
        {
            throw new NotImplementedException();
        }

        public int size()
        {
            return this.collections.Sum(x => x.size());
        }

        public void Add(object item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object item)
        {
            return this.collections.Any(x => x.Contains(item));
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this.size(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(object item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach (var collection in this.collections)
            {
                foreach (var item in collection)
                {
                    yield return item;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var collection in this.collections)
            {
                foreach (var item in collection)
                {
                    yield return item;
                }
            }
        }
    }
}
