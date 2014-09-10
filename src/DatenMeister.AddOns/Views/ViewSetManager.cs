using BurnSystems.Test;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.Entities.FieldInfos;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
using DatenMeister.Pool;
using DatenMeister.WPF.Windows;
using Ninject;
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
    public static class ViewSetManager
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
                viewExtent.Elements().add(tableView);

                var tableViewAsObj = new DatenMeister.Entities.AsObject.FieldInfo.TableView(tableView);
                tableViewAsObj.setName("Types");
                tableViewAsObj.setMainType(DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);
                tableViewAsObj.pushTypesForCreation(DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);
                tableViewAsObj.setAllowDelete(true);
                tableViewAsObj.setAllowEdit(true);
                tableViewAsObj.setAllowNew(true);
                tableViewAsObj.setExtentUri(viewExtent.ContextURI());

                var tableColumns = new DotNetSequence(
                    ViewHelper.ViewTypes,
                    new TextField("Type", ObjectDictionaryForView.TypeBinding),
                    new TextField("Name", "name"),
                    new TextField("Extent URI", "extentUri"),
                    new TextField("Allows Editing", "allowEdit"),
                    new TextField("Allows Deleting", "allowDelete"),
                    new TextField("Allows Creating", "allowNew"),
                    new TextField("Autogenerate Fields", "doAutoGenerateByProperties"));
                tableViewAsObj.setFieldInfos(tableColumns);

                window.RefreshTabs();
            };

            // Add the event, and call yourself
            window.Core.ViewSetInitialized += (x, y) => InitializeViewSet();
            InitializeViewSet();

            window.AddMenuEntry(Localization_DM_Addons.Menu_Views, menuItem);
        }

        private static void InitializeViewSet()
        {
            // Assigns the viewset,
            // First, get the necessary object
            var myPool = PoolResolver.GetDefaultPool();
            var myViewExtent = myPool.GetExtent(ExtentType.View).First();
            var viewManager = Injection.Application.Get<IViewManager>() as DefaultViewManager;

            // Second, create the detail form
            var tableViewForm = DatenMeister.Entities.AsObject.FieldInfo.FormView.create(myViewExtent);
            var tableViewFormAsObj = new DatenMeister.Entities.AsObject.FieldInfo.FormView(tableViewForm);
            tableViewFormAsObj.setAllowDelete(true);
            tableViewFormAsObj.setName("FormView DetailForm");
            var myColumns = new DotNetSequence(
                ViewHelper.ViewTypes,
                new TextField("Name", "name"),
                new TextField("Extent URI", "extentUri"),
                new Checkbox("Allows Editing", "allowEdit"),
                new Checkbox("Allows Deleting", "allowDelete"),
                new Checkbox("Allows Creating", "allowNew"),
                new Checkbox("Autogenerate Fields", "doAutoGenerateByProperties"));

            tableViewFormAsObj.setFieldInfos(myColumns);

            // Third, adds the information to the view manager
            viewManager.Add(
                DatenMeister.Entities.AsObject.FieldInfo.Types.TableView,
                tableViewForm,
                true);
        }
    }
}
