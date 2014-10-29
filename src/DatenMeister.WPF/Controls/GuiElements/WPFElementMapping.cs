using DatenMeister.WPF.Controls.GuiElements.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    /// <summary>
    /// Defines the mapping of the elements from DatenMeister tye to the specific Mapping
    /// </summary>
    public static class WpfElementMapping
    {
        /// <summary>
        /// Maps the type of the given element to a gui element
        /// </summary>
        /// <param name="viewElement">Element, which gets transformed</param>
        /// <returns>Transformed element</returns>
        public static IWpfElementGenerator MapForForm(IElement viewElement)
        {
            var metaClass = viewElement.getMetaClass();
            if (metaClass == DatenMeister.Entities.AsObject.FieldInfo.Types.TextField)
            {
                return new WPFTextField();
            }

            if (metaClass == DatenMeister.Entities.AsObject.FieldInfo.Types.DatePicker)
            {
                return new WpfDatePicker();
            }

            if (metaClass == DatenMeister.Entities.AsObject.FieldInfo.Types.Comment)
            {
                return new WpfComment();
            }

            if (metaClass == DatenMeister.Entities.AsObject.FieldInfo.Types.Checkbox)
            {
                return new WpfCheckbox();
            }

            if (metaClass == DatenMeister.Entities.AsObject.FieldInfo.Types.ReferenceByValue)
            {
                return new WpfDropDownByValue();
            }

            if (metaClass == DatenMeister.Entities.AsObject.FieldInfo.Types.ReferenceByRef)
            {
                return new WpfDropDownByRef();
            }

            if (metaClass == DatenMeister.Entities.AsObject.FieldInfo.Types.MultiReferenceField)
            {
                return new WpfMultiReferenceField();
            }

            if (metaClass == null)
            {
                throw new NotImplementedException("metaClass is null and not known");
            }

            throw new NotImplementedException(metaClass.get("name") + " is not a known WPF Element");
        }

        /// <summary>
        /// Performs the mapping for a certain table
        /// </summary>
        /// <param name="viewElement">View element being used to show the columns</param>
        /// <returns>Returned column information</returns>
        public static GenericColumn MapForTable(IObject viewElement)
        {
            var element = viewElement as IElement;
            if ( element != null )
            {
                var metaClass = element.getMetaClass();
                if (metaClass == DatenMeister.Entities.AsObject.FieldInfo.Types.HyperLinkColumn)
                {
                    return new HyperLinkColumn();
                }
            }

            return new DataDefaultColumn();
        }
    }
}
