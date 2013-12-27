using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.WebServer.Dispatcher;

namespace BurnSystems.WebServer.Modules.UserManagement
{
    public class WebAuthorisationEntry
    {        
        public Func<ContextDispatchInformation, bool> Selector
        {
            get;
            set;
        }

        public TokenCheckpoint Checkpoint
        {
            get;
            set;
        }

        public WebAuthorisationEntry(Func<ContextDispatchInformation, bool> selector, TokenCheckpoint checkpoint)
        {
            this.Selector = selector;
            this.Checkpoint = checkpoint;
        }
    }
}
