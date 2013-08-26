using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Web
{
    public class JsonExtentObject
    {
        public JsonExtentObject()
        {
            this.values = new Dictionary<string, string>();            
        }

        public JsonExtentObject(string id, Dictionary<string, string> values)
        {
            this.id = id;
            this.values = values;
        }

        /// <summary>
        /// Gets or sets the id of the value
        /// </summary>
        public string id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dictionary of values
        /// </summary>
        public Dictionary<string, string> values
        {
            get;
            set;
        }
    }
}
