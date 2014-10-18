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
        public IObject AssociatedViewColumn
        {
            get;
            set;
        }
    }
}
