using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.UserManagement
{
    public interface IWebUserManagement
    {
        IWebUser GetUser(long userId);
        IWebUser GetUser(string username);
        IWebUser GetUser(string username, string password);

        /// <summary>
        /// Updatas the login date for a certain user
        /// </summary>
        /// <param name="userId">Id of the user that shall be updated</param>
        void UpdateLoginDate(long userId, DateTime date);

        /// <summary>
        /// Sets a persistant cookie to user.
        /// The following mechanism is used: 
        /// http://stackoverflow.com/questions/244882/what-is-the-best-way-to-implement-remember-me-for-a-website
        /// </summary>
        /// <param name="series">Series to be stored</param>
        /// <param name="token">Token to be stored</param>
        void SetPersistentCookie(IWebUser user, string series, string token);

        /// <summary>
        /// Checks, if the retrieved persistant cookie is correct. 
        /// The following mechanism is used: 
        /// http://stackoverflow.com/questions/244882/what-is-the-best-way-to-implement-remember-me-for-a-website
        /// The token is removed
        /// </summary>
        /// <param name="series">Series to be checked</param>
        /// <param name="token">Token to be checked</param>
        /// <returns>true, if ok</returns>
        bool CheckPersistentCookie(IWebUser user, string series, string token);

        /// <summary>
        /// Deletes the persistant cookie
        /// </summary>
        /// <param name="series">Series to be deleted</param>
        void DeletePersistentCookie(IWebUser user, string series);
    }
}
