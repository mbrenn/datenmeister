using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Parser
{
    /// <summary>
    /// Stores the template parser for the webservice
    /// </summary>
    public class TemplateParser : ITemplateParser, IDisposable
    {
        /// <summary>
        /// Stores the template service
        /// </summary>
        private BurnSystems.Parser.TemplateParser parser = new BurnSystems.Parser.TemplateParser();

        /// <summary>
        /// Initializes a new instance of the template parser instance
        /// </summary>
        public TemplateParser()
        {
        }

        public string Parse<T>(string template, T model, Dictionary<string, object> bag = null)
        {
            if (bag != null)
            {
                foreach (var pair in bag)
                {
                    this.parser.AddVariable(pair.Key, pair.Value);
                }
            }

            this.parser.AddVariable("Model", model);

            return this.parser.Parse(template);
        }

        /// <summary>
        /// Disposes the parser and including templateservice
        /// </summary>
        public void Dispose()
        {
        }
    }
}
