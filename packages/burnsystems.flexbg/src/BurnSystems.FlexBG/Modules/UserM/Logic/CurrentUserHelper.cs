using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    /// <summary>
    /// Helper to retrieve the current user
    /// </summary>
    public class CurrentUserHelper
    {
        /// <summary>
        /// Name of the current user
        /// </summary>
        public const string Name = "CurrentUser";

        /// <summary>
        /// Gets or sets the authentication
        /// </summary>
        [Inject]
        public IAuthentication Authentication
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user management
        /// </summary>
        [Inject]
        public IUserManagement UserManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <param name="activates">Activation container to be used</param>
        /// <returns>User to be retrieved</returns>
        public static User GetCurrentUser(IActivates activates)
        {
            var helper = activates.Create<CurrentUserHelper>();
            var webUser = helper.Authentication.GetLoggedInUser();
            if (webUser == null)
            {
                return null;
            }

            var user = helper.UserManagement.GetUser(webUser.Id);
            return user;
        }
    }
}
