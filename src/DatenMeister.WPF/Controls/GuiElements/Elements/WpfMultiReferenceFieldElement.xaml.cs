using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using DatenMeister.Pool;
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
                        return asObject.get(this.field.FieldInfo.getPropertyValue()).AsSingle().ToString();
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

            return this.field.DetailObject.get(this.field.FieldInfo.getBinding()).AsReflectiveCollection();
        }

        /// <summary>
        /// Gets all reference objects that the user can select
        /// </summary>
        /// <returns>Enumeration of the referenced object</returns>
        public IReflectiveCollection GetReferenceObjects()
        {
            var poolResolver = Global.Application.Get<IPoolResolver>();
            Ensure.That(poolResolver != null);
            return poolResolver.Resolve(this.field.FieldInfo.getReferenceUrl(), this.field.DetailObject).AsReflectiveCollection();
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
            var listForm = new ListDialog();
            listForm.Show();
            listForm.SetReflectiveCollection(this.GetReferenceObjects(), this.field.State.Settings);

            if (listForm.DialogResult == true)
            {
                // Add the entities

            }
        }

        private void btnDeleteElement_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
