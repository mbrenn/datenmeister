using DatenMeister.AddOns;
using DatenMeister.Logic;
using DatenMeister.Logic.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DatenMeisterGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ApplicationCore core;

        protected override void OnStartup(StartupEventArgs e)
        {
            BurnSystems.Logging.Log.TheLog.FilterLevel = BurnSystems.Logging.LogLevel.Everything;
            BurnSystems.Logging.Log.TheLog.AddLogProvider(new BurnSystems.Logging.DebugProvider());

            var result = DefaultModules.DefaultStartWith<DatenMeisterGuiSettings>(this);
            this.core = result.Core;
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            this.core.Stop();

            base.OnExit(e);
        }
    }
}
