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
            : base(name, binding)
        {
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

        /// <summary>
        /// Gets or sets the information whether the content shall be seen as datetime
        /// and will be parsed to local CultureInfo
        /// </summary>
        public bool isDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the information whether the content is a hyperlink
        /// which will be opened, when the user clicks on it
        /// </summary>
        public bool isHyperlink
        {
            get;
            set;
        }
    }
}
