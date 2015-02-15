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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatenMeister.WPF.Windows.Controls
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class InlineMessageBox : UserControl
    {
        public InlineMessageBox()
        {
            InitializeComponent();
        }

        public string MessageText
        {
            get { return this.txtMessageText.Text; }
            set { this.txtMessageText.Text = value; }
        }
        
        /// <summary>
        /// Shows a Messagebox for the given time and duration and then fades it out
        /// </summary>
        /// <param name="panel">Panel, where the message box will be shown</param>
        /// <param name="text">Text being shown</param>
        /// <param name="duration">Duration before fading out starts</param>
        public static void ShowMessageBox(Panel panel, string text, TimeSpan? duration = null)
        {
            if (!duration.HasValue)
            {
                duration = TimeSpan.FromSeconds(2);
            }

            // Creates the message box itself
            var element = new InlineMessageBox();
            element.MessageText = text;
            panel.Children.Add(element);
            element.MaxWidth = panel.ActualWidth / 2;

            // Defines the fade out animation
            var a = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                FillBehavior = FillBehavior.Stop,
                BeginTime = duration.Value,
                Duration = new Duration(TimeSpan.FromSeconds(0.5))
            };
            var storyboard = new Storyboard();

            storyboard.Children.Add(a);
            Storyboard.SetTarget(a, element);
            Storyboard.SetTargetProperty(a, new PropertyPath(OpacityProperty));
            storyboard.Completed += delegate
            {
                panel.Children.Remove(element);
            };

            storyboard.Begin();
        }
    }
}
