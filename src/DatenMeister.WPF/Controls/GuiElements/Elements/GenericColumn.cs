using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements.Elements
{
    public abstract class GenericColumn : DataGridBoundColumn
    {
        public GenericColumn(IObject associatedViewColumn, string propertyName)
        {
            this.AssociatedViewColumn = associatedViewColumn;
            this.PropertyName = propertyName;
        }

        /// <summary>
        /// Stores the name of the property
        /// </summary>
        public string PropertyName
        {
            get;
            set;
        }
    
        /// <summary>
        /// Stores the associated view column
        /// </summary>
        public IObject AssociatedViewColumn
        {
            get;
            set;
        }
    }
}
