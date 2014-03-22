using DatenMeister.Entities.AsObject.FieldInfo;
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
    /// Interaktionslogik für ObjectForm.xaml
    /// </summary>
    public partial class EntityFormControl : UserControl
    {
        #region Event handlers

        private event EventHandler cancelled;
        private event EventHandler accepted;

        public event EventHandler Cancelled
        {
            add { this.cancelled += value; }
            remove { this.cancelled -= value; }
        }

        public event EventHandler Accepted
        {
            add { this.accepted += value; }
            remove { this.accepted -= value; }
        }

        #endregion

        /// <summary>
        /// Gets or sets the edit mode of the form
        /// </summary>
        public EditMode EditMode
        {
            get;
            set;
        }

        public EntityFormControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stores the form view being used
        /// </summary>
        private FormView formView;

        /// <summary>
        /// Gets or sets the object 
        /// </summary>
        public IObject DetailObject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the table view info
        /// </summary>
        public IObject FormViewInfo
        {
            get { return this.formView.Value; }
            set
            {
                if (value == null)
                {
                    this.formView = null;
                }
                else
                {
                    this.formView = new FormView(value);
                    this.Relayout();
                }
            }
        }

        /// <summary>
        /// Does the relayout
        /// </summary>
        private void Relayout()
        {
            if (this.formView != null)
            {
                var fieldInfos = this.formView.getFieldInfos();

                var currentRow = 0;
                foreach (var fieldInfo in fieldInfos.Cast<IObject>())
                {
                    var name = (fieldInfo.get("title") ?? "").ToString();
                    if (string.IsNullOrEmpty(name))
                    {
                        name = "Unknown";
                    }

                    var nameLabel = new Label();
                    nameLabel.Content = name;
                    Grid.SetRow(nameLabel, currentRow);

                    formGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    formGrid.Children.Add(nameLabel);

                    currentRow++;
                }
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            var ev = this.accepted;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var ev = this.cancelled;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }
    }
}