using System;
using System.Text;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Helper;

namespace BurnSystems.WebServer.Responses
{
    public class StaticContentResponse : BaseDispatcher
    {
        /// <summary>
        /// Stores the value for the last cache update
        /// </summary>
        private static DateTime lastCacheUpdate = DateTime.Now;

        public string ContentType
        {
            get;
            set;
        }

        public byte[] Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content encoding
        /// </summary>
        public Encoding CharSet
        {
            get;
            set;
        }

        public StaticContentResponse(Func<ContextDispatchInformation, bool> filter)
            : base(filter)
        {
        }

        public StaticContentResponse(Func<ContextDispatchInformation, bool> filter, string contentType, byte[] content)
            : base(filter)
        {
            this.ContentType = contentType;
            this.Content = content;
        }

        public StaticContentResponse(Func<ContextDispatchInformation, bool> filter, string contentType, string content)
            : base(filter)
        {
            this.ContentType = contentType;
            this.Content = Encoding.UTF8.GetBytes(content);
            this.CharSet = Encoding.UTF8;
        }

        public override void Dispatch(ObjectActivation.IActivates activates, ContextDispatchInformation info)
        {
            if (info.CheckForCache(lastCacheUpdate, this.Content))
            {
                return;
            }

            info.Context.Response.ContentLength64 = this.Content.Length;

            if (this.CharSet != null)
            {
                info.Context.Response.ContentType = string.Format("{0}; charset={1}", this.ContentType, this.CharSet.WebName);
            }
            else
            {
                info.Context.Response.ContentType = this.ContentType;
            }

            using (var responseStream = info.Context.Response.OutputStream)
            {
                responseStream.Write(this.Content, 0, this.Content.Length);
            }
        }
    }
}
