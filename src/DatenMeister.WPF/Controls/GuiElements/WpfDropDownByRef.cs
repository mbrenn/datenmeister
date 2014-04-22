using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    /// <summary>
    /// Creates a drop down field which gives all possible information
    /// by value
    /// </summary>
    public class WpfDropDownByRef : IWPFElementGenerator
    {
        /// <summary>
        /// Initializes a new instance of the WpfDropDownByValue class
        /// </summary>
        public WpfDropDownByRef()
        {
        }

        public System.Windows.UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state)
        {
            var resolver = new PoolResolver(state.Pool);
            var url = fieldInfo.get("referenceUrl").AsSingle().ToString();
            var propertyValue = fieldInfo.get("propertyValue").AsSingle().ToString();
            var resolvedElements = resolver.ResolveAsObjects(url);

            var currentValue = detailObject == null ?
                null : detailObject.get(fieldInfo.get("name").AsSingle().ToString()).AsSingle().ToString();

            // Creates the dropdown box
            var dropDown = new ComboBox();

            // Retrieve the values and add them
            var values = new List<string>();
            var n = 0;
            var selectedValue = -1;
            foreach (var value in resolvedElements)
            {
                var valueAsIObject = value as IObject;
                var stringValue = valueAsIObject.get(propertyValue).AsSingle().ToString();
                values.Add(stringValue);
                if (currentValue == stringValue)
                {
                    selectedValue = n;
                }

                n++;
            }

            // Sets the item source
            dropDown.ItemsSource = values;
            dropDown.SelectedIndex = selectedValue;

            // Make thing read-only, if appropriate
            if (state.EditMode == EditMode.Read)
            {
                dropDown.IsReadOnly = true;
            }

            return dropDown;
        }

        /// <summary>
        /// Sets the database
        /// </summary>
        /// <param name="detailObject">Detail object to be set</param>
        /// <param name="entry">Entry containing used information and references, etc</param>
        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            var combobox = entry.WPFElement as ComboBox;
            detailObject.set(entry.FieldInfo.get("name").ToString(), combobox.SelectedValue);
        }
    }
}
