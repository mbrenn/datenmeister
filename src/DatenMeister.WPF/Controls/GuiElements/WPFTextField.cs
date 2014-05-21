using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public class WPFTextField : IWPFElementGenerator, IFocusable
    {
        public System.Windows.UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state)
        {
            var textFieldObj = new DatenMeister.Entities.AsObject.FieldInfo.TextField(fieldInfo);

            var textBox = new System.Windows.Controls.TextBox();
            textBox.FontSize = 16;
            textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;

            if (state.EditMode == EditMode.Edit && detailObject != null)
            {
                var fieldName = textFieldObj.getBinding().ToString();
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
            var textFieldObj = new DatenMeister.Entities.AsObject.FieldInfo.TextField(entry.FieldInfo);
            var textBox = entry.WPFElement as TextBox;
            detailObject.set(textFieldObj.getBinding().ToString(), textBox.Text);
        }

        public void Focus(System.Windows.UIElement element)
        {
            var textBox = element as TextBox;
            textBox.Focus();
            textBox.SelectAll();
        }
    }
}
