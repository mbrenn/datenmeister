using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    public class TextActionResult : BaseActionResult, IActionResult
    {
        /// <summary>
        /// Gets or sets the mimetype
        /// </summary>
        public string ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sends the text to be sent
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the TestActionResult class.
        /// </summary>
        /// <param name="text">Text to be returned to browser</param>
        /// <param name="contentType">Contenttype to be used</param>
        public TextActionResult(string text, string contentType)
        {
            this.ContentType = contentType;
            this.Text = text;
        }

        /// <summary>
        /// Executes the request
        /// </summary>
        /// <param name="listenerContext">Listenercontext to be executed</param>
        public void Execute(HttpListenerContext listenerContext, IActivates container)
        {
            listenerContext.Response.ContentType = this.ContentType;
            this.SendResult(listenerContext, this.Text);
        }
    }
}
