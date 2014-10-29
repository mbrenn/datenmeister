using BurnSystems.Serialization;
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
    public class WpfCheckbox : IWPFElementGenerator
    {
        public UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state)
        {
            var checkbox = new DatenMeister.Entities.AsObject.FieldInfo.Checkbox(fieldInfo);

            var wpfCheckbox = new CheckBox();
            wpfCheckbox.LayoutTransform = new ScaleTransform(1.6, 1.6);
            wpfCheckbox.VerticalAlignment = VerticalAlignment.Center;

            if (state.EditMode == EditMode.Edit && detailObject != null)
            {
                var fieldName = checkbox.getBinding();
                var propertyValue = detailObject.get(fieldName);
                if (propertyValue != null)
                {
                    var asSingle = propertyValue.AsSingle();
                    wpfCheckbox.IsChecked = ObjectConversion.ToBoolean(asSingle);
                }
            }

            return wpfCheckbox;
        }

        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            var checkBoxInfo = new DatenMeister.Entities.AsObject.FieldInfo.Checkbox(entry.FieldInfo);
            var checkBox = entry.WPFElement as CheckBox;
            detailObject.set(checkBoxInfo.getBinding(), checkBox.IsChecked);
        }
    }
}
