using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Generator
{
    /// <summary>
    /// Adds a noise layer
    /// </summary>
    public class AddNoiseLayer
    {
        /// <summary>
        /// Gets the voxelmap
        /// </summary>
        public IVoxelMap VoxelMap
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the fieldtype to be set
        /// </summary>
        public byte FieldType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the noise generator
        /// </summary>
        public Func<float> NoiseGeneratorTop
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the noise generator
        /// </summary>
        public Func<float> NoiseGeneratorBottom
        {
            get;
            private set;
        }

        public AddNoiseLayer(IVoxelMap voxelMap, byte fieldType, Func<float> noiseGeneratorTop, Func<float> noiseGeneratorBottom)
        {
            this.VoxelMap = voxelMap;
            this.FieldType = fieldType;
            this.NoiseGeneratorTop = noiseGeneratorTop;
            this.NoiseGeneratorBottom = noiseGeneratorBottom;
        }

        public void Execute(long instanceId)
        {
            var dx = this.VoxelMap.GetInfo(instanceId).SizeX;
            var dy = this.VoxelMap.GetInfo(instanceId).SizeY;

            for (var x = 0; x < dx; x++)
            {
                for (var y = 0; y < dy; y++)
                {
                    var top = this.NoiseGeneratorTop();
                    var bottom = this.NoiseGeneratorBottom();

                    this.VoxelMap.SetFieldType(instanceId, x, y, this.FieldType, top, bottom);
                }
            }
        }
    }
}
