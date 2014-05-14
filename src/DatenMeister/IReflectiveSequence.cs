using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister
{
    public interface IReflectiveSequence : IReflectiveCollection
    {
        void add(int index, object value);

        object get(int index);

        object remove(int index);

        object set(int index, object value);
    }
}
