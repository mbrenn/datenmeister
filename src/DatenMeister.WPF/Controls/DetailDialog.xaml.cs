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
    /// Interaction logic for DetailDialog.xaml
    /// </summary>
    public partial class DetailDialog : Window
    {
        public EntityFormControl DetailForm
        {
            get { return this.detailForm; }
        }

        public DetailDialog()
        {
            InitializeComponent();
        }

        private void detailForm_Accepted(object sender, EventArgs e)
        {
            this.Close();
        }

        private void detailForm_Cancelled(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
