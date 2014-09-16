using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// This interface needs to be implemented by all objects, which proxy a real object.
    /// This is used to find out the real type. 
    /// </summary>
    public interface IProxyObject
    {
        DatenMeister.IObject Value
        {
            get;
        }
    }
}
