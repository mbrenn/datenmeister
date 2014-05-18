using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.WPF.Controls.GuiElements
{
    /// <summary>
    /// This interface should be implemented by each WPFElementGenerator, which is capable to 
    /// give a focus. 
    /// </summary>
    public interface IFocusable
    {
        /// <summary>
        /// Focuses the element
        /// </summary>
        /// <param name="element">Element being focused</param>
        void Focus(UIElement element);
    }
}
