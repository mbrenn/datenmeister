﻿using BurnSystems.Test;
using DatenMeister.DataProvider;
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
using DatenMeister.Pool;
using DatenMeister.Transformations;
using DatenMeister.WPF.Helper;
using DatenMeister.WPF.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace DatenMeister.WPF.Modules.RecentFiles
{
    public static class RecentFileIntegration
    {
        /// <summary>
        /// Adds the support for the recent files to the window
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="wnd"></param>
        public static void AddSupport(IDatenMeisterWindow wnd)
        {
            var pool = PoolResolver.GetDefaultPool();
            var viewExtent = pool.GetExtent(ExtentType.View).First();

            // Includes the View
            var viewFactory = Factory.GetFor(viewExtent);
            wnd.Core.ViewRecentObjects = viewFactory.create(
                DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);
            DatenMeister.Entities.AsObject.FieldInfo.TableView.setName(wnd.Core.ViewRecentObjects, "Recent Files");
            DatenMeister.Entities.AsObject.FieldInfo.TableView.setAllowEdit(wnd.Core.ViewRecentObjects, false);
            DatenMeister.Entities.AsObject.FieldInfo.TableView.setAllowNew(wnd.Core.ViewRecentObjects, false);
            DatenMeister.Entities.AsObject.FieldInfo.TableView.setAllowDelete(wnd.Core.ViewRecentObjects, true);
            DatenMeister.Entities.AsObject.FieldInfo.TableView.setExtentUri(wnd.Core.ViewRecentObjects, ApplicationCore.ApplicationDataUri);

            var fieldInfos = wnd.Core.ViewRecentObjects.get("fieldInfos").AsReflectiveSequence();
            var textField = DatenMeister.Entities.AsObject.FieldInfo.TextField.create(viewFactory);
            DatenMeister.Entities.AsObject.FieldInfo.TextField.setBinding(textField, "name");
            DatenMeister.Entities.AsObject.FieldInfo.TextField.setName(textField, "Name");
            fieldInfos.add(textField);

            textField = DatenMeister.Entities.AsObject.FieldInfo.TextField.create(viewFactory);
            DatenMeister.Entities.AsObject.FieldInfo.TextField.setBinding(textField, "filePath");
            DatenMeister.Entities.AsObject.FieldInfo.TextField.setName(textField, "Storage Path");
            fieldInfos.add(textField);

            viewExtent.Elements().Insert(0, wnd.Core.ViewRecentObjects);

            // Adds the mapping for the recent projects
            var applicationExtent = pool.GetExtent(ExtentType.ApplicationData).First() as XmlExtent;
            Ensure.That(applicationExtent != null, "Application Extent is not XmlExtent");
            applicationExtent.Settings.Mapping.Add(DatenMeister.Entities.AsObject.DM.Types.RecentProject);

            // Updates the view to show the content
            wnd.RefreshTabs();

            wnd.AssociateDetailOpenEvent(
                wnd.Core.ViewRecentObjects, (x) =>
                {
                    var value = x.Value;

                    OnClickOpenFile(wnd, value);
                });

            // Creates the application menu
            foreach (var item in 
                applicationExtent.Elements().FilterByType(DatenMeister.Entities.AsObject.DM.Types.RecentProject)
                    .Select(x=> x.AsIObject()))
            {
                AddRecentFileToMenu(wnd, item);
            }
        }

        /// <summary>
        /// Adds a certain recent file to the menu
        /// </summary>
        /// <param name="wnd">Window, to which the file will be added</param>
        /// <param name="recentFile">The recent file being added</param>
        private static void AddRecentFileToMenu(IDatenMeisterWindow wnd, IObject recentFile)
        {
            var windowAsDatenMeister = wnd as DatenMeisterWindow;
            var value = recentFile;
            var menu = new RibbonApplicationMenuItem();
            menu.Header = Path.GetFileName(DatenMeister.Entities.AsObject.DM.RecentProject.getFilePath(recentFile));
            menu.Click += (x, y) => { OnClickOpenFile(wnd, value); y.Handled = true; };
            windowAsDatenMeister.GetRecentFileRibbon().Items.Add(menu);
        }

        /// <summary>
        /// This method will be called, when the user clicks on one of the items
        /// </summary>
        /// <param name="wnd">Window to be used</param>
        /// <param name="recentFile">Value being a recent project object</param>
        private static void OnClickOpenFile(IDatenMeisterWindow wnd, IObject recentFile)
        {
            var innerPool = PoolResolver.GetDefaultPool();
            var innerViewExtent = innerPool.GetExtent(ExtentType.View).First();

            var filePath = DatenMeister.Entities.AsObject.DM.RecentProject.getFilePath(recentFile);
            if (File.Exists(filePath))
            {
                // Loads and opens the file
                wnd.LoadAndOpenFile(filePath);

                // Removes the view for the recent files
                if (innerViewExtent.Elements().Contains(wnd.Core.ViewRecentObjects))
                {
                    innerViewExtent.Elements().remove(wnd.Core.ViewRecentObjects);
                }

                wnd.RefreshTabs();
            }
            else
            {
                MessageBox.Show(Localization_DatenMeister_WPF.Open_FileDoesNotExist);
            }
        }

        /// Adds a file to the recent file list. 
        /// If the file is already available, it won't be added
        /// </summary>
        /// <param name="filePath">Path of the file to be added</param>
        public static void AddRecentFile(IDatenMeisterWindow wnd, string filePath, string name)
        {
            var core = wnd.Core;
            // Check, if the file is already available
            if (core.ApplicationData.Elements()
                .FilterByType(DatenMeister.Entities.AsObject.DM.Types.RecentProject)
                .Any(x => DatenMeister.Entities.AsObject.DM.RecentProject.getFilePath(x.AsIObject()) == filePath))
            {
                return;
            }

            // Ok, not found, now add it
            var factory = Factory.GetFor(core.ApplicationData);
            var recentProject = factory.CreateInExtent(core.ApplicationData, DatenMeister.Entities.AsObject.DM.Types.RecentProject);
            DatenMeister.Entities.AsObject.DM.RecentProject.setFilePath(recentProject, filePath);
            DatenMeister.Entities.AsObject.DM.RecentProject.setCreated(recentProject, DateTime.Now);
            DatenMeister.Entities.AsObject.DM.RecentProject.setName(recentProject, name);

            AddRecentFileToMenu(wnd, recentProject);
        }
    }
}
