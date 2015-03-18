using DatenMeister.AddOns.Export.Excel;
using DatenMeister.AddOns.Export.Report.Simple;
using DatenMeister.AddOns.Scripting;
using DatenMeister.AddOns.Views;
using DatenMeister.Logic;
using DatenMeister.Logic.Settings;
using DatenMeister.WPF.Modules.RecentFiles;
using DatenMeister.WPF.Windows;
using System.Windows;

namespace DatenMeister.AddOns
{
    public static class DefaultModules
    {
        /// <summary>
        /// Performs a default start of the DatenMeister with all plugins
        /// </summary>
        public static StartResult DefaultStartWith<T>(Application application) where T : IDatenMeisterSettings, new()
        {
            var core = new ApplicationCore();
            DatenMeister.WPF.Modules.IconRepository.Integrate.Perform(core);
            core.Start<T>();
            var wnd = WindowFactory.CreateWindow(core);

            // Other menu helpers
            RecentFileIntegration.AddSupport(wnd);
            AllExtentView.AddExtentView(wnd);

            // Exports the entry to an excel item
            ExcelExportGui.AddMenu(wnd);
            TypeManager.Integrate(wnd);
            ViewSetManager.Integrate(wnd);
            SimpleReportGui.Integrate(wnd);
            ScriptingGui.Integrate(wnd);
            DatenMeister.AddOns.ComplianceSuite.WPF.Plugin.Integrate(wnd);

            // Registers to exit event of the application
            /*if (application != null)
            {
                if (core.Settings != null)
                {
                    core.SaveApplicationData();
                }
            }*/

            return new StartResult()
            {
                Core = core,
                Window = wnd
            };
        }

        /// <summary>
        /// Defines a class, containing the result of start of default modules
        /// </summary>
        public class StartResult
        {
            /// <summary>
            /// Gets or sets the created application core
            /// </summary>
            public ApplicationCore Core
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the datenmeister window
            /// </summary>
            public IDatenMeisterWindow Window
            {
                get;
                set;
            }

            /// <summary>
            /// Stores the created setting
            /// </summary>
            public IDatenMeisterSettings Settings
            {
                get;
                set;
            }
        }
    }
}
