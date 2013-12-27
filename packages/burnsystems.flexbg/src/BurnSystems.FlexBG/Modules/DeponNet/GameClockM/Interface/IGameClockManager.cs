using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameClockM.Interface
{
    public interface IGameClockManager
    {
        /// <summary>
        /// Gets the ticks since the start
        /// </summary>
        /// <param name="instanceId">Id of the instance</param>
        /// <returns></returns>
        long GetTicks(long instanceId);

        /// <summary>
        /// Increments the number of ticks for the given instance
        /// </summary>
        /// <param name="instanceId">Id of the instance</param>
        /// <param name="ticks">Number of ticks</param>
        void IncrementTicks(long instanceId, long ticks);
    }
}
