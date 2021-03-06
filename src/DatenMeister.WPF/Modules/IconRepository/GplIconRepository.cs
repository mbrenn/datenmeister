﻿using BurnSystems.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DatenMeister.WPF.Modules.IconRepository
{
    /// <summary>
    /// Defines the icon repository for the gpl released icons (Gnome)
    /// </summary>
    public class GplIconRepository : IIconRepository
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(GplIconRepository));

        /// <summary>
        /// Defines the conversion between name to filename
        /// </summary>
        private static Dictionary<string, string> nameToFilename = new Dictionary<string, string>();

        static GplIconRepository()
        {
            nameToFilename["show-extents"] = "emblem-documents.png";
            nameToFilename["spreadsheet"] = "x-office-spreadsheet.48.png";
            nameToFilename["report-export"] = "x-office-document.png";

            nameToFilename["compliancesuite"] = "emblem-package.png";
            nameToFilename["viewmanager"] = "emblem-package.png";
            nameToFilename["viewmanager-assigntypes"] = "emblem-package.png";
            nameToFilename["typemanager"] = "emblem-package.png";
        }

        /// <summary>
        /// Gets the icon by name
        /// </summary>
        /// <param name="name">The name of the icon</param>
        /// <returns>The image behind the filename or null, if the icon is not available
        /// in the repository</returns>
        public System.Windows.Media.ImageSource GetIcon(string name)
        {
            try
            {
                string result;
                if (nameToFilename.TryGetValue(name, out result))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource =
                                new Uri("pack://application:,,,/DatenMeister.WPF;component/resources/icons/" + result);
                    image.EndInit();
                    return image;
                }

                logger.Message("Unknown GPL icon: " + name);
                return null;
            }
            catch (Exception exc)
            {
                logger.Fail("Error during loading of " + name + ": " + exc.Message);
                return null;
            }
        }
    }
}
