using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    /// <summary>
    /// Generalizes JsonActionResult and TemplateOrJsonActionResult to ease the 
    /// </summary>
    public interface IValueActionResult
    {
        /// <summary>
        /// Defines the resulting class
        /// </summary>
        object ReturnObject
        {
            get;
        }
    }
}
