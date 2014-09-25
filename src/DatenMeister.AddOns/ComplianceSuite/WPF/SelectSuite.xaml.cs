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

namespace DatenMeister.AddOns.ComplianceSuite.WPF
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SelectSuite : Window
    {
        public SelectSuite()
        {
            InitializeComponent();
        }

        public Tests GetSuite()
        {
            if (this.radioGeneric.IsChecked == true)
            {
                return Tests.Generic;
            }

            if (this.radioXml.IsChecked == true)
            {
                return Tests.Xml;
            }

            return null;
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult  = true;
            this.Close();
        }
    }
}
