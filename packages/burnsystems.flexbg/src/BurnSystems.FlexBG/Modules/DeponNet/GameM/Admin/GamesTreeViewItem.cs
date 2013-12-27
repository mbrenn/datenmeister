using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM.Admin
{
    public class GamesTreeViewItem : BaseTreeViewItem
    {
        public override string ToString()
        {
            return "Games";
        }

        public IEnumerable<Game> GetGames(IActivates activates)
        {
            var gameManagement = activates.Get<IGameManagement>();
            Ensure.That(gameManagement != null, "No GameManagement included");
            
            return gameManagement.GetAll();
        }

        public override IEnumerable<ITreeViewItem> GetChildren(IActivates activates)
        {
            return this.GetGames(activates)
                .Select(x => new GameTreeViewItem(x));
        }
    }
}
