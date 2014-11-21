using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.Transformations;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.Sources
{
    public class AllElementsExtent : IURIExtent
    {
        /// <summary>
        /// Stores the uri for the element
        /// </summary>
        private string uri;

        /// <summary>
        /// Stores the extent types
        /// </summary>
        private ExtentType? extentType;

        /// <summary>
        /// Initializes a new instance of the AllElementsExtent class.
        /// </summary>
        /// <param name="uri"></param>
        public AllElementsExtent(string uri, ExtentType? extentType = null )
        {
            this.uri = uri;
            this.extentType = extentType;
        }

        public string ContextURI()
        {
            return this.uri;
        }

        public IEnumerable<IURIExtent> GetAllExtents()
        {
            Ensure.That(this.Pool != null, "Pool is not set at the moment");

            if (this.extentType == null)
            {
                return this.Pool.ExtentMappings
                    .Where (x=> x.Extent != this)
                    .Select(x => x.Extent);
            }
            else
            {
                return this.Pool.ExtentMappings
                    .Where(x => x.ExtentInfo.ExtentType == extentType && x.Extent != this)
                    .Select(x => x.Extent);
            }
        }

        public IReflectiveSequence Elements()
        {
            var pool = Injection.Application.Get<IPool>();
            return new CollectionToSequenceWrapper(this.GetAllExtents().UnionExtents());
        }

        public IPool Pool
        {
            get;
            set;
        }

        public bool IsDirty
        {
            get
            {
                return this.GetAllExtents().Any(x => x.IsDirty);
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
