using BurnSystems.FlexBG.Modules.DeponNet.GameClockM.Interface;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameClockM
{
    /// <summary>
    /// Manages a bunch of clocks
    /// </summary>
    public class GameClockManager : IGameClockManager
    {
        /// <summary>
        /// Gets or sets the data
        /// </summary>
        [Inject(IsMandatory=true)]
        public LocalGameClockDatabase Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ticks since 
        /// </summary>
        /// <param name="instanceId">Id of the instance</param>
        /// <returns>-1, if not found</returns>
        public long GetTicks(long instanceId)
        {
            lock (this.Data.GameClockStore.GameClockInfos)
            {
                var result = this.Data.GameClockStore.GameClockInfos.Where(x => x.InstanceId == instanceId)
                    .FirstOrDefault();
                if (result == null)
                {
                    return -1;
                }

                return result.Time;
            }
        }

        /// <summary>
        /// Increments the ticks for the given instance
        /// </summary>
        /// <param name="instanceId">Id of the instance</param>
        /// <param name="ticks">Number of ticks to be added</param>
        public void IncrementTicks(long instanceId, long ticks)
        {
            lock (this.Data.GameClockStore.GameClockInfos)
            {
                var result = this.Data.GameClockStore.GameClockInfos.Where(x => x.InstanceId == instanceId)
                    .FirstOrDefault();
                if (result == null)
                {
                    result = new GameClockInfo()
                    {
                        InstanceId = instanceId,
                        Created = DateTime.Now,
                        Time = 0
                    };

                    this.Data.GameClockStore.GameClockInfos.Add(result);
                }

                result.Time += ticks;
            }
        }
    }
}
