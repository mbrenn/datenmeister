using BurnSystems.Logger;
using DatenMeister.DataProvider;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
using DatenMeister.Pool;
using DatenMeister.WPF.Modules.IconRepository;
using DatenMeister.WPF.Windows;
using Ninject;
using System.Linq;
using System.Windows.Controls.Ribbon;

namespace DatenMeister.AddOns.Views
{
    public static class AllExtentView
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(AllExtentView));

        /// <summary>
        /// Adds the view, that the user can be 
        /// </summary>
        /// <param name="window">Window which sall be used</param>
        public static void AddExtentView(IDatenMeisterWindow window)
        {
            var menuItem = new RibbonButton();
            menuItem.Label = Localization_DM_Addons.Menu_ViewExtents;
            menuItem.LargeImageSource = Injection.Application.Get<IIconRepository>().GetIcon("show-extents");

            menuItem.Click += (x, y) =>
            {
                var extentView = PoolResolver.GetDefaultPool().GetExtents(ExtentType.View).First();

                // Creates the view for the extents
                var newView = DatenMeisterPoolExtent.AddView(extentView);
                window.RefreshTabs();

                window.AssociateDetailOpenEvent(newView, (z) =>
                {
                    // Gets the referenced extent
                    var uri = z.Value.AsIObject().getAsSingle("uri").ToString();
                    if (string.IsNullOrEmpty(uri))
                    {
                        logger.Message("No uri has been returned");
                        return;
                    }

                    // Now get the poolresolver
                    var poolResolver = Injection.Application.Get<IPoolResolver>();
                    var extent = poolResolver.Resolve(uri, z.Value) as IURIExtent;
                    if (extent == null)
                    {
                        logger.Message("No extent has been returned for: " + uri.ToString());
                        return;
                    }

                    // Get Factory
                    var factory = Factory.GetFor(extentView);

                    // Creates the view for the extents
                    var extentViewObj = factory.CreateInExtent(extentView, DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);
                    var asObjectExtentview = new DatenMeister.Entities.AsObject.FieldInfo.TableView(extentViewObj);

                    asObjectExtentview.setExtentUri(uri);
                    var instance = poolResolver.Pool.GetInstance(extent);

                    if (instance.extentType == ExtentType.Data)
                    {
                        asObjectExtentview.setAllowDelete(true);
                        asObjectExtentview.setAllowEdit(true);
                        asObjectExtentview.setAllowNew(true);
                    }
                    else
                    {
                        asObjectExtentview.setAllowDelete(false);
                        asObjectExtentview.setAllowEdit(false);
                        asObjectExtentview.setAllowNew(false);
                    }

                    asObjectExtentview.setName(z.Value.AsIObject().getAsSingle("name").ToString());

                    // Gets the referenced extent
                    ViewHelper.AutoGenerateViewDefinition(extent, asObjectExtentview);

                    window.RefreshTabs();
                });
            };

            window.AddMenuEntry(Localization_DM_Addons.Menu_Views, menuItem);
        }
    }
}
