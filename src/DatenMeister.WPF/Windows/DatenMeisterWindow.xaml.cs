using BurnSystems.Logging;
using BurnSystems.Test;
using DatenMeister.DataProvider.Wrapper;
using DatenMeister.DataProvider.Wrapper.EventOnChange;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
using DatenMeister.Pool;
using DatenMeister.Transformations;
using DatenMeister.WPF.Controls;
using DatenMeister.WPF.Modules.IconRepository;
using DatenMeister.WPF.Modules.RecentFiles;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Xml.Linq;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class DatenMeisterWindow : RibbonWindow, IDatenMeisterWindow
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ClassLogger logger = new ClassLogger(typeof(DatenMeisterWindow));

        /// <summary>
        /// Gets or sets the application core
        /// </summary>
        public ApplicationCore Core
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the settings of the datenmeister
        /// </summary>
        public IPublicDatenMeisterSettings Settings
        {
            get { return this.Core.Settings; }
        }

        /// <summary>
        /// Stores the current path
        /// </summary>
        private string pathOfDataExtent = null;

        /// <summary>
        /// Stores the information
        /// </summary>
        private List<TabInformation> listTabs = new List<TabInformation>();

        public DatenMeisterWindow()
        {
            this.InitializeComponent();

            WindowFactory.AutosetWindowSize(this);

            // Replaces the icons, if necessary
            this.ReplaceDefaultIcons();
        }

        public DatenMeisterWindow(ApplicationCore core)
            : this()
        {
            this.Core = core;

            if (core != null)
            {
                if (!string.IsNullOrEmpty(core.Settings.WindowTitle))
                {
                    this.UpdateWindowTitle();
                }
            }
        }

        /// <summary>
        /// Replaces the default icons
        /// </summary>
        private void ReplaceDefaultIcons()
        {
            var iconRepository = Injection.Application.Get<IIconRepository>();
            if (iconRepository != null)
            {
                var image = iconRepository.GetIcon("file-new");
                if (image != null)
                {
                    this.MenuFileNew.ImageSource = image;
                }

                image = iconRepository.GetIcon("file-open");
                if (image != null)
                {
                    this.MenuFileOpen.ImageSource = image;
                    this.MenuFileRecentFiles.ImageSource = image;
                }

                image = iconRepository.GetIcon("file-save");
                if (image != null)
                {
                    this.MenuFileSave.ImageSource = image;
                }

                image = iconRepository.GetIcon("file-saveas");
                if (image != null)
                {
                    this.MenuFileSaveAs.ImageSource = image;
                }

                image = iconRepository.GetIcon("file-about");
                if (image != null)
                {
                    this.MenuFileAbout.ImageSource = image;
                }

                image = iconRepository.GetIcon("file-exit");
                if (image != null)
                {
                    this.MenuFileExit.ImageSource = image;
                }

                image = iconRepository.GetIcon("save-export");
                if (image != null)
                {
                    this.MenuFileExportAsXml.ImageSource = image;
                }
            }
        }

        /// <summary>
        /// Updates the window title, when a change has happened
        /// </summary>
        private void UpdateWindowTitle()
        {
            var applicationTitle = this.Settings.ApplicationName;
            if (string.IsNullOrEmpty(this.pathOfDataExtent))
            {
                this.Title = string.Format("{0} - Depon.Net", applicationTitle);
            }
            else
            {
                this.Title = string.Format(
                    "{0} - {1} - Depon.Net",
                    System.IO.Path.GetFileNameWithoutExtension(this.pathOfDataExtent),
                    applicationTitle);
            }
        }

        /// <summary>
        /// Adds a menuentry to the application window
        /// </summary>
        /// <param name="menuHeadline">Headline of the menu</param>
        /// <param name="menuLine">Menu that shall be added. This tab is required to have the click event already associated</param>
        public void AddMenuEntry(string menuHeadline, UIElement menuItem)
        {
            // Search for a menuitem with the same name
            RibbonTab found = null;
            foreach (var item in this.menuMain.Items)
            {
                var ribbonTab = item as RibbonTab;
                if (ribbonTab.Header.ToString() == menuHeadline)
                {
                    found = ribbonTab;
                }
            }

            // Not found, create a new item
            if (found == null)
            {
                found = new RibbonTab()
                {
                    Header = menuHeadline
                };

                var ribbonGroup = new RibbonGroup();
                found.Items.Add(ribbonGroup);

                // Inserts the new item into the main menu bar. 
                // Last item is the "?"-Menu containing the about dialog. And it shall stay the last item
                // If no item is already in, include it on the first position
                this.menuMain.Items.Insert(Math.Max(0, this.menuMain.Items.Count - 1), found);
            }

            // Now create the item
            (found.Items[0] as RibbonGroup).Items.Add(menuItem);
        }

        public void AssociateDetailOpenEvent(IObject view, Action<DetailOpenEventArgs> action)
        {
            Ensure.That(view != null);

            var found = this.listTabs.Where(x => x.TableViewInfo.AsIObject().Id == view.Id).FirstOrDefault();
            if (found == null)
            {
                logger.Fail("Associate Detail Open Event failed because tab was not found");
            }
            else
            {
                found.TableControl.OpenSelectedViewFunc = action;
            }
        }

        /// <summary>
        /// Recreates the table views for all extents being the view extent. 
        /// If one tab is already opened, the tab will not be recreated. 
        /// </summary>
        /// <param name="assignEvent">This variable defines whether the 
        /// windows shall assign itself on the change event. 
        /// If yes, it will get updated, each time, the window extent changes</param>
        public void RefreshTabs()
        {
            var pool = PoolResolver.GetDefaultPool();
            var viewExtent = pool.GetExtents(ExtentType.View).First();

            Ensure.That(viewExtent != null, "No view extent has been given");

            var filteredViewExtent =
                viewExtent.Elements()
                    .Where(
                        x =>
                        {
                            var metaClass = (x as IElement).getMetaClass();
                            return metaClass.Equals(DatenMeister.Entities.AsObject.FieldInfo.Types.TableView)
                                || metaClass.Equals(DatenMeister.Entities.AsObject.FieldInfo.Types.TreeView);
                        });

            var elements = new List<IObject>();
            var first = true;

            // Goes through each tableinfo
            foreach (var tableInfo in filteredViewExtent)
            {
                var tableInfoObj = tableInfo.AsIObject();
                var name = DatenMeister.Entities.AsObject.Uml.NamedElement.getName(tableInfoObj);
                var extentUri = DatenMeister.Entities.AsObject.FieldInfo.TableView.getExtentUri(tableInfoObj);

                elements.Add(tableInfoObj);

                // Check, if there is already a tab, which hosts the tableInfo
                if (this.listTabs.Any(x => x.TableViewInfo.Equals(tableInfoObj)))
                {
                    // We do not need to recreate it
                    continue;
                }

                ///////////////////////////////////
                // Creates the Elements Factory
                Func<IPool, IReflectiveCollection> elementsFactory = (x) =>
                {
                    var e = x.ResolveByPath(extentUri);
                    if (e == null || e == ObjectHelper.Null)
                    {
                        throw new InvalidOperationException(extentUri + " did return null");
                    }

                    return e.AsReflectiveCollection();
                };

                ///////////////////////////
                // Creates the lsit control itself
                UIElement entityList;
                if ((tableInfoObj as IElement).getMetaClass() == DatenMeister.Entities.AsObject.FieldInfo.Types.TableView)
                {
                    entityList = this.CreateTableLayoutElement(tableInfoObj, elementsFactory);
                }
                else if ((tableInfoObj as IElement).getMetaClass() == DatenMeister.Entities.AsObject.FieldInfo.Types.TreeView)
                {
                    entityList = this.CreateTreeLayoutElement(tableInfoObj, elementsFactory);
                }
                else
                {
                    throw new NotImplementedException("Unknown View Type");
                }

                //////////////////////////////////////////////
                // Creates the grid, where the control is hosted. 
                // The grid will be included into a new tab
                var tab = CreateTab(tableInfoObj, name);
                var grid = new Grid();
                grid.Children.Add(entityList);
                tab.Content = grid;

                this.tabMain.Items.Add(tab);

                Ensure.That(entityList is IListLayout, "Given list is not of type IListLayout");
                this.listTabs.Add(new TabInformation()
                    {
                        Name = name,
                        TabItem = tab,
                        TableControl = entityList as IListLayout,
                        TableViewInfo = tableInfo
                    });

                // The first one, which is added, gets the focus
                if (first)
                {
                    this.Dispatcher.InvokeAsync(() => this.tabMain.SelectedValue = tab);
                    first = false;
                }
            }

            // Now go though the list and remove all views, which are not in listtabs
            foreach (var listTab in this.listTabs.ToList())
            {
                if (!elements.Any(x => x.Id == listTab.TableViewInfo.AsIObject().Id))
                {
                    this.listTabs.Remove(listTab);
                    this.tabMain.Items.Remove(listTab.TabItem);
                }
            }
        }

        /// <summary>
        /// Creates the UI element for the table layout 
        /// </summary>
        /// <param name="tableInfoObj">View information being used for the element</param>
        /// <param name="elementsFactory">The element factory, which is used to retrieve
        /// all the elements</param>
        /// <returns>The created UI Element</returns>
        private UIElement CreateTableLayoutElement(IObject tableInfoObj, Func<IPool, IReflectiveCollection> elementsFactory)
        {
            UIElement entityList;
            var listConfiguration = new TableLayoutConfiguration()
            {
                LayoutInfo = tableInfoObj
            };

            listConfiguration.ElementsFactory = elementsFactory;

            var tableControlList = new EntityTableControl();
            tableControlList.Configure(listConfiguration);
            entityList = tableControlList;
            return entityList;
        }

        /// <summary>
        /// Creates the UI element for the table layout 
        /// </summary>
        /// <param name="tableInfoObj">View information being used for the element</param>
        /// <param name="elementsFactory">The element factory, which is used to retrieve
        /// all the elements</param>
        /// <returns>The created UI Element</returns>
        private UIElement CreateTreeLayoutElement(IObject tableInfoObj, Func<IPool, IReflectiveCollection> elementsFactory)
        {
            UIElement entityList;
            var listConfiguration = new TreeLayoutConfiguration()
            {
                LayoutInfo = tableInfoObj
            };

            listConfiguration.ElementsFactory = elementsFactory;

            var tableControlList = new EntityTreeControl(listConfiguration);
            entityList = tableControlList;
            return entityList;
        }

        /// <summary>
        /// Adds the refreshing event to the view extent, if the given
        /// instance is a event throwing instance
        /// </summary>
        public void RegisterToChangeEvent()
        {
            var pool = PoolResolver.GetDefaultPool();
            var viewExtent = pool.GetExtents(ExtentType.View).First();

            var onChangeEventExtent = WrapperHelper.FindWrappedExtent<EventOnChangeExtent>(viewExtent);
            if (onChangeEventExtent == null)
            {
                logger.Fail("The ViewExtent is not wrapped by EventOnChangeExtent");
            }
            else
            {
                onChangeEventExtent.ChangeInExtent += (x, y) =>
                    {
                        logger.Verbose("View Extent has been changed");

                        this.Dispatcher.BeginInvoke(
                            new Action(() => this.RefreshTabs()),
                            System.Windows.Threading.DispatcherPriority.Background);
                    };

                logger.Verbose("Event connection to View Extent is established");
            }            
        }

        /// <summary>
        /// Creates a tab for the given tableInfo
        /// </summary>
        /// <param name="tableInfoObj">Information object containing the information about the
        /// current tab. </param>
        /// <param name="name">Name of the tabl to be shown</param>
        /// <returns>The created tab item</returns>
        private TabItem CreateTab(IObject tableInfoObj, string name)
        {
            // Creates the tab item
            var tab = new TabItem();
            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition());
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition());
            var textField = new TextBlock();
            textField.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            textField.Margin = new Thickness(0, 0, 0, 2);
            headerGrid.Children.Add(textField);
            textField.Text = name;
            var button = new Button();
            button.Template = this.Resources["Cross"] as ControlTemplate;
            button.Width = 12;
            button.Height = 12;
            button.Margin = new Thickness(8, 0, 0, 0);
            button.Click += (x, y) =>
            {
                tableInfoObj.delete();
                this.RefreshTabs();
            };

            headerGrid.Children.Add(button);
            Grid.SetColumn(button, 2);

            tab.Header = headerGrid;
            return tab;
        }

        /// <summary>
        /// Refreshes all views
        /// </summary>
        private void RefreshAllTabContent()
        {
            foreach (var tab in this.listTabs)
            {
                tab.TableControl.RefreshItems();
            }
        }

        #region Menu triggered File actions

        private void New_Click(object sender, RoutedEventArgs e)
        {
            var pool = Injection.Application.Get<IPool>();

            var userResult = this.DoesUserWantsToSaveData();
            if (userResult == null)
            {
                // User had cancelled the action
                return;
            }

            if (userResult == true)
            {
                // User wants to save
                this.SaveChanges();
            }

            this.Core.PerformInitializationOfViewSet();
            this.Core.PerformInitializeFromScratch();

            // Refreshes the view
            this.RefreshTabs();
            this.RefreshAllTabContent();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = Localization_DatenMeister_WPF.File_Filter;
            if (dialog.ShowDialog(this) == true)
            {
                var filename = dialog.FileName;

                this.LoadAndOpenFile(filename);
            }
        }

        /// <summary>
        /// Loads and opens a file and refreshes the window, so recently loaded extent is included
        /// </summary>
        /// <param name="path">Path of the object to be loaded</param>
        public void LoadAndOpenFile(string filename)
        {
            this.Core.LoadWorkbench(filename);

            // Adds the file to the recent files
            this.AddRecentFile(filename);

            // Refreshes all views
            this.RefreshAllTabContent();
            this.pathOfDataExtent = filename;
            this.UpdateWindowTitle();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.SaveChanges();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            this.SaveChangesAs();
        }

        /// <summary>
        /// Saves the changes
        /// </summary>
        private void SaveChanges(bool askForPathIfNecessary = true)
        {
            if (this.pathOfDataExtent == null)
            {
                if (askForPathIfNecessary)
                {
                    this.SaveChangesAs();
                }
            }
            else
            {
                var pool = PoolResolver.GetDefaultPool();
                var xmlExtent = pool.GetExtents(Logic.ExtentType.Data).First() as XmlExtent;
                Ensure.That(xmlExtent != null);

                // Stores the xml document
                // Will be redone afterwards
                // xmlExtent.XmlDocument.Save(this.pathOfDataExtent);
                xmlExtent.IsDirty = false;

                // Adds the file to the recent files
                this.AddRecentFile(this.pathOfDataExtent);

                MessageBox.Show(this, Localization_DatenMeister_WPF.ChangeHasBeenSaved);

                this.Core.StoreWorkbench(this.pathOfDataExtent);
                this.UpdateWindowTitle();
            }
        }

        /// <summary>
        /// Saves the changes and user may find a new path
        /// </summary>
        private void SaveChangesAs()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = Localization_DatenMeister_WPF.File_Filter;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog(this) == true)
            {
                var filename = dialog.FileName;
                this.pathOfDataExtent = filename;

                this.SaveChanges();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Adds the filepath to the list of recent files
        /// </summary>
        /// <param name="filePath">Path of the file to be added</param>
        public void AddRecentFile(string filePath)
        {
            RecentFileIntegration.AddRecentFile(
                this,
                filePath,
                System.IO.Path.GetFileNameWithoutExtension(filePath));
        }

        #endregion

        /// <summary>
        /// Does the user wants to save the data
        /// </summary>
        /// <returns>true, if user wants to save the data. 
        /// Null</returns>
        private bool? DoesUserWantsToSaveData()
        {            
            var pool = PoolResolver.GetDefaultPool();
            var dataExtent = pool.GetExtents(ExtentType.Data).First();

            if (!dataExtent.IsDirty)
            {
                // Content is not dirty, user will accept that content is not stored
                return false;
            }

            switch (MessageBox.Show(
                   this,
                   Localization_DatenMeister_WPF.QuestionSaveChanges,
                   Localization_DatenMeister_WPF.QuestionSaveChangesTitle,
                   MessageBoxButton.YesNoCancel))
            {
                case MessageBoxResult.Yes:
                    // Content is dirty and user wants to save 
                    return true;
                case MessageBoxResult.No:
                    return false;
                case MessageBoxResult.Cancel:
                    return null;
            }

            // Content is dirty and user does not want to save it
            return null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            switch (this.DoesUserWantsToSaveData())
            {
                case true:
                    this.SaveChanges();
                    break;
                case false:
                    break;
                case null:
                    e.Cancel = true;
                    break;
            }
        }

        private void ExportAsXml_Click(object sender, RoutedEventArgs e)
        {
            var pool = PoolResolver.GetDefaultPool();

            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = Localization_DatenMeister_WPF.File_Filter;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog(this) == true)
            {
                var xmlExtent = pool.GetExtents(Logic.ExtentType.Data).First() as XmlExtent;

                // Prepare extent, receiving the copy
                var copiedExtent = new XmlExtent(
                    XDocument.Parse("<export />"),
                    xmlExtent.Uri);
                copiedExtent.XmlDocument.AddAnnotation(SaveOptions.OmitDuplicateNamespaces);

                // Executes the copying
                ExtentCopier.Copy(xmlExtent, copiedExtent);

                // Stores the xml document
                copiedExtent.XmlDocument.Save(dialog.FileName);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                e.Handled = this.FocusCurrentTab();
            }

            if (e.Key == Key.S && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                this.SaveChanges();
            }
        }

        /// <summary>
        /// Focuses one of the grid cells of the current tab. 
        /// The grid cell, that is selected, but not in focus (that's the way how GridView works)
        /// </summary>
        /// <returns>true, if the focus was successful</returns>
        private bool FocusCurrentTab()
        {
            // Find selected thing
            var tabInfo = this.listTabs.Where(x => x.TabItem == this.tabMain.SelectedItem).FirstOrDefault();
            if (tabInfo != null)
            {
                tabInfo.TableControl.GiveFocusToGridContent();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Opens the about dialog
        /// </summary>
        /// <param name="sender">Sender of the element</param>
        /// <param name="e">Events of the elements</param>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AboutDialog();
            dlg.ShowDialog();
        }

        /// <summary>
        /// Loads the example data that will be used for the start of application
        /// </summary>
        public void LoadExampleData()
        {
            this.Core.PerformInitializeFromScratch();
            this.Core.PerformInitializeExampleData();
        }

        /// <summary>
        /// Gets the ribbon for the open files
        /// </summary>
        /// <returns></returns>
        public RibbonApplicationMenuItem GetRecentFileRibbon()
        {
            return this.MenuFileRecentFiles;
        }
    }
}
