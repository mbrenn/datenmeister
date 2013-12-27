using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.LockMasterM
{
    /// <summary>
    /// Defines the interface for the lock master
    /// </summary>
    public interface ILockMaster
    {
        /// <summary>
        /// Acquires a read lock for the given entity type
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityId">Id of the entity</param>
        /// <returns>Disposable object that shall be disposed if finished</returns>
        IDisposable AcquireReadLock(IEnumerable<LockEntity> entities);

        /// <summary>
        /// Acquires a write lock for the given entity type
        /// </summary>
        /// <param name="entityType">Entity type</param>
        /// <param name="entityId">Id of the entity</param>
        /// <returns>Disposable object that shall be disposed if finished</returns>
        IDisposable AcquireWriteLock(IEnumerable<LockEntity> entities);

        /// <summary>
        /// Defines a relationship between owner and child
        /// </summary>
        /// <param name="ownerEntityType">Entity type of the owner</param>
        /// <param name="childEntityType">Entity type of the child</param>
        void AddRelationShip(int ownerEntityType, int childEntityType);
    }
}