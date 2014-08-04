using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    public class FilterByPropertyTransformation : BaseFilterTransformation
    {
        /// <summary>
        /// The property to be queried
        /// </summary>
        private string property;

        /// <summary>
        /// The filtered value
        /// </summary>
        private object value;

        /// <summary>
        /// Initializes a new instance of FilterByPropertyTransformation class.
        /// </summary>
        /// <param name="collection">Collection to be queried</param>
        /// <param name="property">Property to be queried</param>
        /// <param name="value">Value that has to have the given value</param>
        public FilterByPropertyTransformation(IReflectiveCollection collection, string property, object value)
            : base(collection)
        {
            this.property = property;
            this.value = value;
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
                var myValue = vAsObject.get(property).AsSingle();
                return ObjectHelper.AreEqual(this.value, myValue);
            }

            return false;
        }
    }
}
