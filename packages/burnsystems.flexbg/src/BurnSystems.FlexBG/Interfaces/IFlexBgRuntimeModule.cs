using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Interfaces
{
    /// <summary>
    /// This interface shall be used for all runtime modules that need initialization during 
    /// startup or shutdown
    /// </summary>
    public interface IFlexBgRuntimeModule
    {
        /// <summary>
        /// Starts the module
        /// </summary>
        void Start();

        /// <summary>
        /// Performs a shutdown
        /// </summary>
        void Shutdown();
    }
}
