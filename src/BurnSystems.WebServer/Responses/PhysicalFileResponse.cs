using System.IO;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Helper;
using System.Text;
using System.IO.Compression;

namespace BurnSystems.WebServer.Responses
{
    public class PhysicalFileResponse : IRequestDispatcher
    {
        /// <summary>
        /// Gets or sets converter
        /// </summary>
        [Inject]
        public MimeTypeConverter MimeTypeConverter
        {
            get;
            set;
        }

        public int FileChunkSize
        {
            get;
            set;
        }

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

        /// <summary>
        /// Gets or sets a value indication whether deflating shall be done
        /// </summary>
        public bool DoDeflate
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the PhysicalFileResponse class;
        /// </summary>
        public PhysicalFileResponse()
        {
            this.FileChunkSize = 64 * 1024;
        }

        public bool IsResponsible(ObjectActivation.IActivates container, ContextDispatchInformation info)
        {
            return File.Exists(this.PhysicalPath);
        }

        public void Dispatch(ObjectActivation.IActivates container, ContextDispatchInformation info)
        {
            var extension = Path.GetExtension(this.PhysicalPath);
            var mimeInfo = this.MimeTypeConverter.ConvertFromExtension(extension);

            // Set some header
            if (mimeInfo != null)
            {
                if (mimeInfo.FileRequestDispatcher != null)
                {
                    // Finds file request dispatcher
                    var fileRequestDispatcher = container.Create(mimeInfo.FileRequestDispatcher) as IFileRequestDispatcher;
                    fileRequestDispatcher.PhysicalPath = this.PhysicalPath;
                    fileRequestDispatcher.VirtualPath = this.VirtualPath;

                    fileRequestDispatcher.Dispatch(container, info);

                    // Everything is done
                    return;
                }

                if (mimeInfo.MimeType != null)
                {
                    if (mimeInfo.CharsetEncoding != null)
                    {
                        info.Context.Response.ContentType = string.Format("{0}; charset={1}", mimeInfo.MimeType, mimeInfo.CharsetEncoding.WebName);
                    }
                    else
                    {
                        info.Context.Response.ContentType = mimeInfo.MimeType;
                    }
                }

                if (mimeInfo.DoDeflate)
                {
                    this.DoDeflate = true;
                }
            }

            // File is not sent out at once, file is sent out by by chunks
            if (info.CheckForCache(File.GetLastWriteTime(this.PhysicalPath), Encoding.UTF8.GetBytes(this.PhysicalPath)))
            {
                // File has not been modified
                return;
            }

            Stream httpOutputStream = null;
            DeflateStream deflateStream = null;
            try
            {
                httpOutputStream = info.Context.Response.OutputStream;
                var outputStream = httpOutputStream;

                // Checks, if deflating is ok                
                using (var stream = new FileStream(this.PhysicalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var inputStream = stream;

                    // Check, if we'd like to have a deflating
                    if (this.DoDeflate && info.IsDeflatingAccepted())
                    {
                        deflateStream = new DeflateStream(httpOutputStream, CompressionMode.Compress);
                        outputStream = deflateStream;
                        info.Context.Response.AddHeader("Content-Encoding", "deflate");
                    }
                    else
                    {
                        info.Context.Response.ContentLength64 = stream.Length;
                    }

                    var restSize = stream.Length;
                    var byteBuffer = new byte[this.FileChunkSize];

                    while (restSize > 0)
                    {
                        var read = inputStream.Read(byteBuffer, 0, this.FileChunkSize);
                        outputStream.Write(byteBuffer, 0, read);

                        restSize -= read;
                    }
                }
            }
            finally
            {
                if (deflateStream != null)
                {
                    deflateStream.Flush();
                    deflateStream.Dispose();
                }

                if (httpOutputStream != null)
                {
                    httpOutputStream.Dispose();
                }
            }
        }

        public void FinishDispatch(IActivates container, ContextDispatchInformation context)
        {
        }
    }
}
