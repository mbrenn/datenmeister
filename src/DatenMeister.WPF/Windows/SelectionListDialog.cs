using DatenMeister.DataProvider.DotNet;
using DatenMeister.WPF.Controls;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Windows
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
            this.Configure(configuration);
        }

        /// <summary>
        /// Configures the dialog by using the Table Layout Configuration
        /// </summary>
        /// <param name="configuration">Configuration to be used for layouting</param>
        public override void Configure(TableLayoutConfiguration configuration)
        {
            var tableViewInfo = configuration.GetTableViewInfoAsTableView();
            
            if (tableViewInfo == null)
            {
                // Ok, tableview info is null... we need to create a generic one
                var tableView = new DatenMeister.Entities.FieldInfos.TableView();
                var tableViewObj = Injection.Application.Get<GlobalDotNetExtent>().CreateObject(tableView);
                tableViewInfo = new Entities.AsObject.FieldInfo.TableView(tableViewObj);
                tableViewInfo.setDoAutoGenerateByProperties(true);
                configuration.TableViewInfo = tableViewInfo;
            }

            configuration.UseAsSelectionControl = true;
            base.Configure(configuration);
        }
    }
}
