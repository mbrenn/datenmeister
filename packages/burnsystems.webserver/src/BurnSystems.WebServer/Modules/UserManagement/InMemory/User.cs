using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.UserManagement.InMemory
{
	[Serializable]
	public class User : IWebUser
	{
		/// <summary>
		/// Stores the list of tokens, which are used for current login
		/// </summary>
		private List<PersistentToken> persistenceTokens = new List<PersistentToken> ();
		
		public long Id {
			get;
			private set;
		}

		public string Username {
			get;
			set;
		}

		public Guid TokenId {
			get;
			private set;
		}

		public TokenSet CredentialTokenSet {
			get {
				return new TokenSet (
                    this.Token);
			}
		}

		private string EncryptedPassword {
			get;
			set;
		}

		public User (long id)
		{
			this.Id = id;
			this.TokenId = Guid.NewGuid ();
		}

		public User (long id, string username, string password)
            : this(id)
		{
			this.Username = username;
			this.SetPassword (password);
		}

		public bool IsPasswordCorrect (string password)
		{
			var hash = this.Username + password;
			return this.EncryptedPassword == hash.Sha1 ();
		}

		public void SetPassword (string password)
		{
			this.EncryptedPassword = (this.Username + password).Sha1 ();
		}

		/// <summary>
		/// Creates the token for the user
		/// </summary>
		/// <returns>Token of the user</returns>
		public Token Token {
			get {
				return new Token ()
                {
                    Id = this.TokenId,
                    Name = this.Username
                };
			}
		}

		public DateTime LastLoginDate {
			get;
			set;
		}
		
		public List<PersistentToken> PersistanceTokens {
			get {
				if (this.persistenceTokens == null) {
					return new List<PersistentToken> ();
				}
				
				return this.persistenceTokens;
			}
		}
		
		public void AddOrUpdateToken (string series, string token)
		{
			lock (this.persistenceTokens) {
				var existing = this.persistenceTokens.Where (x => x.Series == series).FirstOrDefault ();
				
				// Checks, if the persistence token has already been set
				if (existing == null) {
					existing = new PersistentToken ();
					this.persistenceTokens.Add (existing);
				}
				
				// Sets the token.
				existing.Token = token;
			}
		}
		
		public void RemoveToken (string series)
		{
			lock (this.persistenceTokens) {
				foreach (var token in this.persistenceTokens.Where (x => x.Series == series).ToList ()) {
					this.persistenceTokens.Remove (token);
				}
			}
		}
		
		public bool CheckAndRemoveToken (string series, string token)
		{
			lock (this.persistenceTokens) {
				var existing = this.persistenceTokens.Where (x => x.Series == series).FirstOrDefault ();
				
				if (existing == null) {
					return false;
				}
			
				this.persistenceTokens.Remove (existing);
				return true;
			}
		}
	}
}
