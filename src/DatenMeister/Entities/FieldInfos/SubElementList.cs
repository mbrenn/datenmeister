using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.FieldInfos
{
    /// <summary>
    /// Shows a list of subelements
    /// </summary>
    public class SubElementList : General
    {
        /// <summary>
        /// Gets or sets the type, that shall be created for new objects
        /// </summary>
        public IObject typeForNew
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the table view for the subitems.
        /// They are not a simple list
        /// </summary>
        public IObject listTableView       
        {
            get;
            set;
        }
    }
}
