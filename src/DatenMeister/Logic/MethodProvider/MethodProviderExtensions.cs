using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.MethodProvider
{
    /// <summary>
    /// Provides some additional methods
    /// </summary>
    public static class MethodProviderExtensions
    {
        public static IMethod AddStaticMethod(this IMethodProvider provider, IObject type, Delegate functionMethod)
        {
            return provider.AddStaticMethod(type, Guid.NewGuid().ToString(), functionMethod);
        }

        /// <summary>
        /// Adds an instance method to the type
        /// </summary>
        /// <param name="type">Instance of the , where function shall be added</param>
        /// <param name="id">Name of the function</param>
        /// <param name="functionMethod"></param>
        /// <returns>The created function</returns>
        public static IMethod AddInstanceMethod(this IMethodProvider provider, IObject instance, Delegate functionMethod)
        {
            return provider.AddInstanceMethod(instance, Guid.NewGuid().ToString(), functionMethod);
        }

        /// <summary>
        /// Adds a type method to the specific type. All instances of the type can now execute
        /// the type
        /// </summary>
        /// <param name="type">Type which shall have the function</param>
        /// <param name="id">Id of the function</param>
        /// <param name="functionMethod">Method of the function</param>
        /// <returns>The created function</returns>
        public static IMethod AddTypeMethod(this IMethodProvider provider, IObject type, Delegate functionMethod)
        {
            return provider.AddTypeMethod(type, Guid.NewGuid().ToString(), functionMethod);
        }
    }
}
