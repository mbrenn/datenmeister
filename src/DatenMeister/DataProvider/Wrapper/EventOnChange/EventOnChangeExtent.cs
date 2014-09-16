using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper.EventOnChange
{
    public class EventOnChangeExtent :
        WrapperExtent<EventOnChangeReflectiveSequence, EventOnChangeElement, EventOnChangeUnspecified>
    {
        /// <summary>
        /// This event is called, when one property
        /// </summary>
        public event EventHandler ChangeInExtent;

        /// <summary>
        /// Throws the OnChangeInEvent event
        /// </summary>
        internal void OnChangeInEvent()
        {
            var ev = this.ChangeInExtent;
            if (ev != null)
            {
                ev(this, EventArgs.Empty);
            }
        }

        public EventOnChangeExtent(IURIExtent extent)
            : base(extent)
        {
        }
    }
}
