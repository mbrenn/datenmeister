using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public class WpfDropDownByConstant : BaseWpfDropDown, IWpfElementGenerator
    {       
        /// <summary>
        /// Gets the current element out of the detailobject
        /// </summary>
        /// <param name="detailObject">Detailobject, where the current element shall be retrieved</param>
        protected override object GetCurrentValue()
        {
            return ObjectHelper.GetCommonValue(this.detailObjects, this.binding).AsSingle();
        }

        protected override IEnumerable<object> GetDropDownValues(IObject fieldInfo)
        {
            return DatenMeister.Entities.AsObject.FieldInfo.ReferenceByConstant.getValues(fieldInfo);
        }

        /// <summary>
        /// This method needs to be overridden to evaluate whether the current value and the other element
        /// are equal
        /// </summary>
        /// <param name="currentValue">The current value, as returned by 'GetCurrentElement'</param>
        /// <param name="otherElement">The other element being compared. This is one of the objects being returned
        /// from resolve path</param>
        /// <returns>true, if the values are equal</returns>
        protected override bool AreValuesEqual(object currentValue, object otherElement)
        {
            if (currentValue == null || otherElement == null)
            {
                return false;
            }

            return currentValue.ToString() == otherElement.ToString();
        }

        protected override Item<object> ConvertToDropDownItem(object value)
        {
            return new Item<object>(value.ToString(), null, value);
        }
    }
}
