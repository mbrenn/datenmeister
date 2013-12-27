using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Defines one column for the voxelmap
    /// </summary>
    public class VoxelMapColumn
    {
        /// <summary>
        /// Stores the data of the items
        /// </summary>
        private List<VoxelMapDataItem> data = new List<VoxelMapDataItem>();

        /// <summary>
        /// Stores the changes
        /// </summary>
        private List<FieldTypeChangeInfo> changes = new List<FieldTypeChangeInfo>();

        /// <summary>
        /// Gets the list of data items
        /// </summary>
        public IList<VoxelMapDataItem> Data
        {
            get {
                lock (this.data)
                {
                    return this.data.ToList();
                }
            }
        }

        /// <summary>
        /// Gets the field type changes
        /// </summary>
        public List<FieldTypeChangeInfo> Changes
        {
            get { return this.changes; }
        }

        /// <summary>
        /// Sets the free data within the column
        /// </summary>
        /// <param name="key">Key of the dataitem</param>
        /// <param name="data">Data to assigned to the key</param>
        public void Set(int key, byte[] data)
        {
            lock (this.data)
            {
                var found = this.data.Where (x=> x.Key == key ).SingleOrDefault();
                if (found == null)
                {
                    found = new VoxelMapDataItem();
                    this.data.Add(found);
                    found.Key = key;
                }

                found.Data = data;
            }
        }

        /// <summary>
        /// Gets the data by key
        /// </summary>
        /// <param name="key">Key to be used</param>
        /// <returns>Retrieved data or null, if not found</returns>
        public byte[] Get(int key)
        {
            lock (this.data)
            {
                var found = this.data.Where(x => x.Key == key).SingleOrDefault();
                if (found == null)
                {
                    return null;
                }

                return found.Data;
            }
        }
    }
}
