using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Dispatcher;

namespace BurnSystems.WebServer
{
    /// <summary>
    /// Dispatches the request, if responsible
    /// </summary>
    public interface IRequestDispatcher
    {
        /// <summary>
        /// Checks, if the dispatcher is responsible for the request
        /// </summary>
        /// <param name="context">Context of Http Request</param>
        /// <returns>true, if this dispatcher shall dispatch the request</returns>
        bool IsResponsible(IActivates container, ContextDispatchInformation information);

        /// <summary>
        /// Dispatches the request
        /// </summary>
        /// <param name="context">Context of the request</param>
        void Dispatch(IActivates container, ContextDispatchInformation information);

        /// <summary>
        /// This method is called after having called <c>Dispatch</c>.
        /// Purpose of the method is to offer an override of Dispatch, where several datastructures
        /// are built up and can be send during <c>FinishDispatch</c>. 
        /// </summary>
        /// <param name="container">Activation container</param>
        /// <param name="information">Information about the request</param>
        void FinishDispatch(IActivates container, ContextDispatchInformation information);
    }
}
