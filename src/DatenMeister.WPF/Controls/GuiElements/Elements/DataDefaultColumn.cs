using DatenMeister.DataProvider;
using DatenMeister.Logic.MethodProvider;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DatenMeister.WPF.Controls.GuiElements.Elements
{
    public class DataDefaultColumn : GenericColumn
    {
        public IObject ViewItem
        {
            get;
            set;
        }

        protected override System.Windows.FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return this.GenerateElement(cell, dataItem);
        }

        protected override System.Windows.FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            var realItem = (dataItem as IProxyObject).Value;
            var result = new TextBlock();
            result.SetBinding(TextBlock.TextProperty, this.Binding);

            var methodProvider = Injection.Application.Get<IMethodProvider>();
            var methodSetBackgroundColor = methodProvider.GetMethodOfInstanceByName(this.ViewItem, "setBackgroundColor");
            if (methodSetBackgroundColor != null)
            {
                var color = methodSetBackgroundColor.Invoke(null, realItem) as DatenMeister.Entities.DM.Primitives.Color;

                if (color != null)
                {
                    cell.Background = new SolidColorBrush(
                        Color.FromArgb(
                            (byte)(color.A * 255),
                            (byte)(color.R * 255),
                            (byte)(color.G * 255),
                            (byte)(color.B * 255)));
                }
            }

            return result;
        }
    }
}
