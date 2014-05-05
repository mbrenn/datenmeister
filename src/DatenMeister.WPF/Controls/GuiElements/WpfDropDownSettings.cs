using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls.GuiElements
{
    /// <summary>
    /// Additional settings for the drop down can be added to the 
    /// </summary>
    public class WpfDropDownSettings
    {
        /// <summary>
        /// Gets or sets the information whether a button shall be added, which allows the user 
        /// to open a detail dialog
        /// </summary>
        public bool ShowDetailButton
        {
            get;
            set;
        }
    }
}
