using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// This class is able to map a meta class from a certain dotnet type
    /// it contains two methods
    /// </summary>
    public interface IMapsMetaClassFromDotNet
    {
        /// <summary>
        /// Gets the meta class by a certain .Net type
        /// </summary>
        /// <param name="dotNetType">The associated .Net Type</param>
        /// <returns>The associated metaclass</returns>
        IObject GetMetaClass(Type dotNetType);

        /// <summary>
        /// Gets the .Net Type by a certain meta class
        /// </summary>
        /// <param name="type">The associated .Net Type</param>
        /// <returns>The .Net Type being used</returns>
        Type GetDotNetType(IObject type);

        /// <summary>
        /// Adds an assignment between the metaclass and .Net Type
        /// </summary>
        /// <param name="type">The meta class</param>
        /// <param name="dotNetType">The .Net Type</param>
        DotNetTypeInformation Add(Type dotNetType, IObject type);
    }
}
