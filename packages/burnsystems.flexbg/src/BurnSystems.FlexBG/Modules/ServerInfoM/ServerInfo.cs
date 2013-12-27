using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.ServerInfoM
{    
    public class ServerInfo
    {
        public string Name
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string AdminName
        {
            get;
            set;
        }

        public string AdminEMail
        {
            get;
            set;
        }

        public string PasswordSalt
        {
            get;
            set;
        }

        public string ServerVersion
        {
            get;
            set;
        }

        public DateTime ServerStartUp
        {
            get;
            set;
        }
    }
}
