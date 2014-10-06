using BurnSystems.Test;
using BurnSystems.UserExceptionHandler;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.Pool;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Entities.AsObject.Uml;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
using DatenMeister.Transformations;
using DatenMeister.WPF.Helper;
using Ninject;
using System;
using System.Collections.Generic;
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
    public partial class EntityTableControl : UserControl
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
            private  set;
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

            var mainType = TableView.getMainType(this.Configuration.TableViewInfo);
            if (mainType == null)
            {
                return ExtentType.View;
            }

            var instance = pool.GetInstance(mainType.Extent);
            if (instance == null)
            {
                return ExtentType.View;
            }

            return DatenMeisterPool.GetMetaExtentType(instance.ExtentType);
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
                var fieldInfos = this.Configuration.GetTableViewInfoAsTableView().getFieldInfos().AsEnumeration();
                if (tableViewInfo.getDoAutoGenerateByProperties() && fieldInfos.Count() == 0)
                {
                    ViewHelper.AutoGenerateViewDefinition(
                        this.Configuration.ElementsFactory(pool), 
                        this.Configuration.TableViewInfo, 
                        true /*order by name*/);
                }

                // Now, create the fields, we might have autogenerated the fields
                var fieldInfosAsObject = this.Configuration.GetTableViewInfoAsTableView().getFieldInfos(); // Needs to be updated
                var asEnumeration = fieldInfosAsObject.AsEnumeration();
                this.gridContent.Columns.Clear();
                foreach (var fieldInfo in asEnumeration.Select(x => x.AsSingle().AsIObject()))
                {
                    var fieldInfoObj = new DatenMeister.Entities.AsObject.FieldInfo.General(fieldInfo);
                    var name = fieldInfoObj.getName();
                    var binding = fieldInfoObj.getBinding();
                    var column = new TableDataGridTextColumn();
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
                    var name = NamedElement.getName(elementType).AsSingle();
                    var btn = new Button();
                    btn.Content = "New " + name;
                    btn.Style = this.gridButtons.Resources["TouchButton"] as Style;
                    btn.Click += (x, y) =>
                    {
                        ShowNewInstanceDialog(elementType);
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
                    AsEnumeration<IObject>();
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

                    this.gridContent.ItemsSource = elements.ToList();
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
            dialog.DetailForm.Accepted += (x, y) => { this.RefreshItems(); };
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
            var mainType = TableView.getMainType(this.Configuration.TableViewInfo);
            var instance = pool.GetInstance(mainType.Extent);
            if (instance != null)
            {
                extentType = instance.ExtentType;
            }

            // Shows the dialog
            if (SelectTypeOfNewObjectDialog.ShowNewOfGenericTypeDialog(
                    this.Configuration.ElementsFactory(pool),
                    extentType)
                != null)
            {
                // Only, if a new item has been created the view needs to be reupdated
                this.RefreshItems();
            }
        }

        /// <summary>
        /// Shows the detail dialog, where the user can modify the content
        /// </summary>
        private DetailDialog ShowDetailDialog()
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
                var readOnly = false;

                // Check, if the dialog to be opened shall be as a read-only dialog
                if (this.Configuration.ViewInfoForDetailView != null &&
                    !FormView.getAllowEdit(this.Configuration.ViewInfoForDetailView))
                {
                    // Nothing to do
                    readOnly = true;
                }

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
                dialog.DetailForm.Accepted += (x, y) => { this.RefreshItems(); };
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
                this.RefreshItems();
            }
            else
            {
                MessageBox.Show(Localization_DatenMeister_WPF.NoElementsSelected);
                return;
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
            if ( exceptionHandling != null )
            {
                exceptionHandling.HandleException(exc);
            }

            // Shows the content in the list window
            this.ErrorMessage.Visibility = System.Windows.Visibility.Visible;
            this.DataTable.Visibility = System.Windows.Visibility.Collapsed;
            this.ErrorMessageContent.Text = exc.ToString();
        }

        /// <summary>
        /// Adds the associated view column to the text grid
        /// </summary>
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
