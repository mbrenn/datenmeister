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
            return this.AsReflectiveSequence();
        }

        public override IReflectiveSequence AsReflectiveSequence()
        {
            return new XmlReflectiveSequence(this);
        }
    }
}
