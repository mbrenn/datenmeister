using DatenMeister.DataProvider;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic.Views;
using DatenMeister.WPF.Controls.GuiElements.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Controls.GuiElements
{
    public class WpfSubElementList : IWpfElementGenerator
    {
        public System.Windows.UIElement GenerateElement(IObject detailObject, IObject fieldInfo, ILayoutHostState state, ElementCacheEntry cacheEntry)
        {
            var subElement = new SubElementList(fieldInfo);

            var multiReferenceField = new WpfSubElementListControl();

            var height = subElement.getHeight();
            if (height <= 0)
            {
                height = 300;
            }

            multiReferenceField.Height = height;

            var tableConfiguration = new TableLayoutConfiguration();
            tableConfiguration.ShowCancelButton = false;
            tableConfiguration.ElementsFactory = 
                (pool) => detailObject.get(subElement.getBinding()).AsReflectiveCollection();

            var typeForNew = subElement.getTypeForNew();
            var tableView = subElement.get("listTableView").AsIObject();

            if (typeForNew != null && tableView == null)
            {
                tableView = Factory.GetFor(fieldInfo).create(Types.TableView);

                ViewHelper.AutoGenerateViewDefinitionByType(
                    typeForNew,
                    tableView);
            }

            tableConfiguration.LayoutInfo = tableView;

            multiReferenceField.Configure(tableConfiguration);

            return multiReferenceField;
        }

        public void SetData(IObject detailObject, ElementCacheEntry entry)
        {
            // Nothing to do here
            // Every change of the subelements will be directly added to the detail object
        }
    }
}
