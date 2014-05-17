using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    public class GenericReflectiveSequence : BaseReflectiveSequence
    {
        private GenericUnspecified unspecified
        {
            get;
            set;
        }

        public GenericReflectiveSequence(GenericUnspecified unspecified)
        {
            this.unspecified = unspecified;
        }

        /// <summary>
        /// Gets the list being associated to the object. 
        /// If the list is not already in, a new list will be created
        /// </summary>
        /// <returns>The associated list</returns>
        private List<object> GetList()
        {
            var value = this.unspecified.Value as List<object>;
            if ( value == null)
            {
                // The list did not create, so create it. 
                value = new List<object>();
                this.unspecified.Value = value;
                this.unspecified.Owner.set(this.unspecified.PropertyName, value);
            }

            return value;
        }

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
            this.GetList()[index] = value;
            return value;
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

        public override object remove(object value)
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
