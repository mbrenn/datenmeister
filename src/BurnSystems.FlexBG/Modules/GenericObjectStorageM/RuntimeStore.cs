using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.GenericObjectStorageM
{
    /// <summary>
    /// Store being used during runtime for faster lookup. 
    /// Will not be persisted to database. This class is not tread-safe
    /// </summary>
    internal class RuntimeStore
    {
        /// <summary>
        /// Stores the entries
        /// </summary>
        private Dictionary<string, List<Entry>> typedEntries = new Dictionary<string, List<Entry>>();

        /// <summary>
        /// Gets the typed entries
        /// </summary>
        public Dictionary<string, List<Entry>> TypedEntries
        {
            get { return this.typedEntries; }
        }

        /// <summary>
        /// Adds an item 
        /// </summary>
        /// <param name="entry">Entry to be added</param>
        public void Add(Entry entry)
        {
            var list = this.GetList(entry.ObjectType);

            if (!list.Any(x => x == entry))
            {
                list.Add(entry);
            }
        }

        public void Remove(Entry entry) 
        {
            var list = this.GetList(entry.ObjectType);
            list.Remove(entry);
        }

        public Entry Get(string typeName, string path) 
        {
            var list = this.GetList(typeName);
            return list.Where(x => x.Path == path).FirstOrDefault();
        }

        public List<Entry> GetList(string typeName) 
        {
            List<Entry> list;
            if (!this.typedEntries.TryGetValue(typeName, out list))
            {
                list = new List<Entry>();
                this.typedEntries[typeName] = list;
            }
            return list;
        }
    }
}
