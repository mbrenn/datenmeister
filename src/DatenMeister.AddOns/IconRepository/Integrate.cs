using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
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
            core.ViewSetInitialized += (x, y) =>
                {
                    Injection.Application.Bind<IIconRepository>().To<GplIconRepository>();
                };
        }
    }
}
