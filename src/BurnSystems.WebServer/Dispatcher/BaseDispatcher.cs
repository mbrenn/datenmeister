using System;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;

namespace BurnSystems.WebServer.Dispatcher
{
    /// <summary>
    /// The base dispatcher, offering filter possibility for responsibility check
    /// </summary>
    public abstract class BaseDispatcher : IRequestDispatcher
    {
        /// <summary>
        /// Stores the filter
        /// </summary>
        private Func<ContextDispatchInformation, bool> filter;

        public BaseDispatcher(Func<ContextDispatchInformation, bool> filter)
        {
            Ensure.That(filter != null);
            this.filter = filter;
        }

        public bool IsResponsible(IActivates container, ContextDispatchInformation info)
        {
            return this.filter(info);
        }

        public abstract void Dispatch(IActivates container, ContextDispatchInformation context);

        public virtual void FinishDispatch(IActivates container, ContextDispatchInformation context)
        {
        }
    }
}
