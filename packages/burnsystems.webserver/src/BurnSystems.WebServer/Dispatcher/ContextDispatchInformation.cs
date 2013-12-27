using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace BurnSystems.WebServer.Dispatcher
{
    /// <summary>
    /// Encapsulates the dispatching relevant information to ease change of urls
    /// </summary>
    public class ContextDispatchInformation
    {
        /// <summary>
        /// Gets or sets the http method
        /// </summary>
        public string HttpMethod
        {
            get;
            set;
        }

        /// <summary>
        /// Request uri
        /// </summary>
        public Uri RequestUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the http listener context
        /// </summary>
        public HttpListenerContext Context
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the number of dispatch tries
        /// </summary>
        private int dispatchTries;
        
        /// <summary>
        /// Stores the maximum number of redirections
        /// </summary>
        private const int MaxDispatchTries = 100;

        /// <summary>
        /// Initializes a new instance of the ContextDispatchInformation 
        /// </summary>
        /// <param name="context"></param>
        public ContextDispatchInformation(HttpListenerContext context)
        {
            this.Context = context;
            this.RequestUrl = context.Request.Url;
            this.HttpMethod = context.Request.HttpMethod;
        }

        internal void IncrementDispatchTry()
        {
            this.dispatchTries++;
            if (this.dispatchTries > MaxDispatchTries)
            {
                throw new InvalidOperationException("Maximum Dispatch-Tries reached");
            }
        }
    }
}
