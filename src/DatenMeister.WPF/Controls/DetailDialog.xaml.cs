using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.Logic.Views;
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
    /// Interaction logic for DetailDialog.xaml
    /// </summary>
    public partial class DetailDialog : Window
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(DetailDialog));

        private FormLayoutConfiguration configuration
        {
            get;
            set;
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

        public DetailDialog(FormLayoutConfiguration configuration)
            : this()
        {
            this.Configure(configuration);
        }

        /// <summary>
        /// Performs the configuration of the form. 
        /// The form is sent to the detailForm
        /// </summary>
        /// <param name="configuration">Configuration to be used</param>
        public void Configure(FormLayoutConfiguration configuration)
        {
            this.configuration = configuration;
            this.detailForm.Configure(configuration);            
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
                if (this.configuration.EditMode == EditMode.New)
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
        public static DetailDialog ShowDialogToCreateTypeOf(
            IObject type, 
            IReflectiveCollection collection, 
            IPublicDatenMeisterSettings settings,
            IObject viewData = null)
        {
            Ensure.That(collection != null);
            Ensure.That(settings != null);

            var extent = collection.Extent;
            Ensure.That(collection.Extent != null);
            Ensure.That(type != null, "No Type has been set that can be used to create a new object");

            var temp = new GenericElement(extent: extent, type: type);
            viewData = GetView(temp, viewData);

            if (viewData == null)
            {
                return null;
            }

            var configuration = new FormLayoutConfiguration();
            configuration.EditMode = EditMode.New;
            configuration.FormViewInfo = viewData;
            configuration.StorageCollection = collection;
            configuration.TypeToCreate = type;
            configuration.Settings = settings;

            var dialog = new DetailDialog(configuration);
            dialog.Show();

            return dialog;
        }

        /// <summary>
        /// Has to be called, when the dialog for an entity shall be shown
        /// </summary>
        /// <param name="value"></param>
        public static DetailDialog ShowDialogFor(
            IObject value,
            IPublicDatenMeisterSettings settings,
            IObject viewData = null, 
            bool readOnly = false)
        {
            Ensure.That(settings != null);

            viewData = GetView(value, viewData);
            if (viewData == null)
            {
                return null;
            }

            // Creates the dialog
            var configuration = new FormLayoutConfiguration();
            configuration.EditMode = readOnly ? EditMode.Read : EditMode.Edit;
            configuration.FormViewInfo = viewData;
            configuration.StorageCollection = value.Extent == null ? null : value.Extent.Elements();
            configuration.DetailObject = value;
            configuration.Settings = settings;
            var dialog = new DetailDialog(configuration);
            dialog.Show();

            return dialog;
        }

        private static IObject GetView(IObject value, IObject viewData)
        {
            // If viewdata is already given, then we do not need to find out the correct view data
            if (viewData == null)
            {
                var viewManager = Injection.Application.Get<IViewManager>();

                if (viewManager == null)
                {
                    logger.Message("No ViewManager for object, so no detail view");
                }
                else
                {
                    // Gets the default view for the object
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

