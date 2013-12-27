using BurnSystems.Logging;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Helper;
using dotless.Core;
using System.IO;
using System.Text;

namespace BurnSystems.WebServer.Responses.FileRequests
{
    class LessFileRequest : BaseDispatcher, IFileRequestDispatcher
    {
        private ILog logger = new ClassLogger(typeof(LessFileRequest));

        private static bool firstRun = true;

        public string PhysicalPath
        {
            get;
            set;
        }

        public string VirtualPath
        {
            get;
            set;
        }

        public LessFileRequest()
            : base(DispatchFilter.All)
        {
        }

        /// <summary>
        /// Dispatches the object
        /// </summary>
        /// <param name="container">Container for activations</param>
        /// <param name="info">Context being used</param>
        public override void Dispatch(ObjectActivation.IActivates container, ContextDispatchInformation info)
        {
            if (firstRun)
            {
                logger.LogEntry(new LogEntry("Using dotless to compile .less-files", LogLevel.Notify));
                logger.LogEntry(new LogEntry("http://www.dotlesscss.com", LogLevel.Notify));
                firstRun = false;
            }

            // File is not sent out at once, file is sent out by by chunks
            if (info.CheckForCache(File.GetLastWriteTime(this.PhysicalPath), Encoding.UTF8.GetBytes(this.PhysicalPath)))
            {
                // File has not been modified
                return;
            }

            var lessFile = File.ReadAllText(this.PhysicalPath);
            var cssFile = Less.Parse(lessFile);

            var bytes = Encoding.UTF8.GetBytes(cssFile);
            info.Context.Response.ContentType = "text/css; charset=utf-8";

            using (var stream = info.Context.Response.OutputStream)
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
