using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.MethodProvider
{
    /// <summary>
    /// Defines the interface that can be used to inject and to use methods for other 
    /// objects. Additional information can be found http://wiki.depon.net/index.php/IMethodProvider
    /// </summary>
    public interface IMethodProvider
    {
        /// <summary>
        /// Adds a static method to a type
        /// </summary>
        /// <param name="type">Type of the objects, which shall be callable</param>
        /// <param name="id">Id of the function to be used</param>
        /// <param name="functionMethod">Method which shall be callable</param>
        /// <returns>Return the function handle</returns>
        IMethod AddStaticMethod(IObject type, string id, Delegate functionMethod);
        
        /// <summary>
        /// Adds an instance method to the specific object
        /// </summary>
        /// <param name="type">Instance, where function shall be added</param>
        /// <param name="id">Name of the function</param>
        /// <param name="functionMethod"></param>
        /// <returns>The created function</returns>
        IMethod AddInstanceMethod(IObject instance, string id, Delegate functionMethod);

        /// <summary>
        /// Adds a type method to the specific type. All instances of the type can now execute
        /// the type
        /// </summary>
        /// <param name="type">Type which shall have the function</param>
        /// <param name="id">Id of the function</param>
        /// <param name="functionMethod">Method of the function</param>
        /// <returns>The created function</returns>
        IMethod AddTypeMethod(IObject type, string id, Delegate functionMethod);

        /// <summary>
        /// Gets the function by a certain id
        /// </summary>
        /// <param name="id">Id, of the function</param>
        /// <returns>The function or null, if not exist</returns>
        IMethod Get(string id);

        /// <summary>
        /// Gets all function on a certain type
        /// </summary>
        /// <param name="type">Type of the function being queried</param>
        /// <returns>Returns the functions</returns>
        IEnumerable<IMethod> GetFunctionsOnType(IObject type);

        /// <summary>
        /// Gets all function on a certain type
        /// </summary>
        /// <param name="instance">Instance of the function being queried</param>
        /// <returns>Returns the functions on the instance</returns>
        IEnumerable<IMethod> GetFunctionsOnInstance(IObject instance);
    }
}
