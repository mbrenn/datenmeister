using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.WPF.Controls.GuiElements
{
    /// <summary>
    /// Being used to cache the active wpf elements and field infos
    /// </summary>
    public class ElementCacheEntry
    {
        public IWPFElementGenerator WPFElementCreator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the created WPF element
        /// </summary>
        public UIElement WPFElement
        {
            get;
            set;
        }

        public IObject FieldInfo
        {
            get;
            set;
        }

        public ElementCacheEntry(IWPFElementGenerator wpfElementCreator, UIElement wpfElement, IObject fieldInfo)
        {
            this.WPFElement = wpfElement;
            this.WPFElementCreator = wpfElementCreator;
            this.FieldInfo = fieldInfo;
        }
    }
}
