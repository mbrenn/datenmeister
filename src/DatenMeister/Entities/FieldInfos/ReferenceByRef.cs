﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class ReferenceByRef : General
    {
        public ReferenceByRef(string name, string binding, string referenceUrl, string propertyValue)
            : base(name, binding)
        {
            this.referenceUrl = referenceUrl;
            this.propertyValue = propertyValue;
        }

        public string propertyValue
        {
            get;
            set;
        }

        public string referenceUrl
        {
            get;
            set;
        }
    }
}
