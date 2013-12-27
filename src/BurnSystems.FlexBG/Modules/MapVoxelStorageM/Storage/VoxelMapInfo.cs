using BurnSystems.FlexBG.Modules.ConfigurationStorageM;
using BurnSystems.Test;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Stores the configuration of the created voxel map
    /// </summary>
    public class VoxelMapInfo
    {
        /// <summary>
        /// Gets or sets the instance id
        /// </summary>
        public long InstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// Size of the voxel map in X
        /// </summary>
        public long SizeX
        {
            get;
            set;
        }

        /// <summary>
        /// Size of the voxel map in Y
        /// </summary>
        public long SizeY
        {
            get;
            set;
        }

        /// <summary>
        /// Gets width or length of the partition. 
        /// This is the length of one partition border.
        /// Putting in 200 means that a partiation has 200x200 fields
        /// </summary>
        public int PartitionLength
        {
            get;
            set;
        }

        /// <summary>
        /// Configurates the voxel map
        /// </summary>
        /// <param name="storage">Storage, where configuration is retrieved</param>
        /// <returns>The created voxel map information</returns>
        public static VoxelMapInfo Configurate(IConfigurationStorage storage)
        {
            var xmlVoxelMapInfo = storage.Documents
                .Elements("FlexBG")
                .Elements("Game")
                .Elements("VoxelMap")
                .Elements("VoxelMapInfo")
                .LastOrDefault();

            Ensure.That(xmlVoxelMapInfo != null, "No configuration found in 'FlexBG/Game/Voxel/VoxelMapInfo'");

            XmlSerializer serializer = new XmlSerializer(typeof(VoxelMapInfo));
            return (VoxelMapInfo) serializer.Deserialize(xmlVoxelMapInfo.CreateReader());
        }

        public override string ToString()
        {
            return string.Format(
                "VoxelMap: {0}, {1}", 
                this.SizeX, 
                this.SizeY);
        }
    }
}
