using BurnSystems.Serialization;
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
    public class WpfCheckbox : IWpfElementGenerator, IPropertyToMultipleValues
    {
        public UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state)
        {
            if (detailObject == null)
            {
                return this.GenerateElement(
                    (IEnumerable<IObject>)null,
                    fieldInfo,
                    state);
            }
            else
            {
                return this.GenerateElement(
                    new IObject[] { detailObject },
                    fieldInfo,
                    state);
            }
        }

        public System.Windows.UIElement GenerateElement(IEnumerable<IObject> detailObjects, IObject fieldInfo, IDataPresentationState state)
        {
            var checkbox = new DatenMeister.Entities.AsObject.FieldInfo.Checkbox(fieldInfo);

            var wpfCheckbox = new CheckBox();            
            wpfCheckbox.VerticalAlignment = VerticalAlignment.Center;

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
