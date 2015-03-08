using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Implemented by all objects, when the factory extent defines
    /// the object 
    /// </summary>
    public interface IHasFactoryExtent
    {
        IURIExtent FactoryExtent
        {
            get;
        }
    }
}
