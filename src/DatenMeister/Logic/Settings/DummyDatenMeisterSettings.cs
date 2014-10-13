using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.Settings
{
    public class DummyDatenMeisterSettings : IDatenMeisterSettings
    {
        public void InitializeForBootUp(ApplicationCore core)
        {
        }

        public void InitializeViewSet(ApplicationCore core)
        {
        }

        public void InitializeFromScratch(ApplicationCore core)
        {
        }

        public void InitializeAfterLoading(ApplicationCore core)
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
            get { return "Dummy"; }
        }

        public string WindowTitle
        {
            get { return "Window Title"; }
        }

        public DataProvider.Xml.XmlSettings ExtentSettings
        {
            get { return DatenMeister.DataProvider.Xml.XmlSettings.Empty; }
        }
    }
}
