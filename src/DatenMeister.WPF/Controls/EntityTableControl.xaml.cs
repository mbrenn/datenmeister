using BurnSystems.Test;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Interaktionslogik für ObjectTable.xaml
    /// </summary>
    public partial class EntityTableControl : UserControl
    {
        /// <summary>
        /// Stores the information of the table view.
        /// It defines how the table should look like
        /// </summary>
        private TableView tableViewInfo;

        /// <summary>
        /// Stores the extent factory to retrieve the extent
        /// </summary>
        private Func<IURIExtent, IURIExtent> extentFactory;

        public IDatenMeisterWindow MainWindow
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the extent that shall be shown
        /// </summary>
        public IURIExtent Extent
        {
            get
            {
                if (this.MainWindow != null && this.MainWindow.Settings.ProjectExtent != null)
                {
                    return this.extentFactory(this.MainWindow.Settings.ProjectExtent);
                }

                else
                {
                    return null;
                }
            }
            set
            {
                this.extentFactory = (x) => value;
                if (this.extentFactory != null)
                {
                    this.RefreshItems();
                }
            }
        }

        /// <summary>
        /// Gets or sets the factory being used to retrieve the extent
        /// </summary>
        public Func<IURIExtent, IURIExtent> ExtentFactory
        {
            get { return this.extentFactory; }
            set
            {
                this.extentFactory = value;
                // Refreshes the items when we get a new extent factory
                this.RefreshItems();
            }
        }

        /// <summary>
        /// Gets or sets the type that shall be created, when user clicks on 'new'.
        /// </summary>
        public IObject MainType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the table view info
        /// </summary>
        public IObject TableViewInfo
        {
            get { return this.tableViewInfo.Value; }
            set
            {
                if (value == null)
                {
                    this.tableViewInfo = null;
                }
                else
                {
                    this.tableViewInfo = new TableView(value);
                    this.Relayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the view information, that will be used for the detail forms
        /// </summary>
        public IObject DetailViewInfo
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the element factory being used to create the element, 
        /// if we are in EditMode = New
        /// </summary>
        public Func<IObject> ElementFactory
        {
            get;
            set;
        }

        public EntityTableControl()
        {
            InitializeComponent();
        }

        public EntityTableControl(IObject tableView)
            : this()
        {
            this.tableViewInfo = new TableView(tableViewInfo);
            this.Relayout();
        }

        /// <summary>
        /// Does the relayout
        /// </summary>
        private void Relayout()
        {
            if (this.tableViewInfo == null)
            {
                // Nothing to do, should not happen
                return;
            }

            // Checks status of buttons
            this.buttonNew.Visibility = this.tableViewInfo.getAllowNew() ?
                System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            this.buttonEdit.Visibility = this.tableViewInfo.getAllowEdit() ?
                System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            this.buttonDelete.Visibility = this.tableViewInfo.getAllowDelete() ?
                System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            foreach (var fieldInfo in this.tableViewInfo.getFieldInfos().Cast<IObject>())
            {
                var title = fieldInfo.get("title").ToString();
                var name = fieldInfo.get("name").ToString();
                var column = new TableDataGridTextColumn();
                column.Header = title;
                column.Binding = new Binding("["+name+"]");
                column.AssociatedViewColumn = fieldInfo;

                this.gridContent.Columns.Add(column);
            }

            // Gets the elements
            this.RefreshItems();
        }

        /// <summary>
        /// Refreshes the list of items
        /// </summary>
        public void RefreshItems()
        {
            if (this.ExtentFactory != null && 
                this.MainWindow != null &&
                this.MainWindow.Settings.ProjectExtent != null)
            {
                var elements = this.ExtentFactory(this.MainWindow.Settings.ProjectExtent)
                    .Elements().Select(x => new ObjectDictionaryForView(x)).ToList();
                this.gridContent.ItemsSource = elements;
            }
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            var dialog = DetailDialog.ShowDialogToCreateTypeOf(this.MainType, this.Extent, this.DetailViewInfo);
            Ensure.That(dialog != null);
            dialog.DetailForm.Accepted += (x, y) => { this.RefreshItems(); };
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            this.ShowDetailDialog();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteCurrentlySelected();
        }

        private void reloadDelete_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshItems();
        }

        private void gridContent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.ShowDetailDialog();
        }

        /// <summary>
        /// Shows the detail dialog, where the user can modify the content
        /// </summary>
        private DetailDialog ShowDetailDialog()
        {
            var selectedItem = this.gridContent.SelectedItem as ObjectDictionary;

            if (selectedItem == null)
            {
                MessageBox.Show(Localization_DatenMeister_WPF.NoObjectSelected);
                return null;
            }
            else
            {
                Ensure.That(selectedItem.Value != null, "selectedItem.Value == null");

                var dialog = DetailDialog.ShowDialogFor(selectedItem.Value, this.DetailViewInfo);
                Ensure.That(dialog != null);
                dialog.DetailForm.Accepted += (x, y) => { this.RefreshItems(); };
                return dialog;
            }
        }

        /// <summary>
        /// Deletes the items that are currently selected
        /// </summary>
        private void DeleteCurrentlySelected()
        {
            var selectedItem = this.gridContent.SelectedItem as ObjectDictionary;

            if (selectedItem.Value != null)
            {
                selectedItem.Value.delete();
                this.RefreshItems();
            }
        }

        private void dataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                var dialog = this.ShowDetailDialog();
                if (dialog != null)
                {
                    var selectedCell = this.gridContent.CurrentCell;
                    if (selectedCell != null)
                    {
                        var column = selectedCell.Column as TableDataGridTextColumn;
                        var name = column.AssociatedViewColumn.get("name").AsSingle().ToString();
                        dialog.SelectFieldWithName(name);
                    }
                }
            }
        }

        private class TableDataGridTextColumn : DataGridTextColumn
        {
            public IObject AssociatedViewColumn
            {
                get;
                set;
            }
        }
    }
}
