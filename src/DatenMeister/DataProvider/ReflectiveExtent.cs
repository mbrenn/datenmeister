using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Defines an extent, that simply wraps a reflective collection
    /// </summary>
    public class ReflectiveExtent : IURIExtent
    {
        string uri;
        Func<IReflectiveSequence> sequence;

        public ReflectiveExtent(Func<IReflectiveSequence> sequence, string uri)
        {
            this.uri = uri;
            this.sequence = sequence;
        }

        public ReflectiveExtent(Func<IReflectiveCollection> collection, string uri)
        {
            this.uri = uri;
            this.sequence = () => new CollectionToSequenceWrapper(collection());
        }

        public string ContextURI()
        {
            return this.uri;
        }

        public IReflectiveSequence Elements()
        {
            return this.sequence(); ;
        }

        public IPool Pool
        {
            get;
            set;
        }

        public bool IsDirty
        {
            get;
            set;
        }
    }
}
