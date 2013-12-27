using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.UserManagement
{
    [Serializable]
    public class Token
    {
        public Token()
        {
        }

        public Token(Guid guid, string name)
        {
            this.Id = guid;
            this.Name = name;
        }

        public Guid Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
