using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Parser;
using BurnSystems.WebServer.Responses;
using BurnSystems.WebServer.Dispatcher;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using BurnSystems.Synchronisation;

namespace BurnSystems.WebServer
{
    /// <summary>
    /// Listener receiving the http requests and dispatching them to the correct place
    /// </summary>
    internal class Listener : IDisposable
    {
        /// <summary>
        /// Defines the class logger
        /// </summary>
        private ClassLogger logger = new ClassLogger(typeof(Listener));

        /// <summary>
        /// Httplistener, which receives the requests
        /// </summary>
        private HttpListener httpListener;

        /// <summary>
        /// Thread, which listens to the http-socket
        /// </summary>
        private Thread httpThread;

        /// <summary>
        /// Value indicating whether the webserver is currently running
        /// </summary>
        private volatile bool isRunning = false;

        /// <summary>
        /// Stoers the activation container
        /// </summary>
        private ActivationBlock activationBlock;

        /// <summary>
        /// Stores the list of request filters
        /// </summary>
        private List<IRequestFilter> requestFilters = new List<IRequestFilter>();

        /// <summary>
        /// Initializes a new instance of the Listener instance
        /// </summary>
        /// <param name="activationBlock">Defines the activation container</param>
        /// <param name="prefixes">Prefixes to be listened to</param>
        internal Listener(ActivationBlock activationBlock, IEnumerable<string> prefixes)
        {
            this.activationBlock = activationBlock;
            this.httpListener = new HttpListener();
            foreach (var prefix in prefixes)
            {
                logger.LogEntry(new LogEntry(
                    string.Format(Localization_WebServer.AddedPrefix, prefix),
                    LogLevel.Notify));

                this.httpListener.Prefixes.Add(prefix);
            }
        }

        /// <summary>
        /// Starts listening
        /// </summary>
        public void StartListening()
        {
            this.requestFilters = this.activationBlock.GetAll<IRequestFilter>().ToList();

            try
            {
                this.httpListener.Start();
                this.isRunning = true;

                this.httpThread = new Thread(new ThreadStart(this.HttpThreadEntry));
                this.httpThread.Start();
            }
            catch (HttpListenerException exc)
            {
                // User requires administrator rights
                string message = string.Format(
                    Localization_WebServer.HttpListenerException,
                    exc.Message);
                Log.TheLog.LogEntry(
                    new LogEntry(message, LogLevel.Critical));

                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Stops listening
        /// </summary>
        public void StopListening()
        {
            if (this.isRunning)
            {
                this.httpListener.Stop();
                this.isRunning = false;
                if (this.httpThread != null)
                {
                    this.httpThread.Join();
                    this.httpThread = null;
                }
            }
        }

        /// <summary>
        /// Thread entry for receiving http-requests
        /// </summary>
        public void HttpThreadEntry()
        {
            try
            {
                while (this.isRunning)
                {
                    var context = this.httpListener.GetContext();
                    try
                    {
                        Task.Factory.StartNew(() => this.ExecuteHttpRequest(context));
                    }
                    catch (Exception exc)
                    {
                        logger.LogEntry(
                            new LogEntry(
                                String.Format(
                                    Localization_WebServer.ExceptionDuringListening,
                                    exc.Message),
                                LogLevel.Message));
                    }
                }
            }
            catch (HttpListenerException)
            {
                // Listener has been stopped.
            }
            catch (InvalidOperationException exc)
            {
                // Might be thrown, when server gets stopped before it has really started
                logger.Fail("Exception during HttpThreadEntry: " + exc.ToString());
            }
        }

        /// <summary>
        /// Executes the http request itself
        /// </summary>
        /// <param name="context"></param>
        private void ExecuteHttpRequest(object value)
        {
            using (var threadWatcher = ThreadWatcher.WatchThread(Thread.CurrentThread, TimeSpan.FromSeconds(600)))
            {
                var context = value as HttpListenerContext;
                if (context == null)
                {
                    throw new ArgumentException("value is not HttpListenerContext");
                }

                var webRequestContainer = new ActivationContainer("WebRequest");
                webRequestContainer.Bind<HttpListenerContext>().ToConstant(context);

                using (var block = new ActivationBlock("WebRequest", webRequestContainer, this.activationBlock))
                {
                    try
                    {
                        var info = new ContextDispatchInformation(context);
                        webRequestContainer.Bind<ContextDispatchInformation>().ToConstant(info);

                        try
                        {
                            // Go through all requestfilters
                            foreach (var filter in this.requestFilters)
                            {
                                var cancel = false;
                                filter.BeforeDispatch(block, info, out cancel);
                                if (cancel)
                                {
                                    // Has been cancelled by request filter
                                    return;
                                }
                            }

                            // Perform the dispatch
                            var found = PerformDispatch(block, info);

                            // Go through all requestfilters
                            foreach (var filter in this.requestFilters)
                            {
                                filter.AfterRequest(block, info);
                            }

                            if (!found)
                            {
                                // Throw 404
                                var errorResponse = this.activationBlock.Create<ErrorResponse>();
                                errorResponse.Set(HttpStatusCode.NotFound);
                                errorResponse.Dispatch(block, info);
                            }

                        }
                        catch (Exception exc)
                        {
                            var errorResponse = this.activationBlock.Create<ErrorResponse>();
                            errorResponse.Set(HttpStatusCode.ServerError);
                            errorResponse.Message = exc.ToString();
                            errorResponse.Dispatch(block, info);

                            logger.LogEntry(LogEntry.Format(
                                LogLevel.Fail,
                                Localization_WebServer.ExceptionDuringWebRequest,
                                context.Request.Url.ToString(),
                                exc.Message));
                        }
                        finally
                        {
                            context.Response.Close();
                        }
                    }
                    catch (Exception exc)
                    {
                        // Default, can't do anything
                        logger.LogEntry(new LogEntry(exc.Message, LogLevel.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Performs the dispatch for the activation block an context dispatch information
        /// </summary>
        /// <param name="block">Block being used</param>
        /// <param name="info">Information about dispatching</param>
        /// <returns>true, if dispatch has been performed</returns>
        public static bool PerformDispatch(IActivates block, ContextDispatchInformation info)
        {
            info.IncrementDispatchTry();

            var found = false;
            foreach (var dispatcher in block.GetAll<IRequestDispatcher>())
            {
                if (dispatcher.IsResponsible(block, info))
                {
                    foreach (var filter in block.GetAll<IRequestFilter>())
                    {
                        var cancel = false;
                        filter.AfterDispatch(block, info, out cancel);
                        if (cancel)
                        {
                            return true;
                        }
                    }

                    dispatcher.Dispatch(block, info);
                    dispatcher.FinishDispatch(block, info);
                    found = true;
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Disposes the webserver
        /// </summary>
        public void Dispose()
        {
            if (this.isRunning)
            {
                this.StopListening();
            }

            if ((this.httpListener as IDisposable)!= null)
            {
                (this.httpListener as IDisposable).Dispose();
            }
        }
    }
}
