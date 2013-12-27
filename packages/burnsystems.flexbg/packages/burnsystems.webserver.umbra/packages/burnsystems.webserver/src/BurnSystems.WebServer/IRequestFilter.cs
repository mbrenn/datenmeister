using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Dispatcher;

namespace BurnSystems.WebServer
{
    /// <summary>
    /// The request filter is called for every HTTP Request and is allowed to
    /// intercept the request or logging the activities. 
    /// </summary>
    public interface IRequestFilter
    {
        /// <summary>
        /// Called before the dispatching starts
        /// </summary>
        /// <param name="container">Container being used </param>
        /// <param name="information">Dispatch information for current context</param>
        /// <param name="cancel">Can be set to true, if request shall be cancelled</param>
        void BeforeDispatch(IActivates container, ContextDispatchInformation information, out bool cancel);

        /// <summary>
        /// Called after dispatching has been done, but before the dispatcher has been executed
        /// </summary>
        /// <param name="container">Container being used </param>
        /// <param name="information">Dispatch information for current context</param>
        /// <param name="cancel">Can be set to true, if request shall be cancelled</param>
        void AfterDispatch(IActivates container, ContextDispatchInformation information, out bool cancel);

        /// <summary>
        /// Called after the the request has beenperformed
        /// </summary>
        /// <param name="container">Container being used</param>
        /// <param name="information">Information used for dispatching</param>
        void AfterRequest(IActivates container, ContextDispatchInformation information);
    }
}
