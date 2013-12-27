using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.ConfigurationStorageM;
using System.Xml.Serialization;
using BurnSystems.Test;
using BurnSystems.ObjectActivation;
using System.Reflection;

namespace BurnSystems.FlexBG.Modules.ServerInfoM
{
    public class ServerInfoProvider : IServerInfoProvider
    {
        [Inject]
        public ServerInfoProvider(IConfigurationStorage storage)
        {
            if (storage == null)
            {
                this.ServerInfo = new ServerInfoM.ServerInfo();
            }
            else
            {
                Ensure.That(storage != null, "IConfigurationStorage not set in Constructor");

                var xmlInfo = storage.Documents
                    .Elements("FlexBG")
                    .Elements("Server")
                    .Elements("ServerInfo")
                    .LastOrDefault();

                Ensure.That(xmlInfo != null, "Xml-Configuration within Xml-node: 'FlexBG/Server/ServerInfo' not found");

                var serializer = new XmlSerializer(typeof(ServerInfo));
                this.ServerInfo = serializer.Deserialize(xmlInfo.CreateReader()) as ServerInfo;
            }
            
            this.ServerInfo.ServerVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.ServerInfo.ServerStartUp = DateTime.Now;
        }

        public ServerInfo ServerInfo
        {
            get;
            private set;
        }
    }
}
