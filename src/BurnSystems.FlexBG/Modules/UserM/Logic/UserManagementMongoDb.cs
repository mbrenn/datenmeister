using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.Database.MongoDb;
using BurnSystems.FlexBG.Modules.UserM.Data;
using BurnSystems.FlexBG.Modules.UserM.Interfaces;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Synchronisation;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BurnSystems.FlexBG.Modules.UserM.Logic
{
    /// <summary>
    /// Defines the usermanagement
    /// </summary>
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class UserManagementMongoDb : UserManagementFramework, IUserManagement, IFlexBgRuntimeModule
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
        public MongoDbConnection Db
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a user to database
        /// </summary>
        /// <param name="user">Information of user to be added</param>
        public override void AddUserToDb(User user)
        {
            user.Id = this.GetNextUserId();
            this.UserCollection.Insert(user);
        }

        /// <summary>
        /// Gets a certain user by id
        /// </summary>
        /// <param name="userId">Id of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public override User GetUser(long userId)
        {
            return this.UserCollection.AsQueryable().Where(x => x.Id == userId).FirstOrDefault();
        }

        /// <summary>
        /// Gets a certain user by username
        /// </summary>
        /// <param name="username">Name of the user to be requested</param>
        /// <returns>Containing the user</returns>
        public override User GetUser(string username)
        {
            return this.UserCollection.AsQueryable().Where(x => x.Username == username).FirstOrDefault();
        }

        /// <summary>
        /// Remvoes user from database
        /// </summary>
        /// <param name="user">User to be removed</param>
        public void RemoveUser(User user)
        {
            this.UserCollection.Remove(Query.EQ("Id", user.Id));

            this.MembershipCollection.Remove(Query.EQ("UserId", user.Id));
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsers()
        {
            return this.UserCollection.AsQueryable().ToList();
        }

        /// <summary>
        /// Adds group to usermanagement
        /// </summary>
        /// <param name="group">Group to be added</param>
        public override void AddGroupToDb(Group group)
        {
            group.Id = this.GetNextGroupId();

            this.GroupCollection.Insert(group);
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Group> GetAllGroups()
        {
            return this.GroupCollection.AsQueryable().ToList();
        }

        /// <summary>
        /// Gets the group
        /// </summary>
        /// <param name="groupId">Id of the group</param>
        public override Group GetGroup(long groupId)
        {
            return this.GroupCollection.AsQueryable().FirstOrDefault(x => x.Id == groupId);
        }

        /// <summary>
        /// Gets the group
        /// </summary>
        /// <param name="groupName">NAme of the group</param>
        public override Group GetGroup(string groupName)
        {
            return this.GroupCollection.AsQueryable().FirstOrDefault(x => x.Name == groupName);
        }

        /// <summary>
        /// Removes a group from database
        /// </summary>
        /// <param name="group">Group to be removed</param>
        public void RemoveGroup(Group group)
        {
            this.GroupCollection.Remove(Query.EQ("Id", group.Id));

            this.MembershipCollection.Remove(Query.EQ("GroupId", group.Id));
        }

        /// <summary>
        /// Adds membership
        /// </summary>
        /// <param name="group">Group to be associated</param>
        /// <param name="user">User to be associated</param>
        public override void AddToGroup(Group group, User user)
        {
            this.RemoveFromGroup(group, user);

            var membership = new Membership()
            {
                GroupId = group.Id,
                UserId = user.Id
            };

            this.MembershipCollection.Insert(membership);
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
        /// Stores the user into database
        /// </summary>
        /// <param name="user">User to be updated</param>
        public override void UpdateUser(User user)
        {
            this.UserCollection.Save(user);
        }

        /// <summary>
        /// Removes membership
        /// </summary>
        /// <param name="group">Group of membership to be removed</param>
        /// <param name="user">User of membership to be removed</param>
        public void RemoveFromGroup(Group group, User user)
        {
            this.MembershipCollection.Remove(
                Query.And(Query.EQ("GroupId", group.Id), Query.EQ("UserId", user.Id)));
        }

        /// <summary>
        /// Checks if user is in group
        /// </summary>
        /// <param name="group">Group to be checked</param>
        /// <param name="user">User to be checked</param>
        public bool IsInGroup(Group group, User user)
        {
            return this.MembershipCollection.AsQueryable().Any(x => x.GroupId == group.Id && x.UserId == user.Id);
        }

        /// <summary>
        /// Gets all groups where user is member
        /// </summary>
        /// <param name="user">User to be checked</param>
        /// <returns>Enumeration of users</returns>
        public IEnumerable<Group> GetGroupsOfUser(User user)
        {
            return this.MembershipCollection.AsQueryable()
                .Where(x => x.UserId == user.Id)
                .Select(x => this.GroupCollection.AsQueryable().Where(y => y.Id == x.GroupId).FirstOrDefault())
                .ToList();
        }

        /// <summary>
        /// Checks, if username is existing
        /// </summary>
        /// <param name="username">Username to be checked</param>
        /// <returns>true, if username is existing</returns>
        public override bool IsUsernameExisting(string username)
        {
            return this.UserCollection.AsQueryable().Any(x => x.Username == username);
        }

        /// <summary>
        /// Checks, if group is existing
        /// </summary>
        /// <param name="groupName">Name of the group</param>
        /// <returns>True, if group is existing</returns>
        public override bool IsGroupExisting(string groupName)
        {
            return this.GroupCollection.AsQueryable().Any(x => x.Name == groupName);
        }

        /// <summary>
        /// Stores the value whether the class already had been initialized. 
        /// If not, MongoDB will get contact with our Entities
        /// </summary>
        private static bool alreadyInitialized = false;

        /// <summary>
        /// Starts the plugin
        /// </summary>
        public void Start()
        {
            // Initializes MongoDB
            if (!alreadyInitialized)
            {
                BsonClassMap.RegisterClassMap<UserDatabaseInfo>();
                BsonClassMap.RegisterClassMap<User>();
                BsonClassMap.RegisterClassMap<Group>();
                BsonClassMap.RegisterClassMap<Membership>();
                alreadyInitialized = true;
            }

            this.VerifyThatAdminIsExisting();
        }

        /// <summary>
        /// Performs a shutdown
        /// </summary>
        public void Shutdown()
        {
        }
        
        /// <summary>
        /// Gets the next user id
        /// </summary>
        /// <returns></returns>
        public long GetNextUserId()
        {
            lock (this.readWriteLock.GetWriteLock())
            {
                var collection = this.Db.Database.GetCollection<UserDatabaseInfo>("UserdatabaseInfo");
                var info = collection.AsQueryable().SingleOrDefault();

                if (info == null)
                {
                    info = new UserDatabaseInfo();
                    info.LastUserId = 1;
                }
                else
                {
                    info.LastUserId++;
                }

                collection.Save(info);

                return info.LastUserId;
            }
        }

        /// <summary>
        /// Gets a specific user by token id
        /// </summary>
        /// <param name="id">Id of the user</param>
        public User GetUserByToken(Guid id)
        {
            lock (this.readWriteLock.GetWriteLock())
            {
                return this.UserCollection.AsQueryable().Where(x => x.TokenId == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the next user id
        /// </summary>
        /// <returns></returns>
        public long GetNextGroupId()
        {
            lock (this.readWriteLock.GetWriteLock())
            {
                var collection = this.Db.Database.GetCollection<UserDatabaseInfo>("UserdatabaseInfo");
                var info = collection.AsQueryable().SingleOrDefault();

                if (info == null)
                {
                    info = new UserDatabaseInfo();
                    info.LastGroupId = 1;
                }
                else
                {
                    info.LastGroupId++;
                }

                collection.Save(info);

                return info.LastGroupId;
            }
        }

        public MongoCollection<User> UserCollection
        {
            get { return this.Db.Database.GetCollection<User>("Users"); }
        }

        public MongoCollection<Group> GroupCollection
        {
            get { return this.Db.Database.GetCollection<Group>("Groups"); }
        }

        public MongoCollection<Membership> MembershipCollection
        {
            get { return this.Db.Database.GetCollection<Membership>("Memberships"); }
        }
    }
}
