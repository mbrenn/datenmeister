using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Admin
{
    public class PlayerTreeViewItem : BaseTreeViewItem
    {
        /// <summary>
        /// Gets or sets the game
        /// </summary>
        public Player Player
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the entity
        /// </summary>
        public override object Entity
        {
            get
            {
                return this.Player;
            }
        }

        /// <summary>
        /// Gets the game tree view
        /// </summary>
        /// <param name="player">Game to be calculated</param>
        public PlayerTreeViewItem(Player player)
        {
            this.Id = player.Id;
            this.Player = player;
            this.Entity = player;
        }

        /// <summary>
        /// Gets the children 
        /// </summary>
        /// <param name="container">Container to be used</param>
        /// <returns>Enumeration of objects</returns>
        public override IEnumerable<ITreeViewItem> GetChildren(ObjectActivation.IActivates container)
        {
            return null;
        }
    }
}
