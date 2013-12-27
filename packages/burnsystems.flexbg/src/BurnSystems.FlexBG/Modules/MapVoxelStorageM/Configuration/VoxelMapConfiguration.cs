using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.ConfigurationStorageM;
using System.Xml.Serialization;
using BurnSystems.ObjectActivation;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Configuration
{
    /// <summary>
    /// Gets the configuration
    /// </summary>
    public class VoxelMapConfiguration : IVoxelMapConfiguration
    {
        [Inject]
        public IConfigurationStorage Configuration
        {
            get;
            set;
        }

        private MapInfo mapInfo = null;

        /// <summary>
        /// Gets the Map info
        /// </summary>
        public MapInfo MapInfo
        {
            get
            {
                if (this.mapInfo == null)
                {
                    var xmlMap = this.Configuration.Documents
                        .Elements("FlexBG")
                        .Elements("Game")
                        .Elements("MapInfo")
                        .LastOrDefault();

                    if (xmlMap == null)
                    {
                        throw new InvalidOperationException("Configuration for FlexBG/Game/Map not found");
                    }

                    var serializer = new XmlSerializer(typeof(MapInfo));
                    this.mapInfo = serializer.Deserialize(xmlMap.CreateReader()) as MapInfo;
                }

                return this.mapInfo;
            }
        }
    }

}
