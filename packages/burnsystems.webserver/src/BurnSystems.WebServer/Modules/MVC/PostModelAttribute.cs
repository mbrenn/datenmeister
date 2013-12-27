using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    /// <summary>
    /// Defines that this parameter shall be filled by POST-Content.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class PostModelAttribute : Attribute
    {
        // This is a positional argument
        public PostModelAttribute()
        {
        }
    }
}
