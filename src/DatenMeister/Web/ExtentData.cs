using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Web
{
    /// <summary>
    /// Stores the extent data, which will be sent as json 
    /// </summary>
    public class ExtentData
    {
        public ExtentData()
        {
            this.columns = new List<ExtentColumnInfo>();
            this.objects = new List<Dictionary<string, string>>();
            this.success = true;
        }

        /// <summary>
        /// Gets or sets a list of column
        /// </summary>
        public List<ExtentColumnInfo> columns
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a list of objects that shall be sent to browser
        /// </summary>
        public List<Dictionary<string, string>> objects
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
