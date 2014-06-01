using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.Logic.Views;
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
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(DetailDialog));

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

        /// <summary>
        /// Selects the field with the given name
        /// </summary>
        /// <param name="name">Name of the field, that should receive the focus</param>
        public void SelectFieldWithName(string name)
        {
            if (this.detailForm != null)
            {
                this.detailForm.FocusFieldWithName(name);
            }
        }

        /// <summary>
        /// Creates the dialog, when user shall define the properties of a new item. 
        /// </summary>
        /// <param name="type">Type of object to be created</param>
        /// <param name="viewData">Available view</param>
        /// <returns>The created type</returns>
        public static DetailDialog ShowDialogToCreateTypeOf(IObject type, IURIExtent extent, IObject viewData = null)
        {
            Ensure.That(type != null, "No Type has ben set that can be used to create a new object");

            var temp = new GenericElement(extent: extent, type: type);
            viewData = GetView(temp, viewData);

            if (viewData == null)
            {
                return null;
            }

            var dialog = new DetailDialog();
            dialog.Pool = extent.Pool;
            dialog.DetailForm.EditMode = EditMode.New;
            dialog.DetailForm.FormViewInfo = viewData;
            dialog.DetailForm.Extent = extent;
            dialog.DetailForm.TypeToCreate = type;
            dialog.Show();

            return dialog;
        }

        /// <summary>
        /// Has to be called, when the dialog for an entity shall be shown
        /// </summary>
        /// <param name="value"></param>
        public static DetailDialog ShowDialogFor(IObject value, IObject viewData = null, bool readOnly = false)
        {
            viewData = GetView(value, viewData);

            if (viewData == null)
            {
                return null;
            }

            var dialog = new DetailDialog();
            dialog.Pool = value.Extent.Pool;
            dialog.DetailForm.EditMode = readOnly ? EditMode.Read : EditMode.Edit;
            dialog.DetailForm.FormViewInfo = viewData;
            dialog.DetailForm.Extent = value.Extent;
            dialog.DetailForm.DetailObject = value;
            dialog.Show();

            return dialog;
        }

        private static IObject GetView(IObject value, IObject viewData)
        {
            if (viewData == null)
            {
                var viewManager = Global.Application.Get<IViewManager>();

                if (viewManager == null)
                {
                    logger.Message("No ViewManager for object, so no detail view");
                }
                else
                {
                    viewData = viewManager.GetDefaultView(value, ViewType.FormView);
                    if (viewData == null)
                    {
                        logger.Message("No default view had been given.");
                    }
                }
            }
            return viewData;
        }
    }
}
