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

        public override void add(int index, object value)
        {
            this.GetList().Insert(index, (T) value);
        }

        public override object get(int index)
        {
            return this.GetList()[index];
        }

        public override object remove(int index)
        {
            var result = this.get(index);
            this.GetList().RemoveAt(index);
            return result;
        }

        public override object set(int index, object value)
        {
            var oldValue = this.GetList()[index];
            this.GetList()[index] = (T) value;
            return oldValue;
        }

        public override bool add(object value)
        {
            this.GetList().Add((T) value);
            return true;
        }

        public override void clear()
        {
            this.GetList().Clear();
        }

        public override bool remove(object value)
        {
            var list = this.GetList();
            var result = list.Contains((T) value);
            this.GetList().Remove((T) value);
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
