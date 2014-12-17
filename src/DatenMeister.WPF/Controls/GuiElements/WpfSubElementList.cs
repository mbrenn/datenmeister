using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public class WpfSubElementList : IWpfElementGenerator
    {
        public System.Windows.UIElement GenerateElement(IObject detailObject, IObject fieldInfo, IDataPresentationState state, ElementCacheEntry cacheEntry)
        {
            throw new NotImplementedException();
        }

        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
