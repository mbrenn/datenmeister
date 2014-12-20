using DatenMeister.DataProvider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.Settings
{
    public class MinimumDatenMeisterSettings :IDatenMeisterSettings
    {       
        public void InitializeForBootUp(ApplicationCore core)
        {
        }

        public void InitializeViewSet(ApplicationCore core)
        {
        }

        public void FinalizeExtents(ApplicationCore core, bool wasLoading)
        {
        }

        public void InitializeForExampleData(ApplicationCore core)
        {
        }

        public void StoreViewSet(ApplicationCore core)
        {
        }

        public string ApplicationName
        {
            get;
            set;
        }

        public string WindowTitle
        {
            get;
            set;
        }

        public DataProvider.Xml.XmlSettings ExtentSettings
        {
            get { return XmlSettings.Empty; }
        }
    }
}
