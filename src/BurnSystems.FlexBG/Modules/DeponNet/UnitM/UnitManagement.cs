using BurnSystems.FlexBG.Modules.DeponNet.Common;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Strategies;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.UnitJobs;
using BurnSystems.FlexBG.Modules.IdGeneratorM;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM
{
    public class UnitManagement : IUnitManagement
    {
        [Inject(IsMandatory = true)]
        public LocalUnitDatabase Data
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IIdGenerator IdGenerator
        {
            get;
            set;
        }

        [Inject]
        public IUnitTypeProvider UnitTypeProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the unit type, if available
        /// </summary>
        /// <param name="unitTypeId">Id of the unit type</param>
        /// <returns>Unittype to be retrieved</returns>
        private UnitType GetUnitType(long unitTypeId)
        {
            if (this.UnitTypeProvider != null)
            {
                return this.UnitTypeProvider.Get(unitTypeId);
            }

            return null;
        }

        /// <summary>
        /// Creates the unit
        /// </summary>
        /// <param name="ownerId">Id of the owner</param>
        /// <param name="unitTypeId">Id of the unit type</param>
        /// <param name="amount">Amount of units</param>
        /// <param name="position">Position of unit</param>
        /// <returns>Id of the created unit</returns>
        public long CreateUnit(long ownerId, long unitTypeId, int amount, ObjectPosition position)
        {
            var unit = new Unit();
            unit.OwnerId = ownerId;
            unit.UnitTypeId= unitTypeId;
            unit.Position = position;
            unit.Id = this.IdGenerator.NextId(EntityType.Unit);
            var unitType = this.GetUnitType(unitTypeId);

            lock (this.Data.SyncObject)
            {
                this.Data.UnitsStore.Units.Add(unit);

                for (var n = 0; n < amount; n++)
                {
                    var instance = new UnitInstance();
                    instance.Id = this.IdGenerator.NextId(EntityType.UnitInstance);
                    instance.IsDead = false;

                    if (unitType != null)
                    {
                        instance.LifePoints = unitType.LifePoints;
                    }

                    unit.Instances.Add(instance);
                }
            }

            return unit.Id;
        }

        /// <summary>
        /// Removes unit from unit storage
        /// </summary>
        /// <param name="unitId">Id of unit to be removed</param>
        public void DissolveUnit(long unitId)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.GetUnit(unitId);
                if (unit != null)
                {
                    this.Data.UnitsStore.Units.Remove(unit);
                }
            }
        }

        public void UpdatePosition(long unitId, ObjectPosition newPosition)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.GetUnit(unitId);
                if (unit != null)
                {
                    unit.Position = newPosition;
                }
            }
        }

        public Unit GetUnit(long unitId)
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.UnitsStore.Units.Where(x => x.Id == unitId).FirstOrDefault();
            }
        }

        public IEnumerable<Unit> GetAllUnits()
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.UnitsStore.Units.ToList();
            }
        }

        public IEnumerable<Unit> GetUnitsOfOwner(long ownerId)
        {
            lock (this.Data.SyncObject)
            {
                return this.Data.UnitsStore.Units.Where(x=>x.OwnerId == ownerId).ToList();
            }
        }

        /// <summary>
        /// Inserts a job for the unit. 
        /// This is just done on data level, no automatic update of current unit task will be performed
        /// </summary>
        /// <param name="unitId">Id of the unit </param>
        /// <param name="job">Job to be added</param>
        /// <param name="position">Position where unit will be found or max value if it shall be inserted at the end</param>
        /// <returns>Id of the ne inserted job</returns>
        public int InsertJob(long unitId, IJob job, int position)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.Data.UnitsStore.Units.Where(x => x.Id == unitId).FirstOrDefault();
                if (unit == null)
                {
                    // Nothing to do here
                    return -1;
                }

                // Alignment of position
                var count = unit.Jobs.Count;
                if (position < 0)
                {
                    position = 0;
                }

                if (position > count)
                {
                    position = count;
                }

                // Performs the insertion
                unit.Jobs.Insert(position, job);

                return position;
            }
        }

        /// <summary>
        /// Removes the job from the unit. 
        /// No automatic update of unit task wil be performed
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="position">Position of job that shall be removed</param>
        public void RemoveJob(long unitId, int position)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.Data.UnitsStore.Units.Where(x => x.Id == unitId).FirstOrDefault();
                if (unit == null)
                {
                    // Nothing to do here
                    return;
                }

                unit.Jobs.RemoveAt(position);
            }
        }

        /// <summary>
        /// Sets the index of the current job
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="currentJobIndex">Index of the next current job</param>
        public void SetCurrentJob(long unitId, int currentJobIndex)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.Data.UnitsStore.Units.Where(x => x.Id == unitId).FirstOrDefault();
                if (unit == null)
                {
                    // Nothing to do here
                    return;
                }

                unit.IndexCurrentJob = currentJobIndex;
            }
        }

        /// <summary>
        /// Sets the unit strategy
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="strategy">Strategy to be implemented</param>
        public void SetUnitStrategy(long unitId, UnitStrategy strategy)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.Data.UnitsStore.Units.Where(x => x.Id == unitId).FirstOrDefault();
                if (unit == null)
                {
                    // Nothing to do here
                    return;
                }

                unit.Strategy = strategy;
            }
        }

        /// <summary>
        /// Sets the unit instance for a specific unit
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        /// <param name="instancePosition">Position of the instance in array</param>
        /// <param name="instance">Instance to be set</param>
        public void SetUnitInstance(long unitId, int instancePosition, UnitInstance instance)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.GetUnit(unitId);
                if (unit != null)
                {
                    unit.Instances[instancePosition] = instance;
                }
            }
        }

        /// <summary>
        /// Removes all dead instances from array
        /// </summary>
        /// <param name="unitId">Id of the unit</param>
        public void RemoveDeadInstances(long unitId)
        {
            lock (this.Data.SyncObject)
            {
                var unit = this.GetUnit(unitId);
                if (unit != null)
                {
                    unit.Instances.RemoveAll(x => x.IsDead || x.LifePoints <= 0);

                    if (unit.Amount == 0)
                    {
                        this.DissolveUnit(unitId);
                    }
                }
            }
        }
    }
}
