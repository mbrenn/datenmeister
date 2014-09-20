using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Opens the list dialog, where the user can select one or more items
    /// </summary>
    public class SelectionListDialog : ListDialog
    {
        /// <summary>
        /// Initializes a new instance of the SelectionListDialog
        /// </summary>
        public SelectionListDialog()
            : base()
        {
            this.ViewInformation.setAllowDelete(false);
            this.ViewInformation.setAllowEdit(false);
            this.ViewInformation.setAllowNew(false);
            this.Table.UseAsSelectionControl = true;
        }
    }
}
