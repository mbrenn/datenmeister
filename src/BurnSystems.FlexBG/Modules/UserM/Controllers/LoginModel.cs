using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.UserM.Controllers
{
    public class LoginModel
    {
        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public bool IsPersistent
        {
            get;
            set;
        }
    }
}
