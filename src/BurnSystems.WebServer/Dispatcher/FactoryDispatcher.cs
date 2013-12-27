using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Dispatcher
{
    /// <summary>
    /// Dispatches the object by creating an instance
    /// </summary>
    /// <typeparam name="T">Type of the dispatchitem to be created</typeparam>
    public class FactoryDispatcher<T> : BaseDispatcher where T : IRequestDispatcher, new()
    {
        /// <summary>
        /// Stores the factory method being used
        /// </summary>
        private Func<T> factoryMethod;

        public FactoryDispatcher(Func<ContextDispatchInformation, bool> filter)
            : base(filter)
        {
            this.factoryMethod = () => new T();
        }

        public FactoryDispatcher(Func<ContextDispatchInformation, bool> filter, Func<T> factoryMethod)
            : base(filter)
        {
            this.factoryMethod = factoryMethod;
        }

        public override void Dispatch(ObjectActivation.IActivates container, ContextDispatchInformation context)
        {
            var instance = this.factoryMethod();
            instance.Dispatch(container, context);
            instance.FinishDispatch(container, context);

            if (instance is IDisposable)
            {
                (instance as IDisposable).Dispose();
            }
        }
    }
}
