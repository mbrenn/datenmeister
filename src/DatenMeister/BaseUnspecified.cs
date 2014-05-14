﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister
{
    public class BaseUnspecified : IUnspecified
    {
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
            private set;
        }

        public BaseUnspecified(IObject owner, string propertyName, object value)
        {
            this.Owner = owner;
            this.PropertyName = propertyName;
            this.Value = value;
        }

        /// <summary>
        /// Returns the object as a single
        /// </summary>
        /// <returns>Gets the object</returns>
        public object AsSingle()
        {
            return this.Value.AsSingle();
        }
    }
}
