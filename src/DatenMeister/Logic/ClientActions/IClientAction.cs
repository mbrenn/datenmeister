using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.ClientActions
{
    /// <summary>
    /// Defines the interface for all client actions
    /// </summary>
    public interface IClientAction
    {
        string type
        {
            get;
        }
    }
}
