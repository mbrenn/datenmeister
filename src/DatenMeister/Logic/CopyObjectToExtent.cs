using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Defines a function being used for copy activities
    /// </summary>
    public class CopyObjectToExtent
    {
        /// <summary>
        /// Copies the source element to the target. 
        /// The metaclasses will not be transferred
        /// </summary>
        /// <param name="source">Source to be copied</param>
        /// <param name="target">Target, where object will be allocated afterwards</param>
        /// <returns>Copied object in target extent</returns>
        public IObject Copy(IObject source, IURIExtent target)
        {
            throw new NotImplementedException();
        }
    }
}
