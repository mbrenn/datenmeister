using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.WebServer.Modules.UserManagement;

namespace BurnSystems.WebServer.UnitTests.UserManagement
{
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public void TestWrongLogin()
        {
            using (var server = ServerTests.CreateServer())
            {
                var cookie = BspxTests.GetCookie();
                var webRequest = BspxTests.GetSessionRequest(
                    cookie,
                    "http://localhost:8081/controller/Users/Login", 
                    "Username=no&Password=Yes");

                var responseValue = BspxTests.GetResponseObject<UserManagementController.LoginResult>(webRequest);
                Assert.That(responseValue.Success, Is.False);                
            }
        }

        [Test]
        public void TestCorrectLogin()
        {
            using (var server = ServerTests.CreateServer())
            {
                var cookie = BspxTests.GetCookie();
                var webRequest = BspxTests.GetSessionRequest(
                    cookie,
                    "http://localhost:8081/controller/Users/Login",
                    "Username=Karl&Password=Heinz");

                var responseValue = BspxTests.GetResponseObject<UserManagementController.LoginResult>(webRequest);
                Assert.That(responseValue.Success, Is.True);
            }
        }

        [Test]
        public void TestUserRetrieval()
        {
            using (var server = ServerTests.CreateServer())
            {
                var cookie = BspxTests.GetCookie();

                var currentUser = BspxTests.GetResponseObject<UserManagementController.GetLoggedInUserResult>(
                    cookie,
                    "http://localhost:8081/controller/Users/CurrentUser");

                Assert.That(currentUser.IsUserLoggedIn, Is.False);

                var responseValue = BspxTests.GetResponseObject<UserManagementController.LoginResult>(
                    cookie,
                    "http://localhost:8081/controller/Users/Login",
                    "Username=Karl&Password=Heinz");
                Assert.That(responseValue.Success, Is.True);

                currentUser = BspxTests.GetResponseObject<UserManagementController.GetLoggedInUserResult>(
                    cookie,
                    "http://localhost:8081/controller/Users/CurrentUser");

                Assert.That(currentUser.IsUserLoggedIn, Is.True);
                Assert.That(currentUser.Username, Is.EqualTo("Karl"));
                Assert.That(currentUser.UserId, Is.EqualTo(1));
            }
        }

        [Test]
        public void TestLogout()
        {
            using (var server = ServerTests.CreateServer())
            {
                var cookie = BspxTests.GetCookie();
                
                var userLoggedIn = BspxTests.GetResponseObject<UserManagementController.IsUserLoggedInResult>(
                    cookie,
                    "http://localhost:8081/controller/Users/IsUserLoggedIn");
                Assert.That(userLoggedIn.IsUserLoggedIn, Is.False);

                var responseValue = BspxTests.GetResponseObject<UserManagementController.LoginResult>(
                    cookie,
                    "http://localhost:8081/controller/Users/Login",
                    "Username=Karl&Password=Heinz");
                Assert.That(responseValue.Success, Is.True);

                userLoggedIn = BspxTests.GetResponseObject<UserManagementController.IsUserLoggedInResult>(
                    cookie,
                    "http://localhost:8081/controller/Users/IsUserLoggedIn");
                Assert.That(userLoggedIn.IsUserLoggedIn, Is.True);

                userLoggedIn = BspxTests.GetResponseObject<UserManagementController.IsUserLoggedInResult>(
                    cookie,
                    "http://localhost:8081/controller/Users/IsUserLoggedIn");
                Assert.That(userLoggedIn.IsUserLoggedIn, Is.True);

                BspxTests.GetResponseObject<UserManagementController.IsUserLoggedInResult>(
                    cookie,
                    "http://localhost:8081/controller/Users/Logout");
                Assert.That(userLoggedIn.IsUserLoggedIn, Is.True);

                userLoggedIn = BspxTests.GetResponseObject<UserManagementController.IsUserLoggedInResult>(
                    cookie,
                    "http://localhost:8081/controller/Users/IsUserLoggedIn");
                Assert.That(userLoggedIn.IsUserLoggedIn, Is.False);
            }
        }
    }
}
