using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister
{
    public interface IPool
    {
        /// <summary>
        /// Resolves the given object
        /// </summary>
        /// <param name="obj">Object to be resolved</param>
        /// <returns>Result of the object that shall be resolved</returns>
        object Resolve(object obj);
    }
}
