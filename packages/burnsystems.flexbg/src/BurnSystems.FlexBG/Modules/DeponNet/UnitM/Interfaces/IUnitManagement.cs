using BurnSystems.FlexBG.Modules.DeponNet.Common;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Strategies;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces
{
    /// <summary>
    /// Interface for unit management, all methods are just performed on data level, no logic update 
    /// will be performed
    /// </summary>
    public interface IUnitManagement
    {
        /// <summary>
        /// Creates a unit
        /// </summary>
        /// <param name="ownerId">Id of the owner</param>
        /// <param name="amount">Amount of unit instances</param>
        /// <param name="unitTypeId">Id of the unit type</param>
        /// <param name="position">Position of the unit</param>
        /// <returns>Id of the new unit</returns>
        long CreateUnit(long ownerId, long unitTypeId, int amount, ObjectPosition position);

        /// <summary>
        /// Dissolves the unit
        /// </summary>
        /// <param name="unitId"></param>
        void DissolveUnit(long unitId);

        void UpdatePosition(long unitId, ObjectPosition newPosition);

        Unit GetUnit(long unitId);

        /// <summary>
        /// Gets all units in database (independent from game). 
        /// </summary>
        /// <returns>Enumeration of units</returns>
        IEnumerable<Unit> GetAllUnits();

        /// <summary>
        /// Gets all units of a certain owner
        /// </summary>
        /// <param name="ownerId">Id of the owner</param>
        /// <returns>Enumeration of units</returns>
        IEnumerable<Unit> GetUnitsOfOwner(long ownerId);

        /// <summary>
        /// Inserts a job for the unit. 
        /// This is just done on data level, no automatic update of current unit task will be performed
        /// </summary>
        /// <param name="unitId">Id of the unit </param>
        /// <param name="job">Job to be added</param>
        /// <param name="position">Position where unit will be found or max value if it shall be inserted at the end</param>
        /// <returns>Id of the ne inserted job</returns>
        int InsertJob(long unitId, IJob job, int position);

        /// <summary>
        /// Removes the job from the unit. 
        /// No automatic update of unit task wil be performed
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="position">Position of job that shall be removed</param>
        void RemoveJob(long unitId, int position);

        /// <summary>
        /// Sets the index of the current job
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="currentJobIndex">Index of the next current job</param>
        void SetCurrentJob(long unitId, int currentJobIndex);

        /// <summary>
        /// Sets the unit strategy
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="strategy">Strategy to be implemented</param>
        void SetUnitStrategy(long unitId, UnitStrategy strategy);
        
        /// <summary>
        /// Sets the unit instance for a specific unit
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="instancePosition">Position of the instance in array</param>
        /// <param name="instance">Instance to be set</param>
        void SetUnitInstance(long unitId, int instancePosition, UnitInstance instance);
        
        /// <summary>
        /// Removes all dead instances from array
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        void RemoveDeadInstances(long unitId);
    }
}
