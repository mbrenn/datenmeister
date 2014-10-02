using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic.Views;
using DatenMeister.WPF.Windows;
using Ninject;
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
    /// Interaction logic for ListDialog.xaml
    /// </summary>
    public partial class ListDialog : Window
    {
        /// <summary>
        /// Stores the selected elements, when the user clicked on 
        /// the "OK" Button
        /// </summary>
        public IEnumerable<IObject> SelectedElements
        {
            get { return this.Table.SelectedElements; }
        }

        /// <summary>
        /// Initializes a new instance of the ListDialog class
        /// </summary>
        public ListDialog()
        {
            this.InitializeComponent();

            // Sets the window size
            WindowFactory.AutosetWindowSize(this, 0.88);
        }

        /// <summary>
        /// Initializes a new instance of the ListDialog class.
        /// </summary>
        /// <param name="configuration"></param>
        public ListDialog(
            TableLayoutConfiguration configuration)
            : this()
        {
            this.Configure(configuration);
        }

        /// <summary>
        /// Sets the configuration for the table
        /// </summary>
        /// <param name="configuration"></param>
        public virtual void Configure(TableLayoutConfiguration configuration)
        {
            this.Table.Configure(configuration);
        }

        private void Table_OkClicked(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }

        private void Table_CancelClicked(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
