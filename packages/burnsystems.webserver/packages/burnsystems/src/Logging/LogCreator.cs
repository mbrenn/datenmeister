//-----------------------------------------------------------------------
// <copyright file="LogCreator.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Logging
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Dies ist eine Klasse mit dessen Hilfe 
    /// ein Log-Objekt über einen Xml-Knoten erzeugt werden kann. 
    /// </summary>
    public static class LogCreator
    {
        /// <summary>
        /// Erzeugt ein neues Log
        /// </summary>
        /// <param name="xmlNode">Xml-Knoten mit den Informationen</param>
        /// <returns>Ein neuerstelltes Log</returns>
        public static Log CreateLog(XmlNode xmlNode)
        {
            var log = new Log();

            // Setzt das Filterlevel
            if (xmlNode.Attributes["filter"] != null)
            {
                string strFilterlevel
                    = xmlNode.Attributes["filter"].InnerText;
                try
                {
                    LogLevel logLevel = (LogLevel)
                        Enum.Parse(typeof(LogLevel), strFilterlevel, true);
                    log.FilterLevel = logLevel;
                }
                catch (FormatException)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            CultureInfo.CurrentUICulture,
                            LocalizationBS.Log_UnknownFilter, 
                            strFilterlevel));
                }
            } 
            
            AddLogProviders(log, xmlNode);

            return log;
        }

        /// <summary>
        /// Adds logproviders to a log by xml-Configuration
        /// </summary>
        /// <param name="log">Log, der erweitert werden soll.</param>
        /// <param name="xmlNode">Xmlnode, storing the configuration of
        /// logproviders</param>
        public static void AddLogProviders(Log log, XmlNode xmlNode)
        {
            // Erzeugt die Logprovider
            foreach (XmlNode xmlProvider in xmlNode.SelectNodes("./logprovider"))
            {
                string type = xmlProvider.Attributes["type"].InnerText;

                switch (type)
                {
                    case "console":
                        log.AddLogProvider(new ConsoleProvider());
                        break;
                    case "file":
                        string strPath =
                            xmlProvider.Attributes["path"].InnerText;
                        log.AddLogProvider(new FileProvider(strPath));
                        break;
                }
            }
        }

        /// <summary>
        /// Adds logproviders to a log by xml-Configuration
        /// </summary>
        /// <param name="log">Log, der erweitert werden soll.</param>
        /// <param name="xmlNode">Xmlnode, storing the configuration of
        /// logproviders</param>
        public static void AddLogProviders(Log log, XContainer xmlNode)
        {
            // Erzeugt die Logprovider
            foreach (var xmlProvider in xmlNode.Elements("logprovider"))
            {
                string type = xmlProvider.Attribute("type").Value;

                switch (type)
                {
                    case "console":
                        log.AddLogProvider(new ConsoleProvider());
                        break;
                    case "file":
                        string strPath =
                            xmlProvider.Attribute("path").Value;
                        log.AddLogProvider(new FileProvider(strPath));
                        break;
                }
            }
        }
    }
}
