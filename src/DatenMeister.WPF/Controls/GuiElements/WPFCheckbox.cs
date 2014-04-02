using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public class WPFCheckbox : IWPFElementGenerator
    {
        public UIElement GenerateElement(IObject detailInfo, IObject fieldInfo, IDataPresentationState state)
        {
            throw new NotImplementedException();
        }

        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
