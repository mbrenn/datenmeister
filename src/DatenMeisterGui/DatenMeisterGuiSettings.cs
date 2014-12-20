using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeisterGui
{
    public class DatenMeisterGuiSettings : DatenMeister.Logic.Settings.MinimumDatenMeisterSettings
    {
        public DatenMeisterGuiSettings()
        {
            this.WindowTitle = "Der DatenMeister";
            this.ApplicationName = "DatenMeister";
        }
    }
}
