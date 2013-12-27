using BurnSystems.FlexBG.Interfaces;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules
{
    /// <summary>
    /// Some helper helper methods
    /// </summary>
    public class FlexBgModule : IFlexBgRuntimeModule
    {
        [Inject(IsMandatory=true)]
        public IActivates Container
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the function that shall be executed on start of the module
        /// </summary>
        public Action<FlexBgModule> OnStart
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the function that shall be executed on start of the module
        /// </summary>
        public Action<FlexBgModule> OnShutdown
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title of the module.
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        public void Start()
        {
            if (this.OnStart != null)
            {
                this.OnStart(this);
            }
        }

        public void Shutdown()
        {
            if (this.OnShutdown != null)
            {
                this.OnShutdown(this);
            }
        }

        public override string ToString()
        {
            return this.Title;
        }

        /// <summary>
        /// The function which shall be called during start up
        /// </summary>
        /// <param name="container">Container to be used</param>
        /// <param name="title">Title of the function</param>
        /// <param name="function">Function that shall be called during start up</param>
        public static void CallDuringStartUp<T>(ActivationContainer container, string title, Action<IActivates, T> function)
        {
            container.Bind<IFlexBgRuntimeModule>().To(
                (x) =>
                {
                    var result = x.Create<FlexBgModule>();
                    result.Title = title;
                    result.OnStart = (y) => function(y.Container, y.Container.Get<T>());
                    return result;
                });
        }
    }
}
