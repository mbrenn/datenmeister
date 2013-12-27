using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Data;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Synchronisation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    /// <summary>
    /// Defines the usermanagement
    /// </summary>
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class UserManagementLocal : UserManagementFramework, IUserManagement, IFlexBgRuntimeModule
    {
        /// <summary>
        /// Stores the read write lock
        /// </summary>
        private ReadWriteLock readWriteLock = new ReadWriteLock();

        /// <summary>
        /// Stores the logger instance for this class
        /// </summary>
        private ILog classLogger = new ClassLogger(typeof(UserManagementLocal));

        /// <summary>
        /// Gets the database storing the users. 
        /// </summary>
        [Inject(IsMandatory = true)]
        public UserDatabase Db
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a certain user to database
        /// </summary>
        /// <param name="user"></param>
        public override void AddUserToDb(User user)
        {
            user.Id = this.Db.Data.GetNextUserId();

            this.Db.Data.Users.Add(user);
        }

        /// <summary>
        /// Gets a certain user by id
        /// </summary>
        /// <param name="userId">Id of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public override User GetUser(long userId)
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Users.Where(x => x.Id == userId).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a certain user by username
        /// </summary>
        /// <param name="username">Name of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public override User GetUser(string username)
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Users.Where(x => x.Username == username).FirstOrDefault();
            }
        }

        /// <summary>
        /// Remvoes user from database
        /// </summary>
        /// <param name="user">User to be removed</param>
        public void RemoveUser(User user)
        {
            using (this.readWriteLock.GetWriteLock())
            {
                this.Db.Data.Users.Remove(user);

                foreach (var found in this.Db.Data.Memberships.Where(x => x.UserId == user.Id).ToList())
                {
                    this.Db.Data.Memberships.Remove(found);
                }
            }
        }

        /// <summary>
        /// Stores the user into database
        /// </summary>
        /// <param name="user">User to be updated</param>
        public override void UpdateUser(User user)
        {
        }

        /// <summary>
        /// Sets the user data for a certain user
        /// </summary>
        /// <param name="user">User to be updated</param>
        /// <param name="key">Key of the userdata</param>
        /// <param name="value">Value of the userdata</param>
        public void SetUserData(User user, string key, object value)
        {
            user.UserData[key] = value;
            this.UpdateUser(user);
        }

        /// <summary>
        /// Gets userdata of a certain type
        /// </summary>
        /// <typeparam name="T">Type of the userdata</typeparam>
        /// <param name="user">User to be used</param>
        /// <param name="key">Key of the userdata</param>
        /// <returns>Returned data or null, if not correct type or not existing</returns>
        public T GetUserData<T>(User user, string key)
        {
            object result;
            if (user.UserData.TryGetValue(key, out result))
            {
                if (result is T)
                {
                    return (T)result;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsers()
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Users.ToList();
            }
        }

        /// <summary>
        /// Adds group to usermanagement
        /// </summary>
        /// <param name="group">Group to be added</param>
        public override void AddGroupToDb(Group group)
        {
            group.Id = this.Db.Data.GetNextGroupId();

            this.Db.Data.Groups.Add(group);
            this.Db.Data.SaveChanges();
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Group> GetAllGroups()
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Groups.ToList();
            }
        }

        /// <summary>
        /// Gets the group
        /// </summary>
        /// <param name="groupId">Id of the group</param>
        public override Group GetGroup(long groupId)
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Groups.FirstOrDefault(x => x.Id == groupId);
            }
        }

        /// <summary>
        /// Gets the group
        /// </summary>
        /// <param name="groupName">NAme of the group</param>
        public override Group GetGroup(string groupName)
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Groups.FirstOrDefault(x => x.Name == groupName);
            }
        }

        /// <summary>
        /// Removes a group from database
        /// </summary>
        /// <param name="group">Group to be removed</param>
        public void RemoveGroup(Group group)
        {
            using (this.readWriteLock.GetWriteLock())
            {
                this.Db.Data.Groups.Remove(group);

                foreach (var found in this.Db.Data.Memberships.Where(x => x.GroupId == group.Id).ToList())
                {
                    this.Db.Data.Memberships.Remove(found);
                }
            }
        }

        /// <summary>
        /// Adds membership
        /// </summary>
        /// <param name="group">Group to be associated</param>
        /// <param name="user">User to be associated</param>
        public override void AddToGroup(Group group, User user)
        {
            using (this.readWriteLock.GetWriteLock())
            {
                this.RemoveFromGroup(group, user);

                var membership = new Membership()
                {
                    GroupId = group.Id,
                    UserId = user.Id
                };

                this.Db.Data.Memberships.Add(membership);
            }
        }

        /// <summary>
        /// Removes membership
        /// </summary>
        /// <param name="group">Group of membership to be removed</param>
        /// <param name="user">User of membership to be removed</param>
        public void RemoveFromGroup(Group group, User user)
        {
            using (this.readWriteLock.GetWriteLock())
            {
                foreach (var found in this.Db.Data.Memberships.Where(x => x.GroupId == group.Id && x.UserId == user.Id).ToList())
                {
                    this.Db.Data.Memberships.Remove(found);
                }
            }
        }

        /// <summary>
        /// Checks if user is in group
        /// </summary>
        /// <param name="group">Group to be checked</param>
        /// <param name="user">User to be checked</param>
        public bool IsInGroup(Group group, User user)
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Memberships.Any(x => x.GroupId == group.Id && x.UserId == user.Id);
            }
        }

        /// <summary>
        /// Gets all groups where user is member
        /// </summary>
        /// <param name="user">User to be checked</param>
        /// <returns>Enumeration of users</returns>
        public IEnumerable<Group> GetGroupsOfUser(User user)
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Memberships
                    .Where(x => x.UserId == user.Id)
                    .Select(x => this.Db.Data.Groups.Where(y => y.Id == x.GroupId).FirstOrDefault())
                    .Where(x => x != null)
                    .ToList();
            }
        }

        /// <summary>
        /// Checks, if username is existing
        /// </summary>
        /// <param name="username">Username to be checked</param>
        /// <returns>true, if username is existing</returns>
        public override bool IsUsernameExisting(string username)
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Users.Any(x => x.Username == username);
            }
        }

        /// <summary>
        /// Checks, if group is existing
        /// </summary>
        /// <param name="groupName">Name of the group</param>
        /// <returns>True, if group is existing</returns>
        public override bool IsGroupExisting(string groupName)
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Groups.Any(x => x.Name == groupName);
            }
        }

        /// <summary>
        /// Starts the plugin
        /// </summary>
        public void Start()
        {
            this.VerifyThatAdminIsExisting();
        }

        /// <summary>
        /// Performs a shutdown
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// Gets a specific user by token id
        /// </summary>
        /// <param name="id">Id of the user</param>
        public User GetUserByToken(Guid id)
        {
            using (this.readWriteLock.GetReadLock())
            {
                return this.Db.Data.Users.Where(x => x.TokenId == id).FirstOrDefault();
            }
        }
    }
}
