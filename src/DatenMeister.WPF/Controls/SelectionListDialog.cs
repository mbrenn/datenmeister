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
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the ListDialog class
        /// </summary>
        /// <param name="elements">Elements to be shown</param>
        /// <param name="settings">Settings for the list</param>
        /// <param name="tableView">View being used for the objects</param>
        public SelectionListDialog(
            IReflectiveCollection elements,
            IPublicDatenMeisterSettings settings,
            IObject tableView)
            : base ( elements, settings, tableView)
        {
            this.Init();
        }

        /// <summary>
        /// Initializes the instance
        /// </summary>
        private void Init()
        {
            this.ViewInformation.setAllowDelete(false);
            this.ViewInformation.setAllowEdit(false);
            this.ViewInformation.setAllowNew(false);
            this.Table.UseAsSelectionControl = true;
        }
    }
}
