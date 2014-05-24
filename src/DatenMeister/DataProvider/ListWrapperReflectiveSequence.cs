using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    public class ListWrapperReflectiveSequence : ListReflectiveSequence
    {
        /// <summary>
        /// Stores the list
        /// </summary>
        private IList<object> list;

        public ListWrapperReflectiveSequence(IList<object> list)
        {
            this.list = list;
        }

        protected override IList<object> GetList()
        {
            return this.list;
        }
    }
}
