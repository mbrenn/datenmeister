using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Transformations
{
    /// <summary>
    /// Gets the elements of an extent, but just returns the elements of a certain type. 
    /// The other elements won't be returned. 
    /// </summary>
    public class FilterByTypeTransformation : ITransformation
    {
        /// <summary>
        /// Initializes a new instance of the FilterByTypeTransformation class
        /// </summary>
        public FilterByTypeTransformation()
        {
        }

        public FilterByTypeTransformation(IURIExtent source, IObject typeToFilter)
        {
            this.typeToFilter = typeToFilter;
            this.source = source;
        }

        public FilterByTypeTransformation(IURIExtent source, string typeToFilter)
        {
            this.nameOfTypeToFilter = typeToFilter;
            this.source = source;
        }

        /// <summary>
        /// Gets or sets the type which shall be used as filtercriteria. 
        /// If this value is null, only the objects will be returned, which have no type
        /// </summary>
        public IObject typeToFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the type, which shall be used as a filtercriteria
        /// </summary>
        public string nameOfTypeToFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pool, where the object is stored
        /// </summary>
        public IPool Pool
        {
            get { return this.source.Pool; }
            set { this.source.Pool = value; }
        }

        /// <summary>
        /// Gets or sets a flag whether the extent is currently dirty
        /// That means, it has unsaved changes
        /// </summary>
        public bool IsDirty
        {
            get { return this.source.IsDirty; }
            set { this.source.IsDirty = value; }
        }

        /// <summary>
        /// Gets or sets the type being used as source for extent
        /// </summary>
        public IURIExtent source
        {
            get;
            set;
        }

        public string ContextURI()
        {
            return this.source.ContextURI();
        }

        public IReflectiveSequence Elements()
        {
            return new FilterByTypeReflectiveSequence(this);
        }

        private class FilterByTypeReflectiveSequence : ProxyReflectiveSequence
        {
            private FilterByTypeTransformation transformation;

            public FilterByTypeReflectiveSequence(FilterByTypeTransformation transformation)
                : base(transformation.source)
            {
                this.transformation = transformation;
            }

            public override IEnumerable<object> getAll()
            {
                if (this.transformation.typeToFilter == null &&
                    string.IsNullOrEmpty(this.transformation.nameOfTypeToFilter))
                {
                    // No filter criteria at all
                    foreach (var obj in this.transformation.source.Elements())
                    {
                        var element = obj as IElement;
                        if (element == null && this.transformation.typeToFilter == null)
                        {
                            // No filter requested, only elements without types are returned
                            yield return element;
                        }
                    }
                }
                else if (this.transformation.typeToFilter != null &&
                    string.IsNullOrEmpty(this.transformation.nameOfTypeToFilter))
                {
                    // Filter by instance
                    foreach (var obj in this.transformation.source.Elements())
                    {
                        var element = obj as IElement;
                        var metaClass = element.getMetaClass();

                        if (element.getMetaClass() == this.transformation.typeToFilter)
                        {
                            // Metaclass is equivalent
                            yield return element;
                        }
                    }
                }
                else if (this.transformation.typeToFilter == null &&
                    !string.IsNullOrEmpty(this.transformation.nameOfTypeToFilter))
                {
                    // Filter by name
                    foreach (var obj in this.transformation.source.Elements())
                    {
                        var element = obj as IElement;
                        var typeName = element.getMetaClass().get("name").AsSingle().ToString();

                        if (typeName == this.transformation.nameOfTypeToFilter)
                        {
                            // Metaclass is equivalent
                            yield return element;
                        }
                    }
                }
                else
                {
                    // Not supported that both elements are set
                    throw new NotImplementedException("TypeToFilter and NameOfTypeToFilter cannot be set at the same time.");
                }
            }
        }
    }
}
