using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    /// <summary>
    /// Stores the general properties for all field informations
    /// </summary>
    public class General
    {
        public General()
        {
        }

        public General(string name, string binding)
        {
            this.name = name;
            this.binding = binding;
        }

        public string name
        {
            get;
            set;
        }

        public string binding
        {
            get;
            set;
        }

        public bool isReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the column width being used for 
        /// the list views
        /// </summary>
        public int columnWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the column. This value is only valid for form views
        /// 0 means an automatic column.
        /// -1 means maximum height as appropriate
        /// </summary>
        public int height
        {
            get;
            set;
        }
    }

}
