using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.Xml
{
    public class XmlUnspecified : BaseUnspecified
    {
        public XmlUnspecified(IObject owner, string propertyName, object value, PropertyValueType propertyValueType)
            : base(owner, propertyName, value, propertyValueType)
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
