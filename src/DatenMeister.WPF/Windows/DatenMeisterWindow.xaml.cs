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

        public Action<DatenMeisterWindow> OnInitializeDatabase
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the title of the application
        /// </summary>
        /// <param name="title">Title of the application</param>
        public void SetTitle(string title)
        {
            this.Title = title;
        }

        public void AddExtentView(string name, AddExtentParameters parameters)
        {
            var tab = new TabItem();
            tab.Header = name;

            var entityList = new EntityTableControl();
            entityList.MainWindow = this;
            entityList.ExtentFactory = parameters.ExtentFactory;
            entityList.TableViewInfo = parameters.TableViewInfo;
            entityList.DetailViewInfo = parameters.DetailViewInfo;
            entityList.ElementFactory = parameters.ElementFactory;

            var grid = new Grid();
            grid.Children.Add(entityList);
            tab.Content = grid;

            this.tabMain.Items.Add(tab);

            this.listTabs.Add(new TabInformation()
                {
                    Name = name,
                    Parameters = parameters,
                    TabItem = tab,
                    TableControl = entityList
                });
        }

        private void RefreshAllViews()
        {
            foreach (var tab in this.listTabs)
            {
                tab.TableControl.RefreshItems();
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (!this.DoesUserAcceptsDataLoss())
            {
                return;
            }

            // Get an empty document
            var newDocument = this.Settings.CreateEmpty();
            
            var extent = new XmlExtent(newDocument, this.Settings.ProjectExtent.ContextURI());
            extent.Settings = this.Settings.ExtentSettings;
            this.Settings.ProjectExtent = extent;
            this.Settings.Pool.Add(extent, null);

            // Refreshes the view
            this.RefreshAllViews();
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
                this.RefreshAllViews();
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
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Does the user accept the data loss? 
        /// </summary>
        /// <returns>true, if user accepts the data loss</returns>
        [Obsolete]
        private bool DoesUserAcceptsDataLoss()
        {
            if (!this.Settings.ProjectExtent.IsDirty)
            {
                // Content is not dirty, user will accept no loss
                return true;
            }

            if (MessageBox.Show(
                   this,
                   Localization_DatenMeister_WPF.ChangesMayBeLost,
                   Localization_DatenMeister_WPF.CloseApplication,
                   MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Content is dirty and user accepts it
                return true;
            }

            // Content is dirty and user does not accept it
            return false;
        }

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
            if (!this.DoesUserAcceptsDataLoss())
            {
                e.Cancel = true;
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
    }
}
