using BurnSystems.ObjectActivation;
using BurnSystems.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using BurnSystems.Test;

namespace BurnSystems.WebServer.Modules.Sessions
{
    /// <summary>
    /// This class is responsible to store and load the sessions
    /// </summary>
    public class SessionStorage : IDisposable
    {
        /// <summary>
        /// Defines a new logger
        /// </summary>
        private static ClassLogger logger = new ClassLogger(typeof(SessionStorage));

        /// <summary>
        /// Stores the configuration
        /// </summary>
        private SessionConfiguration configuration;

        /// <summary>
        /// Stores the session container as cached instance
        /// </summary>
        private SessionContainer container = null;

        /// <summary>
        /// Initializes a new instance of the SessionStorage class
        /// </summary>
        /// <param name="configuration"></param>
        [Inject]
        public SessionStorage(SessionConfiguration configuration)
        {
            Ensure.IsNotNull(configuration);
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets the session container
        /// </summary>
        public SessionContainer SessionContainer
        {
            get
            {
                lock (logger)
                {
                    if (this.container == null)
                    {
                        return this.Load();
                    }

                    return this.container;
                }
            }
        }

        /// <summary>
        /// Loads the container
        /// </summary>
        /// <returns>Session container to be loaded</returns>
        private SessionContainer Load()
        {
            try
            {
                if (File.Exists(this.configuration.StoragePath))
                {
                    using (var fileStream = new FileStream(this.configuration.StoragePath, FileMode.Open))
                    {
                        var formatter = new BinaryFormatter();
                        this.container = formatter.Deserialize(fileStream) as SessionContainer;
                    }

                    logger.LogEntry(new LogEntry(Localization_WebServer.SessionsLoading, LogLevel.Notify));
                }
            }
            catch (Exception exc)
            {
                logger.LogEntry(
                    LogEntry.Format(LogLevel.Fail, Localization_WebServer.SessionsLoadingException, exc.Message));
                throw new InvalidOperationException
                    ("Exception occured during loading, delete " + this.configuration.StoragePath + ", if this error occurs more often");
            }

            if (this.container == null)
            {
                this.container = new SessionContainer();
            }

            return this.container;
        }

        private void Store()
        {
            if (this.container == null)
            {
                // Nothing to do here
                return;
            }

            try
            {
                var directory = Path.GetDirectoryName(this.configuration.StoragePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var fileStream = new FileStream(this.configuration.StoragePath, FileMode.Create))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, this.container);
                }

                logger.LogEntry(new LogEntry(Localization_WebServer.SessionsStoring, LogLevel.Notify));
            }
            catch (Exception exc)
            {
                logger.LogEntry(
                    LogEntry.Format(LogLevel.Fail, Localization_WebServer.SessionsLoadingException, exc.Message));
            }
        }

        public void Dispose()
        {
            this.Store();
        }
    }
}

