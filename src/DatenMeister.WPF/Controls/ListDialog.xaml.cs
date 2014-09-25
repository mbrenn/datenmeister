using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic.Views;
using DatenMeister.WPF.Windows;
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
    /// Interaction logic for ListDialog.xaml
    /// </summary>
    public partial class ListDialog : Window
    {
        /// <summary>
        /// Gets or sets the view information
        /// </summary>
        public TableView ViewInformation
        {
            get;
            private set;
        }

        /// <summary>
        /// Stores the selected elements, when the user clicked on 
        /// the "OK" Button
        /// </summary>
        public IEnumerable<IObject> SelectedElements
        {
            get { return this.Table.SelectedElements; }
        }

        /// <summary>
        /// Initializes a new instance of the ListDialog class
        /// </summary>
        public ListDialog()
        {
            this.InitializeComponent();

            var value = new GenericObject();
            this.Table.TableViewInfo = value;
            this.ViewInformation = new TableView(value);

            WindowFactory.AutosetWindowSize(this, 0.88);
        }

        /// <summary>
        /// Initializes a new instance of the ListDialog class
        /// </summary>
        /// <param name="elements">Elements to be shown</param>
        /// <param name="settings">Settings for the list</param>
        /// <param name="tableView">View being used for the objects</param>
        public ListDialog(
            IReflectiveCollection elements,
            IPublicDatenMeisterSettings settings,
            IObject tableView)
            : this()
        {
            Ensure.That(elements != null);
            Ensure.That(settings != null);
            Ensure.That(tableView != null);

            this.Table.TableViewInfo = tableView;
            this.SetReflectiveCollection(elements, settings, false);
        }

        /// <summary>
        /// Sets the reflective collection
        /// </summary>
        /// <param name="elementsFactory">The element factory being used 
        /// to retrieve the elements which shall be shown</param>
        /// <param name="settings">The settings to be used</param>
        /// <param name="doAutoGenerateView">true, if the columns shall be automatically generated</param>
        public void SetReflectiveCollection(
            Func<IPool, IReflectiveCollection> elementsFactory, 
            IPublicDatenMeisterSettings settings,
            bool doAutoGenerateView = true)
        {
            var pool = Injection.Application.Get<IPool>();

            if (doAutoGenerateView)
            {
                ViewHelper.AutoGenerateViewDefinition(elementsFactory(pool), this.Table.TableViewInfo, true);
            }

            this.Table.Settings = settings;
            this.Table.ElementsFactory = elementsFactory;
        }

        /// <summary>
        /// Sets the reflective collection
        /// </summary>
        /// <param name="elements">The element being shown</param>
        /// <param name="settings">The settings to be used</param>
        /// <param name="doAutoGenerateView">true, if the columns shall be automatically generated</param>
        public void SetReflectiveCollection(
            IReflectiveCollection elements,
            IPublicDatenMeisterSettings settings,
            bool doAutoGenerateView = true)
        {
            if (doAutoGenerateView)
            {
                ViewHelper.AutoGenerateViewDefinition(elements, this.Table.TableViewInfo, true);
            }

            this.Table.Settings = settings;
            this.Table.ElementsFactory = (x) => elements;

            this.Table.Relayout();
        }

        private void Table_OkClicked(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }

        private void Table_CancelClicked(object sender, EventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
