using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    /// <summary>
    /// Returns every item only once. 
    /// </summary>
    public class MakeUniqueTransformation : BaseTransformation
    {
        /// <summary>
        /// Initializes a new instance of the MakeUniqueTransformation class
        /// </summary>
        /// <param name="collection">Collection to be used</param>
        public MakeUniqueTransformation(IReflectiveCollection collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Gets all the items
        /// </summary>
        /// <returns>Enumeration of items to be retrieved</returns>
        public override IEnumerable<object> getAll()
        {
            return this.source.Distinct();
        }
    }
}
