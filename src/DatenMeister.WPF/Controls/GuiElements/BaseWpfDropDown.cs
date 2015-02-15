using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Pool;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public abstract class BaseWpfDropDown : BasePropertyToMultipleValue, IFocusable, IPropertyToMultipleValues
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
        protected IEnumerable<object> resolvedElements;

        /// <summary>
        /// Stores the object, that shall be shown and/or evaluated
        /// </summary>
        protected IEnumerable<IObject> detailObjects;

        /// <summary>
        /// Stores the WPF element for the drop down
        /// </summary>
        protected ComboBox dropDown;

        /// <summary>
        /// Stores the current object
        /// </summary>
        private object currentElement;

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

        public override System.Windows.UIElement GenerateElement(IEnumerable<IObject> detailObjects, IObject fieldInfo, ILayoutHostState state, ElementCacheEntry entry)
        {
            var settings = new WpfDropDownSettings();

            // Fills the variable and creates the combobox
            this.binding = General.getBinding(fieldInfo); // Stores the name of the property
            var resolvedElements = this.GetDropDownValues(fieldInfo);

            this.resolvedElements = resolvedElements;
            this.detailObjects = detailObjects;

            this.Configure(settings);

            // Creates the dropdown box
            var stackPanel = new Grid();
            stackPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            this.dropDown = new ComboBox();
            this.dropDown.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetColumn(this.dropDown, 0);

            this.currentElement = this.GetCurrentValue();
            this.RefreshDropDownElements();

            // Make thing read-only, if appropriate
            if (state.EditMode == EditMode.Read)
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
                        if (selectedItem.OriginalObject != null)
                        {
                            this.currentElement = selectedItem.OriginalObject;
                            var dialog = DetailDialog.ShowDialogFor(
                                selectedItem.OriginalObject);
                            dialog.DetailForm.Accepted += (a, b) =>
                            {
                                this.RefreshDropDownElements();
                            };
                        }
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
                    var item = this.ConvertToDropDownItem(value);
                    values.Add(item);
                }

                values = values.OrderBy(x => (x as Item<object>).Title).ToList();

                // Finds the item, that needs to be selected
                var n = 0;
                foreach (var value in values.Select(x => x.Value))
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
        /// Converts the referenced item to a drop down item
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Item being added to the drop down</returns>
        protected abstract Item<object> ConvertToDropDownItem(object value);

        /// <summary>
        /// Gets all drop down values for the field. 
        /// </summary>
        /// <param name="fieldInfo">Field information, which might contain additional information 
        /// how to retrieve the object</param>
        /// <returns>Enumeration of drop down values</returns>
        protected abstract IEnumerable<object> GetDropDownValues(IObject fieldInfo);

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
        /// Sets the data
        /// </summary>
        /// <param name="detailObject">Detail object to be set</param>
        /// <param name="entry">Entry containing used information and references, etc</param>
        public override void SetData(IObject detailObject, ElementCacheEntry entry)
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
