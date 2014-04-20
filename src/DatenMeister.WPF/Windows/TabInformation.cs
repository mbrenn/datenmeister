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
        public string Name
        {
            get;
            set;
        }

        public TabItem TabItem
        {
            get;
            set;
        }

        public EntityTableControl TableControl
        {
            get;
            set;
        }

        public AddExtentParameters Parameters
        {
            get;
            set;
        }
    }
}
