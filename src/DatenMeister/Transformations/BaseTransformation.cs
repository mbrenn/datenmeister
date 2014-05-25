using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    public abstract class BaseTransformation  : ITransformation
    {
        /// <summary>
        /// Gets or sets the source of the transformation
        /// </summary>
        public IURIExtent source
        {
            get;
            set;
        }

        public string ContextURI()
        {
            return this.source.ContextURI();
        }

        public abstract IReflectiveSequence Elements();

        public IPool Pool
        {
            get
            {
                return this.source.Pool;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsDirty
        {
            get
            {
                return this.source.IsDirty;
            }
            set
            {
                this.source.IsDirty = value;
            }
        }
    }
}
