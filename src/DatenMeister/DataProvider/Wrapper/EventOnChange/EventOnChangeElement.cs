using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper.EventOnChange
{
    public class EventOnChangeElement : WrapperElement
    {
        public override void delete()
        {
            base.delete();
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
        }

        public override void set(string propertyName, object value)
        {
            base.set(propertyName, value);
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
        }

        public override bool unset(string propertyName)
        {
            var result = base.unset(propertyName);
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
            return result;
        }
    }
}
