using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Common
{
    public interface IListWrapperReflectiveSequence
    {
        /// <summary>
        /// Gets the list as an instance
        /// </summary>
        /// <returns>Gets the list being used as container</returns>
        IEnumerable GetList();
    }
}
