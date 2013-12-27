using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.GenericObjectStorageM
{
    /// <summary>
    /// This object is being serialized into database
    /// </summary>
    [Serializable]
    public class Store
    {
        /// <summary>
        /// Stores the complete entries 
        /// </summary>
        private List<Entry> entries = new List<Entry>();

        /// <summary>
        /// Gets the entries
        /// </summary>
        public List<Entry> Entries
        {
            get { return this.entries; }
        }
    }
}
