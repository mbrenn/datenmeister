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
    public abstract class BaseWpfDropDown
    {
        /// <summary>
        /// Stores the name of the property being used to retrieve the value 
        /// at the target instance to be shown
        /// </summary>
        protected string propertyValue;

        /// <summary>
        /// Stores the name of the property, who will receive the selected property
        /// </summary>
        protected string propertyName;

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
        /// Generates the element 
        /// </summary>
        /// <param name="detailObject">Detail object containing all the parameters</param>
        /// <param name="fieldInfo">Fieldinformation being used</param>
        /// <param name="state">Status of the presentation</param>
        /// <returns>Generated UI element</returns>
        public System.Windows.UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state)
        {
            // Fills the variable and creates the combobox
            var resolver = new PoolResolver(state.Pool);
            var referenceUrl = fieldInfo.get("referenceUrl").AsSingle().ToString();
            this.propertyValue = fieldInfo.get("propertyValue").AsSingle().ToString();
            this.propertyName = fieldInfo.get("name").AsSingle().ToString(); // Stores the name of the property
            this.resolvedElements = resolver.ResolveAsObjects(referenceUrl);
            this.detailObject = detailObject;

            // Creates the dropdown box
            this.dropDown = new ComboBox();
            this.dropDown.FontSize = 16;

            var currentElement = this.GetCurrentValue();

            // Retrieve the values and add them
            var values = new List<object>();
            var n = 0;
            var selectedValue = -1;
            foreach (var value in resolvedElements)
            {
                var valueAsIObject = value as IObject;
                var stringValue = valueAsIObject.get(propertyValue).AsSingle().ToString();

                var item = new Item<object>(stringValue, this.GetValue(valueAsIObject));
                values.Add(item);

                if (this.AreValuesEqual(currentElement, value))
                {
                    selectedValue = n;
                }

                n++;
            }

            // Sets the item source
            this.dropDown.ItemsSource = values;
            this.dropDown.SelectedIndex = selectedValue;

            // Make thing read-only, if appropriate
            if (state.EditMode == EditMode.Read)
            {
                this.dropDown.IsReadOnly = true;
            }

            return this.dropDown;
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
            var combobox = entry.WPFElement as ComboBox;
            var selectedObject = combobox.SelectedValue as Item<object>;

            if (selectedObject != null)
            {
                detailObject.set(entry.FieldInfo.get("name").ToString(), selectedObject.Value);
            }
        }

        public class Item<T>
        {
            public string Title
            {
                get;
                set;
            }

            public T Value
            {
                get;
                set;
            }

            public Item(string title, T value)
            {
                this.Title = title;
                this.Value = value;
            }

            public override string ToString()
            {
                return this.Title;
            }
        }
    }
}
