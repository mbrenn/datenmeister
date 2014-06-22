using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    public class GenericExtent: IURIExtent
    {
        /// <summary>
        /// Stores the uri
        /// </summary>
        private string uri;

        /// <summary>
        /// Stores the elements being associated to the generic extent
        /// </summary>
        private List<object> elements = new List<object>();

        /// <summary>
        /// Initializes a new instance of the GenericExtent class.
        /// </summary>
        /// <param name="uri">Uri of the extent</param>
        public GenericExtent(string uri)
        {
            this.uri  = uri;
        }

        /// <summary>
        /// Gets the context uri
        /// </summary>
        /// <returns></returns>
        public string ContextURI()
        {
            return this.uri;
        }

        /// <summary>
        /// Gets the elements in the extent
        /// </summary>
        /// <returns>Elements of the current extent</returns>
        public IReflectiveSequence Elements()
        {
            return new ListWrapperReflectiveSequence<object>(this, elements);
        }

        /// <summary>
        /// Gets or sets the pool being associated to the generic extent
        /// </summary>
        public IPool Pool
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the information whether the current extent is dirty
        /// </summary>
        public bool IsDirty
        {
            get;
            set;
        }
    }
}
