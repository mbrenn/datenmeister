using BurnSystems.Test;
using DatenMeister.DataProvider.Xml;
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
        /// Stores the current path
        /// </summary>
        private string currentPath = null;

        /// <summary>
        /// Stores the database to be used for the project
        /// </summary>
        //private Database database = new Database();

        public DatenMeisterWindow()
        {
            this.InitializeComponent();
        }

        public Action<DatenMeisterWindow> OnInitializeDatabase
        {
            get;
            set;
        }

        public void AddExtent(string name, AddExtentParameters parameters)
        {
            var tab = new TabItem();
            tab.Header = name;

            var entityList = new EntityTableControl();
            entityList.ExtentFactory = parameters.ExtentFactory;
            entityList.TableViewInfo = parameters.TableViewInfo;
            entityList.DetailViewInfo = parameters.DetailViewInfo;
            entityList.ElementFactory = parameters.ElementFactory;

            var grid = new Grid();
            grid.Children.Add(entityList);
            tab.Content = grid;

            this.tabMain.Items.Add(tab);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if (dialog.ShowDialog(this) == true)
            {
                /*var loadedFile = XDocument.Load(dialog.FileName);
                var extent = new XmlExtent(loadedFile, Database.uri);
                this.database.ReplaceDatabase(extent);

                this.tablePersons.RefreshItems();
                this.tableTasks.RefreshItems();*/
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentPath == null)
            {
                this.SaveAs_Click(sender, e);
                return;
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new Microsoft.Win32.SaveFileDialog();
            if (dialog.ShowDialog(this) == true)
            {
                /*var xmlExtent = (this.database.ProjectExtent) as XmlExtent;
                Ensure.That(xmlExtent != null);

                // Stores the xml document
                xmlExtent.XmlDocument.Save(dialog.FileName);*/
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show(
                this, 
                Localization_DatenMeister_WPF.ChangesMayBeLost,
                Localization_DatenMeister_WPF.CloseApplication, 
                MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

    }
}
