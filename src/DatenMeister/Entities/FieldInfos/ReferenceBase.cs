using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class ReferenceBase : General
    {
        public ReferenceBase(string name, string binding, string referenceUrl, string propertyValue)
        {
            this.binding = binding;
            this.name = name;
            this.propertyValue = propertyValue;
            this.referenceUrl = referenceUrl;
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
