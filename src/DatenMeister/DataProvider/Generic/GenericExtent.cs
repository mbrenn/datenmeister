using DatenMeister.DataProvider.Common;
using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Generic
{
    public class GenericExtent: IURIExtent
    {
        #region Static instance

        /// <summary>
        /// Gets or sets the global generic extent
        /// </summary>
        public static GenericExtent Global
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the class
        /// </summary>
        static GenericExtent()
        {
            Global = new GenericExtent("http:///datenmeister_genericextent");
        }

        #endregion

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
        /// <returns>Gets the context uri</returns>
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
            return new GenericExtentReflectiveSequence(this, elements);
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

        public class GenericExtentReflectiveSequence : ListWrapperReflectiveSequence<object>
        {
            public GenericExtentReflectiveSequence(IURIExtent extent, IList<object> elements)
                : base(extent, elements)
            {
            }

            public override void add(int index, object value)
            {
                this.SetExtentIfPossible(value);
                base.add(index, value);
            }

            public override bool add(object value)
            {
                this.SetExtentIfPossible(value);
                return base.add(value);
            }

            public override bool addAll(IReflectiveSequence elements)
            {
                foreach (var element in elements)
                {
                    this.SetExtentIfPossible(element);
                }

                return base.addAll(elements);
            }

            public override object set(int index, object value)
            {
                this.SetExtentIfPossible(value);
                return base.set(index, value);
            }

            /// <summary>
            /// Sets the extent, if possible to the given value
            /// </summary>
            /// <param name="value">Value to be set</param>
            private void SetExtentIfPossible(object value)
            {
                if (value is GenericObject)
                {
                    (value as GenericObject).Extent = this.Extent;
                }
            }
        }
    }
}
