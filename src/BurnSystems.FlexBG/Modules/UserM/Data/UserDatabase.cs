using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.UserQueryM;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Synchronisation;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BurnSystems.FlexBG.Modules.UserM.Data
{
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class UserDatabase : IFlexBgRuntimeModule
    {
        /// <summary>
        /// Gets the database storing the users. 
        /// </summary>
        private UserDatabaseLocal data = new UserDatabaseLocal();

        /// <summary>
        /// Stores the sync object
        /// </summary>
        private ReadWriteLock sync = new ReadWriteLock();

        /// <summary>
        /// Gets the users
        /// </summary>
        public UserDatabaseLocal Data 
        {
            get { return this.data; }
        }

        /// <summary>
        /// Gets the synchronization object
        /// </summary>
        protected ReadWriteLock Sync
        {
            get { return this.sync; }
        }

        [Inject]
        public IUserQuery UserQuery
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the logger instance for this class
        /// </summary>
        private static ILog classLogger = new ClassLogger(typeof(UserDatabase));

        /// <summary>
        /// Stores the filepath to user data
        /// </summary>
        private string filePath = "data/users.db";

        /// <summary>
        /// Loads database from file
        /// </summary>
        private void LoadFromFile()
        {
            if (!File.Exists(this.filePath))
            {
                classLogger.LogEntry(LogLevel.Message, "No file for UserManagementLocal existing, creating empty database");
                return;
            }

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    this.data = formatter.Deserialize(stream) as UserDatabaseLocal;
                }
            }
            catch (Exception exc)
            {
                classLogger.LogEntry(LogLevel.Fatal, "Loading for userdatabase failed: " + exc.Message);

                if (this.UserQuery == null || this.UserQuery.Ask(
                        "Shall a new database be created?",
                        new[] { "y", "n" },
                        "n") == "n")
                {
                    throw;
                }
                else
                {
                    classLogger.LogEntry(LogLevel.Message, "New database will be created");
                }
            }
        }

        /// <summary>
        /// Stores database to file
        /// </summary>
        private void StoreToFile()
        {
            if (!Directory.Exists("data"))
            {
                Directory.CreateDirectory("data");
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this.data);
            }
        }

        /// <summary>
        /// Starts the usermanagement
        /// </summary>
        public void Start()
        {
            this.LoadFromFile();
        }

        /// <summary>
        /// Stops the usermanagement
        /// </summary>
        public void Shutdown()
        {
            this.StoreToFile();
        }
    }
}
