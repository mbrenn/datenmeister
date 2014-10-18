using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements.Elements
{
    public class DataDefaultColumn : GenericColumn
    {
        protected override System.Windows.FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return this.GenerateElement(cell, dataItem);
        }

        protected override System.Windows.FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var result = new TextBlock();
            result.SetBinding(TextBlock.TextProperty, this.Binding);
            return result;
        }
    }
}
