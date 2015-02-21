using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.Common
{
    public abstract class ListReflectiveSequence<T> : BaseReflectiveSequence
    {
        /// <summary>
        /// Gets or sets a value whether the list is readonly
        /// </summary>
        public bool IsReadOnly
        {
            get;
            set;
        }

        public ListReflectiveSequence(IURIExtent extent)
            : base(extent)
        {
        }

        /// <summary>
        /// Gets the list being associated to the object. 
        /// If the list is not already in, a new list will be created
        /// </summary>
        /// <returns>The associated list</returns>
        protected abstract IList<T> GetList();

        /// <summary>
        /// This class will be called, when something has been changed in the extent. 
        /// Might be used to set a dirty flag
        /// </summary>
        public virtual void OnChange()
        {
            if (this.Extent != null)
            {
                this.Extent.IsDirty = true;
            }
        }

        /// <summary>
        /// Checks, if read-only flag is set. 
        /// If that is the case, an exception is thrown
        /// </summary>
        private void EnsureThatNotReadOnly()
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException("This list is read-only");
            }
        }

        /// <summary>
        /// This method is called, when a conversion from the element to be added to the store format 
        /// is necessary. Per default, an explicit time conversion will be used. 
        /// This might also contain more complex function.
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted value</returns>
        public virtual T ConvertInstanceToInternal(object value)
        {
            return (T)value;
        }

        /// <summary>
        /// This method is called, when an internal object is sent out to a caller. 
        /// Per default, the list content is sent out without any modification
        /// </summary>
        /// <param name="item">Item to be sent out</param>
        /// <returns>The sent out item</returns>
        public virtual object ConvertInternalToInstance(T item)
        {
            return item;
        }

        public override void add(int index, object value)
        {
            this.EnsureThatNotReadOnly();

            this.GetList().Insert(index, this.ConvertInstanceToInternal(value));
            this.OnChange();
        }

        public override object get(int index)
        {
            return this.ConvertInternalToInstance(this.GetList()[index]);
        }

        public override object remove(int index)
        {
            this.EnsureThatNotReadOnly();

            var result = this.get(index);
            this.GetList().RemoveAt(index);
            this.OnChange();
            return result;
        }

        public override object set(int index, object value)
        {
            this.EnsureThatNotReadOnly();

            var oldValue = this.GetList()[index];
            this.GetList()[index] = this.ConvertInstanceToInternal(value);
            this.OnChange();
            return oldValue;
        }

        public override bool add(object value)
        {
            this.EnsureThatNotReadOnly();

            this.GetList().Add(this.ConvertInstanceToInternal(value));
            this.OnChange();
            return true;
        }

        public override void clear()
        {
            this.EnsureThatNotReadOnly();

            this.GetList().Clear();
            this.OnChange();
        }

        public override bool remove(object value)
        {
            this.EnsureThatNotReadOnly();

            var converted = this.ConvertInstanceToInternal(value);
            var list = this.GetList();
            var result = list.Contains(converted);
            this.GetList().Remove(converted);
            this.OnChange();
            return result;
        }

        public override int size()
        {
            return this.GetList().Count;
        }

        public override IEnumerable<object> getAll()
        {
            foreach (var item in this.GetList())
            {
                yield return this.ConvertInternalToInstance(item);
            }
        }
    }
}
