using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Views
{
    public class FieldInfo
    {
        public string name
        {
            get;
            set;
        }

        public string title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the read only status of the column
        /// </summary>
        public bool isReadonly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height in pixels
        /// </summary>
        public double height
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width in pixels
        /// </summary>
        public double width
        {
            get;
            set;
        }
    }
}
