using BurnSystems.Collections;
using BurnSystems.WebServer.Modules.UserManagement;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.UserM.Models
{
    /// <summary>
    /// Stores the userinformation
    /// </summary>
    [Serializable]
    public class User
    {
        public User()
        {
            this.PremiumTill = DateTime.Now;
            this.TokenId = Guid.NewGuid();
        }

        private long id;

        private string username;

        private string encryptedPassword;

        private string eMail;

        private string activationKey;

        private string apiKey;

        private DateTime premiumTill;

        private bool isActive;

        private bool hasAgreedToTOS;

        private bool hasNoCredentials;

        private Guid tokenId;

        /// <summary>
        /// Stores the tokens 
        /// </summary>
        private Dictionary<string, string> persistantTokens = new Dictionary<string, string>();

        /// <summary>
        /// Stores the userdata of the complete class
        /// </summary>
        private Dictionary<string, object> userData;

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public virtual long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public virtual string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        /// <summary>
        /// Gets or sets the encrypted password
        /// </summary>
        public virtual string EncryptedPassword
        {
            get { return this.encryptedPassword; }
            set { this.encryptedPassword = value; }
        }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        public virtual string EMail
        {
            get { return this.eMail; }
            set { this.eMail = value; }
        }

        /// <summary>
        /// Gets or sets the activation key
        /// </summary>
        public virtual string ActivationKey
        {
            get { return this.activationKey; }
            set { this.activationKey = value; }
        }

        /// <summary>
        /// Gets or sets the API key
        /// </summary>
        public virtual string APIKey
        {
            get { return this.apiKey; }
            set { this.apiKey = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the user is a premium user
        /// </summary>
        public virtual bool IsPremium
        {
            get { return DateTime.Now < this.PremiumTill; }
        }

        /// <summary>
        /// Gets or sets a date until user is premium
        /// </summary>
        public virtual DateTime PremiumTill
        {
            get { return this.premiumTill; }
            set { this.premiumTill = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active
        /// </summary>
        public virtual bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user has agreed to the terms of service
        /// </summary>
        public virtual bool HasAgreedToTOS
        {
            get { return this.hasAgreedToTOS; }
            set { this.hasAgreedToTOS = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user has no credentials. 
        /// This can happen by Facebook-Login
        /// </summary>
        public virtual bool HasNoCredentials
        {
            get { return this.hasNoCredentials; }
            set { this.hasNoCredentials = value; }
        }

        /// <summary>
        /// Gets or sets the dictionary for persistant tokens
        /// </summary>
        public Dictionary<string, string> PersistantTokens
        {
            get { return this.persistantTokens; }
            set { this.persistantTokens = value; }
        }

        public Guid TokenId
        {
            get { return this.tokenId; }
            set { this.tokenId = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the given email is valid
        /// </summary>
        public bool IsEmailValid
        {
            get
            {
                return this.EMail.IsValidEmail();
            }
        }

        /// <summary>
        /// Converts to string
        /// </summary>
        /// <returns>User that had been converted to string</returns>
        public override string ToString()
        {
            return this.Username;
        }

        /// <summary>
        /// Gets the user data array
        /// </summary>
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        internal Dictionary<string, object> UserData
        {
            get
            {
                if (this.userData == null)
                {
                    this.userData = new Dictionary<string, object>();
                }

                return this.userData;
            }
            set
            {
                this.userData = value;
            }
        }
    }
}
