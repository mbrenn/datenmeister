using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public class WpfCheckbox : BasePropertyToMultipleValue, IWpfElementGenerator, IPropertyToMultipleValues
    {
        public override System.Windows.UIElement GenerateElement(
            IEnumerable<IObject> detailObjects, 
            IObject fieldInfo, 
            ILayoutHostState state, 
            ElementCacheEntry entry)
        {
            var checkbox = new DatenMeister.Entities.AsObject.FieldInfo.Checkbox(fieldInfo);

            var wpfCheckbox = new CheckBox();
            wpfCheckbox.VerticalAlignment = VerticalAlignment.Center;
            wpfCheckbox.Click += (x, y) => entry.OnChangeContent();

            if (state.EditMode == EditMode.Edit && detailObjects != null)
            {
                var fieldName = checkbox.getBinding();
                var propertyValue = ObjectHelper.GetCommonValue(detailObjects, fieldName);
                if (propertyValue == ObjectHelper.Different)
                {
                    wpfCheckbox.IsChecked = null;
                }
                else if (propertyValue != null)
                {
                    wpfCheckbox.IsChecked = ObjectConversion.ToBoolean(propertyValue);
                }
            }

            return wpfCheckbox;
        }

        public override void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            var checkBoxInfo = new DatenMeister.Entities.AsObject.FieldInfo.Checkbox(entry.FieldInfo);
            var checkBox = entry.WPFElement as CheckBox;
            detailObject.set(checkBoxInfo.getBinding(), checkBox.IsChecked);
        }
    }
}
