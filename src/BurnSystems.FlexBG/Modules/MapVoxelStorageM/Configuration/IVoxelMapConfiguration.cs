using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Configuration
{
    public interface IVoxelMapConfiguration
    {
        /// <summary>
        /// Gets the map information
        /// </summary>
        MapInfo MapInfo
        {
            get;
        }
    }
}
