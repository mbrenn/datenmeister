using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Admin
{
    public class PlayersTreeViewItem : BaseTreeViewItem
    {
        public override string ToString()
        {
            return "Players";
        }

        public IEnumerable<Player> GetPlayers(IActivates activates)
        {
            var playerManagement = activates.Get<IPlayerManagement>();
            Ensure.That(playerManagement != null, "No PlayerManagement included");

            return playerManagement.GetAllPlayers();
        }

        public override IEnumerable<ITreeViewItem> GetChildren(IActivates activates)
        {
            return this.GetPlayers(activates)
                .Select(x => new PlayerTreeViewItem(x));
        }
    }
}
