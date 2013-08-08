using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    public interface IURIExtent
    {
        void StoreChanges();

        string ContextURI();

        IEnumerable<IObject> Elements();
    }
}
