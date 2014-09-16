using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Defines some extensions
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Creates a factory by extent...
        /// </summary>
        /// <param name="provider">Provider to be used</param>
        /// <param name="extent">Extents to be used</param>
        /// <returns>Created factory</returns>
        public static IFactory CreateFor(this IFactoryProvider provider, IURIExtent extent)
        {
            Ensure.That(extent != null);
            return provider.CreateFor(extent.GetType(), extent);
        }
    }
}
