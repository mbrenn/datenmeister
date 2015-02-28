using DatenMeister.Logic;
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
    public class WpfDropDownByValue : BaseReferenceWpfDropDown
    {
        /// <summary>
        /// Initializes a new instance of the WpfDropDownByValue class
        /// </summary>
        public WpfDropDownByValue()
        {
        }

        protected override bool AreValuesEqual(object currentValue, object otherElement)
        {
            var valueAsIObject = otherElement as IObject;
            if (valueAsIObject != null)
            {
                return false;
            }

            var stringValue = valueAsIObject.getAsSingle(propertyValue).ToString();
            return currentValue.ToString() == stringValue;
        }

        protected override object GetValue(IObject otherElement)
        {
            return otherElement.getAsSingle(this.propertyValue).ToString();
        }

        protected override object GetCurrentValue()
        {
            var value = ObjectHelper.GetCommonValue(this.detailObjects, this.binding);

            var currentValue = value.ToString();
            return currentValue;
        }
    }
}
