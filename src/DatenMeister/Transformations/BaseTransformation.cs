using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    public abstract class BaseTransformation  : ITransformation
    {
        /// <summary>
        /// Gets or sets the source of the transformation
        /// </summary>
        public IReflectiveCollection source
        {
            get;
            set;
        }

        public BaseTransformation(IReflectiveCollection collection)
        {
            this.source = collection;
        }

        public abstract IEnumerable<object> getAll();

        #region Standard Implementation of the Base Transformation

        public IURIExtent Extent
        {
            get
            {
                return this.source.Extent;
            }
        }

        public bool add(object value)
        {
            return this.source.add(value);
        }

        public bool addAll(IReflectiveSequence value)
        {
            return this.source.addAll(value);
        }

        public void clear()
        {
            throw new NotImplementedException("Clear is not implemented at BaseTransformation");
        }

        public bool remove(object value)
        {
            return this.source.remove(value);
        }

        public int size()
        {
            return this.getAll().Count();
        }

        public void Add(object item)
        {
            this.source.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException("Clear is not implemented at BaseTransformation");
        }

        public bool Contains(object item)
        {
            return this.getAll().Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            throw new NotImplementedException("CopyTo is not implemented at BaseTransformation");
        }

        public int Count
        {
            get
            {
                return this.getAll().Count();
            }
        }

        public bool IsReadOnly
        {
            get { return this.source.IsReadOnly; }
        }

        public bool Remove(object item)
        {
            return this.source.Remove(item);
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach (var item in this.getAll())
            {
                yield return item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var item in this.getAll())
            {
                yield return item;
            }
        }

        #endregion
    }
}
