using DatenMeister.WPF.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Stores the information about a created tab
    /// </summary>
    public class TabInformation
    {
        /// <summary>
        /// Gets or sets the name of the tab
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the instance of the windows control being used to host the list
        /// </summary>
        public TabItem TabItem
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the instance of the control, which show the table control
        /// </summary>
        public EntityTableControl TableControl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the table view information
        /// </summary>
        public IObject TableViewInfo
        {
            get;
            set;
        }
    }
}
