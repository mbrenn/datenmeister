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
    public class RecurseObjectTransformation : IURIExtent
    {
        public IURIExtent source
        {
            get;
            set;
        }

        public string ContextURI()
        {
            return "Temp";
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

        public IEnumerable<IObject> Elements()
        {
            this.CheckSource();

            foreach (var element in this.source.Elements())
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
                foreach (var pair in (value as IObject).GetAll())
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

        /// <summary>
        /// Creates a new object
        /// </summary>
        /// <returns>Object that had been created</returns>
        public IObject CreateObject()
        {
            this.CheckSource();

            return this.source.CreateObject();
        }

        /// <summary>
        /// Removes a given object
        /// </summary>
        /// <param name="element">Element to be removed</param>
        public void RemoveObject(IObject element)
        {
            this.CheckSource();

            this.source.RemoveObject(element);
        }
    }
}
