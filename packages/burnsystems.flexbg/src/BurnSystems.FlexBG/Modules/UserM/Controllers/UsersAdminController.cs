using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;

namespace BurnSystems.FlexBG.Modules.UserM.Controllers
{
    /// <summary>
    /// Controller for user admin
    /// </summary>
    public class UsersAdminController : BurnSystems.WebServer.Modules.MVC.Controller
    {
        [Inject(IsMandatory = true)]
        public IUserManagement UserManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the password of the user
        /// </summary>
        /// <param name="model">Model to be used</param>
        [WebMethod]
        public IActionResult SetPassword([PostModel] AdminSetPasswordModel model)
        {
            var user = this.UserManagement.GetUser(model.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User with " + model.Id + " not found");
            }

            if (string.IsNullOrWhiteSpace ( model.NewPassword ))
            {
                throw new InvalidOperationException ( "Password is empty");
            }

            this.UserManagement.SetPassword(user, model.NewPassword);

            return this.Json(
                new
                {
                    success = true
                });
        }

        [WebMethod]
        public IActionResult UpdateProfile([PostModel] AdminUpdateProfileModel model)
        {
            var user = this.UserManagement.GetUser(model.Id);
            if (user == null)
            {
                throw new InvalidOperationException("User with " + model.Id + " not found");
            }

            this.UserManagement.SetUserData(user, UserDataTokens.DisplayName, model.Displayname);

            return this.Json(
                new
                {
                    success = true
                });
        }
    }
}
