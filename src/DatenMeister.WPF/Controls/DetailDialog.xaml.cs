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
        /// <summary>
        /// Gets or sets the pool
        /// </summary>
        public IPool Pool
        {
            get { return this.detailForm.Pool; }
            set { this.detailForm.Pool = value; }
        }

        public EntityFormControl DetailForm
        {
            get
            {
                return this.detailForm;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.UpdateWindowTitle();
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

        /// <summary>
        /// Updates the title of the window
        /// </summary>
        public void UpdateWindowTitle()
        {
            if (this.detailForm == null)
            {
                this.Title = "Detail Item View";
            }
            else
            {
                if (this.detailForm.EditMode == EditMode.New)
                {
                    this.Title = "New Item";
                }
                else
                {
                    this.Title = "Edit Item";
                }
            }
        }
    }
}
