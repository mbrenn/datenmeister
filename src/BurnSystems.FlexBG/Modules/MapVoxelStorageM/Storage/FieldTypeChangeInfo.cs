using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Stores the information about the field type change. 
    /// float.MaxValue is in sky and negative values are below ground
    /// </summary>
    public struct FieldTypeChangeInfo
    {
        /// <summary>
        /// Stores the height where change occurs
        /// </summary>
        public float ChangeHeight; 

        /// <summary>
        /// Stores the type of the field type
        /// </summary>
        public byte FieldType;

        public void Init()
        {
            this.ChangeHeight = float.MaxValue;
            this.FieldType = 0;
        }

        /// <summary>
        /// Converts field type info to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Height: {0}, New Type: {1}", this.ChangeHeight, this.FieldType);
        }
    }
}
