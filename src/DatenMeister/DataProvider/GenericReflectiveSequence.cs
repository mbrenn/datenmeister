using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    public class GenericReflectiveSequence : ListReflectiveSequence<object>
    {
        private GenericUnspecified unspecified
        {
            get;
            set;
        }

        public GenericReflectiveSequence(GenericUnspecified unspecified)
            : base(unspecified.Owner.Extent)
        {
            this.unspecified = unspecified;
        }

        /// <summary>
        /// Gets the list being associated to the object. 
        /// If the list is not already in, a new list will be created
        /// </summary>
        /// <returns>The associated list</returns>
        protected override IList<object> GetList()
        {
            var value = this.unspecified.Value as List<object>;
            if (value == null)
            {
                // The list did not create, so create it. 
                value = new List<object>();
                this.unspecified.Value = value;
                this.unspecified.Owner.set(this.unspecified.PropertyName, value);
            }

            return value;
        }
    }
}
