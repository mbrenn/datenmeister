using BurnSystems.ObjectActivation;
using DatenMeister.DataProvider;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic.Views;
using DatenMeister.WPF.Windows;
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

        public ListDialog()
        {
            this.InitializeComponent();

            var value = new GenericObject();
            this.Table.TableViewInfo = value;
            this.ViewInformation = new TableView(value);
            this.ViewInformation.setAllowDelete(false);
            this.ViewInformation.setAllowEdit(false);
            this.ViewInformation.setAllowNew(false);
            this.Table.UseAsSelectionControl = true;

            WindowFactory.AutosetWindowSize(this, 0.88);
        }

        public void SetReflectiveCollection(Func<IPool, IReflectiveCollection> elementsFactory, IPublicDatenMeisterSettings settings)
        {
            var pool = Global.Application.Get<IPool>();
            ViewHelper.AutoGenerateViewDefinition(elementsFactory(pool), this.Table.TableViewInfo, true);
            this.Table.Settings = settings;
            this.Table.ElementsFactory = elementsFactory;
        }

        public void SetReflectiveCollection(IReflectiveCollection elements, IPublicDatenMeisterSettings settings)
        {
            ViewHelper.AutoGenerateViewDefinition(elements, this.Table.TableViewInfo, true);
            this.Table.Settings = settings;
            this.Table.ElementsFactory = (x) => elements;

            this.Table.Relayout();
        }

        private void Table_OkClicked(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
