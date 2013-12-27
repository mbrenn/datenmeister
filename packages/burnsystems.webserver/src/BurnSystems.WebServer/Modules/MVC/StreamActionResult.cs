using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    public class StreamActionResult : IActionResult
    {
        /// <summary>
        /// GEts or sets the stream chunk size
        /// </summary>
        public int StreamChunkSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stream to be returnd
        /// </summary>
        public Stream ResultStream
        {
            get;
            set;
        }

        public void Execute(System.Net.HttpListenerContext listenerContext, IActivates container)
        {
            this.StreamChunkSize = 64 * 1024;
            listenerContext.Response.ContentLength64 = this.ResultStream.Length;

            using (var responseStream = listenerContext.Response.OutputStream)
            {
                listenerContext.Response.ContentLength64 = responseStream.Length;

                var restSize = this.ResultStream.Length;
                var byteBuffer = new byte[this.StreamChunkSize];

                while (restSize > 0)
                {
                    var read = this.ResultStream.Read(byteBuffer, 0, this.StreamChunkSize);
                    responseStream.Write(byteBuffer, 0, read);

                    restSize -= read;
                }
            }
        }
    }
}
