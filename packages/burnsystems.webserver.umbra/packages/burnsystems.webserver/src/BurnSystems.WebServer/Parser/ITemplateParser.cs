using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Parser
{
    /// <summary>
    /// Defines the template parser
    /// </summary>
    public interface ITemplateParser
    {
        /// <summary>
        /// Parses the template
        /// </summary>
        /// <param name="template">Template to be parsed</param>
        /// <param name="model">Model to be used</param>
        /// <param name="cacheName">Name of the cache</param>
        string Parse<T>(string template, T model, Dictionary<string, object> bag = null);
    }
}
