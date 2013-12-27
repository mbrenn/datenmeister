using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.BackgroundWorkerM.Interface
{
    /// <summary>
    /// Defines an interface that stores the last execute time
    /// </summary>
    public interface IBackgroundTask
    {
        /// <summary>
        /// Gets the next execution time
        /// </summary>
        /// <param name="container">Container to be used</param>
        /// <returns>Datetime of the next executeion time</returns>
        DateTime GetNextExecutionTime(IActivates container);

        /// <summary>
        /// Refreshes the time
        /// </summary>
        /// <param name="container">Container to be executed</param>
        void Execute(IActivates container);
    }
}
