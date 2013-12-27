using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    public class VoxelMapDataItem
    {
        /// <summary>
        /// Stores the key being used to access the data
        /// </summary>
        private int key;

        /// <summary>
        /// Stores the data itself
        /// </summary>
        private byte[] data;

        /// <summary>
        /// Gets the key to identify the data tiem
        /// </summary>
        public int Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        /// <summary>
        /// Gets the data identifying the data item
        /// </summary>
        public byte[] Data
        {
            get{ return this.data; }
            set{ this.data = value; }
        }
    }
}
