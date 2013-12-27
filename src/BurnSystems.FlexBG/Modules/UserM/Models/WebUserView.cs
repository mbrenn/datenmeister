using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.Test;
using BurnSystems.WebServer.Modules.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Models
{
    /// <summary>
    /// Gives a view of an user as an IWebUser interface
    /// </summary>
    public class WebUserView : IWebUser
    {
        private User user;
        private IUserManagement userManagement;

        /// <summary>
        /// Initializes a new instance of the WebUserView
        /// </summary>
        /// <param name="userManagement">UserManagement to be used to access the user</param>
        /// <param name="user">User to be set</param>
        public WebUserView(IUserManagement userManagement, User user)
        {
            Ensure.That(userManagement != null);
            Ensure.That(user != null);

            this.userManagement = userManagement;
            this.user = user;
        }

        public long Id
        {
            get { return this.user.Id; }
        }

        public string Username
        {
            get { return this.user.Username; }
        }

        public Token Token
        {
            get { return new Token(this.user.TokenId, this.user.Username); }
        }

        public TokenSet CredentialTokenSet
        {
            get
            {
                var result = new TokenSet(
                    this.Token);
                foreach (var group in this.userManagement.GetGroupsOfUser(this.user))
                {
                    result.Add(
                        new Token(
                            group.TokenId,
                            group.Name));
                }

                return result;
            }
        }

        public bool IsPasswordCorrect(string password)
        {
            return this.userManagement.IsPasswordCorrect(this.user, password);
        }

        public void SetPassword(string password)
        {
            this.userManagement.SetPassword(this.user, password);
        }
    }
}
