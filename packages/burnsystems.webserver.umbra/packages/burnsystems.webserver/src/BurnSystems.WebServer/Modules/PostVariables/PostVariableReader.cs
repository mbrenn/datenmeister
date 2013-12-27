using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using BurnSystems.ObjectActivation;
using BurnSystems.Logging;
using System.IO;
using BurnSystems.Collections;
using BurnSystems.Net;
using System.Web;
using BurnSystems.Test;

namespace BurnSystems.WebServer.Modules.PostVariables
{
    /// <summary>
    /// Reads the postvariables from Http-Context and offers them as a dictionary
    /// </summary>
    public class PostVariableReader : IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(PostVariableReader));
             
        /// <summary>
        /// Gets or sets the http Listener context
        /// </summary>
        private HttpListenerContext ListenerContext
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the configuration
        /// </summary>
        private PostVariableReaderConfig Config
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the post variables
        /// </summary>
        private NiceDictionary<string, string> postVariables = new NiceDictionary<string, string>();
        
        /// <summary>
        /// Stores the uploaded files
        /// </summary>
        private NiceDictionary<string, WebFile> files =
            new NiceDictionary<string, WebFile>();

        /// <summary>
        /// Gets post variable
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get { return this.postVariables[key]; }
        }

        /// <summary>
        /// Gets the uploaded files
        /// </summary>
        public NiceDictionary<string, WebFile> Files
        {
            get { return this.files; }
        }

        [Inject]
        public PostVariableReader(PostVariableReaderConfig config, HttpListenerContext context)
        {
            Ensure.That(config != null);
            Ensure.That(context != null);
            
            this.Config = config;
            this.ListenerContext = context;

            // Performs the reading
            this.ReadPostVariables();
        }

        /// <summary>
        /// Reads the post variables from inputstream
        /// </summary>
        private void ReadPostVariables()
        {
            if (this.ListenerContext.Request.ContentType != null &&
                this.ListenerContext.Request.ContentType.StartsWith("multipart/form-data"))
            {
                this.ReadPostFromFormData();
            }
            else
            {
                this.ReadPostFromWWWUrlEncoded();
            }

            // Removes .x und .y.
            // These variables are set by some browsers, when the user clicks on imagebuttons
            foreach (var pair in new Dictionary<string, string>(this.postVariables))
            {
                if (pair.Key.EndsWith(".x") || pair.Value.EndsWith(".y"))
                {
                    var leftPart = pair.Key.Substring(0, pair.Key.Length - 2);
                    this.postVariables[leftPart]
                        = pair.Value;
                }
            }
        }

        /// <summary>
        /// Liest die POST-Variablen, die Form-Data
        /// </summary>
        private void ReadPostFromFormData()
        {
            // Retrieves the boundary from form data
            var contentTypes =
                this.ListenerContext.Request.ContentType.Split(new[] { ';' });
            var boundary = string.Empty;
            foreach (var part in contentTypes)
            {
                var position = part.IndexOf('=');
                if (position == -1)
                {
                    continue;
                }

                var leftPart = part.Substring(0, position).Trim();
                var rightPart = part.Substring(position + 1).Trim();

                if (leftPart.Equals("boundary", StringComparison.InvariantCultureIgnoreCase))
                {
                    boundary = rightPart;
                }
            }

            // Checks if boundary is found
            if (string.IsNullOrEmpty(boundary)) 
            {
                throw new InvalidOperationException(
                    Localization_WebServer.InvalidMultipart);
            }

            // Reads the complete stream
            using (var stream = this.ListenerContext.Request.InputStream)
            {
                var multipartReader = new MultipartFormDataReader(boundary);
                multipartReader.MaxStreamSize = this.Config.MaxPostLength;
                var data = multipartReader.ReadStream(stream);

                foreach (var part in data.Parts)
                {
                    if (string.IsNullOrEmpty(
                        part.ContentDisposition["name"]))
                    {
                        // Form data without name cannot be handled
                        continue;
                    }

                    var name = part.ContentDisposition["name"];
                    if (name[0] == '"')
                    {
                        name = name.Substring(1, name.Length - 2);
                    }

                    if (string.IsNullOrEmpty(
                        part.ContentDisposition["filename"]))
                    {
                        // Just a normal POST-Variable
                        var content = UTF8Encoding.UTF8.GetString(part.Content);
                        this.postVariables[name] =
                            content.Trim();
                    }
                    else
                    {
                        // A complete file 
                        var webFile = new WebFile(
                            part.ContentDisposition["filename"],
                            part.Content);
                        this.files[name] = webFile;
                    }
                }
            }
        }

        /// <summary>
        /// Reads urlencoded variables
        /// </summary>
        private void ReadPostFromWWWUrlEncoded()
        {
            var requestLength = this.ListenerContext.Request.ContentLength64;

            if (requestLength > this.Config.MaxPostLength)
            {
                var logMessage = new LogEntry(
                    Localization_WebServer.MaximumPostLengthExceeded,
                    LogLevel.Fail);
                Log.TheLog.LogEntry(logMessage);
            }

            var postLength = Convert.ToInt32(Math.Min(this.Config.MaxPostLength, requestLength));

            if (postLength == 0)
            {
                // Nothing to do here!
                return;
            }

            byte[] bytes = new byte[postLength];
            this.ListenerContext.Request.InputStream.Read(bytes, 0, postLength);

            using (var reader = new StreamReader(
                this.ListenerContext.Request.InputStream))
            {
                // Reads maximum size
                var content = this.ListenerContext.Request.ContentEncoding.GetString(
                    bytes);

                if (content.StartsWith("{") && content.EndsWith("}"))
                {
                    logger.Message("POST Content is probable a JSON object, use application/json to forward to JSON Deserializer");
                }

                foreach (var part in content.Split('&'))
                {
                    int positionEqual = part.IndexOf('=');
                    if (positionEqual == -1)
                    {
                        // Somethin invalid
                        continue;
                    }

                    var leftPart = part.Substring(0, positionEqual);
                    var rightPart = part.Substring(positionEqual + 1);

                    this.postVariables[HttpUtility.UrlDecode(leftPart)] =
                        HttpUtility.UrlDecode(rightPart);
                }
            }
        }

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            return this.postVariables.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.postVariables.GetEnumerator();
        }
    }
}
