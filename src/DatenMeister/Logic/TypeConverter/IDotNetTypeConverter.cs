using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.TypeConverter
{
    /// <summary>
    /// Converts a dotnet type converter to an IObjects type and adds it to the given extent
    /// </summary>
    public interface IDotNetTypeConverter
    {
        /// <summary>
        /// Converts the given dotnet type to an IObject type and adds it to the given extent 
        /// </summary>
        /// <param name="type">Type to be converted</param>
        /// <returns>Converted object</returns>
        IObject Convert(IURIExtent extent, Type type);
    }
}
