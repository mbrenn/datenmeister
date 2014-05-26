using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DatenMeister.WPF.Helper
{
    /// <summary>
    /// Defines some menu helper, which can be used for some default entries
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// Adds the view, that the user can be 
        /// </summary>
        /// <param name="window">Window which sall be used</param>
        public static void AddExtentView(IDatenMeisterWindow window, IURIExtent extentView)
        {
            var menuItem = new MenuItem();
            menuItem.Header = Localization_DatenMeister_WPF.Menu_ViewExtents;
            menuItem.Click += (x, y) =>
                {
                    // Creates the view for the extents
                    var newView = DatenMeisterPoolExtent.AddView(extentView);
                    window.RefreshViews();

                    window.AssociateDetailOpenEvent(newView, (z) =>
                    {
                        var factory = Factory.GetFor(extentView);
                        // Creates the view for the extents
                        var extentViewObj = factory.CreateInExtent(extentView, DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);
                        var asObjectExtentview = new DatenMeister.Entities.AsObject.FieldInfo.TableView(extentViewObj);

                        asObjectExtentview.setExtentUri(z.Value.AsSingle().AsIObject().get("uri").AsSingle().ToString());
                        asObjectExtentview.setAllowDelete(false);
                        asObjectExtentview.setAllowEdit(false);
                        asObjectExtentview.setAllowNew(false);
                        asObjectExtentview.setName(z.Value.AsSingle().AsIObject().get("name").AsSingle().ToString());
                        asObjectExtentview.setFieldInfos(new DotNetSequence(
                            new global::DatenMeister.Entities.FieldInfos.TextField("Name", "name")));

                        window.RefreshViews();
                    });
                };

            window.AddMenuEntry(Localization_DatenMeister_WPF.Menu_View, menuItem);
        }
    }
}
