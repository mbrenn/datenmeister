using BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.UnitRulesM
{
    /// <summary>
    /// Implements all interfaces that are necessary for unit rules
    /// </summary>
    public interface IUnitJobRules
    {
        /// <summary>
        /// Adds a job to the unit
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="job">Job to be added</param>
        /// <param name="position">Position, where job shall be added</param>
        void AddJob(long unitId, IJob job, int position);

        /// <summary>
        /// Cancels current job
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        void CancelCurrentJob(long unitId);

        /// <summary>
        /// Executes the current job, depending on the world clock. 
        /// </summary>
        /// <param name="unitId">Id of the unit, whose job shall be executed</param>
        void ExecuteJob(long unitId);

        /// <summary>
        /// Executes the job for all units. 
        /// </summary>
        void ExecuteJobForAllUnits();
    }
}
