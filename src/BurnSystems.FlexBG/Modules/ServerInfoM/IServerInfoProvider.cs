using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.ServerInfoM
{
    /// <summary>
    /// Queries the gameinfo
    /// </summary>
    public interface IServerInfoProvider
    {
        ServerInfo ServerInfo
        {
            get;
        }
    }
}
