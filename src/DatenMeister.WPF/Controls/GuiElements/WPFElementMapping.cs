﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls.GuiElements
{
    /// <summary>
    /// Defines the mapping of the elements from DatenMeister tye to the specific Mapping
    /// </summary>
    public static class WPFElementMapping
    {
        /// <summary>
        /// Maps the type of the given element to a gui element
        /// </summary>
        /// <param name="viewElement">Element, which gets transformed</param>
        /// <returns>Transformed element</returns>
        public static IWPFElementGenerator Map(IElement viewElement)
        {
            var metaClass = viewElement.getMetaClass();
            if (metaClass == DatenMeister.Entities.AsObject.FieldInfo.Types.TextField)
            {
                return new WPFTextField();
            }

            throw new NotImplementedException(metaClass.get("name")  + " is not a known WPF Element");
        }
    }
}