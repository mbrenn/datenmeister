using System;

namespace BurnSystems.WebServer.Dispatcher
{
    /// <summary>
    /// Performs a relocation with all uris, where we have a match
    /// </summary>
    public class RelocationDispatcher : BaseDispatcher
    {
        /// <summary>
        /// Gets the url, where we will be relocated to. 
        /// </summary>
        public Uri ToUrl
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the RelocationDispatcher class.
        /// </summary>
        /// <param name="filter">Filter being used</param>
        /// <param name="toUrl">Target url</param>
        public RelocationDispatcher(Func<ContextDispatchInformation, bool> filter, Uri toUrl)
            : base(filter)
        {
            this.ToUrl = toUrl;
        }

        /// <summary>
        /// Initializes a new instance of the RelocationDispatcher class.
        /// </summary>
        /// <param name="filter">Url, where relocation starts being used</param>
        /// <param name="toUrl">Target url</param>
        public RelocationDispatcher(string fromUrl, Uri toUrl)
            : base(DispatchFilter.ByExactUrl(fromUrl))
        {
            this.ToUrl = toUrl;
        }

        /// <summary>
        /// Initializes a new instance of the RelocationDispatcher class.
        /// </summary>
        /// <param name="filter">Url, where relocation starts being used</param>
        /// <param name="toUrl">Target url</param>
        public RelocationDispatcher(string fromUrl, string toUrl)
            : base(DispatchFilter.ByExactUrl(fromUrl))
        {
            this.ToUrl = new Uri(toUrl, UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Initializes a new instance of the RelocationDispatcher class.
        /// </summary>
        /// <param name="filter">Url, where relocation starts being used</param>
        /// <param name="toUrl">Target url</param>
        public RelocationDispatcher(Func<ContextDispatchInformation, bool> filter, string toUrl)
            : base(filter)
        {
            this.ToUrl = new Uri(toUrl, UriKind.RelativeOrAbsolute);
        }
        
        /// <summary>
        /// Dispatches the instance
        /// </summary>
        /// <param name="container">Container for activationstructures</param>
        /// <param name="context">Context information being used</param>
        public override void Dispatch(ObjectActivation.IActivates container, ContextDispatchInformation context)
        {
            context.RequestUrl = new Uri(context.RequestUrl, this.ToUrl);
            Listener.PerformDispatch(container, context);
        }
    }
}
