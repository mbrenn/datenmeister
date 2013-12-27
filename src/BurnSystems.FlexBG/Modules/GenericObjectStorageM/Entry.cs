using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.GenericObjectStorageM
{
    /// <summary>
    /// Stores the entries
    /// </summary>
    [Serializable]
    public class Entry
    {
        private string objectType;

        private string path;

        private object value;

        public Entry(Type objectType, string path, object value)
        {
            this.objectType = objectType.FullName;
            this.path = path;
            this.value = value;
        }

        /// <summary>
        /// Gets the object type
        /// </summary>
        public string ObjectType
        {
            get { return this.objectType; }
        }

        /// <summary>
        /// Gets the path
        /// </summary>
        public string Path
        {
            get { return this.path; }
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
