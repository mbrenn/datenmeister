using BurnSystems.FlexBG.Interfaces;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.BackgroundWorkerM.Interface
{
    /// <summary>
    /// Defines the interface for the background worker
    /// </summary>
    public interface IBackgroundWorker : IFlexBgRuntimeModule
    {
        /// <summary>
        /// Adds a worker to the background thread
        /// </summary>
        /// <param name="id">Id of the worker to be added. This id will also be used for removal</param>
        /// <param name="obj">Object that can be added to this request and will be sent to nextTimeFunc and action</param>
        /// <param name="backgroundTask">Function, which defines the task to be executed
        /// If the time has elapsed, this method will be reasked, so action is not called when the timing has changed</param>
        /// <param name="action">Action which shall be called</param>
        void Add(string id, IBackgroundTask backgroundTask);

        /// <summary>
        /// Removes a specific worker
        /// </summary>
        /// <param name="id">Id of the worker to be removed</param>
        void Remove(string id);
    }
}
