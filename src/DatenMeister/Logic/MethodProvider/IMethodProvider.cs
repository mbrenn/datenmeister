using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.MethodProvider
{
    /// <summary>
    /// Defines the interface that can be used to inject and to use methods for other 
    /// objects.
    /// </summary>
    public interface IMethodProvider
    {
        /// <summary>
        /// Adds a static method to a type
        /// </summary>
        /// <param name="type">Type of the function which shall be callable</param>
        /// <param name="name">Name of the function to be used</param>
        /// <param name="functionMethod">Method which shall be callable</param>
        /// <returns>Return the function handle</returns>
        IFunction AddStaticMethod(IObject type, string name, Delegate functionMethod);
    }
}
