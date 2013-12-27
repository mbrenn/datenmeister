using BurnSystems.FlexBG.Modules.DeponNet.MapM.Admin;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM.Admin
{
    public class GameTreeViewItem : BaseTreeViewItem
    {
        /// <summary>
        /// Gets or sets the game
        /// </summary>
        public Game Game
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
                return this.Game;
            }
        }

        /// <summary>
        /// Gets the game tree view
        /// </summary>
        /// <param name="game">Game to be calculated</param>
        public GameTreeViewItem(Game game)
        {
            this.Id = game.Id;
            this.Game = game;
            this.Entity = game;
        }

        /// <summary>
        /// Gets the children 
        /// </summary>
        /// <param name="container">Container to be used</param>
        /// <returns>Enumeration of objects</returns>
        public override IEnumerable<ITreeViewItem> GetChildren(ObjectActivation.IActivates container)
        {
            return new ITreeViewItem[]
            {
                new MapTreeViewItem(container, (this.Entity as Game).Id)
            };
        }
    }
}
