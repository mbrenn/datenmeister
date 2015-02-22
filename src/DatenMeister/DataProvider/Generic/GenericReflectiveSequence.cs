using DatenMeister.DataProvider.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.Generic
{
    public class GenericReflectiveSequence : ListReflectiveSequence<object>
    {
        private IList<object> value
        {
            get;
            set;
        }

        public GenericReflectiveSequence(IURIExtent extent, List<object> value)
            : base (extent)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the list being associated to the object. 
        /// If the list is not already in, a new list will be created
        /// </summary>
        /// <returns>The associated list</returns>
        protected override IList<object> GetList()
        {
            return this.value;
        }
    }
}
