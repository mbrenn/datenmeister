﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Web
{
    /// <summary>
    /// Stores the information about the column
    /// </summary>
    public class JsonExtentColumnInfo
    {
        /// <summary>
        /// Gets or sets the name of the column
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title of the column as shown in header
        /// </summary>
        public string title
        {
            get;
            set;
        }
    }
}
