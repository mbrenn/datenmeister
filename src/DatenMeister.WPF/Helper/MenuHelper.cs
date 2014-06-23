﻿using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.Entities.AsObject.FieldInfo;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
using DatenMeister.Pool;
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
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(MenuHelper));

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
                    window.RefreshTabs();

                    window.AssociateDetailOpenEvent(newView, (z) =>
                    {
                        // Gets the referenced extent
                        var uri = z.Value.AsSingle().AsIObject().get("uri").AsSingle().ToString();
                        if (string.IsNullOrEmpty(uri))
                        {
                            logger.Message("No uri has been returned");
                            return;
                        }

                        // Now get the poolresolver
                        var extent = Global.Application.Get<IPoolResolver>().Resolve(uri, z.Value) as IURIExtent;
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
                        asObjectExtentview.setAllowDelete(false);
                        asObjectExtentview.setAllowEdit(false);
                        asObjectExtentview.setAllowNew(false);
                        asObjectExtentview.setName(z.Value.AsSingle().AsIObject().get("name").AsSingle().ToString());

                        // Gets the referenced extent
                        ViewHelper.AutoGenerateViewDefinition(extent, asObjectExtentview);;

                        window.RefreshTabs();
                    });
                };

            window.AddMenuEntry(Localization_DatenMeister_WPF.Menu_View, menuItem);
        }
    }
}
