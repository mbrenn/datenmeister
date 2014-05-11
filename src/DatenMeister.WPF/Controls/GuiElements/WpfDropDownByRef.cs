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
    public class WpfDropDownByRef : BaseWpfDropDown, IWPFElementGenerator
    {
        /// <summary>
        /// Initializes a new instance of the WpfDropDownByValue class
        /// </summary>
        public WpfDropDownByRef()
        {
        }

        protected override void Configure(WpfDropDownSettings settings)
        {
            settings.ShowDetailButton = true;
        }

        /// <summary>
        /// Gets the current element
        /// </summary>
        /// <param name="detailObject">Detal</param>
        /// <returns></returns>
        protected override object GetCurrentValue()
        {
            // Gets the current element
            var currentValue = this.detailObject == null ?
                null : this.detailObject.get(this.propertyName).AsSingle() as IObject;
            return currentValue;
        }

        protected override bool AreValuesEqual(object currentValue, object otherElement)
        {
            var valueAsIObject = otherElement as IObject;
            var currentValueAsIObject = currentValue as IObject;
            if (valueAsIObject == null || currentValueAsIObject == null)
            {
                return false;
            }
            else
            {
                return currentValueAsIObject.Id == valueAsIObject.Id;
            }
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        /// <param name="detailObject">The detail object being used to retrieve the value</param>
        /// <returns>The object being returned</returns>
        protected override object GetValue(IObject detailObject)
        {
            return detailObject;
        }
    }
}
