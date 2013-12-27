using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.Sessions;
using BurnSystems.Test;
using BurnSystems.WebServer.Dispatcher;
using System.Net;
using BurnSystems.WebServer.Modules.Cookies;

namespace BurnSystems.WebServer.Modules.UserManagement
{
    public class Authentication : IAuthentication
    {
        /// <summary>
        /// Defines the cookie name
        /// </summary>
        private const string cookieName = "BSAuthCookie";

        private const string sessionUserId = "Authentication.UserId";

        private const string sessionTokenSet = "Authentication.TokenSet";

        private const string sessionPersistent = "Authentication.Persistent";

        /// <summary>
        /// Gets or sets the current session
        /// </summary>
        [Inject(IsMandatory = true)]
        public Session Session
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current session
        /// </summary>
        [Inject(IsMandatory = true)]
        public IWebUserManagement UserManagement
        {
            get;
            set;
        }

        [Inject]
        public ICookieManagement Cookies
        {
            get;
            set;
        }

        /// <summary>
        /// Performs the login
        /// </summary>
        /// <param name="username">Name of the user</param>
        /// <param name="password">Password of user (or at least assumed password)</param>
        /// <param name="isPersistant">true, if credentials shall be stored in cookie</param>
        /// <returns>Retrieved webuser</returns>
        public IWebUser LoginUser(string username, string password, bool isPersistant = false)
        {
            var user = this.UserManagement.GetUser(username, password);
            if (user == null)
            {
                return null;
            }

            return this.LoginUser(user, isPersistant);
        }

        /// <summary>
        /// Performs the login by user id. 
        /// Somehow safety critical
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Retrieved webuser</returns>
        public IWebUser LoginUser(long userId)
        {
            var user = this.UserManagement.GetUser(userId);
            if (user == null)
            {
                return null;
            }

            return this.LoginUser(user, false);
        }

        private IWebUser LoginUser(IWebUser user, bool isPersistant)
        {
            this.Session[sessionUserId] = user.Id;
            this.Session[sessionTokenSet] = user.CredentialTokenSet;
            this.UserManagement.UpdateLoginDate(user.Id, DateTime.Now);

            if (isPersistant)
            {
                // Store cookies over lifetime, add some additional secrets
                this.AssignPersistentCookie(user);
            }

            return user;
        }

        /// <summary>
        /// Performs the logout
        /// </summary>
        public void LogoutUser()
        {
            if (this.IsUserLoggedIn())
            {
                this.RemovePersistentCookie();

                this.Session.Remove(sessionUserId);
                this.Session.Remove(sessionTokenSet);
            }
        }

        /// <summary>
        /// Gets or sets whether the user is currently logged in.
        /// The current session and a persistance login will be used.
        /// </summary>
        /// <returns>True, if user had been logged in</returns>
        public bool IsUserLoggedIn()
        {
            if (this.Session[sessionUserId] != null)
            {
                return true;
            }

            return false;

            // Commented out because multiple requests interfer with series change.
            //return this.IsLoggedInByPersistentCookie();
        }

        public IWebUser GetLoggedInUser()
        {
            if (!this.IsUserLoggedIn())
            {
                return null;
            }

            var user = this.UserManagement.GetUser(
                 Convert.ToInt64(this.Session[sessionUserId]));

            return user;
        }

        /// <summary>
        /// Gets a list of tokens of the current user
        /// </summary>
        /// <returns>List of Tokens</returns>
        public TokenSet GetTokensOfLogin()
        {
            if (!this.IsUserLoggedIn())
            {
                return null;
            }

            return this.Session[sessionTokenSet] as TokenSet;
        }

        /// <summary>
        /// Assigns a persistant cookie to the user that is used to reauthenticate the user
        /// Idea taken from: http://stackoverflow.com/questions/244882/what-is-the-best-way-to-implement-remember-me-for-a-website
        /// </summary>
        /// <param name="user">User, which shall be assigned</param>
        /// <param name="series">Series which shall be reused</param>
        private void AssignPersistentCookie(IWebUser user, string series = null)
        {
            if (string.IsNullOrEmpty(series))
            {
                series = StringManipulation.SecureRandomString(32);
            }

            var token = StringManipulation.SecureRandomString(32);

            var cookieValue = string.Format(
                "{0}|{1}|{2}",
                user.Id,
                series,
                token);

            var cookie = new Cookie(
                cookieName,
                cookieValue);
            cookie.Expires = DateTime.Now.AddDays(30);
            cookie.Path = "/";
            cookie.HttpOnly = true;

            this.UserManagement.SetPersistentCookie(user, series, token);
            this.Session[sessionPersistent] = series;

            this.Cookies.AddCookie(cookie);
        }

        /// <summary>
        /// Checks, if the user is logged in via persistent cookie
        /// </summary>
        /// <returns>true, if user is logged in by persistent cookie</returns>
        public bool IsLoggedInByPersistentCookie()
        {
            // Check, if user is logged via permanent cookie
            var cookie = this.Cookies.GetCookie(cookieName);
            if (cookie != null)
            {
                // Check for cookie
                var splitted = cookie.Split(new[] { '|' });
                if (splitted.Length != 3)
                {
                    return false;
                }
                var userId = Convert.ToInt64(splitted[0]);
                var series = splitted[1];
                var token = splitted[2];

                // Get user
                var user = this.UserManagement.GetUser(userId);
                if (user == null)
                {
                    // User not found
                    return false;
                }

                if (this.UserManagement.CheckPersistentCookie(user, series, token))
                {
                    // Login, create new token
                    this.AssignPersistentCookie(user, series);

                    // Perform the login
                    this.Session[sessionUserId] = user.Id;
                    this.Session[sessionTokenSet] = user.CredentialTokenSet;

                    this.UserManagement.UpdateLoginDate(user.Id, DateTime.Now);

                    // Everything OK... Hopefully
                    return true;
                }
                else
                {
                    // No login
                    return false;
                }
            }

            // No cookie
            return false;
        }

        /// <summary>
        /// Removes the persistent cookie
        /// </summary>
        private void RemovePersistentCookie()
        {
            // Checks, if we have a permanent login that needs to be stopped
            if (this.Session[sessionPersistent] != null)
            {
                var user = this.UserManagement.GetUser(Convert.ToInt64(this.Session[sessionUserId]));
                if (user != null)
                {
                    this.UserManagement.DeletePersistentCookie(
                        user,
                        this.Session[sessionPersistent] as string);

                    // Remove persistent cookie, if existing
                    this.Cookies.DeleteCookie(cookieName);
                }
            }
        }
    }
}
