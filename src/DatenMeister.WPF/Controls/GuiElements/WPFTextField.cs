using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public class WPFTextField : IWPFElementGenerator
    {
        public System.Windows.UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state)
        {
            var textFieldObject = new DatenMeister.Entities.AsObject.FieldInfo.TextField(fieldInfo);

            var textBox = new System.Windows.Controls.TextBox();
            textBox.FontSize = 16;
            textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;

            if (state.EditMode == EditMode.Edit && detailObject != null)
            {
                var fieldName = fieldInfo.get("name").ToString();
                var propertyValue = detailObject.get(fieldName);
                if (propertyValue != null)
                {
                    textBox.Text = propertyValue.AsSingle().ToString();
                }
            }

            return textBox;
        }

        /// <summary>
        /// Sets the data by the element cache information
        /// </summary>
        /// <param name="detailObject">Object, which shall receive the information</param>
        /// <param name="entry">Cache entry, which has the connection between WPF element and fieldinfo</param>        
        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            var textBox = entry.WPFElement as TextBox;
            detailObject.set(entry.FieldInfo.get("name").ToString(), textBox.Text);
        }
    }
}
