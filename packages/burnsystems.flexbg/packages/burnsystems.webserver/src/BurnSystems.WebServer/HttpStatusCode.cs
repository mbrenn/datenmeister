using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer
{
    public class HttpStatusCode
    {
        public int Code
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }

        public HttpStatusCode(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public static readonly HttpStatusCode OK = new HttpStatusCode(200, "OK");
                
        public static readonly HttpStatusCode Unauthorized = new HttpStatusCode(401, "Unauthorized");

        public static readonly HttpStatusCode Forbidden = new HttpStatusCode(403, "Forbidden");

        public static readonly HttpStatusCode NotFound = new HttpStatusCode(404, "Not Found");

        public static readonly HttpStatusCode ServerError = new HttpStatusCode(500, "Server Error");

        public static HttpStatusCode Convert(int code)
        {
            switch (code)
            {
                case 200:
                    return OK;
                case 401:
                    return Unauthorized;
                case 403:
                    return Forbidden;
                case 500:
                    return ServerError;
                default:
                    throw new ArgumentException("Value of 'code' is not known");
            }
        }
    }
}
