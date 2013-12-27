using BurnSystems.FlexBG.Modules.DeponNet;
using BurnSystems.FlexBG.Modules.LockMasterM;
using BurnSystems.FlexBG.Modules.ServerInfoM;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.FlexBG.Modules.UserQueryM;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    /// <summary>
    /// Defines the most common function of the usermanagement logic that can be you used by the 
    /// database dependent function of the usermanagement
    /// </summary>
    public abstract class UserManagementFramework
    {
        #region Constant names for administrator

        /// <summary>
        /// Defines the name of the Administrator
        /// </summary>
        public const string AdminName = "Admin";

        /// <summary>
        /// Defines the name of the Administrator
        /// </summary>
        public const string GroupAdminName = "Administrators";

        #endregion

        /// <summary>
        /// Stores the logger instance for this class
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(UserManagementFramework));

        /// <summary>
        /// Gets or sets the lockmaster to be used
        /// </summary>
        [Inject(IsMandatory = true)]
        public ILockMaster LockMaster
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IServerInfoProvider GameInfoProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the configuration of the usermanagement
        /// </summary>
        [Inject(IsMandatory = true)]
        public UserManagementConfig Configuration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user query
        /// </summary>
        [Inject]
        public IUserQuery UserQuery
        {
            get;
            set;
        }

        /// <summary>
        /// The implementation to retrieve a specific user from database
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Found user</returns>
        public abstract User GetUser(long userId);

        /// <summary>
        /// The implementation to retrieve a specific user from database
        /// </summary>
        /// <param name="username">Name of the user</param>
        /// <returns>Found user</returns>
        public abstract User GetUser(string usernasme);

        /// <summary>
        /// Stores the changes of the user into database
        /// </summary>
        /// <param name="user">User to be saved</param>
        /// <returns>Found user</returns>
        public abstract void UpdateUser(User user);

        /// <summary>
        /// Internal function that directly adds a user to database, without any other check
        /// </summary>
        /// <param name="user">User to be added to databse</param>
        public abstract void AddUserToDb(User user);

        /// <summary>
        /// Internal function that directly adds a user to database, without any other check
        /// </summary>
        /// <param name="user">User to be added to databse</param>
        public abstract void AddGroupToDb(Group user);

        /// <summary>
        /// Checks, whether the given username is already existing
        /// </summary>
        /// <param name="username">Username to be checked</param>
        /// <returns>true, if username is already existing</returns>
        public abstract bool IsUsernameExisting(string username);

        /// <summary>
        /// Checks, if group is existing
        /// </summary>
        /// <param name="groupName">Name of the group</param>
        /// <returns>True, if group is existing</returns>
        public abstract bool IsGroupExisting(string groupName);

        /// <summary>
        /// Adds a membership to database
        /// </summary>
        /// <param name="group">Group, which gets a new member</param>
        /// <param name="user">User, which shall be added</param>
        public abstract void AddToGroup(Group group, User user);

        /// <summary>
        /// Gets group by group id
        /// </summary>
        /// <param name="groupId">Id of the group</param>
        /// <returns>Found group or null</returns>
        public abstract Group GetGroup(long groupId);

        /// <summary>
        /// Gets group by groupname
        /// </summary>
        /// <param name="groupName">Name of the group</param>
        /// <returns>Found group or null</returns>
        public abstract Group GetGroup(string groupName);

        #region Helper functions to acquire read and write locks

        /// <summary>
        /// Acquires the readlock
        /// </summary>
        /// <returns></returns>
        private IDisposable AcquireReadLock()
        {
            return this.LockMaster.AcquireReadLock(EntityType.User, 1);
        }

        /// <summary>
        /// Acquires the write lock
        /// </summary>
        /// <returns></returns>
        private IDisposable AcquireWriteLock()
        {
            return this.LockMaster.AcquireWriteLock(EntityType.User, 1);
        }

        #endregion

        /// <summary>
        /// Adds a user to database
        /// </summary>
        /// <param name="user">Information of user to be added</param>
        public long AddUser(User user)
        {
            using (this.AcquireWriteLock())
            {
                if (this.IsUsernameExisting(user.Username))
                {
                    throw new UserManagementException(
                        UserManagementExceptionReason.UsernameExisting,
                        "Username existing");
                }

                if (user.HasAgreedToTOS == false)
                {
                    throw new UserManagementException(
                        UserManagementExceptionReason.NoAcceptTos,
                        "The Terms of Services have not been accepted");
                }

                if (string.IsNullOrWhiteSpace(user.Username))
                {
                    throw new UserManagementException(
                        UserManagementExceptionReason.NoUsername,
                        "The username is empty");
                }

                if (string.IsNullOrWhiteSpace(user.EMail))
                {
                    throw new UserManagementException(
                        UserManagementExceptionReason.NoEmail,
                        "The email is empty");
                }

                if (!user.IsEmailValid)
                {
                    throw new UserManagementException(
                        UserManagementExceptionReason.InvalidEmail,
                        "Invalid email address");
                }

                if (string.IsNullOrEmpty(user.ActivationKey))
                {
                    user.ActivationKey = StringManipulation.SecureRandomString(32);
                }

                if (string.IsNullOrEmpty(user.APIKey))
                {
                    user.APIKey = StringManipulation.SecureRandomString(32);
                }

                this.AddUserToDb(user);

                return user.Id;
            }
        }

        /// <summary>
        /// Adds group to usermanagement
        /// </summary>
        /// <param name="group">Group to be added</param>
        public long AddGroup(Group group)   
        {
            using (this.AcquireWriteLock())
            {
                Ensure.IsNotNull(group);

                if (string.IsNullOrEmpty(group.Name))
                {
                    throw new UserManagementException(
                        UserManagementExceptionReason.NoGroupTitle,
                        "No group title");
                }

                this.AddGroupToDb(group);

                return group.Id;
            }
        }

        /// <summary>
        /// Gets a certain user by username
        /// </summary>
        /// <param name="username">Name of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public virtual User GetUser(string username, string password)
        {
            using (this.AcquireReadLock())
            {
                var user = this.GetUser(username);
                if (user == null)
                {
                    return null;
                }

                if (this.IsPasswordCorrect(user, password))
                {
                    return user;
                }

                // Password is not correct
                return null;
            }
        }

        /// <summary>
        /// Checks, if the given password is correct for a certain user
        /// </summary>
        /// <param name="user">User to be checked</param>
        /// <param name="password">Password to be checked</param>
        /// <returns>true, if password is correct</returns>
        public bool IsPasswordCorrect(User user, string password)
        {
            Ensure.That(user != null);
            return user.EncryptedPassword == CreatePasswordHash(user, password);
        }

        /// <summary>
        /// Encrypts the password for a certain user. 
        /// It is important that the username does not get changed after encryption
        /// </summary>
        /// <param name="user">User whose password gets encrypted</param>
        /// <param name="password">Password to be encrypted</param>
        public void SetPassword(User user, string password)
        {            
            Ensure.That(user != null);
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("password is empty");
            }

            user.EncryptedPassword = this.CreatePasswordHash(user, password);

            this.UpdateUser(user);
        }

        /// <summary>
        /// Creates the password hash for a specific user. 
        /// Be aware that all passwords get invalid, if you modify this function
        /// </summary>
        /// <param name="user">User, for whom a password will be created</param>
        /// <param name="password">Password to be used</param>
        /// <returns>Password hash</returns>
        private string CreatePasswordHash(User user, string password)
        {
            var completePassword = user.Id.ToString("") + user.TokenId.ToString() + password + this.GameInfoProvider.ServerInfo.PasswordSalt;
            return completePassword.Sha1();
        }

        /// <summary>
        /// Changes the username of the given username.
        /// It will be checked whether the username is allowed and is already existing
        /// </summary>
        /// <param name="user">User to be checked</param>
        /// <param name="newUsername">New Username to be used</param>
        public void ChangeUsername(User user, string newUsername)
        {
            using (this.AcquireWriteLock())
            {
                var oldUsername = user.Username;

                // Check, if new username is equal to old username. 
                // If yes, do nothing
                if (user.Username == newUsername)
                {
                    return;
                }
                
                // Checks, if new username is already existing
                if (this.GetUser(newUsername) != null)
                {
                    throw new UserManagementException(
                        UserManagementExceptionReason.UsernameExisting,
                        "Username existing");
                }

                // Is username valid
                if (string.IsNullOrWhiteSpace(newUsername))
                {
                    throw new UserManagementException(
                        UserManagementExceptionReason.NoUsername,
                        "The username is empty");
                }

                // Everything ok, perform the change
                user.Username = newUsername;
                logger.Notify("'" + oldUsername + "' has been renamed to '" + newUsername + "'");

                this.UpdateUser(user);
            }
        }

        public void SetPersistantCookie(long userId, string series, string token)
        {
            using (this.AcquireWriteLock())
            {
                var user = this.GetUser(userId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                user.PersistantTokens[series] = token;
                this.UpdateUser(user);
            }
        }

        public bool CheckPersistantCookie(long userId, string series, string token)
        {
            using (this.AcquireReadLock())
            {
                var user = this.GetUser(userId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                string tokenCheck;

                if (user.PersistantTokens.TryGetValue(series, out tokenCheck))
                {
                    if (token == tokenCheck)
                    {
                        user.PersistantTokens.Remove(series);
                        this.UpdateUser(user);
                        return true;
                    }
                    else
                    {
                        logger.Fail("Perhabs Security breach of cookie for user " + userId);

                        // Clears all cookies
                        user.PersistantTokens.Clear();
                        this.UpdateUser(user);
                    }
                }

                return false;
            }
        }

        public void DeletePersistantCookie(long userId, string series)
        {
            using (this.AcquireWriteLock())
            {
                var user = this.GetUser(userId);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }

                user.PersistantTokens.Remove(series);
                this.UpdateUser(user);
            }
        }

        /// <summary>
        /// Verifies that administrator is existing
        /// </summary>
        public void VerifyThatAdminIsExisting()
        {
            bool adminCreationRequired = false;

            using (this.AcquireReadLock())
            {
                adminCreationRequired = !this.IsUsernameExisting(AdminName) || !this.IsGroupExisting(GroupAdminName);
            }

            if (adminCreationRequired)
            {
                if (this.UserQuery != null &&
                    this.UserQuery.Ask(
                        "No Administrator found. Shall an administrator be created?",
                        new[] { "y", "n" },
                        "y") == "y")
                {
                    logger.LogEntry(LogLevel.Message, "Administrator is initialized");
                    this.InitAdmin();
                }
            }
        }

        /// <summary>
        /// Initializes the admin, if necessary
        /// </summary>
        public void InitAdmin()
        {
            if (!this.IsUsernameExisting(AdminName))
            {
                logger.Message("Creating user with name: " + AdminName);
                var password = this.UserQuery.Ask(
                    "Give password: ",
                    null,
                    this.GameInfoProvider.ServerInfo.PasswordSalt);


                var user = new User();
                user.Username = AdminName;
                user.EMail = this.GameInfoProvider.ServerInfo.AdminEMail;
                user.HasAgreedToTOS = true;
                user.HasNoCredentials = false;
                user.IsActive = true;

                this.AddUser(user);

                this.SetPassword(user, password);
            }

            if (!this.IsGroupExisting(GroupAdminName))
            {
                var group = new Group();
                group.Name = GroupAdminName;
                group.TokenId = Group.AdministratorsToken;

                this.AddGroup(group);
            }

            var adminUser = this.GetUser(AdminName);
            var adminGroup = this.GetGroup(GroupAdminName);

            this.AddToGroup(adminGroup, adminUser);
        }
    }    
}
