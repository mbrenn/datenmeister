using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    public interface ITransformation : IReflectiveCollection
    {
        IReflectiveCollection source
        {
            get;
            set;
        }
    }
}
