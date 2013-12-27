using BurnSystems.Collections;
using BurnSystems.FlexBG.Modules.BackgroundWorkerM.Interface;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.BackgroundWorkerM.Logic
{
    /// <summary>
    /// Stores the information about one worker
    /// </summary>
    class WorkerItem : IHasKey
    {
        /// <summary>
        /// Initializes a new instance of the WorkerItem class. 
        /// </summary>
        public WorkerItem()
        {
            this.AssumedNextTime = DateTime.MinValue;
        }

        /// <summary>
        /// Gets or sets the key
        /// </summary>
        public string Key
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the assumed next time. 
        /// </summary>
        public DateTime AssumedNextTime
        {
            get;
            set;
        }

        public IBackgroundTask Task
        {
            get;
            set;
        }

        public DateTime GetNextTime(IActivates container)
        {
            return this.Task.GetNextExecutionTime(container);
        }

        public void Execute(IActivates container)
        {
            this.Task.Execute(container);
        }
    }
}
