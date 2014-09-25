using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    public class FilterByExtentTypeTransformation : BaseFilterTransformation
    {
        /// <summary>
        /// The property to be queried
        /// </summary>
        private ExtentType extentType;

        /// <summary>
        /// Initializes a new instance of FilterByPropertyTransformation class.
        /// </summary>
        /// <param name="collection">Collection to be queried</param>
        /// <param name="property">Property to be queried</param>
        /// <param name="value">Value that has to have the given value</param>
        public FilterByExtentTypeTransformation(IReflectiveCollection collection, ExtentType extentType)
            : base(collection)
        {
            this.extentType = extentType;
        }

        /// <summary>
        /// Checks, if the value is in filtered collection
        /// </summary>
        /// <param name="v">Value to be checked</param>
        /// <returns>true, if value is in</returns>
        public override bool IsIn(object v)
        {
            var vAsObject = v as IObject;
            if (vAsObject != null)
            {
                var extent = vAsObject.Extent;
                if (extent != null)
                {
                    return extent.Pool.GetInstance(extent.ContextURI()).ExtentType == this.extentType;
                }
            }

            return false;
        }
    }
}
