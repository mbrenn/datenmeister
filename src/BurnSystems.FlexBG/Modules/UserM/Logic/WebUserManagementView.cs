using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Modules.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    /// <summary>
    /// Defines the webuser management
    /// </summary>
    public class WebUserManagementView : IWebUserManagement
    {
        private IUserManagement usermanagement;

        [Inject]
        public WebUserManagementView(IUserManagement local)
        {
            Ensure.That(local != null, "IUserManagement is null");

            this.usermanagement = local;
        }

        public IWebUser GetUser(long userId)
        {
            var user = this.usermanagement.GetUser(userId);
            if (user == null)
            {
                return null;
            }

            return new WebUserView(this.usermanagement, user);
        }
    
        public IWebUser GetUser(string username)
        {
            var user = this.usermanagement.GetUser(username);
            if (user == null)
            {
                return null;
            }

            return new WebUserView(this.usermanagement, user);
        }

        public IWebUser GetUser(string username, string password)
        {
            var user = this.usermanagement.GetUser(username, password);
            if (user == null)
            {
                return null;
            }

            return new WebUserView(this.usermanagement, user);
        }


        public void SetPersistentCookie(IWebUser webUser, string series, string token)
        {
            this.usermanagement.SetPersistantCookie(webUser.Id, series, token);            
        }

        public bool CheckPersistentCookie(IWebUser webUser, string series, string token)
        {            
            return this.usermanagement.CheckPersistantCookie(webUser.Id, series, token);
        }

        public void DeletePersistentCookie(IWebUser webUser, string series)
        {
            this.usermanagement.DeletePersistantCookie(webUser.Id, series);
        }


        /// <summary>
        /// Updates the login date for a certain user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="date">Date which shall be set</param>
        public void UpdateLoginDate(long userId, DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }

            var user = this.usermanagement.GetUser(userId);
            if (user != null)
            {
                this.usermanagement.SetUserData(user, UserDataTokens.LastLogin, date);
            }
        }
    }
}
