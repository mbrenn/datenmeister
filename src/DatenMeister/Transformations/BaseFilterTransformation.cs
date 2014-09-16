using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    public abstract class BaseFilterTransformation : BaseTransformation
    {
        public BaseFilterTransformation(IReflectiveCollection collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Checks, if the given object shall be in the transformation
        /// </summary>
        /// <param name="v">Value to be checked</param>
        /// <returns>true, if the object is in</returns>
        public abstract bool IsIn(object v);

        /// <summary>
        /// Gets all items, which satisfy the filter condition
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<object> getAll()
        {
            foreach (var value in this.source)
            {
                if ( this.IsIn(value))
                {
                    yield return value;
                }
            }
        }
    }
}
