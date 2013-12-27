using BurnSystems.FlexBG.Interfaces;
using BurnSystems.FlexBG.Modules.ConfigurationStorageM;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Configuration;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.ObjectActivation;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM
{
    /// <summary>
    /// Loads the module
    /// </summary>
    public class Module 
    {
        /// <summary>
        /// Binds the voxelmap
        /// </summary>
        /// <param name="container">Container to be used</param>
        public static void Load(ActivationContainer container)
        {
            var loader = new PartitionLoader();
            var cache = new PartitionCache(loader);
            container.Bind<IPartitionLoader>().ToConstant(cache);
            container.Bind<IVoxelMapConfiguration>().To<VoxelMapConfiguration>();

            container.Bind<IVoxelMap>().To<VoxelMap>();
        }
    }
}
