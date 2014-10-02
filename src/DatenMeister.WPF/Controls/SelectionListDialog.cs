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
        }

        /// <summary>
        /// Initializes the selection List dialog
        /// </summary>
        /// <param name="configuration">The configuration to be used</param>
        public SelectionListDialog(TableLayoutConfiguration configuration)
        {
        }

        /// <summary>
        /// Configures the dialog by using the Table Layout Configuration
        /// </summary>
        /// <param name="configuration">Configuration to be used for layouting</param>
        public override void Configure(TableLayoutConfiguration configuration)
        {
            var tableViewInfo = configuration.TableViewInfoAsTableView;
            tableViewInfo.setAllowDelete(false);
            tableViewInfo.setAllowEdit(false);
            tableViewInfo.setAllowNew(false);
            base.Configure(configuration);
        }
    }
}
