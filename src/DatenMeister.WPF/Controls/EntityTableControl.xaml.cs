using BurnSystems.Test;
using BurnSystems.UserExceptionHandler;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Entities.AsObject.Uml;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
using DatenMeister.Pool;
using DatenMeister.WPF.Controls.GuiElements;
using DatenMeister.WPF.Controls.GuiElements.Elements;
using DatenMeister.WPF.Helper;
using DatenMeister.WPF.Windows;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Interaktionslogik für ObjectTable.xaml
    /// </summary>
    public partial class EntityTableControl : UserControl, IListLayout
    {
        /// <summary>
        /// Gets or sets the configuration of the class
        /// </summary>
        protected TableLayoutConfiguration Configuration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text that shall be used for filtering the items in the current view
        /// </summary>
        private string filterByText;

        /// <summary>
        /// This event handler is executed, when user clicked on the ok button
        /// </summary>
        public event EventHandler OkClicked;

        /// <summary>
        /// This event handler is executed, when user clicked on the cancel button
        /// </summary>
        public event EventHandler CancelClicked;

        /// <summary>
        /// Stores the information whether the list ist configured 
        /// </summary>
        private bool isConfigured = false;

        /// <summary>
        /// Gets or sets the type that shall be created, when user clicks on 'new'.
        /// </summary>
        private IObject mainType; 
        
        /// <summary>
        /// Stores the last column, which has been used to perform sorting
        /// </summary>
        private GenericColumn lastSortedColumn;

        /// <summary>
        /// Stores the last direction of the sorting
        /// </summary>
        private ListSortDirection? lastSortedDirection;

        /// <summary>
        /// This function will be used to open a view of the currently selected item 
        /// </summary>
        public Action<DetailOpenEventArgs> OpenSelectedViewFunc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the selected elements, when the user clicked on 
        /// the "OK" Button
        /// </summary>
        public IEnumerable<IObject> SelectedElements
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the EntityTableControl class
        /// </summary>
        public EntityTableControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the EntityTableControl class
        /// </summary>
        /// <param name="configuration">Configuration to be used</param>
        public EntityTableControl(TableLayoutConfiguration configuration)
            : this()
        {
            this.Configure(configuration);
        }

        /// <summary>
        /// Configures the table view
        /// </summary>
        /// <param name="configuration"></param>
        public void Configure(TableLayoutConfiguration configuration)
        {
            Ensure.That(configuration != null, "No Configuration is given");
            this.Configuration = configuration;
            this.isConfigured = true;
            this.Relayout();
            this.RefreshItems();
        }

        /// <summary>
        /// Gets or sets the meta extent type being queried, when user clicks on 'New by Type'
        /// </summary>
        public ExtentType GetMetaExtentType()
        {
            if (!this.isConfigured)
            {
                throw new InvalidOperationException("The view is not configured");
            }

            var pool = Injection.Application.Get<IPool>();

            var mainType = TableView.getMainType(this.Configuration.LayoutInfo);
            if (mainType == null)
            {
                return ExtentType.View;
            }

            var instance = pool.GetInstance(mainType.Extent);
            if (instance == null)
            {
                return ExtentType.View;
            }

            return DatenMeisterPool.GetMetaExtentType(instance.extentType);
        }

        /// <summary>
        /// Does the relayout
        /// </summary>
        public void Relayout()
        {
            try
            {
                // Checks, whether the window is properly configured
                if (!this.isConfigured)
                {
                    throw new InvalidOperationException("The window is not properly configured");
                }

                // Does the usual work
                var pool = Injection.Application.Get<IPool>();
                var tableViewInfo = this.Configuration.GetTableViewInfoAsTableView();

                if (tableViewInfo == null)
                {
                    // Nothing to do, should not happen
                    return;
                }

                // Checks status of buttons
                this.buttonNew.Visibility = tableViewInfo.getAllowNew() && !this.Configuration.UseAsSelectionControl ?
                    System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                this.buttonNewByType.Visibility = tableViewInfo.getAllowNew() && !this.Configuration.UseAsSelectionControl ?
                    System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                this.buttonEdit.Visibility = tableViewInfo.getAllowEdit() && !this.Configuration.UseAsSelectionControl ?
                    System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                this.buttonDelete.Visibility = tableViewInfo.getAllowDelete() && !this.Configuration.UseAsSelectionControl ?
                    System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                this.buttonOk.Visibility = this.Configuration.UseAsSelectionControl ?
                    System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                this.buttonCancel.Visibility = this.Configuration.ShowCancelButton ?
                    System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

                // Creates the new buttons
                if (this.mainType == null)
                {
                    this.mainType = tableViewInfo.getMainType();
                }

                this.CreateNewInstanceButtons();

                //  Checks, if auto generation is necessary
                var fieldInfos = this.Configuration.GetTableViewInfoAsTableView().getFieldInfos();
                if (tableViewInfo.getDoAutoGenerateByProperties() && fieldInfos.Count() == 0)
                {
                    ViewHelper.AutoGenerateViewDefinition(
                        this.Configuration.ElementsFactory(pool),
                        this.Configuration.LayoutInfo,
                        true /*order by name*/);
                }

                // Now, create the fields, we might have autogenerated the fields
                var fieldInfosAsObject = this.Configuration.GetTableViewInfoAsTableView().getFieldInfos(); // Needs to be updated
                this.gridContent.Columns.Clear();
                foreach (var fieldInfo in fieldInfosAsObject.Select(x => x.AsIObject()))
                {
                    var fieldInfoObj = new DatenMeister.Entities.AsObject.FieldInfo.General(fieldInfo);
                    var name = fieldInfoObj.getName();
                    var binding = fieldInfoObj.getBinding();
                    var column = WpfElementMapping.MapForTable(this.Configuration.LayoutInfo, fieldInfo, binding);
                    column.Header = name;
                    column.Binding = new Binding("[" + binding + "]");
                    column.AssociatedViewColumn = fieldInfo;

                    var width = fieldInfoObj.getColumnWidth();
                    if (width != 0)
                    {
                        column.Width = new DataGridLength(width);
                    }

                    this.gridContent.Columns.Add(column);
                }

                // Gets the elements
                this.RefreshItems();
            }
            catch (Exception exc)
            {
                this.HandleException(exc);
            }
        }

        /// <summary>
        /// Creates the buttons to create new instances
        /// </summary>
        private void CreateNewInstanceButtons()
        {
            // Checks, if we have additional buttons to create new instances
            var typesForCreation = this.Configuration.GetTableViewInfoAsTableView().getTypesForCreation();
            if (typesForCreation != null && typesForCreation != ObjectHelper.NotSet && typesForCreation != ObjectHelper.Null)
            {
                foreach (var elementType in typesForCreation)
                {
                    var name = NamedElement.getName(elementType.AsIObject());
                    var btn = new Button();
                    btn.Content = "New " + name;
                    btn.Style = this.gridButtons.Resources["TouchButton"] as Style;
                    btn.Click += (x, y) =>
                    {
                        ShowNewInstanceDialog(elementType.AsIObject());
                    };

                    this.areaToolbar.Children.Insert(0, btn);
                }
            }
        }

        /// <summary>
        /// Gets the elements as a reflective collection
        /// </summary>
        /// <returns>The reflective collection</returns>
        public IReflectiveCollection GetElements()
        {
            var pool = Injection.Application.Get<IPool>();
            Ensure.That(this.Configuration.ElementsFactory != null, "No Elementsfactory is set");

            return this.Configuration.ElementsFactory(pool);
        }

        public IEnumerable<IObject> GetFieldInfos()
        {
            if (this.Configuration.GetTableViewInfoAsTableView() != null)
            {
                return this.
                    Configuration.GetTableViewInfoAsTableView().
                    getFieldInfos().
                    Select(x => x.AsIObject());
            }

            return null;
        }

        /// <summary>
        /// Refreshes the list of items
        /// </summary>
        public void RefreshItems()
        {
            try
            {
                if (this.Configuration.ElementsFactory != null)
                {
                    var elements = this.GetElements()
                        .Select(x =>
                            new ObjectDictionaryForView(x.AsIObject(), this.GetFieldInfos()));

                    if (!string.IsNullOrEmpty(this.filterByText))
                    {
                        elements = elements.Where(x =>
                            ObjectDictionaryForView.FilterByText(x, this.filterByText));
                    }

                    // Resorts the columns, if they had been sorted before
                    if (this.lastSortedColumn != null)
                    {
                        if (this.lastSortedDirection == ListSortDirection.Ascending)
                        {
                            elements = elements.OrderBy(
                                x => x.Get(this.lastSortedColumn.PropertyName));
                        }
                        else
                        {
                            elements = elements.OrderByDescending(
                                x => x.Get(this.lastSortedColumn.PropertyName));
                        }
                    }

                    this.gridContent.ItemsSource = elements.ToList();
                    
                    // Resorts the columns, if they had been sorted before
                    if (this.lastSortedColumn != null)
                    {
                        this.lastSortedColumn.SortDirection = this.lastSortedDirection;
                    }
                }
            }
            catch (Exception exc)
            {
                this.HandleException(exc);
            }
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
            if (e.ChangedButton == MouseButton.Left)
            {
                if (this.Configuration.UseAsSelectionControl)
                {
                    this.AcceptSelectedElements(e);
                }
                else
                {
                    // Checks, what the user has selected, only click within the form 
                    // is considered as valid
                    var dep = (DependencyObject)e.OriginalSource;

                    // Iteratively traverse the visual tree an check, if the user clicked on the DataGridColumnHeader
                    while ((dep != null) &&
                            !(dep is DataGridCell) &&
                            !(dep is DataGridColumnHeader))
                    {
                        dep = VisualTreeHelper.GetParent(dep);

                        if (dep is DataGridColumnHeader)
                        {
                            return;
                        }
                    }

                    this.ShowDetailDialog();
                }
            }
        }

        /// <summary>
        /// Shows the new dialog for the main type being 
        /// applied used
        /// </summary>
        private void ShowNewDialog()
        {
            ShowNewInstanceDialog(this.mainType);
        }

        /// <summary>
        /// Shows the oppurtunity to create a new instance
        /// </summary>
        /// <param name="newType">Type to be created</param>
        private void ShowNewInstanceDialog(IObject newType)
        {
            if (!this.Configuration.GetTableViewInfoAsTableView().getAllowNew())
            {
                // Nothing to do
                return;
            }

            var dialog = DetailDialog.ShowDialogToCreateTypeOf(
                newType,
                this.GetElements(),
                this.Configuration.ViewInfoForDetailView);
            Ensure.That(dialog != null);
            dialog.DetailForm.Accepted += (x, y) =>
            {
                this.NotifyContentChanged();
            };
        }

        /// <summary>
        /// Shows a list of all available types and the user can select one. 
        /// The selected item will be created.
        /// </summary>
        private void ShowNewOfGenericTypeDialog()
        {
            var pool = Injection.Application.Get<IPool>();

            if (!this.Configuration.GetTableViewInfoAsTableView().getAllowNew())
            {
                // Nothing to do
                return;
            }

            // Tries to fiendout the extent type
            var extentType = ExtentType.Type;
            var mainType = TableView.getMainType(this.Configuration.LayoutInfo);

            if (mainType != null)
            {
                var instance = pool.GetInstance(mainType.Extent);
                if (instance != null)
                {
                    extentType = instance.extentType;
                }
            }

            // Shows the dialog
            var newItem = SelectTypeOfNewObjectDialog.ShowNewOfGenericTypeDialog(
                    this.Configuration.ElementsFactory(pool),
                    extentType);
            if (newItem != null)
            {
                // Only, if a new item has been created the view needs to be reupdated
                this.NotifyContentChanged();

                // If item was created, open the detail form
                var dialog = DetailDialog.ShowDialogFor(
                    newItem,
                    null,
                    false);

                if (dialog != null)
                {
                    dialog.DetailForm.Accepted += (x, y) =>
                    {
                        this.NotifyContentChanged();
                    };
                }
            }
        }

        /// <summary>
        /// Shows the detail dialog, where the user can modify the content
        /// </summary>
        private DetailDialog ShowDetailDialog()
        {
            var readOnly = false;

            // Check, if the dialog to be opened shall be as a read-only dialog
            if (this.Configuration.ViewInfoForDetailView != null &&
                !FormView.getAllowEdit(this.Configuration.ViewInfoForDetailView))
            {
                // Nothing to do
                readOnly = true;
            }

            var numberOfSelectedItems = this.gridContent.SelectedItems.Count;

            if (numberOfSelectedItems <= 1)
            {
                var selectedItem = this.gridContent.SelectedItem as ObjectDictionary;

                // Checks, if there is an function handler associated to the
                // given table. View. If there is a function, call the associated function handler
                if (this.OpenSelectedViewFunc != null && selectedItem != null)
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
                    // No element has been selected, so try to create a new on
                    this.ShowNewDialog();
                    return null;
                }
                else
                {

                    Ensure.That(selectedItem.Value != null, "selectedItem.Value == null");

                    var dialog = DetailDialog.ShowDialogFor(
                        selectedItem.Value,
                        this.Configuration.ViewInfoForDetailView,
                        readOnly);

                    if (dialog == null)
                    {
                        MessageBox.Show(
                            Localization_DatenMeister_WPF.NoItemDialogFound);

                        return null;
                    }

                    // When user accepts the changes, all items in current view shall be refreshed. 
                    dialog.DetailForm.Accepted += (x, y) =>
                    {
                        this.NotifyContentChanged();
                    };
                    return dialog;
                }
            }
            else
            {
                var selectedItems = this.gridContent.SelectedItems;
                var convertedItems = new List<IObject>();
                foreach (var item in selectedItems)
                {
                    var convertedItem = (item as ObjectDictionary).Value;
                    convertedItems.Add(convertedItem);
                }
                
                // Multiple dialog
                var dialog = DetailDialog.ShowDialogFor(
                    convertedItems,
                    this.Configuration.ViewInfoForDetailView,
                    readOnly);

                if (dialog == null)
                {
                    MessageBox.Show(
                        Localization_DatenMeister_WPF.NoItemDialogFound);

                    return null;
                }

                // When user accepts the changes, all items in current view shall be refreshed. 
                dialog.DetailForm.Accepted += (x, y) =>
                {
                    this.NotifyContentChanged();
                };
                return dialog;

            }
        }

        /// <summary>
        /// Deletes the items that are currently selected
        /// </summary>
        private void DeleteCurrentlySelected()
        {
            // Go through all the selected items and remove them
            var any = false;
            foreach (var selectedItem in this.gridContent.SelectedItems)
            {
                var sAsObjectDictionary = selectedItem as ObjectDictionary;
                if (sAsObjectDictionary == null)
                {
                    continue;
                }

                any = true;
                if (sAsObjectDictionary.Value != null)
                {
                    sAsObjectDictionary.Value.delete();
                }
            }

            // If, something has been removed, perform the update
            if (any)
            {
                NotifyContentChanged();
            }
            else
            {
                MessageBox.Show(Localization_DatenMeister_WPF.NoElementsSelected);
                return;
            }
        }

        /// <summary>
        /// Notifies that content has changed. This will refresh 
        /// the list and throw the ItemUpdated event of the configuration
        /// </summary>
        private void NotifyContentChanged()
        {
            this.RefreshItems();
            this.Configuration.OnItemUpdated();
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

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            var ev = this.CancelClicked;
            if (ev != null)
            {
                ev(this, e);
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
                        var column = selectedCell.Column as GenericColumn;
                        var name = column.AssociatedViewColumn.getAsSingle("name").ToString();
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

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                this.GiveFocusToGridContent();
            }
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.filterByText = this.txtFilter.Text;
            this.RefreshItems();
        }

        /// <summary>
        /// Gives the focus to the grid content by finding the first column of selected row.
        /// </summary>
        public void GiveFocusToGridContent()
        {
            DataGridHelper.GiveFocusToContent(this.gridContent);
        }

        /// <summary>
        /// Is called, when an exception within the layout function needs to be handled
        /// </summary>
        /// <param name="exc">Exception to be handled</param>
        private void HandleException(Exception exc)
        {
            var exceptionHandling = Injection.Application.TryGet<IExceptionHandling>();
            if (exceptionHandling != null)
            {
                exceptionHandling.HandleException(exc);
            }

            // Shows the content in the list window
            this.ErrorMessage.Visibility = System.Windows.Visibility.Visible;
            this.DataTable.Visibility = System.Windows.Visibility.Collapsed;
            this.ErrorMessageContent.Text = exc.ToString();
        }

        private void gridContent_Sorting(object sender, DataGridSortingEventArgs e)
        {
            switch (e.Column.SortDirection)
            {
                case null:
                    this.lastSortedDirection = ListSortDirection.Ascending;
                    break;
                case ListSortDirection.Ascending:
                    this.lastSortedDirection = ListSortDirection.Descending;
                    break;
                case ListSortDirection.Descending:
                    this.lastSortedDirection = ListSortDirection.Ascending;
                    break;
            }

            var asGenericColumn = e.Column as GenericColumn;
            Ensure.That(asGenericColumn != null);

            this.lastSortedColumn = asGenericColumn;

            e.Handled = true;
            this.RefreshItems();
        }

        private void gridContent_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                this.gridContent.ContextMenu = null;
                var selectedItems = this.gridContent.SelectedItems;

                if (selectedItems.Count == 1 && this.Configuration.UpdateContextMenu != null)
                {
                    var contextMenu = new ContextMenu();
                    var dictionary = selectedItems[0] as ObjectDictionary;

                    this.Configuration.UpdateContextMenu(contextMenu, dictionary.Value);

                    if (contextMenu.Items.Count > 0)
                    {
                        this.gridContent.ContextMenu = contextMenu;
                        //this.gridContent.ContextMenu.IsOpen = true;
                    }
                }
            }
        }
    }
}
