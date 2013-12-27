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
    public class CurrentWebUserHelper
    {
        /// <summary>
        /// Name of the current user
        /// </summary>
        public const string Name = "CurrentWebUser";

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
        public IWebUserManagement UserManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current user
        /// </summary>
        /// <param name="activates">Activation container to be used</param>
        /// <returns>User to be retrieved</returns>
        public static IWebUser GetCurrentUser(IActivates activates)
        {
            var helper = activates.Create<CurrentWebUserHelper>();
            return helper.Authentication.GetLoggedInUser();
        }
    }
}
