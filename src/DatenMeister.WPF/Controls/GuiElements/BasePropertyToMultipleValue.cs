using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public abstract class BasePropertyToMultipleValue :  IWpfElementGenerator, IPropertyToMultipleValues
    {
        public abstract System.Windows.UIElement GenerateElement(IEnumerable<IObject> detailObject, IObject fieldInfo, ILayoutHostState state, ElementCacheEntry cacheEntry);

        public System.Windows.UIElement GenerateElement(IObject detailObject, IObject fieldInfo, ILayoutHostState state, ElementCacheEntry cacheEntry)
        {
            if (detailObject == null)
            {
                return this.GenerateElement(
                    (IEnumerable<IObject>)null,
                    fieldInfo,
                    state,
                    cacheEntry);
            }
            else
            {
                return this.GenerateElement(
                    new IObject[] { detailObject },
                    fieldInfo,
                    state,
                    cacheEntry);
            }
        }

        public abstract void SetData(IObject detailObject, ElementCacheEntry entry);
    }
}
