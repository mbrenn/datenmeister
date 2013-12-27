//-----------------------------------------------------------------------
// <copyright file="SimpleLock.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Synchronisation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BurnSystems.Interfaces;
    using BurnSystems.Test;

    /// <summary>
    /// Eine einfache Locking-Klasse, die für das Locken 
    /// eines ILockable-Objektes zustündig ist
    /// </summary>
    public class SimpleLock : IDisposable
    {
        /// <summary>
        /// The lockable structure, which is used by this simple lock
        /// </summary>
        private ILockable lockable;

        /// <summary>
        /// Flag, if object was disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the SimpleLock class. The object
        /// <c>lockable</c> is locked. 
        /// </summary>
        /// <param name="lockable">Das Objekt, das zu sperren ist</param>
        public SimpleLock(ILockable lockable)
        {
            Ensure.IsNotNull(lockable);

            this.lockable = lockable;
            this.lockable.Lock();
        }

        /// <summary>
        /// Finalizes an instance of the SimpleLock class.
        /// </summary>
        ~SimpleLock()
        {
            this.Dispose(false);
        }

        #region IDisposable Member

        /// <summary>
        /// Entsperrt das Objekt wieder
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes und unlocks the element
        /// </summary>
        /// <param name="disposing">Called by Dispose</param>
        private void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.disposed = true;
                this.lockable.Unlock();
            }
        }       

        #endregion
    }
}
