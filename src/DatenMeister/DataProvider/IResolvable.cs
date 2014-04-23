using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Defines elements which can be resolved
    /// </summary>
    public interface IResolvable
    {
        /// <summary>
        /// Resolves the object with the given pool
        /// </summary>
        /// <param name="pool">Pool which is used to resolve the object</param>
        /// <param name="context">The context which is used to resolve the object</param>
        /// <returns>Resolved object</returns>
        object Resolve(IPool pool, IObject context);
    }
}
