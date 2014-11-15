using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Stores the configuration for all layouts which can be used as a list
    /// </summary>
    public class ListLayoutConfiguration
    {
        /// <summary>
        /// Gets or sets the table view information
        /// </summary>
        public IObject LayoutInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the factory being used for the reflective collections
        /// </summary>
        public Func<IPool, IReflectiveCollection> ElementsFactory
        {
            get;
            set;
        }
    }
}
