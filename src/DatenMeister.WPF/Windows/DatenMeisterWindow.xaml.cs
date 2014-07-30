using BurnSystems.Logging;
using BurnSystems.Test;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
using DatenMeister.Pool;
using DatenMeister.Transformations;
using DatenMeister.WPF.Controls;
using DatenMeister.WPF.Helper;
using DatenMeister.WPF.Modules.RecentFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        public IDatenMeisterSettings Settings
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

            AutosetWindowSize(this);
        }

        public static void AutosetWindowSize(Window wnd, double ratio = 1.0)
        {
            var width = wnd.Width;
            var height = wnd.Height;

            var newWidth = System.Windows.SystemParameters.PrimaryScreenWidth / 2 * ratio;
            var newHeight = System.Windows.SystemParameters.PrimaryScreenHeight / 2 * ratio;

            wnd.Left -= newWidth / 2;
            wnd.Top -= newHeight / 2;
            wnd.Width = newWidth;
            wnd.Height = newHeight;
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
                this.menuMain.Items.Insert(this.menuMain.Items.Count - 1, found);
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
        public void RefreshTabs()
        {
            Ensure.That(this.Settings.ViewExtent != null, "No view extent has been given");

            var filteredViewExtent =
                this.Settings.ViewExtent.Elements()
                    .FilterByType(DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);

            var elements = new List<IObject>();
            var first = true;

            // Goes through each tableinfo
            foreach (var tableInfo in filteredViewExtent)
            {
                var tableInfoObj = tableInfo.AsIObject();
                elements.Add(tableInfoObj);

                // Check, if there is already a tab, which hosts the tableInfo
                if (this.listTabs.Any(x => x.TableViewInfo.AsIObject().Id == tableInfoObj.Id))
                {
                    // We do not need to recreate it
                    continue;
                }

                var tableViewInfo = new DatenMeister.Entities.AsObject.FieldInfo.TableView(tableInfoObj);

                var extentUri = tableViewInfo.getExtentUri();
                Ensure.That(!string.IsNullOrEmpty(extentUri), "ExtentURI has not been given");

                var name = tableViewInfo.getName();
                var tab = CreateTab(tableInfoObj, name);

                // Creates the list control
                var entityList = new EntityTableControl();
                entityList.Settings = this.Settings;
                entityList.ElementsFactory = (x) =>
                    {
                        return x.ResolveByPath(extentUri).AsReflectiveCollection();
                    };

                entityList.TableViewInfo = tableViewInfo;
                entityList.MainType = tableViewInfo.getMainType();

                // Creates the grid being used
                var grid = new Grid();
                grid.Children.Add(entityList);
                tab.Content = grid;

                this.tabMain.Items.Add(tab);

                this.listTabs.Add(new TabInformation()
                    {
                        Name = name,
                        TabItem = tab,
                        TableControl = entityList,
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
            headerGrid.Children.Add(textField);
            textField.Text = name;
            var button = new Button();
            button.Content = "X";
            button.FontSize = 10;
            button.FontWeight = FontWeights.Bold;
            button.Width = 16;
            button.Height = 16;
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

            // Get an empty document
            var newDocument = this.Settings.CreateEmpty();

            var extent = new XmlExtent(newDocument, this.Settings.ProjectExtent.ContextURI());
            extent.Settings = this.Settings.ExtentSettings;
            this.Settings.ProjectExtent = extent;
            this.Settings.Pool.Add(extent, null, "DatenMeister Data");

            // Refreshes the view
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
            var loadedFile = XDocument.Load(filename);

            // Loads the extent into the same uri
            var extent = new XmlExtent(loadedFile, this.Settings.ProjectExtent.ContextURI());

            // Sets the settings and stores it into the main window. The old one gets removed
            extent.Settings = this.Settings.ExtentSettings;
            this.Settings.ProjectExtent = extent;
            this.Settings.Pool.Add(extent, filename, ExtentNames.DataExtent);

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
        private void SaveChanges()
        {
            if (this.pathOfDataExtent == null)
            {
                this.SaveChangesAs();
                return;
            }
            else
            {
                var xmlExtent = (this.Settings.ProjectExtent) as XmlExtent;
                Ensure.That(xmlExtent != null);

                // Stores the xml document
                xmlExtent.XmlDocument.Save(this.pathOfDataExtent);

                // Adds the file to the recent files
                this.AddRecentFile(this.pathOfDataExtent);

                MessageBox.Show(this, Localization_DatenMeister_WPF.ChangeHasBeenSaved);

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
                var xmlExtent = (this.Settings.ProjectExtent) as XmlExtent;
                Ensure.That(xmlExtent != null);

                // Stores the xml document
                var filename = dialog.FileName;
                xmlExtent.XmlDocument.Save(filename);
                this.pathOfDataExtent = filename;

                // Adds the file to the recent files
                this.AddRecentFile(filename);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Adds the filepath to the list of recent files
        /// </summary>
        /// <param name="filePath"></param>
        public void AddRecentFile(string filePath)
        {
            RecentFileIntegration.AddRecentFile(
                this.Core,
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
            if (!this.Settings.ProjectExtent.IsDirty)
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
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = Localization_DatenMeister_WPF.File_Filter;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog(this) == true)
            {
                var xmlExtent = (this.Settings.ProjectExtent) as XmlExtent;

                // Prepare extent, receiving the copy
                var copiedExtent = new XmlExtent(
                    XDocument.Parse("<export />"),
                    xmlExtent.Uri,
                    xmlExtent.Settings);
                var pool = new DatenMeisterPool();
                pool.Add(copiedExtent, null);

                // Initialize database
                if (xmlExtent.Settings != null && xmlExtent.Settings.InitDatabaseFunction != null)
                {
                    xmlExtent.Settings.InitDatabaseFunction(copiedExtent.XmlDocument);
                }

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
        }

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

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new AboutDialog();
            dlg.ShowDialog();
        }
    }
}
