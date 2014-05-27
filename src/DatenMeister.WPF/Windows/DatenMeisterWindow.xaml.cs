using BurnSystems.Logging;
using BurnSystems.Test;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
using DatenMeister.Transformations;
using DatenMeister.WPF.Controls;
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
using System.Xml.Linq;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class DatenMeisterWindow : Window, IDatenMeisterWindow
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ClassLogger logger = new ClassLogger(typeof(DatenMeisterWindow));

        /// <summary>
        /// Gets or sets the datenmeister settings
        /// </summary>
        public IDatenMeisterSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the current path
        /// </summary>
        private string currentPath = null;

        /// <summary>
        /// Stores the information
        /// </summary>
        private List<TabInformation> listTabs = new List<TabInformation>();

        public DatenMeisterWindow()
        {
            this.InitializeComponent();

            var width = this.Width;
            var height = this.Height;

            var newWidth = System.Windows.SystemParameters.PrimaryScreenWidth / 2;
            var newHeight = System.Windows.SystemParameters.PrimaryScreenHeight / 2;

            this.Left -= newWidth / 2;
            this.Top -= newHeight/ 2;
            this.Width = newWidth;
            this.Height = newHeight;
        }

        /// <summary>
        /// Sets the title of the application
        /// </summary>
        /// <param name="title">Title of the application</param>
        public void SetTitle(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Adds a menuentry to the application window
        /// </summary>
        /// <param name="menuHeadline">Headline of the menu</param>
        /// <param name="menuLine">Menu that shall be added. This tab is required to have the click event already associated</param>
        public void AddMenuEntry(string menuHeadline, MenuItem menuLine)
        {
            // Search for a menuitem with the same name
            MenuItem found = null;
            foreach (var item in this.menuMain.Items)
            {
                var menuItem = item as MenuItem;
                if (menuItem.Header.ToString() == menuHeadline)
                {
                    found = menuItem;
                }
            }

            // Not found, create a new item
            if (found == null)
            {
                found = new MenuItem()
                {
                    Header = menuHeadline
                };

                this.menuMain.Items.Add(found);
            }

            // Now create the item
            found.Items.Add(menuLine);
        }

        public void AssociateDetailOpenEvent(IObject view, Action<DetailOpenEventArgs> action)
        {
            var found = this.listTabs.Where(x => x.TableViewInfo == view).FirstOrDefault();
            if (found == null)
            {
                logger.Message("Associate Detail Open Event failed because tab was not found");
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
        public void RefreshTab()
        {
            Ensure.That(this.Settings.ViewExtent != null, "No view extent has been given");

            var filteredViewExtent = 
                this.Settings.ViewExtent.FilterByType(DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);

            var elements = new List<IObject>();
            var first = true;

            // Goes through each tableinfo
            foreach (var tableInfo in filteredViewExtent.Elements())
            {
                var tableInfoObj = tableInfo.AsIObject();
                elements.Add(tableInfoObj);

                // Check, if there is already a tab, which hosts the tableInfo
                if (this.listTabs.Any(x => x.TableViewInfo == tableInfo))
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
                entityList.MainWindow = this;
                entityList.ExtentFactory = (x) =>
                    {
                        return this.Settings.Pool.ResolveByPath(extentUri) as IURIExtent;
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
                if (!elements.Any(x => x == listTab.TableViewInfo))
                {
                    this.listTabs.Remove(listTab);
                    this.tabMain.Items.Remove(listTab.TabItem);
                }
            }
        }

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
                this.RefreshTab();
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
            if (this.DoesUserWantsToSaveData())
            {
                this.SaveChanges();
            }

            // Get an empty document
            var newDocument = this.Settings.CreateEmpty();
            
            var extent = new XmlExtent(newDocument, this.Settings.ProjectExtent.ContextURI());
            extent.Settings = this.Settings.ExtentSettings;
            this.Settings.ProjectExtent = extent;
            this.Settings.Pool.Add(extent, null);

            // Refreshes the view
            this.RefreshAllTabContent();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = Localization_DatenMeister_WPF.File_Filter;
            if (dialog.ShowDialog(this) == true)
            {
                var loadedFile = XDocument.Load(dialog.FileName);

                // Loads the extent into the same uri
                var extent = new XmlExtent(loadedFile, this.Settings.ProjectExtent.ContextURI());

                // Sets the settings and stores it into the main window. The old one gets removed
                extent.Settings = this.Settings.ExtentSettings;
                this.Settings.ProjectExtent = extent;
                this.Settings.Pool.Add(extent, dialog.FileName);

                // Refreshes all views
                this.RefreshAllTabContent();
            }
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
            if (this.currentPath == null)
            {
                this.SaveChangesAs();
                return;
            }
            else
            {
                var xmlExtent = (this.Settings.ProjectExtent) as XmlExtent;
                Ensure.That(xmlExtent != null);

                // Stores the xml document
                xmlExtent.XmlDocument.Save(this.currentPath);

                MessageBox.Show(this, Localization_DatenMeister_WPF.ChangeHasBeenSaved);
            }
        }

        /// <summary>
        /// Saves the changes and user may find a new path
        /// </summary>
        private void SaveChangesAs()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = Localization_DatenMeister_WPF.File_Filter;
            if (dialog.ShowDialog(this) == true)
            {
                var xmlExtent = (this.Settings.ProjectExtent) as XmlExtent;
                Ensure.That(xmlExtent != null);

                // Stores the xml document
                xmlExtent.XmlDocument.Save(dialog.FileName);
                this.currentPath = dialog.FileName;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        /// <summary>
        /// Does the user wants to save the data
        /// </summary>
        /// <returns>true, if user wants to save the data</returns>
        private bool DoesUserWantsToSaveData()
        {
            if (!this.Settings.ProjectExtent.IsDirty)
            {
                // Content is not dirty, user will accept that content is not stored
                return false;
            }

            if (MessageBox.Show(
                   this,
                   Localization_DatenMeister_WPF.QuestionSaveChanges,
                   Localization_DatenMeister_WPF.QuestionSaveChangesTitle,
                   MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Content is dirty and user wants to save 
                return true;
            }

            // Content is dirty and user does not want to save it
            return false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DoesUserWantsToSaveData())
            {
                this.SaveChanges();
            }
        }

        private void ExportAsXml_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Filter = Localization_DatenMeister_WPF.File_Filter;
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
    }
}
