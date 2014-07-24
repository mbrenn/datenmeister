using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Implemented by all objects, where an extent is necessary 
    /// to create the object
    /// </summary>
    public interface IHasFactoryExtent
    {
        IURIExtent FactoryExtent
        {
            get;
        }
    }
}
