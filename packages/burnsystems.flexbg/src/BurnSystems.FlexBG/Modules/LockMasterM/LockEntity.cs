using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.LockMasterM
{
    /// <summary>
    /// Defines the entity to be locked
    /// </summary>
    public class LockEntity
    {
        /// <summary>
        /// Defines the type of the entity to be locked
        /// </summary>
        public int EntityType
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the id of the entity to be locked
        /// </summary>
        public long EntityId
        {
            get;
            set;
        }

        public LockEntity(int entityType, long entityId)
        {
            this.EntityType = entityType;
            this.EntityId = entityId;
        }

        public override bool Equals(object obj)
        {
            var entity = obj as LockEntity;
            if (obj == null)
            {
                return false;
            }

            return this.Equals(entity);
        }

        public bool Equals(LockEntity entity)
        {
            return this.EntityId == entity.EntityId && this.EntityType == entity.EntityType;
        }

        public override int GetHashCode()
        {
            return this.EntityType.GetHashCode() ^ this.EntityId.GetHashCode();
        }
    }
}
