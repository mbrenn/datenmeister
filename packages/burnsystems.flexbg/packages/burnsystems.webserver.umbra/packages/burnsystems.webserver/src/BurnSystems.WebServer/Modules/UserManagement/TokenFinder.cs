using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;

namespace BurnSystems.WebServer.Modules.UserManagement
{
    /// <summary>
    /// Returns tokens of users or groups
    /// </summary>
    public class TokenFinder
    {
        [Inject]
        public IWebUserManagement UserManagement
        {
            get;
            set;
        }

        public TokenFinder()
        {
        }

        public TokenFinder(IWebUserManagement userManagement)
        {
            this.UserManagement = userManagement;
        }

        /// <summary>
        /// Gets token of user by name
        /// </summary>
        /// <param name="username">Name of the user</param>
        /// <returns>Token of the user</returns>
        public Token GetTokenOfUser(string username)
        {
            var user = this.UserManagement.GetUser(username);
            return user.Token;
        }
    }
}
