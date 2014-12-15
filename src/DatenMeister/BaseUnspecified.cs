using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister
{
    public abstract class BaseUnspecified : IUnspecified
    {
        public PropertyValueType PropertyValueType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owner of the object
        /// </summary>
        public IObject Owner
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the propertyname behind the given property
        /// </summary>
        public string PropertyName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the object itself
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        public BaseUnspecified(IObject owner, string propertyName, object value, PropertyValueType propertyValueType)
        {
            this.Owner = owner;
            this.PropertyName = propertyName;
            this.Value = value;
            this.PropertyValueType = propertyValueType;
        }

        /// <summary>
        /// Returns the object as a single
        /// </summary>
        /// <returns>Gets the object</returns>
        public object AsSingle()
        {
            return this.Value.AsSingle();
        }

        /// <summary>
        /// Gets the hashcode
        /// </summary>
        /// <returns>The hashcode</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Checks, if the two objects are equal
        /// </summary>
        /// <param name="obj">Object to be checked</param>
        /// <returns>true, if both objects are equal</returns>
        public override bool Equals(object obj)
        {
            var asBaseUnspecified = obj as BaseUnspecified;
            if (asBaseUnspecified != null)
            {
                if (this.PropertyValueType != asBaseUnspecified.PropertyValueType)
                {
                    return false;
                }

                if (this.PropertyValueType == DatenMeister.PropertyValueType.Single)
                {
                    return this.Value.AsSingle().Equals(asBaseUnspecified.Value.AsSingle());
                }

                return this.Value.Equals(asBaseUnspecified.Value);
            }

            return false;
        }

        /// <summary>
        /// Returns the object as a reflective collection
        /// </summary>
        /// <returns>The returned collection</returns>
        public abstract IReflectiveCollection AsReflectiveCollection();

        /// <summary>
        /// Returns the object as a reflective sequence
        /// </summary>
        /// <returns>The returned sequence</returns>
        public abstract IReflectiveSequence AsReflectiveSequence();
    }
}
