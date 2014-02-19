using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.ClientActions
{
    public class RefreshBrowserWindow: IClientAction
    {
        public string type
        {
            get { return "RefreshBrowserWinder"; }
        }
    }
}
