using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Web
{
    /// <summary>
    /// Defines the interface for the servermanager
    /// </summary>
    public interface IServerManager
    {
        /// <summary>
        /// Initializes the server manager
        /// </summary>
        void Init();

        /// <summary>
        /// Gets the server pool
        /// </summary>
        /// <returns>Gets the pool being used by a server</returns>
        IPool GetServerPool();

        /// <summary>
        /// Gets the data pool
        /// </summary>
        /// <returns>The received data pool</returns>
        IPool GetDataPool();
    }
}
