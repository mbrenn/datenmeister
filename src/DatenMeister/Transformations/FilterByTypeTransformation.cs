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
    public class FilterByTypeTransformation : BaseTransformation
    {
        public FilterByTypeTransformation(IReflectiveCollection source, IObject typeToFilter)
            : base(source)
        {
            this.typeToFilter = typeToFilter;
        }

        public FilterByTypeTransformation(IReflectiveCollection source, string typeToFilter)
            : base(source)
        {
            this.nameOfTypeToFilter = typeToFilter;
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

        public override IEnumerable<object> getAll()
        {
            if (this.typeToFilter == null &&
                string.IsNullOrEmpty(this.nameOfTypeToFilter))
            {
                // No filter criteria at all
                foreach (var obj in this)
                {
                    var element = obj as IElement;
                    if (element == null && this.typeToFilter == null)
                    {
                        // No filter requested, only elements without types are returned
                        yield return element;
                    }
                }
            }
            else if (this.typeToFilter != null &&
                string.IsNullOrEmpty(this.nameOfTypeToFilter))
            {
                // Filter by instance
                foreach (var obj in this.source)
                {
                    var element = obj as IElement;
                    var metaClass = element.getMetaClass();

                    if (element.getMetaClass() == this.typeToFilter)
                    {
                        // Metaclass is equivalent
                        yield return element;
                    }
                }
            }
            else if (this.typeToFilter == null &&
                !string.IsNullOrEmpty(this.nameOfTypeToFilter))
            {
                // Filter by name
                foreach (var obj in this.source)
                {
                    var element = obj as IElement;
                    var typeName = element.getMetaClass().get("name").AsSingle().ToString();

                    if (typeName == this.nameOfTypeToFilter)
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
