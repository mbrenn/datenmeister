using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.WebServer.Responses.FileRequests;

namespace BurnSystems.WebServer.Helper
{
    public class MimeTypeConverter
    {
        /// <summary>
        /// Stores the list of known mimetypes
        /// </summary>
        private List<MimeTypeInfo> infos = new List<MimeTypeInfo>();

        public void Add(MimeTypeInfo info)
        {
            this.infos.Add(info);
        }

        /// <summary>
        /// Converts given file extenstion to mimetype or returns null, if unknwon
        /// </summary>
        /// <param name="fileExtension">File extension to be converted</param>
        /// <returns>Found mimetype or null</returns>
        public MimeTypeInfo ConvertFromExtension(string fileExtension)
        {
            return
                this.infos
                    .Where(x => x.FileExtension == fileExtension)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Gets the default mimetype converter with the most important mimetypes
        /// </summary>
        public static MimeTypeConverter Default
        {
            get
            {
                var result = new MimeTypeConverter();

                result.Add(new MimeTypeInfo(".txt", "text/plain", Encoding.UTF8)
                    {
                        DoDeflate = true
                    });
                result.Add(new MimeTypeInfo(".html", "text/html", Encoding.UTF8)
                    {
                        DoDeflate = true
                    });
                result.Add(new MimeTypeInfo(".htm", "text/html", Encoding.UTF8)
                    {
                        DoDeflate = true
                    });

                result.Add(new MimeTypeInfo(".css", "text/css", Encoding.UTF8)
                    {
                        DoDeflate = true
                    });

                result.Add(new MimeTypeInfo(".js", "application/javascript", Encoding.UTF8)
                    {
                        DoDeflate = true
                    });

                result.Add(new MimeTypeInfo(".jpg", "image/jpeg"));
                result.Add(new MimeTypeInfo(".png", "image/png"));
                result.Add(new MimeTypeInfo(".gif", "image/gif"));
                result.Add(new MimeTypeInfo(".svg", "image/svg+xml"));

                result.Add(new MimeTypeInfo(".bspx", typeof(BspxFileRequest)));
                result.Add(new MimeTypeInfo(".less", typeof(LessFileRequest)));

                return result;
            }
        }
    }
}
