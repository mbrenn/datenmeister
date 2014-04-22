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
        /// Stores the extent
        /// </summary>
        private Func<IURIExtent, IURIExtent> extent;

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
                if (this.MainWindow != null && this.MainWindow.ProjectExtent != null)
                {
                    return this.extent(this.MainWindow.ProjectExtent);
                }

                else
                {
                    return null;
                }
            }
            set
            {
                this.extent = (x) => value;
                if (this.extent != null)
                {
                    this.RefreshItems();
                }
            }
        }

        public Func<IURIExtent, IURIExtent> ExtentFactory
        {
            get { return this.extent; }
            set
            {
                this.extent = value;
                // Refreshes the items when we get a new extent factory
                this.RefreshItems();
            }
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
                var column = new DataGridTextColumn();
                column.Header = title;
                column.Binding = new Binding("["+name+"]");

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
                this.MainWindow.ProjectExtent != null)
            {
                var elements = this.ExtentFactory(this.MainWindow.ProjectExtent)
                    .Elements().Select(x => new ObjectDictionary(x)).ToList();
                this.gridContent.ItemsSource = elements;
            }
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            var form = new DetailDialog();
            form.Pool = this.Extent.Pool;
            form.DetailForm.EditMode = EditMode.New;
            form.DetailForm.FormViewInfo = this.DetailViewInfo;
            form.DetailForm.ElementFactory = this.ElementFactory;

            form.DetailForm.Accepted += (x, y) => { this.RefreshItems(); };
            form.Show();
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
        private void ShowDetailDialog()
        {
            var selectedItem = this.gridContent.SelectedItem as ObjectDictionary;

            if (selectedItem == null)
            {
                MessageBox.Show(Localization_DatenMeister_WPF.NoObjectSelected);
            }
            else
            {
                Ensure.That(selectedItem.Value != null, "selectedItem.Value == null");

                var form = new DetailDialog();
                form.Pool = this.Extent.Pool;
                form.DetailForm.EditMode = EditMode.Edit;
                form.DetailForm.FormViewInfo = this.DetailViewInfo;
                form.DetailForm.ElementFactory = this.ElementFactory;
                form.DetailForm.DetailObject = selectedItem.Value;

                form.DetailForm.Accepted += (x, y) => { this.RefreshItems(); };
                form.Show();
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
    }
}
