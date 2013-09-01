using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    public class DotNetExtent : IURIExtent
    {
        /// <summary>
        /// Stores the context uri
        /// </summary>
        private string contextUri;

        /// <summary>
        /// Stores the elements
        /// </summary>
        private List<IObject> elements = new List<IObject>();

        public DotNetExtent(string contextUri)
        {
            this.contextUri = contextUri;
        }

        /// <summary>
        /// Stores the changes
        /// </summary>
        public void StoreChanges()
        {            
            // Not required. 
        }

        /// <summary>
        /// Gets the context uri
        /// </summary>
        /// <returns>The context uri</returns>
        public string ContextURI()
        {
            return this.contextUri;
        }

        /// <summary>
        /// Gets the elements
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IObject> Elements()
        {
            return this.elements;
        }

        /// <summary>
        /// Adds an object to the extent
        /// </summary>
        /// <param name="element">Element to be added</param>
        public void Add(object element)
        {
            this.elements.Add(new DotNetObject(this, element));
        }

        public IObject CreateObject()
        {
            throw new InvalidOperationException("Dotnet objects cannot be created, no type information is available");
        }

        public void RemoveObject(IObject element)
        {
            lock (this.elements)
            {
                this.elements.Remove(element);
            }
        }
    }
}
