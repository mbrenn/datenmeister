using BurnSystems.License;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DatenMeister.WPF.Modules.IconRepository
{
    /// <summary>
    /// Single source to retrieve the icons for the project
    /// </summary>
    [NonGPLInterface]
    public interface IIconRepository
    {
        /// <summary>
        /// Gets the icon by name
        /// </summary>
        /// <param name="name">The name of the icon</param>
        /// <returns>The image behind the filename or null, if the icon is not available
        /// in the repository</returns>
        ImageSource GetIcon(string name);
    }
}
