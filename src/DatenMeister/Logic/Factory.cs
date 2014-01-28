﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Implements the IFactory class in a very simple way by just calling 
	/// IURIExtent.CreateObject
    /// </summary>
    public class Factory : IFactory
    {
        private IURIExtent extent;

        public Factory(IURIExtent extent)
        {
            this.extent = extent;
        }
        
        public IObject create(IObject type)
        {
            return extent.CreateObject();
        }
    }
}
