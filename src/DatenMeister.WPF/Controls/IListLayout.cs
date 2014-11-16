using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Defines the list layout
    /// </summary>
    public interface IListLayout
    {
        /// <summary>
        /// Refreshes all the items in the list
        /// </summary>
        void RefreshItems();

        /// <summary>
        /// Gives the focus to inner elements. 
        /// This is required for datagrid
        /// </summary>
        void GiveFocusToGridContent();

        /// <summary>
        /// Gets or sets the action that has to be performed, when the user clicks on a specific element
        /// </summary>
        Action<DetailOpenEventArgs> OpenSelectedViewFunc
        {
            get;
            set;
        }
    }
}
