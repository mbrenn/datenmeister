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
        public GenericUnspecified(IObject owner, string propertyName, object value)
            : base(owner, propertyName, value)
        {
        }
    }
}
