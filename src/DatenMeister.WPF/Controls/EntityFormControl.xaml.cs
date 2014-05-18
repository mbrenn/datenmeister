using BurnSystems.Test;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.WPF.Controls.GuiElements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class EntityFormControl : UserControl, IDataPresentationState
    {
        /// <summary>
        /// Stores the list of wpf elements
        /// </summary>
        private List<ElementCacheEntry> wpfElements = new List<ElementCacheEntry>();

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

        /// <summary>
        /// Defines the display mode
        /// </summary>
        public DisplayMode DisplayMode
        {
            get { return Controls.DisplayMode.Form; }
        }

        /// <summary>
        /// Gets or sets the element factory being used to create the element, 
        /// if we are in EditMode = New
        /// </summary>
        [Obsolete]
        public Func<IObject> ElementFactory
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

        public IPool Pool
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the extent, where this item is assigned to. 
        /// Relevant, if DetailObject == null and dialog has been opened to create a new 
        /// item. 
        /// </summary>
        public IURIExtent Extent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object 
        /// </summary>
        public IObject DetailObject
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type to be created, when user likes to create a new type. 
        /// May also be null, if there is no specific type. Untyped instances are also supported. 
        /// </summary>
        public IObject TypeToCreate
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
        /// Called, when user control had been initialized
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Arguments of event</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Relayout();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Ensure.That(this.Extent != null, "Extent has not been set");
            }
        }

        /// <summary>
        /// Does the relayout
        /// </summary>
        private void Relayout()
        {
            this.wpfElements.Clear();

            if (this.formView != null)
            {
                var fieldInfos = this.formView.getFieldInfos();

                var currentRow = 0;
                // Goes through each element
                foreach (var fieldInfo in fieldInfos.Cast<IObject>())
                {
                    // Creates the key element for the form
                    var name = (fieldInfo.get("title") ?? "").ToString();
                    if (string.IsNullOrEmpty(name))
                    {
                        name = "Unknown";
                    }

                    var nameLabel = new Label();
                    nameLabel.Content = string.Format("{0}: ", name);
                    nameLabel.Margin = new Thickness(10, 5, 10, 5);
                    nameLabel.FontSize = 16;
                    Grid.SetRow(nameLabel, currentRow);

                    formGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    formGrid.Children.Add(nameLabel);

                    // Creates the value element for the form
                    var fieldInfoAsElement = fieldInfo as IElement;
                    var wpfElementCreator = WPFElementMapping.Map(fieldInfoAsElement);
                    var wpfElement = wpfElementCreator.GenerateElement(this.DetailObject, fieldInfo, this);
                    if (wpfElement is FrameworkElement)
                    {
                        (wpfElement as FrameworkElement).Margin = new Thickness(10, 5, 10, 5);
                    }

                    Grid.SetRow(wpfElement, currentRow);
                    Grid.SetColumn(wpfElement, 1);
                    formGrid.Children.Add(wpfElement);

                    this.wpfElements.Add(new ElementCacheEntry(wpfElementCreator, wpfElement, fieldInfo));

                    currentRow++;
                }
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // Creates Detailobject if necessary
            if (this.EditMode == Controls.EditMode.New)
            {
                this.DetailObject = this.Extent.CreateObject(this.TypeToCreate);
                Ensure.That(this.DetailObject != null, "Element Factory has not returned a value");
            }

            // Store values into object
            foreach (var cacheEntry in this.wpfElements)
            {
                cacheEntry.WPFElementCreator.SetData(this.DetailObject, cacheEntry);
            }

            // And now throw the event for the window
            var ev = this.accepted;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Do nothing...

            // And throw the event for the window
            var ev = this.cancelled;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Selects the field with the given name
        /// </summary>
        /// <param name="name">Name of the field, that should receive the focus</param>
        internal void SelectFieldWithName(string name)
        {
            foreach (var element in this.wpfElements)
            {
                var elementName = element.FieldInfo.get("name").AsSingle().ToString();
                if (elementName == name)
                {
                    var asFocusable = element.WPFElementCreator as IFocusable;
                    if (asFocusable != null)
                    {
                        asFocusable.Focus(element.WPFElement);
                    }
                    else
                    {
                        element.WPFElement.Focus();
                    }
                }
            }
        }
    }
}