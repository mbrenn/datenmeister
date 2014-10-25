using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.MethodProvider
{
    /// <summary>
    /// Enumerates the possible method types.
    /// See http://wiki.depon.net/index.php/IMethodProvider
    /// </summary>
    public enum MethodType
    {
        /// <summary>
        /// The method is allocated to the type itself. The method will be called without 
        /// having a certain instance. This method scope is equal to the static methods in other program languages. 
        /// </summary>
        StaticMethod,

        /// <summary>
        /// The method is allocated to all instances of the type. This method will be called in context of a 
        /// certain instance. This method scope is equal to the instance method in other program languages. 
        /// </summary>
        TypeMethod,
        
        /// <summary>
        /// The method is just allocated to a single instance. The method can only be called in context 
        /// within this instance. This method call is equal to assigning a function to a single instance. 
        /// The function cannot be called in context of other functions. 
        /// </summary>
        InstanceMethod
    }
}
