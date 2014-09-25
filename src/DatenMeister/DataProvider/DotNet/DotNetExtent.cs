using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BurnSystems.Test;

namespace DatenMeister.DataProvider.DotNet
{
    public class DotNetExtent : IURIExtent
    {
        /// <summary>
        /// Gets or sets the pool, where the object is stored
        /// </summary>
        public IPool Pool
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mapping between .Net Types and DatenMeister Types
        /// </summary>
        public DotNetTypeMapping Mapping
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a flag whether the extent is currently dirty
        /// That means, it has unsaved changes
        /// </summary>
        public bool IsDirty
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
        public IReflectiveSequence Elements()
        {
            lock (this.elements)
            {
                return new DotNetExtentReflectiveSequence(this);
            }
        }

        /// <summary>
        /// Adds the default mapping
        /// </summary>
        public void AddDefaultMappings()
        {
            Ensure.That(DatenMeister.Entities.AsObject.Uml.Types.Type != null, "'DatenMeister.Entities.AsObject.Uml.Types.Type' Type is not set.");
            this.Mapping.Add(typeof(DatenMeister.Entities.UML.Type), DatenMeister.Entities.AsObject.Uml.Types.Type);
        }

        /// <summary>
        /// Creates a reflective sequence for all items within the sequence
        /// </summary>
        private class DotNetExtentReflectiveSequence : ListWrapperReflectiveSequence<IObject>
        {
            private DotNetExtent extent;
            public DotNetExtentReflectiveSequence(DotNetExtent extent)
                : base(extent, extent.elements)
            {
                this.extent = extent;
            }

            public override IObject ConvertInstanceToInternal(object value)
            {
                if (value is DotNetObject)
                {
                    return value as DotNetObject;
                }
                else
                {
                    return new DotNetObject(this.extent.Elements(), value);
                }
            }
        }
    }
}
