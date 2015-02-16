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

namespace DatenMeister.WPF.Controls.GuiElements.Elements
{
    /// <summary>
    /// Interaction logic for WpfSubElementList.xaml
    /// </summary>
    public partial class WpfSubElementListControl : UserControl
    {
        public WpfSubElementListControl()
        {
            InitializeComponent();
        }

        public void Configure(TableLayoutConfiguration configuration)
        {
            this.tableControl.Configure(configuration);
        }
    }
}
