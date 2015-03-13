using DatenMeister.DataProvider.Wrapper;
using DatenMeister.DataProvider.Xml;
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
using System.Windows.Threading;

namespace DatenMeister.WPF.Windows.Addons
{
    /// <summary>
    /// Interaction logic for XmlExtentViewer.xaml
    /// </summary>
    public partial class XmlExtentViewer : Window
    {
        public XmlExtentViewer()
        {
            InitializeComponent();
        }

        public IURIExtent ViewedExtent
        {
            get;
            set;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ViewedExtent == null)
            {
                MessageBox.Show("No extent is given");
                this.Close();
                return;
            }
            var viewedExtent = this.ViewedExtent;
            this.SetExtent(viewedExtent);
        }

        private void SetExtent(IURIExtent viewedExtent)
        {
            var wrapperExtent = viewedExtent as IWrapperExtent;
            if (wrapperExtent != null)
            {
                this.SetExtent(wrapperExtent.Unwrap());
                return;
            }

            var xmlExtent = viewedExtent as XmlExtent;
            if (xmlExtent == null)
            {
                MessageBox.Show("Only Xml Extents can be shown here");
                this.Close();
                return;
            }

            this.RefreshContent(xmlExtent);

            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background, this.Dispatcher);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += (x, y) => this.RefreshContent(xmlExtent);
            dispatcherTimer.Start();
        }

        private void RefreshContent(XmlExtent extent)
        {
            this.txtContent.Text = extent.XmlDocument.ToString();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
