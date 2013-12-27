using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Filters
{
    public class DenyByUrlFilter : IRequestFilter
    {
        private Func<ContextDispatchInformation, bool> requestFilter;

        public DenyByUrlFilter(Func<ContextDispatchInformation, bool> filter)
        {
            this.requestFilter = filter;
        }

        public void BeforeDispatch(IActivates container, Dispatcher.ContextDispatchInformation information, out bool cancel)
        {
            if (requestFilter(information))
            {
                var errorResponse = container.Create<ErrorResponse>();
                errorResponse.Set(HttpStatusCode.Forbidden);
                errorResponse.Dispatch(container, information);
                cancel = true;
            }
            else
            {
                cancel = false;
            }
        }

        public void AfterDispatch(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation information, out bool cancel)
        {
            cancel = false;
        }

        public void AfterRequest(ObjectActivation.IActivates container, Dispatcher.ContextDispatchInformation information)
        {
        }
    }
}
