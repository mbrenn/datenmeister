using BurnSystems.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Helper
{
    /// <summary>
    /// Helper to load and store from and into serialized file
    /// </summary>
    public static class SerializedFile
    {
        /// <summary>
        /// Stores the logger instance for this class
        /// </summary>
        private static ILog classLogger = new ClassLogger(typeof(SerializedFile));

        /// <summary>
        /// Stores the data directory
        /// </summary>
        private static string dataDirectory = "data";

        /// <summary>
        /// Loads database from file
        /// </summary>
        public static T LoadFromFile<T>(string filename, Func<T> createDefault) where T : class
        {
            var filePath = Path.Combine(dataDirectory, filename);

            if (!File.Exists(filePath))
            {
                classLogger.LogEntry(LogLevel.Message, "No file for " + filename);
                return createDefault();
            }

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
                    T result = formatter.Deserialize(stream) as T;

                    classLogger.LogEntry(LogLevel.Notify, "Serialized File: '" + filename + "' loaded");

                    return result;

                }
            }
            catch (Exception exc)
            {
                classLogger.LogEntry(LogLevel.Fatal, "Loading for " + filename + " failed: " + exc.Message);

                return createDefault();
            }
        }

        /// <summary>
        /// Stores database to file
        /// </summary>
        public static void StoreToFile<T>(string filename, T db)
        {
            var filePath = Path.Combine(dataDirectory, filename);
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, db);

                classLogger.LogEntry(LogLevel.Notify, "Serialized File: '" + filename + "' stored");
            }
        }

        /// <summary>
        /// Clears the complete data directory recursively
        /// </summary>
        public static void ClearCompleteDataDirectory()
        {
            var currentDirectory = dataDirectory;

            ClearDirectory(currentDirectory);
        }

        /// <summary>
        /// Clears the given directory
        /// </summary>
        /// <param name="directory">Directory to be cleared</param>
        private static void ClearDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            foreach (var subDirectory in Directory.GetDirectories(directory))
            {
                ClearDirectory(subDirectory);

                classLogger.LogEntry(LogEntry.Format(LogLevel.Message, "Removing directory: " + subDirectory));
                Directory.Delete(subDirectory);
            }

            foreach (var file in Directory.GetFiles(directory))
            {
                classLogger.LogEntry(LogEntry.Format(LogLevel.Message, "Removing file: " + file));
                File.Delete(file);
            }
        }
    }
}
