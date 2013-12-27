using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.UserManagement
{
    public interface IAuthentication
    {
        /// <summary>
        /// Performs the login for the user
        /// </summary>
        /// <param name="username">Name of the user</param>
        /// <param name="password">Password of the user</param>
        /// <param name="isPersistant">Gets or sets a value indicating whether the login shall be persistant,
        /// even if browser had been closed</param>
        /// <returns>Found user, if successful, otherwise null</returns>
        IWebUser LoginUser(string username, string password, bool isPersistant = false);

        /// <summary>
        /// Performs the login for the user
        /// </summary>
        /// <param name="userId">Id of the user, which is doing the login</param>
        /// <returns>Found user, if successful, otherwise null</returns>
        IWebUser LoginUser(long userId);

        /// <summary>
        /// Performs logout of the user
        /// </summary>
        void LogoutUser();

        /// <summary>
        /// Checks, if a user is logged in.
        /// This function does not perform a check by persistent cookie!
        /// Use IsLoggedInByPersistentCookie. This function also performs the login
        /// </summary>
        /// <returns>Checks, if user is logged in</returns>
        bool IsUserLoggedIn();

        /// <summary>
        /// Gets the currently logged in user
        /// </summary>
        /// <returns>User, who is logged in</returns>
        IWebUser GetLoggedInUser();

        /// <summary>
        /// Gets a list of tokens of the current user
        /// </summary>
        /// <returns>List of Tokens</returns>
        TokenSet GetTokensOfLogin();

        /// <summary>
        /// Checks, if the user is logged in via persistent cookie.
        /// This function also performs the login
        /// </summary>
        /// <returns>true, if user is logged in by persistent cookie</returns>
        bool IsLoggedInByPersistentCookie();
    }
}
