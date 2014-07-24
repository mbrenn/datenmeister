using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// This interaface shall be implemented by all objects, which are aware of their corresponding extent type
    /// </summary>
    public interface IKnowsExtentType
    {
        /// <summary>
        /// Shall return the content type
        /// </summary>
        Type ExtentType
        {
            get;
        }
    }
}
