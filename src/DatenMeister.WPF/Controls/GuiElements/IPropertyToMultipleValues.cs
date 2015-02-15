using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.WPF.Controls.GuiElements
{
    /// <summary>
    /// The implementation of this interface indicates whether it
    /// makes sense to show the property when the user has selected multiple elements. 
    /// For simple elements, like checkbox or textbox, it would make sense, for other elements, 
    /// like lists, it is very hard to understand, whether the list will be created
    /// only once or multiple times.
    /// </summary>
    public interface IPropertyToMultipleValues
    {
        /// <summary>
        /// Generates the WPF element for the Gui Element
        /// </summary>
        /// <param name="detailObject">The object that shall be shown</param>
        /// <param name="fieldInfo">Stores the information of the element</param>
        /// <param name="state">The current state of the element</param>
        /// <returns>The returned element</returns>
        UIElement GenerateElement(IEnumerable<IObject> detailObject, IObject fieldInfo, ILayoutHostState state, ElementCacheEntry cacheEntry);
    }
}
