using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.Test;
using BurnSystems.ObjectActivation;

namespace BurnSystems.WebServer.Modules.UserManagement.InMemory
{
	public class UserManagement : IWebUserManagement
	{
		[Inject]
		public UserStorage Storage {
			get;
			set;
		}

		public IWebUser GetUser (long userId)
		{
			lock (this.Storage) {
				return this.Storage.Users
                    .Where (x => x.Id == userId)
                    .FirstOrDefault ();
			}
		}

		public IWebUser GetUser (string username)
		{
			lock (this.Storage) {
				return this.Storage.Users
                    .Where (x => x.Username == username)
                    .FirstOrDefault ();
			}
		}

		public IWebUser GetUser (string username, string password)
		{
			lock (this.Storage) {
				return this.Storage.Users
                    .Where (x => x.Username == username && x.IsPasswordCorrect (password))
                    .FirstOrDefault ();
			}
		}

		public void SetPersistentCookie (IWebUser webUser, string series, string token)
		{
			var user = webUser as User;
			Ensure.That (user != null, "User is not of type User");
			
			user.AddOrUpdateToken (series, token);
		}

		public bool CheckPersistentCookie (IWebUser webUser, string series, string token)
		{
			var user = webUser as User;
			Ensure.That (user != null, "User is not of type User");
			
			return user.CheckAndRemoveToken (series, token);
		}

		public void DeletePersistentCookie (IWebUser webUser, string series)
		{
			var user = webUser as User;
			Ensure.That (user != null, "User is not of type User");
			
			user.RemoveToken (series);
		}

		public void UpdateLoginDate (long userId, DateTime date)
		{
			var user = this.GetUser (userId) as User;
			if (user != null) {
				user.LastLoginDate = date;
			}
		}
	}
}
