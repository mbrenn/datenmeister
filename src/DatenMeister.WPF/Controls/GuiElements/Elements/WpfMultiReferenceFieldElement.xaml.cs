using BurnSystems.Test;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Pool;
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

namespace DatenMeister.WPF.Controls.GuiElements.Elements
{
    /// <summary>
    /// Interaction logic for WpfMultiReferenceFieldElement.xaml
    /// </summary>
    public partial class WpfMultiReferenceFieldElement : UserControl
    {
        /// <summary>
        /// Stores the field
        /// </summary>
        private WpfMultiReferenceField field;

        /// <summary>
        /// Initializes a new instance of the WpfMultiReferenceFieldElement class
        /// </summary>
        public WpfMultiReferenceFieldElement()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the WpfMultiReferenceFieldElement class
        /// </summary>
        /// <param name="field">Field to be set</param>
        public WpfMultiReferenceFieldElement(WpfMultiReferenceField field)
        {
            this.field = field;
            this.InitializeComponent();
        }

        public void RefreshData()
        {
            var collection = this.GetObjectProperty();
            if (collection == null || collection.Count == 0)
            {
                this.SetStatus(Status.HasNoElements);
            }
            else
            {
                this.SetStatus(Status.HasElements);

                // Shows the elements
                this.listElements.ItemsSource =
                    collection.Select(x =>
                    {
                        var asObject = x.AsIObject();
                        var text = asObject.getAsSingle(this.field.FieldInfo.getPropertyValue()).ToString();
                        return new ElementInList(asObject, text);
                    });
            }
        }

        /// <summary>
        /// Gets the object property, being set by this element
        /// </summary>
        /// <returns></returns>
        public IReflectiveCollection GetObjectProperty()
        {
            if ( this.field.DetailObject == null)
            {
                return null;
            }

            return this.field.DetailObject.getAsReflectiveSequence(this.field.FieldInfo.getBinding());
        }

        /// <summary>
        /// Gets all reference objects that the user can select
        /// </summary>
        /// <returns>Enumeration of the referenced object</returns>
        public IReflectiveCollection GetReferenceObjects()
        {
            var poolResolver = Injection.Application.Get<IPoolResolver>();
            Ensure.That(poolResolver != null);
            return ObjectConversion.ToReflectiveCollection(poolResolver.Resolve(
                    this.field.FieldInfo.getReferenceUrl(), 
                    this.field.DetailObject));
        }

        /// <summary>
        /// Sets the status and shows or hides the necessary elements
        /// </summary>
        /// <param name="status"></param>
        public void SetStatus(Status status)
        {
            switch (status)
            {
                case Status.HasNoElements:
                    this.panelNoElements.Visibility = System.Windows.Visibility.Visible;
                    this.panelHasElements.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case Status.HasElements:
                    this.panelNoElements.Visibility = System.Windows.Visibility.Collapsed;
                    this.panelHasElements.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    throw new NotImplementedException("Unknown status");
            }
        }

        /// <summary>
        /// Stores the possible status values of this element
        /// </summary>
        public enum Status
        {
            HasNoElements,
            HasElements
        }

        private class ElementInList
        {
            public IObject Object
            {
                get;
                set;
            }

            public string Text
            {
                get;
                set;
            }

            public ElementInList(IObject obj, string text)
            {
                this.Object = obj;
                this.Text = text;
            }

            public override string ToString()
            {
                return this.Text;
            }
        }

        private void btnAddElement_Click(object sender, RoutedEventArgs e)
        {
            var configuration = new TableLayoutConfiguration()
            {
                LayoutInfo = MultiReferenceField.getTableViewInfo(this.field.FieldInfo)
            };
            configuration.SetElements(this.GetReferenceObjects());

            var listForm = new SelectionListDialog(configuration);
            listForm.ShowDialog();

            if (listForm.DialogResult == true)
            {
                // Add the entities
                foreach (var item in listForm.SelectedElements)
                {
                    this.GetObjectProperty().add(item);
                }

                this.RefreshData();
            }
        }

        private void btnDeleteElement_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.listElements.SelectedItems.Cast<ElementInList>().Select(x => x.Object))
            {
                this.GetObjectProperty().remove(item);
            }

            this.RefreshData();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.GetObjectProperty().clear();
            this.RefreshData();
        }
    }
}
