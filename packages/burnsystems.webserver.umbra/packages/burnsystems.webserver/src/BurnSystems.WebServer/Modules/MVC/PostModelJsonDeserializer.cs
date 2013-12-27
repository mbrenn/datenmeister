using BurnSystems.ObjectActivation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    public class PostModelJsonDeserializer
    {
        private JsonSerializer serializer = new JsonSerializer();

        /// <summary>
        /// Gets or sets the http listener context.
        /// The Deserialized object is retrieved from the POST data
        /// </summary>
        [Inject(IsMandatory = true)]
        public HttpListenerContext context
        {
            get;
            set;
        }

        /// <summary>
        /// Performs the deserialization in dependency of the given request and the needed type of object
        /// </summary>
        /// <param name="typeOfObject">Type of the object to be retrieved</param>
        /// <returns></returns>
        public object Deserialize(Type typeOfObject)
        {
            var fullDebug = true;
            using (var reader = new StreamReader(this.context.Request.InputStream))
            {
                if (fullDebug)
                {
                    var readText = reader.ReadToEnd();
                    using (var stringReader = new StringReader(readText))
                    {
                        var returnedObject = serializer.Deserialize(stringReader, typeOfObject);
                        return returnedObject;
                    }
                }
                else
                {
                    return serializer.Deserialize(reader, typeOfObject);
                }
            }
        }
    }
}
