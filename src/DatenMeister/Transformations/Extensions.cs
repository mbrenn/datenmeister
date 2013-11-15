using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    /// <summary>
    /// Defines the extension methods
    /// </summary>
    public static class Extensions
    {
        public static IURIExtent Recurse(this IURIExtent extent)
        {
            var transformation = new RecurseObjectTransformation();
            transformation.source = extent;
            return transformation;
        }
    }
}
