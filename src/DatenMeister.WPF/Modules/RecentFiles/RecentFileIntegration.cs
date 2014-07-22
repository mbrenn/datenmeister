using DatenMeister.DataProvider;
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

namespace DatenMeister.WPF.Modules.RecentFiles
{
    public class RecentFileIntegration
    {
        /// <summary>
        /// Adds the support for the recent files to the window
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="wnd"></param>
        public static void AddSupport( IDatenMeisterWindow wnd)
        {
            var viewFactory = Factory.GetFor(wnd.Settings.ViewExtent);
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

            wnd.Settings.ViewExtent.Elements().Insert(0, wnd.Core.ViewRecentObjects);

            wnd.RefreshTabs();

            wnd.AssociateDetailOpenEvent(
                wnd.Core.ViewRecentObjects, (x) =>
            {
                var filePath = DatenMeister.Entities.AsObject.DM.RecentProject.getFilePath(x.Value);
                if (File.Exists(filePath))
                {
                    wnd.LoadAndOpenFile(filePath);
                    wnd.Settings.ViewExtent.Elements().remove(wnd.Core.ViewRecentObjects);
                    wnd.RefreshTabs();
                }
                else
                {
                    MessageBox.Show(Localization_DatenMeister_WPF.Open_FileDoesNotExist);
                }
            });
        }

        /// <summary>
        /// Adds a file to the recent file list. 
        /// If the file is already available, it won't be added
        /// </summary>
        /// <param name="filePath">Path of the file to be added</param>
        public static void AddRecentFile(ApplicationCore core, string filePath, string name)
        {
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
        }
    }
}
