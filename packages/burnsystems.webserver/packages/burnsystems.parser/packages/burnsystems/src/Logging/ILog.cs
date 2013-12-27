//-----------------------------------------------------------------------
// <copyright file="ILog.cs" company="Martin Brenn">
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
    using System;

    /// <summary>
    /// Ein sehr schmales Interface, das eine Logging-Schnittstelle zur
    /// Verfügung stellt. Über dieses Interface können keine neuen Log-Provider
    /// zur Verfügung gestellt werden. 
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Fügt einen neuen Logeintrag zur Datenbank hinzu
        /// </summary>
        /// <param name="entry">Entry to be logged</param>
        void LogEntry(LogEntry entry);
    }
}
