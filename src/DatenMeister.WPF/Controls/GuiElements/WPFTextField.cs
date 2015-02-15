using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public class WpfTextField : BasePropertyToMultipleValue, IWpfElementGenerator, IFocusable, IPropertyToMultipleValues
    {
        /// <summary>
        /// Sets the data by the element cache information
        /// </summary>
        /// <param name="detailObject">Object, which shall receive the information</param>
        /// <param name="entry">Cache entry, which has the connection between WPF element and fieldinfo</param>
        public override void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            var textFieldObj = new DatenMeister.Entities.AsObject.FieldInfo.TextField(entry.FieldInfo);

            if (!textFieldObj.isReadOnly() && !ObjectDictionaryForView.IsSpecialBinding(textFieldObj.getBinding()))
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

        public override System.Windows.UIElement GenerateElement(IEnumerable<IObject> detailObjects, IObject fieldInfo, ILayoutHostState state, ElementCacheEntry cacheEntry)
        {
            var textFieldObj = new DatenMeister.Entities.AsObject.FieldInfo.TextField(fieldInfo);

            var textBox = new System.Windows.Controls.TextBox();
            textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            var height = textFieldObj.getHeight();
            textBox.TextChanged += (x, y) => cacheEntry.OnChangeContent();

            if (textFieldObj.isMultiline())
            {
                textBox.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
                textBox.AcceptsReturn = true;
                textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                textBox.TextWrapping = System.Windows.TextWrapping.Wrap;
                if (height >= 0)
                {
                    textBox.Height = 100;
                }
            }

            if ((state.EditMode == EditMode.Edit || state.EditMode == EditMode.Read) && detailObjects != null)
            {
                var propertyName = textFieldObj.getBinding().ToString();

                var commonValue = ObjectHelper.GetCommonValue(detailObjects, propertyName);
                                
                if ( commonValue == null 
                    || commonValue == ObjectHelper.NotSet
                    || commonValue == ObjectHelper.Different)
                {
                    textBox.Text = string.Empty;
                }
                else
                {
                    textBox.Text = commonValue.AsSingle().ToString();
                }

                // Do we have a read-only flag
                if (state.EditMode == EditMode.Read || textFieldObj.isReadOnly()
                    || ObjectDictionaryForView.IsSpecialBinding(textFieldObj.getBinding()))
                {
                    textBox.IsReadOnly = true;
                    textBox.IsReadOnlyCaretVisible = true;
                }
            }

            return textBox;
        }
    }
}
