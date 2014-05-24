using DatenMeister.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Stores the extent data, which will be sent as json 
    /// </summary>
    public class JsonExtentData
    {
        public JsonExtentData()
        {
            this.objects = new List<object>();
            this.success = true;
        }

        public JsonExtentInfo extent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a list of objects that shall be sent to browser
        /// </summary>
        public List<object> objects
        {
            get;
            set;
        }

        public bool success
        {
            get;
            set;
        }
    }
}
