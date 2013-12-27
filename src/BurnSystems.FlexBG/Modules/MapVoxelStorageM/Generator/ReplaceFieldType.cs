using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Generator
{
    /// <summary>
    /// Replaces a fieldtype
    /// </summary>
    public class ReplaceFieldType
    {
        /// <summary>
        /// Gets or sets the fieldtype which shall be replaced
        /// </summary>
        public byte OldFieldType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the fieldtype being replaced
        /// </summary>
        public byte NewFieldType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the voxelmap
        /// </summary>
        public IVoxelMap VoxelMap
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the filter which decides whether the fieldtypes on the specific field 
        /// shall be replaced
        /// </summary>
        public Func<int, int, bool> Filter
        {
            get;
            set;
        }

        public ReplaceFieldType(IVoxelMap voxelMap, byte oldFieldType, byte newFieldType, Func<int, int, bool> filter = null)
        {
            this.OldFieldType = oldFieldType;
            this.NewFieldType = newFieldType;
            this.VoxelMap = voxelMap;
            this.Filter = filter;
        }

        /// <summary>
        /// Performs the replacement
        /// </summary>
        public void Execute(long instanceId)
        {
            var dx = this.VoxelMap.GetInfo(instanceId).SizeX;
            var dy = this.VoxelMap.GetInfo(instanceId).SizeY;

            for (var x = 0; x < dx; x++)
            {
                for (var y = 0; y < dy; y++)
                {
                    if (this.Filter != null && !this.Filter(x, y))
                    {
                        continue;
                    }

                    var column = this.VoxelMap.GetColumn(instanceId, x, y);

                    var modified = false;
                    for (var n = 0; n < column.Changes.Count; n++)
                    {
                        if (column.Changes[n].FieldType == this.OldFieldType)
                        {
                            var change = column.Changes[n];
                            change.FieldType = this.NewFieldType;
                            column.Changes[n] = change;
                            modified = true;
                        }
                    }

                    if (modified)
                    {
                        this.VoxelMap.SetColumn(instanceId, x, y, column);
                    }
                }
            }
        }

        public static void Execute(IVoxelMap voxelMap, long instanceId, byte oldFieldType, byte newFieldType)
        {
            var replace = new ReplaceFieldType(voxelMap, oldFieldType, newFieldType);
            replace.Execute(instanceId);
        }
    }
}
