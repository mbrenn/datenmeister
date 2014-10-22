using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DatenMeister.AddOns.IconRepository
{
    /// <summary>
    /// Defines the icon repository for the gpl released icons (Gnome)
    /// </summary>
    public class GplIconRepository : IIconRepository
    {
        public System.Windows.Media.ImageSource GetIcon(string iconFilename)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource =
                        new Uri("pack://application:,,,/DatenMeister.AddOns;component/resources/icons/" + iconFilename);
            image.EndInit();
            return image;
        }
    }
}
