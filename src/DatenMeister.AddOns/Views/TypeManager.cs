using BurnSystems.Test;
using DatenMeister.Logic.Views;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DatenMeister.AddOns.Views
{
    /// <summary>
    /// Integrates the type manager into the DatenMeister
    /// </summary>
    public static class TypeManager
    {
        /// <summary>
        /// Integrates the type manager into DatenMeister
        /// </summary>
        /// <param name="window">Window, where the Type Manager will be integrated</param>
        public static void Integrate(IDatenMeisterWindow window)
        {
            var menuItem = new MenuItem();
            menuItem.Header = Localization_DM_Addons.Menu_TypeManager;
            menuItem.Click += (x, y) =>
                {
                    Ensure.That(window.Settings.TypeExtent != null, "No Type extent has been defined");
                    Ensure.That(window.Settings.ViewExtent != null, "No Type extent has been defined");

                    var typeExtent = window.Settings.TypeExtent;
                    var viewExtent = window.Settings.ViewExtent;
                    var tableView = DatenMeister.Entities.AsObject.FieldInfo.TableView.create(viewExtent);
                    var tableViewAsObj = new DatenMeister.Entities.AsObject.FieldInfo.TableView(tableView);
                    tableViewAsObj.setName("Types");
                    tableViewAsObj.setMainType(DatenMeister.Entities.AsObject.Uml.Types.Type);
                    tableViewAsObj.setAllowDelete(true);
                    tableViewAsObj.setAllowEdit(true);
                    tableViewAsObj.setAllowNew(true);
                    tableViewAsObj.setExtentUri(window.Settings.TypeExtent.ContextURI());

                    ViewHelper.AutoGenerateViewDefinition(typeExtent, tableViewAsObj, true);

                    viewExtent.Elements().add(tableView);

                    window.RefreshTabs();
                };

            window.AddMenuEntry(Localization_DM_Addons.Menu_Views, menuItem);
        }
    }
}
