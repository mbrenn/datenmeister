using DatenMeister.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations.GroupBy
{
    /// <summary>
    /// The reflective sequence being used for grouping objects. 
    /// Each element contain a key and 
    /// </summary>
    public class GroupByReflectiveSequence : ListWrapperReflectiveSequence<GroupByObject>
    {
        /// <summary>
        /// Initializs a new instance of the GroupByReflectiveSequence
        /// </summary>
        /// <param name="list"></param>
        public GroupByReflectiveSequence(IList<GroupByObject> list)
            : base(list)
        {
        }
    }
}
