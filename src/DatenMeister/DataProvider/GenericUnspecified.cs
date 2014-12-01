using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Implements the IUnspecified interface matching to GenericObject
    /// </summary>
    public class GenericUnspecified : BaseUnspecified
    {
        public GenericUnspecified(IObject owner, string propertyName, object value, PropertyValueType propertyValueType)
            : base(owner, propertyName, value, propertyValueType)
        {
            if (ObjectConversion.IsEnumeration(value))
            {
                this.PropertyValueType = DatenMeister.PropertyValueType.Enumeration;
            }
            else
            {
                this.PropertyValueType = DatenMeister.PropertyValueType.Single;
            }            
        }

        public override IReflectiveCollection AsReflectiveCollection()
        {
            if (this.Value is IReflectiveCollection)
            {
                return this.Value as IReflectiveCollection;
            }

            return this.AsReflectiveSequence();
        }

        public override IReflectiveSequence AsReflectiveSequence()
        {
            if ( this.Value is IReflectiveSequence)
            {
                return this.Value as IReflectiveSequence;
            }

            return new GenericReflectiveSequence(this);
        }
    }
}
