using DatenMeister.Logic;
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

            if (textFieldObj.isMultiline())
            {
                textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
                textBox.AcceptsReturn = true;
                textBox.Height = 100;
            }

            if ((state.EditMode == EditMode.Edit || state.EditMode == EditMode.Read) && detailObject != null)
            {
                var detailObjectForView = new ObjectDictionaryForView(detailObject);
                var fieldName = textFieldObj.getBinding().ToString();

                var propertyValue = detailObjectForView[fieldName];
                if (propertyValue != null && propertyValue != ObjectHelper.NotSet)
                {
                    textBox.Text = propertyValue.AsSingle().ToString();
                }

                // Do we have a read-only flag
                if (state.EditMode == EditMode.Read || textFieldObj.isReadOnly())
                {
                    textBox.IsReadOnly = true;
                    textBox.IsReadOnlyCaretVisible = true;
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

            if (!textFieldObj.isReadOnly())
            {
                var textBox = entry.WPFElement as TextBox;
                detailObject.set(textFieldObj.getBinding().ToString(), textBox.Text);
            }
        }

        /// <summary>
        /// Sets a focus on the given element
        /// </summary>
        /// <param name="element">Element, where focus is put on </param>
        public void Focus(System.Windows.UIElement element)
        {
            var textBox = element as TextBox;
            textBox.Focus();
            textBox.SelectAll();
        }
    }
}
