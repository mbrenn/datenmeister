using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using BurnSystems.WebServer.Parser;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Dispatcher;
using System.Web.Script.Serialization;

namespace BurnSystems.WebServer.Responses
{
    /// <summary>
    /// Returns an error page
    /// </summary>
    public class ErrorResponse : BaseDispatcher
    {
        [Inject(Server.TemplateParserBindingName)]
        public TemplateParser TemplateParser
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the ErrorResponse class.
        /// </summary>
        public ErrorResponse()
            : base(DispatchFilter.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ErrorResponse class.
        /// </summary>
        public ErrorResponse(HttpStatusCode code)
            : base(DispatchFilter.None)
        {
            this.Set(code);
        }

        public int Code
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public void Set(HttpStatusCode code)
        {
            Ensure.That(code != null);
            this.Code = code.Code;
            this.Message = code.Message;
            this.Title = code.Message;
        }

        /// <summary>
        /// Gives response to listener context
        /// </summary>
        /// <param name="info">Context to be used</param>
        public override void Dispatch(IActivates container, ContextDispatchInformation info)
        {
            Ensure.That(this.Code != 0, "Code is 0");
            Ensure.That(this.Message != null, "Code is null");
            Ensure.That(this.TemplateParser != null, "this.TemplateParser == null");

            var content = Localization_WebServer.Error;

            var model = new
            {
                title = this.Title,
                url = info.Context.Request.Url.ToString(),
                message = this.Message,
                code = this.Code.ToString()
            };

            string resultText;
            var requestedWithHeader = info.Context.Request.Headers["X-Requested-With"];
            if (requestedWithHeader != null && requestedWithHeader.Contains("XMLHttpRequest"))
            {
                var serializer = new JavaScriptSerializer();
                resultText = serializer.Serialize(model);
                info.Context.Response.ContentType = "application/json";
            }
            else
            {
                resultText = this.TemplateParser.Parse(content, model, null);
            }

            info.Context.Response.StatusCode = this.Code;
            using (var response = info.Context.Response.OutputStream)
            {
                var bytes = Encoding.UTF8.GetBytes(resultText);
                response.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// Throws a 404 page
        /// </summary>
        /// <param name="container">Container to be used</param>
        /// <param name="context">HTTP Context</param>
        public static void Throw404(ObjectActivation.IActivates container, ContextDispatchInformation info, string additionalMessage = null)
        {
            var errorResponse = container.Create<ErrorResponse>();
            errorResponse.Set(HttpStatusCode.NotFound);

            if (!string.IsNullOrEmpty(additionalMessage))
            {
                errorResponse.Message = additionalMessage;
            }

            errorResponse.Dispatch(container, info);
        }
    }
}
