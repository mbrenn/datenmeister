using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.DM.Primitives
{
    /// <summary>
    /// Defines the color to be used
    /// </summary>
    public class Color
    {
        /// <summary>
        /// Gets or sets the red color
        /// </summary>
        public double R
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the green color
        /// </summary>
        public double G
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the blue color
        /// </summary>
        public double B
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the alpha value. 
        /// May be a value between 0 and 1. 1.0 means non-transparent, 0.0 means fully transparent
        /// </summary>
        public double A
        {
            get;
            set;
        }
    }
}
