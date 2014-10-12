using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.MethodProvider
{
    /// <summary>
    /// The function being used 
    /// </summary>
    public interface IMethod
    {
        /// <summary>
        /// Gets the id of the function
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Gets the method type
        /// </summary>
        MethodType MethodType
        {
            get;
        }

        /// <summary>
        /// Invokes the given function within the given context and the given parameters
        /// </summary>
        /// <param name="context">Context to be used (equivalent to a this)</param>
        /// <param name="parameter">Parameters being sent to the function</param>
        /// <returns>The returned object after of the call</returns>
        object Invoke(object context, params object[] parameter);
    }
}
