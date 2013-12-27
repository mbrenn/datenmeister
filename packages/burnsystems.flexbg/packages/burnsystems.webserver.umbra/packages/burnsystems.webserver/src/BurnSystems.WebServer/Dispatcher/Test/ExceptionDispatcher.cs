using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace BurnSystems.WebServer.Dispatcher.Test
{
    public class ExceptionDispatcher : BaseDispatcher
    {
        public ExceptionDispatcher(Func<ContextDispatchInformation, bool> filter)
            : base(filter)
        {
        }

        public override void Dispatch(ObjectActivation.IActivates activates, ContextDispatchInformation context)
        {
            throw new InvalidOperationException("This is your exception. <b>Catch it, if you can.</b>");
        }
    }
}
