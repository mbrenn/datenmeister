using BurnSystems.FlexBG.Modules.ServerInfoM;
using BurnSystems.FlexBG.Modules.MailSenderM;
using BurnSystems.FlexBG.Modules.UserM.Data;
using BurnSystems.FlexBG.Modules.UserM.Logic;
using BurnSystems.FlexBG.Modules.UserM.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Test.UserM
{
    /// <summary>
    /// Performs some test affecting user, groups and tokensets
    /// </summary>
    [TestFixture]
    public class WebUserTests
    {
        private static UserManagementLocal PrepareTests()
        {
            var result = new UserManagementLocal();
            result.Db = new UserDatabase();

            return result;
        }

        private static User CreateUserEntity(string username)
        {
            var user = new User();
            user.Username = username;
            user.EMail = username + "@depon.net";
            user.HasAgreedToTOS = true;
            return user;
        }

        [Test]
        public void TestAddandRetrieveOfUser()
        {
            var mgmt = PrepareTests();

            var user1 = CreateUserEntity("abc");
            var user2 = CreateUserEntity("test");

            mgmt.AddUser(user1);
            mgmt.AddUser(user2);

            var userCopy1 = mgmt.GetUser(user1.Id);
            var userCopy2 = mgmt.GetUser(user2.Id);

            Assert.That(userCopy1.Id != userCopy2.Id);
            Assert.That(userCopy1.Username, Is.EqualTo("abc"));
            Assert.That(userCopy2.Username, Is.EqualTo("test"));
        }

        [Test]
        public void RetrieveTokenSetForGroup()
        {
            var mgmt = PrepareTests();
            var webView = new WebUserManagementView(mgmt);

            var user = CreateUserEntity("abc");
            mgmt.AddUser(user);

            var group = new Group();
            group.Name = "Test";
            mgmt.AddGroup(group);

            mgmt.AddToGroup(group, user);

            var webUser = webView.GetUser("abc");
            Assert.That(webUser != null);

            var tokenSet1 = webUser.CredentialTokenSet;
            Assert.That(tokenSet1.Tokens.Any(x => x.Id == user.TokenId), Is.True);
            Assert.That(tokenSet1.Tokens.Any(x => x.Id == group.TokenId), Is.True);

            mgmt.RemoveFromGroup(group, user);
            var tokenSet2 = webUser.CredentialTokenSet;
            Assert.That(tokenSet2.Tokens.Any(x => x.Id == user.TokenId), Is.True);
            Assert.That(tokenSet2.Tokens.Any(x => x.Id == group.TokenId), Is.False);
        }
    }
}
