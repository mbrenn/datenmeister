using System;
using System.Collections.Generic;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Defines that the implementing class is capable to activate objects
    /// </summary>
    public interface IActivates
    {
        /// <summary>
        /// Gets all object by enablers
        /// </summary>
        /// <param name="enablers">Enumeration of relevant enablers</param>
        /// <returns>The enabled object or null, if no object can be enabled</returns>
        IEnumerable<object> GetAll(IEnumerable<IEnabler> enablers);

        /// <summary>
        /// Checks, if the container knows a binding to the specific enablers
        /// </summary>
        /// <param name="enablers">Enablers to be tested</param>
        /// <returns>true, if container or block knows how to activate an object by the enablers</returns>
        bool Has(IEnumerable<IEnabler> enablers);

        /// <summary>
        /// This event is thrown when a binding has changed
        /// </summary>
        event EventHandler BindingChanged;
    }
}