using BurnSystems.FlexBG.Modules.Database.MongoDb;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using BurnSystems.FlexBG.Modules.UserM.Logic;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.ServerInfoM;

namespace BurnSystems.FlexBG.Test.Database.MongoDb
{
    /// <summary>
    /// Tests the user addition in Mongodatabase
    /// </summary>
    [TestFixture]
    public class UserTests
    {
        public static IActivates Init()
        {
            var result = MongoDbTests.Init() as ActivationContainer;

            result.Bind<IUserManagement>().To<UserManagementMongoDb>();
            result.Bind<IServerInfoProvider>().ToConstant(
                new ServerInfoProvider(null)).AsSingleton();
            result.Bind<UserManagementConfig>().ToConstant(
                new UserManagementConfig());

            var userDb = result.Get<IUserManagement>() as UserManagementMongoDb;
            userDb.UserCollection.Drop();
            userDb.GroupCollection.Drop();
            userDb.MembershipCollection.Drop();

                    
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
        public void TestAddAndRetrievalDirectly()
        {
            var container = Init();
            var dbConnection = container.Get<MongoDbConnection>();
            var database = dbConnection.Database;

            var users = database.GetCollection<User>("users");
            users.Drop();

            var newUser  = new User();
            newUser.Username = "Testusername";
            newUser.TokenId = Guid.NewGuid();
            newUser.PremiumTill = DateTime.UtcNow.AddDays(2);
            newUser.IsActive = true;
            newUser.Id = 12;
            newUser.EMail = "brenn@depon.net";
            users.Insert(newUser);

            var foundUsers = users.Count();
            Assert.That(foundUsers, Is.EqualTo(1));

            var foundUser = users.AsQueryable<User>().First();
            Assert.That(foundUser != null);

            // Let's see
            Assert.That(foundUser.Username, Is.EqualTo(newUser.Username));
            Assert.That(foundUser.TokenId, Is.EqualTo(newUser.TokenId));
            Assert.That((foundUser.PremiumTill - newUser.PremiumTill).TotalSeconds < 1);
            Assert.That(foundUser.IsActive, Is.EqualTo(newUser.IsActive));
            Assert.That(foundUser.Id, Is.EqualTo(newUser.Id));
            Assert.That(foundUser.EMail, Is.EqualTo(newUser.EMail));
        }

        [Test]
        public void TestAddandRetrieveOfUserViaWebUser()
        {
            var init =  Init();
            var mgmt = init.Get<IUserManagement>();

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
        public void RetrieveTokenSetForGroupViaWebUser()
        {
            var init = Init();
            var mgmt = init.Get<IUserManagement>();
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
