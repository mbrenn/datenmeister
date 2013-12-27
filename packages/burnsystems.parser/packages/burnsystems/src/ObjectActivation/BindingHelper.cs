using System;
using System.Collections.Generic;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Small helper to ease binding.
    /// </summary>
    public class BindingHelper
    {
        /// <summary>
        /// Gets or sets the enumeration of enablers 
        /// </summary>
        internal IEnumerable<IEnabler> Enablers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the activation info
        /// </summary>
        internal ActivationInfo ActivationInfo
        {
            get;
            set;
        }
    }
}