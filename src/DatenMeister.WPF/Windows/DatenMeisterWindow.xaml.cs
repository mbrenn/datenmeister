using BurnSystems.Test;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Transformations;
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
    public partial class DatenMeisterWindow : Window
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

        private void Window_Initialized(object sender, EventArgs e)
        {
            /*
            var umlTypes = DatenMeister.Entities.AsObject.Uml.Types.Init();
            var fieldInfoTypes = DatenMeister.Entities.AsObject.FieldInfo.Types.Init();

            // Initializes the database itself
            this.database.Init();

            // Create some persons
            var person = database.ProjectExtent.CreateObject(Database.Types.Person);
            person.set("name", "Martin Brenn");
            person.set("email", "brenn@depon.net");
            person.set("phone", "0151/560");
            person.set("title", "Project Lead");

            person = database.ProjectExtent.CreateObject(Database.Types.Person);
            person.set("name", "Martina Brenn");
            person.set("email", "brenna@depon.net");
            person.set("phone", "0151/650");
            person.set("title", "Project Support");

            person = database.ProjectExtent.CreateObject(Database.Types.Task);
            person.set("name", "My First Task");
            person.set("startdate", DateTime.Now);
            person.set("enddate", DateTime.Now.AddYears(1));
            person.set("finished", false);

            // Initializes the views
            this.tablePersons.ExtentFactory = () => this.database.ProjectExtent.FilterByType(Database.Types.Person);
            this.tablePersons.TableViewInfo = Database.Views.PersonTable;
            this.tablePersons.DetailViewInfo = Database.Views.PersonDetail;
            this.tablePersons.ElementFactory = () => database.ProjectExtent.CreateObject(Database.Types.Person);
            this.tableTasks.ExtentFactory = () => this.database.ProjectExtent.FilterByType(Database.Types.Task);
            this.tableTasks.TableViewInfo = Database.Views.TaskTable;
            this.tableTasks.DetailViewInfo = Database.Views.TaskDetail;
            this.tableTasks.ElementFactory = () => database.ProjectExtent.CreateObject(Database.Types.Task);*/
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
