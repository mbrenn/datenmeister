using BurnSystems.FlexBG.Modules.DeponNet.PlayerM;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.TownM.Interface;
using BurnSystems.FlexBG.Modules.LockMasterM;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.DataProvider
{
    /// <summary>
    /// Gets the information for a complete player, including resources
    /// </summary>
    public class PlayerInfoController : Controller
    {
        [Inject(IsMandatory = true)]
        public IResourceManagement ResourceManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public ITownManagement TownManagement
        {
            get;
            set;
        }

        [Inject(ByName = DeponPlayersController.CurrentPlayerName, IsMandatory = true)]
        public Player CurrentPlayer
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public ILockMaster LockMaster
        {
            get;
            set;
        }

        [WebMethod]
        public IActionResult GetCurrentPlayer()
        {
            using (this.LockMaster.AcquireReadLock())
            {
                var playerData = this.CurrentPlayer.AsJson();
                var resourcesOfPlayer = this.ResourceManagement.GetResources(EntityType.Player, this.CurrentPlayer.Id);
                var convertedResources = this.ResourceManagement.AsJson(resourcesOfPlayer);

                // Get start position
                var startPosition = this.TownManagement.GetTownsOfPlayer(this.CurrentPlayer.Id).Where(x => x.IsCapital).First().Position;

                return this.Json(
                    new
                    {
                        playerResources = convertedResources,
                        player = playerData,
                        startPosition = startPosition.AsJson(),
                        success = true
                    });
            }
        }
    }
}
