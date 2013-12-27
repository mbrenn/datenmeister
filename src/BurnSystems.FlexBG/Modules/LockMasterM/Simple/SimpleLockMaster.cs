using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.LockMasterM.Simple
{
    /// <summary>
    /// A very simple lock master that just locks the complete game, but checks whether
    /// we have the right order for entity types. 
    /// </summary>
    public class SimpleLockMaster : ILockMaster
    {
        /// <summary>
        /// Used for synchronization
        /// </summary>
        private ReaderWriterLockSlim syncObject = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public IDisposable AcquireReadLock(IEnumerable<LockEntity> entities)
        {
            this.syncObject.EnterReadLock();

            return new LockScope(
                () =>
                {
                    this.syncObject.ExitReadLock();
                });
        }

        public IDisposable AcquireWriteLock(IEnumerable<LockEntity> entities)
        {
            // Perform lock
            this.syncObject.EnterWriteLock();
            return new LockScope(
                () =>
                {
                    this.syncObject.ExitWriteLock();
                });
        }

        /// <summary>
        /// Adds a relationship between two entity types
        /// </summary>
        /// <param name="parentEntityType">Type id of the owner</param>
        /// <param name="childEntityType">Type if of the parent</param>
        public void AddRelationShip(int parentEntityType, int childEntityType)
        {
        }
    }
}
