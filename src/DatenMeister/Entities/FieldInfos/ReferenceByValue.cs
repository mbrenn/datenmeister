using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class ReferenceByValue : General
    {
        public ReferenceByValue(string title, string name, string referenceUrl, string propertyValue)
        {
            this.title = title;
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
