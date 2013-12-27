//-----------------------------------------------------------------------
// <copyright file="ILockable.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Interfaces
{
    /// <summary>
    /// This interface is used for all objects, which should 
    /// be locked by <c>SimpleLock</c>.
    /// </summary>
    public interface ILockable 
    {
        /// <summary>
        /// Locks an object
        /// </summary>
        void Lock();

        /// <summary>
        /// Unlocks an object
        /// </summary>
        void Unlock();
    }
}
