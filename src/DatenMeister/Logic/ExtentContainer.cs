﻿using DatenMeister.Entities.DM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Maps the extent information and the extent itself together
    /// </summary>
    public class ExtentContainer
    {
        /// <summary>
        /// Gets or sets the information for the extent
        /// </summary>
        public ExtentInfo Info
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the associated extent
        /// </summary>
        public IURIExtent Extent
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the ExtentMapping
        /// </summary>
        /// <param name="info">Information of the extent</param>
        /// <param name="extent">Extent being associated</param>
        public ExtentContainer(ExtentInfo info, IURIExtent extent)
        {
            this.Info = info;
            this.Extent = extent;
        }

        /// <summary>
        /// Converts the container to a string
        /// </summary>
        /// <returns>The container being converted to a string</returns>
        public override string ToString()
        {
            return this.Info.ToString();
        }
    }
}
