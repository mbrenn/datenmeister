using BurnSystems.FlexBG.Modules.BackgroundWorkerM.Interface;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.BackgroundWorkerM.Logic
{
    /// <summary>
    /// Returns a predicate, which executes an event by real time
    /// </summary>
    public class ExecuteByRealTime : IBackgroundTask
    {
        /// <summary>
        /// Stores the date of the last execution
        /// </summary>
        private DateTime lastExecution = DateTime.MinValue;

        /// <summary>
        /// Stores the intervaltimes
        /// </summary>
        private TimeSpan intervalTime;

        /// <summary>
        /// Stores the function that shall be executed, if timeinterval has gone
        /// </summary>
        private Action<IActivates> executionFunction;

        public ExecuteByRealTime(TimeSpan intervalTime, Action<IActivates> executionFunction)
        {
            Ensure.IsNotNull(executionFunction);

            this.intervalTime = intervalTime;
            this.executionFunction = executionFunction;
        }

        /// <summary>
        /// Gets the next execution time
        /// </summary>
        /// <param name="container">Container to be used</param>
        /// <returns>Datetime of next</returns>
        public DateTime GetNextExecutionTime(ObjectActivation.IActivates container)
        {
            return this.lastExecution + this.intervalTime;
        }

        /// <summary>
        /// Refreshes the time
        /// </summary>
        /// <param name="container"></param>
        public void Execute(ObjectActivation.IActivates container)
        {
            this.lastExecution = DateTime.Now;
            this.executionFunction(container);
        }
    }
}
