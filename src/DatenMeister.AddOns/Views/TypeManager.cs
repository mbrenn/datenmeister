using BurnSystems.Test;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
using DatenMeister.Pool;
using DatenMeister.WPF.Modules.IconRepository;
using DatenMeister.WPF.Windows;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

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
            var menuItem = new RibbonButton();
            menuItem.Label = Localization_DM_Addons.Menu_TypeManager;
            menuItem.LargeImageSource = Injection.Application.Get<IIconRepository>().GetIcon("typemanager");
            menuItem.Click += (x, y) =>
                {
                    var pool = PoolResolver.GetDefaultPool();
                    var typeExtent = pool.GetExtent(ExtentType.Type).First();
                    var viewExtent = pool.GetExtent(ExtentType.View).First();

                    Ensure.That(typeExtent != null, "No Type extent has been defined");
                    Ensure.That(viewExtent != null, "No View extent has been defined");

                    var tableView = DatenMeister.Entities.AsObject.FieldInfo.TableView.create(viewExtent);
                    var tableViewAsObj = new DatenMeister.Entities.AsObject.FieldInfo.TableView(tableView);
                    tableViewAsObj.setName("Types");
                    tableViewAsObj.setMainType(DatenMeister.Entities.AsObject.Uml.Types.Type);
                    tableViewAsObj.setAllowDelete(true);
                    tableViewAsObj.setAllowEdit(true);
                    tableViewAsObj.setAllowNew(true);
                    tableViewAsObj.setExtentUri(typeExtent.ContextURI());

                    ViewHelper.AutoGenerateViewDefinition(typeExtent, tableViewAsObj, true);

                    viewExtent.Elements().add(tableView);

                    window.RefreshTabs();
                };

            window.AddMenuEntry(Localization_DM_Addons.Menu_Views, menuItem);
        }
    }
}
