using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.ClientActions
{
    public class ClientActionInformation
    {
        public ClientActionInformation()
        {
            this.actions = new List<IClientAction>();
        }

        public List<IClientAction> actions
        {
            get;
            private set;
        }
    }
}
