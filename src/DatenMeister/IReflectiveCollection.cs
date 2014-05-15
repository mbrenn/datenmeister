using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister
{
    public interface IReflectiveCollection : ICollection<object>
    {
        bool add(object value);

        bool addAll(IReflectiveSequence value);

        void clear();

        object remove(object value);

        int size();
    }
}
