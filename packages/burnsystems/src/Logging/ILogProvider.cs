//-----------------------------------------------------------------------
// <copyright file="ILogProvider.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Logging
{
    /// <summary>
    /// With the implementation of this interface, the logprovider can be used
    /// in the logging
    /// </summary>
    public interface ILogProvider
    {
        /// <summary>
        /// This method is called once with the insertion of the logprovider
        /// in the log
        /// </summary>
        void Start();

        /// <summary>
        /// This method is called one with the removal or with the disposing
        /// of the log
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Called, when a new logentry has to be logged
        /// </summary>
        /// <param name="entry">Entry to be logged</param>
        void DoLog(LogEntry entry);
    }
}
