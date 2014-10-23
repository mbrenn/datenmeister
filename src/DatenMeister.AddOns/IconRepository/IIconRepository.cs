using BurnSystems.License;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DatenMeister.AddOns.IconRepository
{
    /// <summary>
    /// Single source to retrieve the icons for the project
    /// </summary>
    [NonGPLInterface]
    public interface IIconRepository
    {
        /// <summary>
        /// Gets the icon for a 
        /// </summary>
        /// <param name="iconFilename"></param>
        /// <returns>The image behind the filename</returns>
        ImageSource GetIcon(string iconFilename);
    }
}
