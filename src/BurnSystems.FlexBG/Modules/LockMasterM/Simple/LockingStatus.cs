using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.LockMasterM.Simple
{
    /// <summary>
    /// Stores the locking status. 
    /// This should be used as a thread local storage
    /// </summary>
    public class LockingStatus
    {
        /// <summary>
        /// Stores a list of locked entities.
        /// </summary>
        private List<LockEntity> lockedEntities = new List<LockEntity>();

        /// <summary>
        /// Stores a list of all entity types that are locked
        /// </summary>
        private List<int> lockedEntityTypes = new List<int>();

        public void SetEntityType(int entityType)
        {
            while (this.lockedEntityTypes.Count <= entityType)
            {
                this.lockedEntityTypes.Add(0);
            }

            this.lockedEntityTypes[entityType]++;
        }

        public void ResetEntityType(int entityType)
        {
            while (this.lockedEntityTypes.Count <= entityType)
            {
                this.lockedEntityTypes.Add(0);
            }

            this.lockedEntityTypes[entityType]--;

            if (this.lockedEntityTypes[entityType] < 0)
            {
                throw new InvalidOperationException("Entity Type has been released to often: " + entityType);
            }
        }

        public bool IsEntityTypeSet(int entityType)
        {
            if (entityType < this.lockedEntityTypes.Count)
            {
                return this.lockedEntityTypes[entityType] > 0;
            }

            return false;
        }

        public void Add(LockEntity entity)
        {
            this.lockedEntities.Add(entity);
        }

        public void Remove(LockEntity entity)
        {
            this.lockedEntities.RemoveAll(x => x.Equals(entity));
        }

        public bool IsEntityLocked(LockEntity entity)
        {
            return this.lockedEntities.Any(x => x.Equals(entity));
        }
    }
}
