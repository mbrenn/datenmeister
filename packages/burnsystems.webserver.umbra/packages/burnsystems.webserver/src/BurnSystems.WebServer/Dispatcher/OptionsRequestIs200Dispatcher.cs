using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Dispatcher
{
    /// <summary>
    /// Defines a dispatcher that will always return 200, if an options request has been received
    /// </summary>
    public class OptionsRequestIs200Dispatcher : IRequestDispatcher
    {
        public bool IsResponsible(ObjectActivation.IActivates container, ContextDispatchInformation information)
        {
            if (information.Context.Request.HttpMethod.ToUpper() == "OPTIONS")
            {
                return true;
            }

            return false;
        }

        public void Dispatch(ObjectActivation.IActivates container, ContextDispatchInformation information)
        {
            information.Context.Response.StatusCode = 200;
        }

        public void FinishDispatch(ObjectActivation.IActivates container, ContextDispatchInformation information)
        {
            // Nothing to do here
        }
    }
}
