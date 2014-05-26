using BurnSystems.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Wraps a list in a reflective sequence
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListNonGenericWrapperReflectiveSequence : ListNonGenericReflectiveSequence
    {
        /// <summary>
        /// Stores the list
        /// </summary>
        private IList list;

        public ListNonGenericWrapperReflectiveSequence(IList list)
        {
            Ensure.That(list != null, "Parameter list is null");
            this.list = list;
        }

        /// <summary>
        /// Gets the list from the wrapper
        /// </summary>
        /// <returns></returns>
        protected override IList GetList()
        {
            return this.list;
        }
    }
}
