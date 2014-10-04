using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls
{
    public interface IDataPresentationState
    {
        /// <summary>
        /// Gets the current edit mode of the element
        /// </summary>
        EditMode EditMode
        {
            get;
        }

        /// <summary>
        /// Gets the display mode of the element. 
        /// May be table or form
        /// </summary>
        DisplayMode DisplayMode
        {
            get;
        }

        IPublicDatenMeisterSettings Settings
        {
            get;
        }
    }
}
