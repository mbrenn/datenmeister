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
        private List<AdditionalColumnInfo> additionalColumns = new List<AdditionalColumnInfo>();

        public IWpfElementGenerator WPFElementCreator
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

        /// <summary>
        /// Gets the list of additional columns
        /// </summary>
        public List<AdditionalColumnInfo> AdditionalColumns
        {
            get { return this.additionalColumns; }
        }

        public ElementCacheEntry(IObject fieldInfo)
        {
            this.FieldInfo = fieldInfo;
        }

        public ElementCacheEntry(IWpfElementGenerator wpfElementCreator, UIElement wpfElement, IObject fieldInfo)
        {
            this.WPFElement = wpfElement;
            this.WPFElementCreator = wpfElementCreator;
            this.FieldInfo = fieldInfo;
        }

        public class AdditionalColumnInfo
        {
            /// <summary>
            /// Defines a function, which is used to retrieve the checklist status
            /// </summary>
            public Func<bool> GetChecklistStatus
            {
                get;
                set;
            }

            /// <summary>
            /// Gets the object, which is determined whether the checkbox is still checked.
            /// If the return value is null, the value is not set and the next checkbox will be
            /// used as a trigger
            /// </summary>
            public Func<bool, object> ValueFunction
            {
                get;
                set;
            }
        }
    }
}
