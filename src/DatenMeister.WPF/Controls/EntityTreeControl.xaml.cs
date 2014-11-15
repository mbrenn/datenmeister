using BurnSystems.Test;
using DatenMeister.Logic;
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
    /// Interaction logic for EntityTreeControl.xaml
    /// </summary>
    public partial class EntityTreeControl : UserControl, IListLayout
    {
        /// <summary>
        /// Stores the items that shall be shown
        /// </summary>
        public TreeLayoutConfiguration Configuration
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the EntityTreeControl
        /// </summary>
        public EntityTreeControl()
        {
            this.InitializeComponent();
        }

        public EntityTreeControl(TreeLayoutConfiguration configuration)
        {
            this.Configuration = configuration;
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets all the elements being returned by the elements factory
        /// </summary>
        /// <returns>Collection, containing all the elements</returns>
        private IReflectiveCollection GetElements()
        {
            var pool = Injection.Application.Get<IPool>();
            Ensure.That(this.Configuration.ElementsFactory != null, "No Elementsfactory is set");

            var elements = this.Configuration.ElementsFactory(pool);
            return elements;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.RefreshItems();
        }

        public void RefreshItems()
        {
            var elements = GetElements();

            this.treeView.ItemsSource = elements.Select(x => this.ConvertToTreeViewItem(x.AsIObject()));
        }

        /// <summary>
        /// Converts the given item to a treeview item, which will be shown in the treeview 
        /// </summary>
        /// <param name="item">Item to be converted</param>
        /// <returns>The treeview item being used</returns>
        private TreeViewItem ConvertToTreeViewItem(IObject item, List<IObject> alreadyCovered = null)
        {
            if ( alreadyCovered == null )
            {
                alreadyCovered = new List<IObject>();
            }

            var treeViewItem = new TreeViewItem();
            treeViewItem.Header = new ObjectDictionaryForView(item)["name"];

            foreach (var pair in item.getAll().ToList())
            {
                var tempList = new List<TreeViewItem>();
                var asEnumeration = pair.Value.AsEnumeration();
                foreach (var subItem in asEnumeration.Where(x => x is IObject).Select(x => x.AsIObject()))
                {
                    if (!alreadyCovered.Any(x => subItem == x))
                    {
                        var treeSubItem = this.ConvertToTreeViewItem(subItem, alreadyCovered);
                        tempList.Add(treeSubItem);
                    }
                }

                if (tempList.Count > 0)
                {
                    var propertyItem = new TreeViewItem()
                    {
                        Header = pair.PropertyName
                    };

                    foreach (var temp in tempList)
                    {
                        propertyItem.Items.Add(temp);
                    }

                    treeViewItem.Items.Add(propertyItem);
                }
            }

            return treeViewItem;
        }

        public void GiveFocusToGridContent()
        {
        }

        /// <summary>
        /// Defines what shall happen, when the user doubleclicks on a specific element
        /// </summary>
        public Action<DetailOpenEventArgs> OpenSelectedViewFunc
        {
            get;
            set;
        }
    }
}
