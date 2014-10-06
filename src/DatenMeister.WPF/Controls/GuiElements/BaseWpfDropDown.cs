using BurnSystems.ObjectActivation;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public abstract class BaseWpfDropDown : IFocusable
    {
        /// <summary>
        /// Stores the name of the property being used to retrieve the value 
        /// at the target instance to be shown
        /// </summary>
        protected string propertyValue;

        /// <summary>
        /// Stores the name of the property, who will receive the selected property
        /// </summary>
        protected string binding;

        /// <summary>
        /// Stores an enumeration of all objects
        /// </summary>
        protected IEnumerable<IObject> resolvedElements;

        /// <summary>
        /// Stores the object, that shall be shown and/or evaluated
        /// </summary>
        protected IObject detailObject;

        /// <summary>
        /// Stores the WPF element for the drop down
        /// </summary>
        protected ComboBox dropDown;

        /// <summary>
        /// Stores the pool resolver
        /// </summary>
        private IPoolResolver resolver;

        /// <summary>
        /// Stores the current object
        /// </summary>
        private object currentElement;

        /// <summary>
        /// Gets or sets the value whether the element is just read-only
        /// </summary>
        private bool isReadOnly;

        /// <summary>
        /// Configures the the drop down. May be overridden, if necessary.
        /// The method will be called after all protected variables have been set 
        /// and ComboBox and their instances has been created.
        /// </summary>
        /// <param name="settings">Settings for configuration</param>
        protected virtual void Configure(WpfDropDownSettings settings)
        {
            // Nothing to do per default
        }

        /// <summary>
        /// Generates the element 
        /// </summary>
        /// <param name="detailObject">Detail object containing all the parameters</param>
        /// <param name="fieldInfo">Fieldinformation being used</param>
        /// <param name="state">Status of the presentation</param>
        /// <returns>Generated UI element</returns>
        public System.Windows.UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state)
        {
            var settings = new WpfDropDownSettings();

            // Fills the variable and creates the combobox
            var fieldInfoObj = new ReferenceBase(fieldInfo);
            this.resolver = PoolResolver.GetDefault(PoolResolver.GetDefaultPool());
            var referenceUrl = fieldInfoObj.getReferenceUrl();
            this.propertyValue = fieldInfoObj.getPropertyValue();
            this.binding = fieldInfoObj.getBinding(); // Stores the name of the property
            this.resolvedElements = this.resolver.ResolveAsObjects(referenceUrl);
            this.detailObject = detailObject;

            this.Configure(settings);

            // Creates the dropdown box
            var stackPanel = new Grid();
            stackPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            this.dropDown = new ComboBox();
            this.dropDown.FontSize = 16;
            this.dropDown.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetColumn(this.dropDown, 0);

            this.currentElement = this.GetCurrentValue();
            this.RefreshDropDownElements();

            // Make thing read-only, if appropriate
            if (state.EditMode == EditMode.Read || this.isReadOnly)
            {
                this.dropDown.IsReadOnly = true;
                this.dropDown.IsEnabled = false;
            }

            stackPanel.Children.Add(this.dropDown);

            // Checks, if we have a detail button
            if (settings.ShowDetailButton)
            {
                var button = new Button();
                button.Content = "Detail";
                button.Click += (x, y) =>
                {
                    var selectedItem = this.dropDown.SelectedItem as Item<object>;
                    if (selectedItem == null)
                    {
                        MessageBox.Show("Nothing selected");
                    }
                    else
                    {
                        this.currentElement = selectedItem.OriginalObject;
                        var dialog = DetailDialog.ShowDialogFor(
                            selectedItem.OriginalObject);
                        dialog.DetailForm.Accepted += (a, b) =>
                            {
                                this.RefreshDropDownElements();
                            };
                    }
                };

                stackPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                button.Margin = new Thickness(10, 0, 0, 0);
                button.Padding = new Thickness(10, 0, 10, 0);
                button.FontSize = 16;
                Grid.SetColumn(button, 1);
                stackPanel.Children.Add(button);
            }

            return stackPanel;
        }

        private void RefreshDropDownElements()
        {
            if (this.resolvedElements == null)
            {
                this.dropDown.ItemsSource = null;
            }
            else
            {
                // Retrieve the values and add them
                var values = new List<Item<object>>();
                var selectedValue = -1;

                // Selectes the item
                foreach (var value in this.resolvedElements)
                {
                    var valueAsIObject = value as IObject;
                    var stringValue = valueAsIObject.get(propertyValue).AsSingle().ToString();

                    var item = new Item<object>(stringValue, valueAsIObject, this.GetValue(valueAsIObject));
                    values.Add(item);
                }

                values = values.OrderBy(x => (x as Item<object>).Title).ToList();

                // Finds the item, that needs to be selected
                var n = 0;
                foreach (var value in values.Select(x=> x.Value))
                {
                    if (this.AreValuesEqual(this.currentElement, value))
                    {
                        selectedValue = n;
                    }

                    n++;
                }
                
                // Sets the item source
                this.dropDown.ItemsSource = values;
                this.dropDown.SelectedIndex = selectedValue;
            }
        }

        /// <summary>
        /// Gets the current element out of the detailobject
        /// </summary>
        /// <param name="detailObject">Detailobject, where the current element shall be retrieved</param>
        protected abstract object GetCurrentValue();

        /// <summary>
        /// This method needs to be overridden to evaluate whether the current value and the other element
        /// are equal
        /// </summary>
        /// <param name="currentValue">The current value, as returned by 'GetCurrentElement'</param>
        /// <param name="otherElement">The other element being compared. This is one of the objects being returned
        /// from resolve path</param>
        /// <returns>true, if the values are equal</returns>
        protected abstract bool AreValuesEqual(object currentValue, object otherElement);

        /// <summary>
        /// Gets the value as returned from the objects being returned from resolve path. 
        /// This value will be set to the 'DetailObject', if the user has selected the given
        /// element in the drop down box. 
        /// </summary>
        /// <param name="otherElement">The detail object being used</param>
        /// <returns>Returned object</returns>
        protected abstract object GetValue(IObject otherElement);

        /// <summary>
        /// Sets the data
        /// </summary>
        /// <param name="detailObject">Detail object to be set</param>
        /// <param name="entry">Entry containing used information and references, etc</param>
        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            var combobox = this.dropDown;
            var selectedObject = combobox.SelectedValue as Item<object>;
            var fieldInfoObj = new ReferenceBase(entry.FieldInfo);

            if (selectedObject != null)
            {
                detailObject.set(fieldInfoObj.getBinding(), selectedObject.Value);
            }
        }

        public class Item<T>
        {
            public string Title
            {
                get;
                set;
            }

            public IObject OriginalObject
            {
                get;
                set;

            }

            public T Value
            {
                get;
                set;
            }

            public Item(string title, IObject originalObject, T value)
            {
                this.Title = title;
                this.OriginalObject = originalObject;
                this.Value = value;
            }

            public override string ToString()
            {
                return this.Title;
            }
        }

        public void Focus(UIElement element)
        {
            this.dropDown.Focus();
        }
    }
}
