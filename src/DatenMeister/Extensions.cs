using DatenMeister.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    public static class Extensions
    {
        public static JsonExtentInfo ToJson(this IURIExtent extent)
        {
            return new JsonExtentInfo()
            {
                uri = extent.ContextURI(),
                type = extent.GetType().FullName
            };
        }
    }
}
