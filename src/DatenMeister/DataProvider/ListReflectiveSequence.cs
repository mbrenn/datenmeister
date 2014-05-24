using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    public abstract class ListReflectiveSequence<T> : BaseReflectiveSequence
    {

        public ListReflectiveSequence()
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
        /// <returns></returns>
        public virtual void OnChange()
        {
        }

        /// <summary>
        /// This method is called, when a conversion to the store format 
        /// is necessary. Per default, an explicit time conversion will be used. 
        /// This might also contain more complex functn
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted value</returns>
        public virtual T ConvertInstanceTo(object value)
        {
            return (T)value;
        }

        public override void add(int index, object value)
        {
            this.GetList().Insert(index, this.ConvertInstanceTo(value));
            this.OnChange();
        }

        public override object get(int index)
        {
            return this.GetList()[index];
        }

        public override object remove(int index)
        {
            var result = this.get(index);
            this.GetList().RemoveAt(index);
            this.OnChange();
            return result;
        }

        public override object set(int index, object value)
        {
            var oldValue = this.GetList()[index];
            this.GetList()[index] = this.ConvertInstanceTo(value);
            this.OnChange();
            return oldValue;
        }

        public override bool add(object value)
        {
            this.GetList().Add(this.ConvertInstanceTo(value));
            this.OnChange();
            return true;
        }

        public override void clear()
        {
            this.GetList().Clear();
            this.OnChange();
        }

        public override bool remove(object value)
        {
            var converted = this.ConvertInstanceTo(value);
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
                yield return item;
            }
        }
    }
}
