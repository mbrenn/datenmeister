using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Configuration
{
    /// <summary>
    /// Defines the map info
    /// </summary>
    public class MapInfo
    {
        /// <summary>
        /// Gets the list of textures
        /// </summary>
        public List<Texture> Textures
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the texture
        /// </summary>
        public class Texture
        {
            [XmlAttribute]
            public byte FieldType
            {
                get;
                set;
            }

            [XmlAttribute]
            public string TexturePath
            {
                get;
                set;
            }

            public override string ToString()
            {
                return this.FieldType + " " + this.TexturePath;
            }
        }
    }
}
