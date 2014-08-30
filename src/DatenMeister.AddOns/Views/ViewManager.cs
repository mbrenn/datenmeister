using BurnSystems.Test;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
using DatenMeister.Pool;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Ribbon;

namespace DatenMeister.AddOns.Views
{
    /// <summary>
    /// Integrates the view manager into the DatenMeister
    /// </summary>
    public static class ViewManager
    {
        /// <summary>
        /// Integrates the type manager into DatenMeister
        /// </summary>
        /// <param name="window">Window, where the Type Manager will be integrated</param>
        public static void Integrate(IDatenMeisterWindow window)
        {
            var menuItem = new RibbonButton();
            menuItem.Label = Localization_DM_Addons.Menu_ViewManager;
            menuItem.LargeImageSource = AddOnHelper.LoadIcon("emblem-package.png");
            menuItem.Click += (x, y) =>
            {
                var pool = PoolResolver.GetDefaultPool();
                var viewExtent = pool.GetExtent(ExtentType.View).First();

                Ensure.That(viewExtent != null, "No View extent has been defined");

                var tableView = DatenMeister.Entities.AsObject.FieldInfo.TableView.create(viewExtent);
                var tableViewAsObj = new DatenMeister.Entities.AsObject.FieldInfo.TableView(tableView);
                tableViewAsObj.setName("Types");
                tableViewAsObj.setMainType(DatenMeister.Entities.AsObject.Uml.Types.Type);
                tableViewAsObj.setAllowDelete(true);
                tableViewAsObj.setAllowEdit(true);
                tableViewAsObj.setAllowNew(true);
                tableViewAsObj.setExtentUri(viewExtent.ContextURI());

                ViewHelper.AutoGenerateViewDefinition(viewExtent, tableViewAsObj, true);

                viewExtent.Elements().add(tableView);

                window.RefreshTabs();
            };

            window.AddMenuEntry(Localization_DM_Addons.Menu_Views, menuItem);
        }
    }
}
