using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    public class ReferenceByValue : ReferenceBase
    {
        public ReferenceByValue(string name, string binding, string referenceUrl, string propertyValue)
            : base(name, binding, referenceUrl, propertyValue)
        {
        }
    }
}
