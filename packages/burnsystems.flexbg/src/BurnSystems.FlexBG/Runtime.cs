using System;
using System.IO;
using System.Xml.Linq;
using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.ConfigurationStorageM;
using BurnSystems.Logging;
using BurnSystems.Test;
using BurnSystems.ObjectActivation;

namespace BurnSystems.FlexBG
{
    /// <summary>
    /// Defines the location, where the FlexBG-Core is running
    /// </summary>
    public enum RuntimeLocation
    {
        Frontend,
        Backend
    }

    /// <summary>
    /// Offers framework for using the FlexBGCore. 
    /// The runtime eases starting and stopping of FlexBG
    /// </summary>
    public class Runtime
    {
        /// <summary>
        /// Stores the activation container being used by FlexBG
        /// </summary>
        private IActivates activationContainer;

        /// <summary>
        /// Stores the path to the configuration folder
        /// </summary>
        public string ConfigurationFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the logger
        /// </summary>
        private ILog logger = new ClassLogger(typeof(Runtime));

        /// <summary>
        /// Initializes a new instance of the Runtime class
        /// </summary>
        /// <param name="activationContainer">Activation container to be used</param>
        /// <param name="location">Location, where runtime is started</param>
        public Runtime(IActivates activationContainer, string configurationFolder)
        {
            this.activationContainer = activationContainer;
            this.ConfigurationFolder = configurationFolder;

            Ensure.That(Directory.Exists(configurationFolder), "Configuration-Path not found: " + configurationFolder);
        }

        /// <summary>
        /// Performs the startup of the core. 
        /// </summary>
        public void StartUpCore()
        {
            logger.LogEntry(LogLevel.Message, "Starting BurnSystems.FlexBG");

            // Looks for configuration. 
            // First check for './local', afterwards for './generic'
            var effectivePath = Path.Combine(this.ConfigurationFolder, "local");
            if (!Directory.Exists(effectivePath))
            {
                effectivePath = Path.Combine(this.ConfigurationFolder, "generic");
                Ensure.That(Directory.Exists(effectivePath), "Configuration-Path not found: " + effectivePath);
            }

            // Binds Configurationstorage
            this.LoadConfiguration(effectivePath);

            this.ActivatePlugins();
        }

        /// <summary>
        /// Loads the configuration from the effective path
        /// </summary>
        /// <param name="effectivePath">Path, where configuration is stored. </param>
        private void LoadConfiguration(string effectivePath)
        {
            logger.LogEntry(LogLevel.Notify, "Reading Configuration from: " + effectivePath);

            // Gets all Xml-Documents
            var files = Directory.GetFiles(effectivePath, "*.xml");
            var configurationStorage = this.activationContainer.Get<IConfigurationStorage>();
            Ensure.That(configurationStorage != null, "No IConfigurationStorage Interface in FlexBG-Runtime");

            foreach (var file in files)
            {
                logger.LogEntry(LogLevel.Notify, "Reading Configuration file: " + Path.GetFileName(file));
                var document = XDocument.Load(file);
                configurationStorage.Add(document);
            }
        }

        /// <summary>
        /// Activate all plugins containing implementing the IFlexBgRuntimeModule
        /// </summary>
        private void ActivatePlugins()
        {
            logger.LogEntry(LogLevel.Notify, "Starting Modules");

            foreach (var runtimeModule in this.activationContainer.GetAll<IFlexBgRuntimeModule>())
            {
                logger.LogEntry(LogLevel.Notify, "Starting Module: " + runtimeModule.ToString());
                runtimeModule.Start();
            }
        }

        /// <summary>
        /// Performs the shutting down
        /// </summary>
        public void ShutdownCore()
        {
            logger.LogEntry(LogLevel.Message, "Shutdown of BurnSystems.FlexBG");

            foreach (var runtimeModule in this.activationContainer.GetAll<IFlexBgRuntimeModule>())
            {
                logger.LogEntry(LogLevel.Notify, "Shutdown of Module: " + runtimeModule.ToString());
                runtimeModule.Shutdown();
            }
        }
    }
}