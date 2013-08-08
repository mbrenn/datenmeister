using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    public interface IObject
    {
        object Get(string propertyName);

        KeyValuePair<string, object> GetAll();

        bool IsSet(string propertyName);

        void Set(string propertyName, object value);

        void Unset(string propertyName);
    }
}
