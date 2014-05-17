using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.Xml
{
    public class XmlUnspecified : BaseUnspecified
    {
        public XmlUnspecified(IObject owner, string propertyName, object value)
            : base(owner, propertyName, value)
        {
        }

        public override IReflectiveCollection AsReflectiveCollection()
        {
            throw new NotImplementedException();
        }

        public override IReflectiveSequence AsReflectiveSequence()
        {
            throw new NotImplementedException();
        }
    }
}
