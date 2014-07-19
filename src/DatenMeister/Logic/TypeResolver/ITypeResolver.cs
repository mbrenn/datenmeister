using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.TypeResolver
{
    /// <summary>
    /// Resolves the name of a type to its instance
    /// </summary>
    interface ITypeResolver
    {
        /// <summary>
        /// Returned type by name
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <returns>Found type</returns>
        IObject GetType(string typeName);
    }
}
