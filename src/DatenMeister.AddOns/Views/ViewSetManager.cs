﻿using BurnSystems.Test;
using DatenMeister.DataProvider.DotNet;
using DatenMeister.Entities.FieldInfos;
using DatenMeister.Logic;
using DatenMeister.Logic.Views;
using DatenMeister.Pool;
using DatenMeister.WPF.Controls;
using DatenMeister.WPF.Modules.IconRepository;
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
            menuItem.LargeImageSource = Injection.Application.Get<IIconRepository>().GetIcon("viewmanager");
            menuItem.Click += (x, y) =>
            {
                var pool = PoolResolver.GetDefaultPool();
                var viewExtent = pool.GetExtents(ExtentType.View).First();

                Ensure.That(viewExtent != null, "No View extent has been defined");

                //var tableView = DatenMeister.Entities.AsObject.FieldInfo.TreeView.create(viewExtent);
                var tableView = DatenMeister.Entities.AsObject.FieldInfo.TableView.create(viewExtent);
                viewExtent.Elements().add(tableView);

                var tableViewAsObj = new DatenMeister.Entities.AsObject.FieldInfo.TableView(tableView);
                tableViewAsObj.setName("Types");
                tableViewAsObj.setMainType(DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);
                tableViewAsObj.pushTypesForCreation(DatenMeister.Entities.AsObject.FieldInfo.Types.FormView);
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

            var assignAuto = new RibbonButton();
            assignAuto.Label = Localization_DM_Addons.Menu_ViewManagerAssignTypes;
            assignAuto.LargeImageSource = Injection.Application.Get<IIconRepository>().GetIcon("viewmanager-assigntypes");
            assignAuto.Click += (x, y) =>
            {
                var viewManager = Injection.Application.Get<IViewManager>() as DefaultViewManager;
                var globalDotNetExtent = Injection.Application.Get<GlobalDotNetExtent>();
                var entriesAsObject = globalDotNetExtent.CreateReflectiveSequence(viewManager.Entries);

                var listView = new TableView();
                listView.fieldInfos = new List<object>(new[]{
                    new TextField("View", "View"),
                    new TextField("MetaClass", "MetaClass"),
                    new TextField("Is Default", "IsDefault")
                });

                var detailView = new FormView();
                detailView.fieldInfos = new List<Object>(new object[]{
                    new ReferenceByRef("View", "View", "datenmeister://datenmeister/all/extenttype/View", "name"),
                    new ReferenceByRef("MetaClass", "MetaClass", "datenmeister://datenmeister/all/extenttype/Type", "name"),
                    new Checkbox("Is Default", "IsDefault")
                });

                detailView.allowEdit = false;

                listView.mainType =
                    globalDotNetExtent.GetIObjectForType(typeof(DatenMeister.Logic.Views.DefaultViewManager.ViewEntry));

                var listConfiguration = new TableLayoutConfiguration()
                {
                    LayoutInfo = globalDotNetExtent.CreateObject(listView),
                    ViewInfoForDetailView = globalDotNetExtent.CreateObject(detailView)
                };

                listConfiguration.SetElements(entriesAsObject);
                var assignDialog = new ListDialog(listConfiguration);
                assignDialog.ShowDialog();
            };

            // Add the event, and call yourself
            window.Core.ViewSetFinalized += (x, y) => InitializeViewSet();

            window.AddMenuEntry(Localization_DM_Addons.Menu_Views, assignAuto);
            window.AddMenuEntry(Localization_DM_Addons.Menu_Views, menuItem);
        }

        /// <summary>
        /// Creates the detail view for the objects
        /// </summary>
        private static void InitializeViewSet()
        {
            // Assigns the viewset,
            // First, get the necessary object
            var myPool = PoolResolver.GetDefaultPool();
            var myViewExtent = myPool.GetExtents(ExtentType.View).First();
            var viewManager = Injection.Application.Get<IViewManager>() as DefaultViewManager;

            CreateForTableView(myViewExtent, viewManager);
            CreateForFormView(myViewExtent, viewManager);
        }

        /// <summary>
        /// Creates the detail view for the table view
        /// </summary>
        /// <param name="myViewExtent">Extent to be used</param>
        /// <param name="viewManager">ViewManager to be used</param>
        private static void CreateForTableView(IURIExtent myViewExtent, DefaultViewManager viewManager)
        {
            /////////////////////////////////////////////
            // For the tableView
            // Second, create the detail form
            var tableViewForm = DatenMeister.Entities.AsObject.FieldInfo.FormView.create(myViewExtent);
            var tableViewFormAsObj = new DatenMeister.Entities.AsObject.FieldInfo.FormView(tableViewForm);
            tableViewFormAsObj.setAllowDelete(true);
            tableViewFormAsObj.setName("Form for DatenMeister.Views.TableView");
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
            myViewExtent.Elements().add(tableViewForm);
        }

        /// <summary>
        /// Creates the detail view for the table view
        /// </summary>
        /// <param name="myViewExtent">Extent to be used</param>
        /// <param name="viewManager">ViewManager to be used</param>
        private static void CreateForFormView(IURIExtent myViewExtent, DefaultViewManager viewManager)
        {
            /////////////////////////////////////////////
            // For the tableView
            // Second, create the detail form
            var tableViewForm = DatenMeister.Entities.AsObject.FieldInfo.FormView.create(myViewExtent);
            var tableViewFormAsObj = new DatenMeister.Entities.AsObject.FieldInfo.FormView(tableViewForm);
            tableViewFormAsObj.setAllowDelete(true);
            tableViewFormAsObj.setName("Form for DatenMeister.Views.FormView");
            var myColumns = new DotNetSequence(
                ViewHelper.ViewTypes,
                new TextField("Name", "name"),
                new Checkbox("Autogenerate Fields", "doAutoGenerateByProperties"));

            tableViewFormAsObj.setFieldInfos(myColumns);

            // Third, adds the information to the view manager
            viewManager.Add(
                DatenMeister.Entities.AsObject.FieldInfo.Types.FormView,
                tableViewForm,
                true);
            myViewExtent.Elements().add(tableViewForm);
        }
    }
}
