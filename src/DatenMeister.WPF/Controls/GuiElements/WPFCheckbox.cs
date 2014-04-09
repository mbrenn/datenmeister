using BurnSystems.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public class WPFCheckbox : IWPFElementGenerator
    {
        public UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state)
        {
            var checkbox = new DatenMeister.Entities.AsObject.FieldInfo.Checkbox(fieldInfo);

            var wpfCheckbox = new CheckBox();

            if (state.EditMode == EditMode.Edit && detailObject != null)
            {
                var fieldName = fieldInfo.get("name").ToString();
                var propertyValue = detailObject.get(fieldName);
                if (propertyValue != null)
                {
                    var asSingle = propertyValue.AsSingle();
                    wpfCheckbox.IsChecked = Extensions.ToBoolean(asSingle);
                }
            }

            return wpfCheckbox;
        }

        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            var checkBox = entry.WPFElement as CheckBox;
            detailObject.set(entry.FieldInfo.get("name").ToString(), checkBox.IsChecked);
        }
    }
}
