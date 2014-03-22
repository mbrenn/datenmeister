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
        /// Gets or sets the mapping between .Net Types and DatenMeister Types
        /// </summary>
        public DotNetTypeMapping Mapping
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the context uri
        /// </summary>
        private string extentUri;

        /// <summary>
        /// Stores the elements
        /// </summary>
        private List<IObject> elements = new List<IObject>();

        /// <summary>
        /// Initializes a new instance of the DotNetExtent
        /// </summary>
        /// <param name="extentUri">Uri of the context</param>
        public DotNetExtent(string extentUri)
        {
            this.Mapping = new DotNetTypeMapping();
            this.extentUri = extentUri;
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
            return this.extentUri;
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

        public IObject CreateObject(IObject type)
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
