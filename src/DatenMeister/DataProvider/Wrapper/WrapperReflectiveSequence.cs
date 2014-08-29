using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper
{
    public class WrapperReflectiveSequence : IReflectiveSequence
    {
        /// <summary>
        /// Stores the inner sequence
        /// </summary>
        private IReflectiveSequence inner;

        public IReflectiveSequence Inner
        {
            get { return this.inner; }
            set { this.inner = value; }
        }

        /// <summary>
        /// Gets or sets the wrapped extent
        /// </summary>
        internal IWrapperExtent WrapperExtent
        {
            get;
            set;
        }
        
        public WrapperReflectiveSequence()
        {
        }

        public WrapperReflectiveSequence(IWrapperExtent extent, IReflectiveSequence sequence)
        {
            this.WrapperExtent = extent;
            this.inner = sequence;
        }

        public IReflectiveSequence Unwrap()
        {
            return this.inner;
        }

        public virtual void add(int index, object value)
        {
            this.inner.add(index, value);
        }

        public virtual object get(int index)
        {
            var result = this.inner.get(index);
            return this.WrapperExtent.Convert(result);
        }

        public virtual object remove(int index)
        {
            return this.inner.remove(index);
        }

        public virtual object set(int index, object value)
        {
            return this.inner.set(index, value);
        }

        public IURIExtent Extent
        {
            get
            {
                return this.WrapperExtent;
            }
        }

        public virtual bool add(object value)
        {
            return this.inner.add(value);
        }

        public virtual bool addAll(IReflectiveSequence value)
        {
            return this.inner.addAll(value);
        }

        public virtual void clear()
        {
            this.inner.clear();
        }

        public virtual bool remove(object value)
        {
            return this.inner.remove(value);
        }

        public virtual int size()
        {
            return this.inner.size();
        }

        public virtual void Add(object item)
        {
            this.inner.Add(item);
        }

        public virtual void Clear()
        {
            this.inner.Clear();
        }

        public virtual bool Contains(object item)
        {
            return this.inner.Contains(item);
        }

        public virtual void CopyTo(object[] array, int arrayIndex)
        {
            this.inner.CopyTo(array, arrayIndex);
        }

        public virtual int Count
        {
            get { return this.inner.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return this.inner.IsReadOnly; }
        }

        public virtual bool Remove(object item)
        {
            return this.inner.Remove(item);
        }

        public virtual IEnumerator<object> GetEnumerator()
        {
            foreach (var item in this.inner)
            {
                yield return this.WrapperExtent.Convert(item);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var item in this.inner)
            {
                yield return this.WrapperExtent.Convert(item);
            }
        }

        public virtual int IndexOf(object item)
        {
            return this.inner.IndexOf(item);
        }

        public virtual void Insert(int index, object item)
        {
            this.inner.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            this.inner.RemoveAt(index);
        }

        public virtual object this[int index]
        {
            get
            {
                return this.WrapperExtent.Convert(this.inner[index]);
            }
            set
            {
                this.inner[index] = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is WrapperReflectiveSequence)
            {
                return Equals((obj as WrapperReflectiveSequence).Unwrap());
            }

            return this.Inner.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Inner.GetHashCode();
        }
    }
}
