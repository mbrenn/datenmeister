using DatenMeister.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations.GroupBy
{
    /// <summary>
    /// Stores the information about the current group by object 
    /// </summary>
    public class GroupByObject : GenericObject
    {
        /// <summary>
        /// Gets or sets the key for the grouping
        /// </summary>
        public object key
        {
            get { return this.get("key").AsSingle(); }
        }

        /// <summary>
        /// Gets or sets the values for the reflective sequence
        /// </summary>
        public IReflectiveSequence values
        {
            get { return this.get("values").AsReflectiveSequence(); }
        }

        /// <summary>
        /// Initializes a new instance of the GroupByObject
        /// </summary>
        /// <param name="extent">Extent to be associated</param>
        /// <param name="key">Key of the grouping</param>
        /// <param name="values">Values of the grouping</param>
        public GroupByObject(IURIExtent extent, object key, IReflectiveSequence values)
            : base(extent)
        {
            this.set("key", key);
            this.set("values", values);
        }
    }
}
