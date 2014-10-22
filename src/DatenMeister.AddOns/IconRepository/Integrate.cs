using BurnSystems.Test;
using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.IconRepository
{
    /// <summary>
    /// Integrates the icon repository
    /// </summary>
    public static class Integrate
    {
        /// <summary>
        /// Performs the integration
        /// </summary>
        public static void Perform(ApplicationCore core)
        {
            if (File.Exists("DatenMeister.Icons.dll"))
            {
                var dllPath = Path.Combine(Environment.CurrentDirectory, "DatenMeister.Icons.dll");
                var assembly = Assembly.LoadFile(dllPath);
                Ensure.That(assembly != null, "'DatenMeister.Icons.dll' could not be loaded");

                var type = assembly.GetType("DatenMeister.Icons.NiceIconsRepository");
                Ensure.That(type != null, "Type 'DatenMeister.Icons.NiceIconsRepository' is not found");

                var method = type.GetMethod("Free");
                Ensure.That(method != null, "Method Free was expected");
                method.Invoke(null, new object[] { "Accept Axialis Icons" });

                core.ViewSetInitialized += (x, y) =>
                {
                    Injection.Application.Bind<IIconRepository>().To(type);
                };
            }
            else
            {
                core.ViewSetInitialized += (x, y) =>
                    {
                        Injection.Application.Bind<IIconRepository>().To<GplIconRepository>();
                    };
            }
        }
    }
}

