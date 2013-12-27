using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Filters
{
    /// <summary>
    /// Denies all requests
    /// </summary>
    public class DenyAllFilter : IRequestFilter
    {
        public void BeforeDispatch(IActivates container, Dispatcher.ContextDispatchInformation information, out bool cancel)
        {
            var errorResponse = container.Create<ErrorResponse>();
            errorResponse.Set (HttpStatusCode.Forbidden);
            errorResponse.Dispatch(container, information);
            cancel = true;
        }

        public void AfterDispatch(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation information, out bool cancel)
        {
            cancel = true;
        }

        public void AfterRequest(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation information)
        {
        }
    }
}
