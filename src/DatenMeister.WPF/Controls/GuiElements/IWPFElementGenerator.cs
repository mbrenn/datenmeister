using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.WPF.Controls.GuiElements
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWPFElementGenerator
    {
        /// <summary>
        /// Generates the WPF element for the Gui Element
        /// </summary>
        /// <param name="detailObject">The object that shall be shown</param>
        /// <param name="fieldInfo">Stores the information of the element</param>
        /// <param name="state">The current state of the element</param>
        /// <returns>The returned element</returns>
        UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state);
        
        /// <summary>
        /// Sets the data by the element cache information. This method will be used to 
        /// retrieve the user information and store it into the object <c>detailObject</c>.
        /// </summary>
        /// <param name="detailObject">Object, which shall receive the information</param>
        /// <param name="entry">Cache entry, which has the connection between WPF element and fieldinfo</param>    
        void SetData(IObject detailObject, ElementCacheEntry entry);
    }
}
