using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DatenMeister.AddOns
{
    static class AddOnHelper
    {
        public static BitmapImage LoadIcon(string iconFilename)
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
