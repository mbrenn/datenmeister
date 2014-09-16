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

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Interaction logic for ExceptionInfoWindow.xaml
    /// </summary>
    public partial class ExceptionInfoWindow : Window
    {
        public ExceptionInfoWindow()
        {
            InitializeComponent();
        }

        public void SetException(Exception exc)
        {
            this.txtExceptionText.Text = exc.ToString();

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.txtExceptionText.Text);
            MessageBox.Show(Localization_DatenMeister_WPF.ExceptionToClipboard);
        }
    }
}
