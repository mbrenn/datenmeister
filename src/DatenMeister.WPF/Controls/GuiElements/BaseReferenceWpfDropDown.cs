using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public abstract class BaseReferenceWpfDropDown : BaseWpfDropDown
    {
        /// <summary>
        /// Converts the referenced item to a drop down item
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Item being added to the drop down</returns>
        protected override Item<object> ConvertToDropDownItem(object value)
        {
            var valueAsIObject = value as IObject;
            var stringValue = valueAsIObject.getAsSingle(propertyValue).ToString();

            var item = new Item<object>(stringValue, valueAsIObject, this.GetValue(valueAsIObject));
            return item;
        }

        /// <summary>
        /// Gets the value as returned from the objects being returned from resolve path. 
        /// This value will be set to the 'DetailObject', if the user has selected the given
        /// element in the drop down box. 
        /// </summary>
        /// <param name="otherElement">The detail object being used</param>
        /// <returns>Returned object</returns>
        protected abstract object GetValue(IObject otherElement);

        /// <summary>
        /// Gets the objects, which are used to fill the drop down.
        /// The resolved objects will be converted via ToString or name of the IElement
        /// </summary>
        /// <param name="fieldInfo">FieldInformation being used</param>
        /// <returns>Enumeration of the drop down values</returns>
        protected override IEnumerable<object> GetDropDownValues(IObject fieldInfo)
        {
            var fieldInfoObj = new ReferenceBase(fieldInfo);
            var resolver = PoolResolver.GetDefault(PoolResolver.GetDefaultPool());
            var referenceUrl = fieldInfoObj.getReferenceUrl();
            this.propertyValue = fieldInfoObj.getPropertyValue();
            var resolvedElements = resolver.ResolveAsObjects(referenceUrl);
            return resolvedElements;
        }
    }
}
