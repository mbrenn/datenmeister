using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class TextField : General
    {
        public TextField()
        {
        }

        public TextField(string name, string binding)
        {
            this.binding = binding;
            this.name = name;
        }

        public int width
        {
            get;
            set;
        }

        public int height
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value whether the text box shall be shown as a multiline item. 
        /// </summary>
        public bool isMultiline
        {
            get;
            set;
        }
    }
}
