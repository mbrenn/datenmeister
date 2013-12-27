using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.UserManagement.InMemory
{
    /// <summary>
    /// Just a user storage
    /// </summary>
    [Serializable]
    public class UserStorage
    {
        /// <summary>
        /// Stores the users
        /// </summary>
        private List<User> users = new List<User>();

        /// <summary>
        /// Gets the users
        /// </summary>
        public List<User> Users
        {
            get { return this.users; }
        }
    }
}
