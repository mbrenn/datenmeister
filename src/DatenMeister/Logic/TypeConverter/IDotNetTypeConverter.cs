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
        /// <param name="extent">The extent, where the object shall be used</param>
        /// <param name="type">Type to be converted</param>
        /// <returns>Converted object</returns>
        IObject Convert(IURIExtent extent, Type type);

        /// <summary>
        /// Converts the given dotnet type to an IObject type and uses the given Factory for conversion
        /// </summary>
        /// <param name="factory">Factory to be used to create the item</param>
        /// <param name="type">Type to be converted</param>
        /// <param name="callBackInnerTypes">This action is called, when the type has a
        /// property of another type. 
        /// The method does not call itself, since it might be necesssary to skip
        /// the item.</param>
        /// <returns>Converted object</returns>
        IObject Convert(IFactory factory, Type type, Action<Type> callBackInnerTypes = null);
    }
}
