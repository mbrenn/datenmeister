using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    /// <summary>
    /// Performs a recursion, where every element and its subelements are returned
    /// </summary>
    public class RecurseObjectTransformation : BaseTransformation
    {
        public RecurseObjectTransformation(IReflectiveCollection collection) :
            base(collection)
        {
        }

        /// <summary>
        /// Just checks, if source is null
        /// </summary>
        private void CheckSource()
        {
            if (this.source == null)
            {
                throw new InvalidOperationException("source is null");
            }
        }

        public override IEnumerable<object> getAll()
        {
            foreach (var element in this.source)
            {
                foreach (var yields in this.Recurse(element))
                {
                    yield return yields;
                }
            }
        }

        private IEnumerable<IObject> Recurse(object value)
        {
            // If IObject or IEnumeration, otherwise return nothing
            if (value is IObject)
            {
                yield return value as IObject;
                foreach (var pair in (value as IObject).getAll())
                {
                    foreach (var x in this.Recurse(pair.Value))
                    {
                        yield return x;
                    }
                }
            }

            if (value is IEnumerable)
            {
                foreach (var item in (value as IEnumerable))
                {
                    foreach (var x in this.Recurse(item))
                    {
                        yield return x;
                    }
                }
            }
        }
    }
}
