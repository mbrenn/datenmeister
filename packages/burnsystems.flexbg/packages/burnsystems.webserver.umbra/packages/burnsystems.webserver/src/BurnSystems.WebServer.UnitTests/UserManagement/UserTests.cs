using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.WebServer.Modules.UserManagement.InMemory;
using BurnSystems.Test;

namespace BurnSystems.WebServer.UnitTests.UserManagement
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void TestInMemoryPassword()
        {
            var user1 = new User(1);
            var user2 = new User(2, "Karl", "Heinz");

            Assert.That(user1.Id, Is.EqualTo(1));
            Assert.That(user2.Id, Is.EqualTo(2));
            Assert.That(user2.Username, Is.EqualTo("Karl"));
            Assert.That(user2.IsPasswordCorrect("Heinz"), Is.True);
            user2.SetPassword("Otto");
            Assert.That(user2.IsPasswordCorrect("Heinz"), Is.False);
            Assert.That(user2.IsPasswordCorrect("Otto"), Is.True);
        }

        [Test]
        public void TestInMemoryUserManagement()
        {
            var userDb = new Modules.UserManagement.InMemory.UserManagement();
            userDb.Storage = new UserStorage();

            var user1 = new User(1);
            var user2 = new User(2, "Karl", "Heinz");

            userDb.Storage.Users.Add(user1);
            userDb.Storage.Users.Add(user2);

            var userFound = userDb.GetUser("Karl");
            Assert.That(userFound, Is.Not.Null);
            Assert.That(userFound.IsPasswordCorrect("Heinz"), Is.True);

            userFound = userDb.GetUser("Otto");
            Assert.That(userFound, Is.Null);

            userFound = userDb.GetUser("Karl", "Heinz");
            Assert.That(userFound, Is.Not.Null);

            userFound = userDb.GetUser("Karl", "Otto");
            Assert.That(userFound, Is.Null);

            userFound = userDb.GetUser("Karl", "heinz");
            Assert.That(userFound, Is.Null);
        }
    }
}
