using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper.EventOnChange
{
    public class EventOnChangeReflectiveSequence : WrapperReflectiveSequence
    {
        public override void add(int index, object value)
        {
            base.add(index, value);
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
        }

        public override bool add(object value)
        {
            var result = base.add(value);
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
            return result;
        }

        public override void Clear()
        {
            base.Clear();
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
        }

        public override void clear()
        {
            base.clear();
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
        }

        public override void Insert(int index, object item)
        {
            base.Insert(index, item);
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
        }

        public override object remove(int index)
        {
            var result =  base.remove(index);
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
            return result;
        }

        public override bool Remove(object item)
        {
            var result = base.Remove(item);
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
            return result;
        }

        public override void RemoveAt(int index)
        {
            base.RemoveAt(index);
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
        }

        public override object set(int index, object value)
        {
            var result = base.set(index, value);
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
            return result;
        }

        public override bool remove(object value)
        {
            var result = base.remove(value);
            (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
            return result;
        }

        public override object this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
                (this.WrapperExtent as EventOnChangeExtent).OnChangeInEvent();
            }
        }
    }
}
