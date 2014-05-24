using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    public abstract class ListReflectiveSequence : BaseReflectiveSequence
    {

        public ListReflectiveSequence()
        {
        }

        /// <summary>
        /// Gets the list being associated to the object. 
        /// If the list is not already in, a new list will be created
        /// </summary>
        /// <returns>The associated list</returns>
        protected abstract IList<object> GetList();

        public override void add(int index, object value)
        {
            this.GetList().Insert(index, value);
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
            this.GetList()[index] = value;
            return oldValue;
        }

        public override bool add(object value)
        {
            this.GetList().Add(value);
            return true;
        }

        public override void clear()
        {
            this.GetList().Clear();
        }

        public override bool remove(object value)
        {   
            return this.GetList().Remove(value);
        }

        public override int size()
        {
            return this.GetList().Count;
        }

        public override IEnumerable<object> getAll()
        {
            return this.GetList();
        }
    }
}
