using BurnSystems.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.Common
{
    /// <summary>
    /// Wraps a list in a reflective sequence
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListWrapperReflectiveSequence<T> : ListReflectiveSequence<T>, IListWrapperReflectiveSequence
    {
        /// <summary>
        /// Stores the list
        /// </summary>
        private IList<T> list;

        public ListWrapperReflectiveSequence(IURIExtent extent, IList<T> list)
            : base(extent)
        {
            Ensure.That(list != null, "Parameter list is null");
            this.list = list;
        }

        /// <summary>
        /// Gets the list from the wrapper
        /// </summary>
        /// <returns></returns>
        protected override IList<T> GetList()
        {
            return this.list;
        }

        /// <summary>
        /// Gets the list as an instance
        /// </summary>
        /// <returns>Gets the list being used as container</returns>
        IEnumerable IListWrapperReflectiveSequence.GetList()
        {
            return this.list;
        }
    }
}
