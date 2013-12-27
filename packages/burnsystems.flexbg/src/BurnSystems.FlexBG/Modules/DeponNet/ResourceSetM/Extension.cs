using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM
{
    /// <summary>
    /// Helper class for resources
    /// </summary>
    public static class Extension
    {        
        /// <summary>
        /// Gets the sum of all resourcesets in the enumeration
        /// </summary>
        /// <param name="resourceSets">Enumeration of resourcesets</param>
        /// <returns>Summed resourceset</returns>
        public static ResourceSet Sum(this IEnumerable<ResourceSet> resourceSets)
        {
            var result = new ResourceSet();

            foreach (var item in resourceSets)
            {
                result.Add(item);
            }

            return result;
        }

    }
}
