using BurnSystems.Test;
using DatenMeister.DataProvider.Pool;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic;
using DatenMeister.Transformations;
using DatenMeister.WPF.Helper;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private Func<IPool, IReflectiveCollection> elementsFactory;

        /// <summary>
        /// Gets or sets the value whether the table control shall be used
        /// as a selection control. 
        /// If true, the buttons for modification will be removed, but the 
        /// OK-button will be added
        /// </summary>
        public bool UseAsSelectionControl
        {
            get;
            set;
        }

        public IDatenMeisterSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// This event handler is executed, when user clicked on the ok button
        /// </summary>
        public event EventHandler OkClicked;

        /// <summary>
        /// Defines the extent that shall be shown
        /// </summary>
        public IURIExtent Extent
        {
            set
            {
                this.elementsFactory = (x) => value.Elements();
                if (this.elementsFactory != null)
                {
                    this.RefreshItems();
                }
            }
        }

        public Func<IPool, IReflectiveCollection> ElementsFactory
        {
            get
            {
                return this.elementsFactory;
            }

            set
            {
                this.elementsFactory = value;

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
        public Func<IObject> NewElementFactory
        {
            get;
            set;
        }

        /// <summary>
        /// This function will be used to open a view of the currently selected item 
        /// </summary>
        public Action<DetailOpenEventArgs> OpenSelectedViewFunc
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the selected elements, when the user clicked on 
        /// the "OK" Button
        /// </summary>
        public IEnumerable<IObject> SelectedElements
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
        public void Relayout()
        {
            if (this.tableViewInfo == null)
            {
                // Nothing to do, should not happen
                return;
            }

            // Checks status of buttons
            this.buttonNew.Visibility = this.tableViewInfo.getAllowNew() && !this.UseAsSelectionControl ?
                System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            this.buttonNewByType.Visibility = this.tableViewInfo.getAllowNew() && !this.UseAsSelectionControl ?
                System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            this.buttonEdit.Visibility = this.tableViewInfo.getAllowEdit() && !this.UseAsSelectionControl ?
                System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            this.buttonDelete.Visibility = this.tableViewInfo.getAllowDelete() && !this.UseAsSelectionControl ?
                System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            this.buttonOk.Visibility = this.UseAsSelectionControl ? 
                System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            foreach (var fieldInfo in this.tableViewInfo.getFieldInfos().AsEnumeration().Select (x=> x.AsSingle().AsIObject()))
            {
                var fieldInfoObj = new DatenMeister.Entities.AsObject.FieldInfo.General(fieldInfo);
                var name = fieldInfoObj.getName();
                var binding = fieldInfoObj.getBinding();
                var column = new TableDataGridTextColumn();
                column.Header = name;
                column.Binding = new Binding("["+binding+"]");
                column.AssociatedViewColumn = fieldInfo;

                this.gridContent.Columns.Add(column);
            }

            // Gets the elements
            this.RefreshItems();
        }

        /// <summary>
        /// Gets the elements as a reflective collection
        /// </summary>
        /// <returns>The reflective collection</returns>
        public IReflectiveCollection GetElements()
        {
            Ensure.That(this.ElementsFactory != null, "No Elementsfactory is set");
            Ensure.That(this.Settings != null, "Settings for DatenMeister are not set");
         
            return this.ElementsFactory(this.Settings.Pool);
        }

        public IEnumerable<IObject> GetFieldInfos()
        {
            if (this.tableViewInfo != null)
            {
                return this.
                    tableViewInfo.
                    getFieldInfos().
                    AsEnumeration<IObject>();
            }

            return null;
        }

        /// <summary>
        /// Refreshes the list of items
        /// </summary>
        public void RefreshItems()
        {
            if (this.ElementsFactory != null)
            {
                var elements = this.GetElements()
                    .Select(x => 
                        new ObjectDictionaryForView(x.AsIObject(), this.GetFieldInfos())).ToList();
                this.gridContent.ItemsSource = elements;
            }
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            this.ShowNewDialog();
        }

        private void buttonNewByType_Click(object sender, RoutedEventArgs e)
        {
            this.ShowNewOfGenericTypeDialog();
        }

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            this.ShowDetailDialog();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            this.DeleteCurrentlySelected();
        }

        private void buttonReload_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshItems();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            this.AcceptSelectedElements(e);
        }

        /// <summary>
        /// Accepts the selected elements 
        /// </summary>
        /// <param name="e"></param>
        private void AcceptSelectedElements(RoutedEventArgs e)
        {
            var selectedItems = this.gridContent.SelectedItems;

            this.SelectedElements = selectedItems.Cast<ObjectDictionaryForView>().Select(x => x.Value).ToArray();

            var ev = this.OkClicked;
            if (ev != null)
            {
                ev(this, e);
            }
        }

        private void gridContent_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.UseAsSelectionControl)
            {
                this.AcceptSelectedElements(e);
            }
            else
            {
                this.ShowDetailDialog();
            }
        }

        private void ShowNewDialog()
        {
            if (!DatenMeister.Entities.AsObject.FieldInfo.FormView.getAllowNew(this.tableViewInfo))
            {
                // Nothing to do
                return;
            }

            var dialog = DetailDialog.ShowDialogToCreateTypeOf(this.MainType, this.GetElements(), this.Settings, this.DetailViewInfo);
            Ensure.That(dialog != null);
            dialog.DetailForm.Accepted += (x, y) => { this.RefreshItems(); };
        }

        private void ShowNewOfGenericTypeDialog()
        {
            if (!DatenMeister.Entities.AsObject.FieldInfo.FormView.getAllowNew(this.tableViewInfo))
            {
                // Nothing to do
                return;
            }

            var dialog = new ListDialog();
            var allTypes = 
                new AllItemsReflectiveCollection(this.Settings.Pool)
                .FilterByType(DatenMeister.Entities.AsObject.Uml.Types.Type);
            dialog.SetReflectiveCollection(allTypes, this.Settings);
            if (dialog.ShowDialog() == true)
            {
                this.RefreshItems();
            }
        }

        /// <summary>
        /// Shows the detail dialog, where the user can modify the content
        /// </summary>
        private DetailDialog ShowDetailDialog()
        {
            var selectedItem = this.gridContent.SelectedItem as ObjectDictionary;

            if (this.OpenSelectedViewFunc != null)
            {
                this.OpenSelectedViewFunc(
                    new DetailOpenEventArgs()
                    {
                        Collection = this.GetElements(),
                        Value = selectedItem.Value
                    });

                return null;
            }
            else if (selectedItem == null)
            {
                this.ShowNewDialog();
                return null;
            }
            else
            {
                var readOnly = false;
                
                // Check, if the dialog to be opend shall be a read-only dialog
                if (!DatenMeister.Entities.AsObject.FieldInfo.FormView.getAllowEdit(this.tableViewInfo))
                {
                    // Nothing to do
                    readOnly = true;
                }

                Ensure.That(selectedItem.Value != null, "selectedItem.Value == null");

                var dialog = DetailDialog.ShowDialogFor(
                    selectedItem.Value, 
                    this.Settings, 
                    this.DetailViewInfo, 
                    readOnly);

                if (dialog == null)
                {
                    MessageBox.Show(
                        Localization_DatenMeister_WPF.NoItemDialogFound);

                    return null;
                }

                // When user accepts the changes, all items shall be refreshed. 
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
                // Edit currently selected property
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

                e.Handled = true;
            }
        }

        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                // Deletes the currently selected item
                this.DeleteCurrentlySelected();
                e.Handled = true;
            }

            if (e.Key == Key.Insert)
            {
                this.ShowNewDialog();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Gives the focus to the grid content by finding the first column of selected row.
        /// </summary>
        public void GiveFocusToGridContent()
        {
            DataGridHelper.GiveFocusToContent(this.gridContent);
        }

        private class TableDataGridTextColumn : DataGridTextColumn
        {
            public IObject AssociatedViewColumn
            {
                get;
                set;
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                this.GiveFocusToGridContent();
            }
        }
    }
}
