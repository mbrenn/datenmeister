using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.Sessions
{
    /// <summary>
    /// Implements the session interface
    /// </summary>
    public interface ISessionInterface
    {
        /// <summary>
        /// Reads the session
        /// </summary>
        /// <returns>Session to be read</returns>
        Session GetSession();
    }
}
