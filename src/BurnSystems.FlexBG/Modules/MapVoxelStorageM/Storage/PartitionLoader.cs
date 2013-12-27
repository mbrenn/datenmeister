using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System.IO;
using System.Xml.Serialization;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// This is the database for reading and writing of partitions.
    /// The methods within this database are not thread-safe
    /// </summary>
    public class PartitionLoader : IPartitionLoader
    {
        /// <summary>
        /// Defines the logger for the partition
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(PartitionLoader));

        /// <summary>
        /// Gets the path to database
        /// </summary>
        public string DatabasePath
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the PartitionLoader class
        /// </summary>
        /// <param name="info">Voxelmap info</param>
        [Inject]
        public PartitionLoader()
            : this("data/map/")
        {
        }

        /// <summary>
        /// Initializes a new instance of the PartitionLoader class
        /// </summary>
        /// <param name="info">Voxelmap info</param>
        /// <param name="databasePath">Path of database</param>
        public PartitionLoader(string databasePath)
        {
            this.DatabasePath = databasePath;

            if (!Directory.Exists(databasePath))
            {
                Directory.CreateDirectory(databasePath);
            }
        }

        /// <summary>
        /// Removes all database files
        /// </summary>
        public void Clear()
        {
            foreach (var file in Directory.GetFiles(this.DatabasePath, "mapinfo-*.xml"))
            {
                File.Delete(Path.Combine(this.DatabasePath, file));
            }

            foreach (var file in Directory.GetFiles(this.DatabasePath, "*.vox"))
            {
                File.Delete(Path.Combine(this.DatabasePath, file));
            }
        }

        /// <summary>
        /// Gets the path and filename for a certain partition
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>Path to partition</returns>
        public string GetPathForPartition(long instanceId, int x, int y)
        {
            return Path.Combine(
                this.DatabasePath,
                string.Format("{2}-{0}-{1}.vox", x, y, instanceId));
        }

        /// <summary>
        /// Gets the path and filename for a certain partition
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <returns>Path to partition</returns>
        public string GetPathForPartition(Partition partition)
        {
            return this.GetPathForPartition(partition.InstanceId, partition.PartitionX, partition.PartitionY);
        }

        /// <summary>
        /// Gets the path for info file
        /// </summary>
        /// <param name="instanceId">Id of the instance</param>
        /// <returns>Path for file</returns>
        private string GetPathForInfoFile(long instanceId)
        {
            return Path.Combine(this.DatabasePath, "mapinfo-" + instanceId + ".xml");
        }

        /// <summary>
        /// Stores the info data
        /// </summary>
        public void StoreInfoData(long instanceId, VoxelMapInfo info)
        {
            var serializer = new XmlSerializer(typeof(VoxelMapInfo));

            using (var fileStream = new FileStream(this.GetPathForInfoFile(instanceId), FileMode.Create))
            {
                serializer.Serialize(fileStream, info);
            }
        }

        /// <summary>
        /// Loads the info data from generic file
        /// </summary>
        /// <returns></returns>
        public VoxelMapInfo LoadInfoData(long instanceId)
        {
            var path = this.GetPathForInfoFile(instanceId);
            if (File.Exists(path))
            {
                var result = LoadInfoData(path);
                result.InstanceId = instanceId;     // For moved or old mapinfos, which have not set the InstanceId
                return result;
            }

            return null;
        }

        /// <summary>
        /// Loads the info data from generic file
        /// </summary>
        /// <param name="filePath">Path, where mapinfo.xml is stored</param>
        /// <returns>Found voxel map</returns>
        private static VoxelMapInfo LoadInfoData(string filePath)
        {
            var serializer = new XmlSerializer(typeof(VoxelMapInfo));

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                return serializer.Deserialize(fileStream) as VoxelMapInfo;
            }
        }

        /// <summary>
        /// Loads a partition from drive
        /// </summary>
        /// <param name="x">X-Coordinate of the partition</param>
        /// <param name="y">Y-Coordinate of the partition</param>
        /// <returns>Loaded partition</returns>
        public Partition LoadPartition(long instanceId, int x, int y)
        {
            var info = this.LoadInfoData(instanceId);
            Ensure.That(info != null, "info == null, Map-Info not stored?");
            Ensure.That(
                x >= 0 && x < info.SizeX / info.PartitionLength &&
                y >= 0 && y < info.SizeY / info.PartitionLength);

            var partition = new Partition(instanceId, x, y, info.PartitionLength);

            var filePath = this.GetPathForPartition(instanceId, x, y);
            if (File.Exists(filePath))
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    partition.Load(fileStream);
                } 
                
                logger.LogEntry(LogLevel.Verbose, "Loaded Partition: " + x + ", " + y);
            }
            else
            {
                // Default
                partition.InitFields();
                logger.LogEntry(LogLevel.Verbose, "Initialized Partition: " + x + ", " + y);
            }

            return partition;
        }

        /// <summary>
        /// Stores a certain partition on drive
        /// </summary>
        /// <param name="partition">Partition to be stored</param>
        public void StorePartition(Partition partition)
        {
            var filePath = this.GetPathForPartition(partition);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                partition.Store(fileStream);

                logger.LogEntry(LogLevel.Verbose, "Stored Partition: " + partition.ToString());
            }
        }
    }
}
