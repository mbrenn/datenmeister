using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Admin
{
    public class PlayersAdminController : Controller
    {
        [Inject(IsMandatory = true)]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        [Inject]
        public ActivationBlock ActivationBlock
        {
            get;
            set;
        }

        [WebMethod]
        public IActionResult DropPlayer([PostModel] Models.DropPlayerModel model)
        {
            var player = this.PlayerManagement.GetPlayer(model.Id);
            if (player == null)
            {
                return this.SuccessJson(false);
            }

            using (var block = DeponGamesController.CreateActivationBlockInGameScope(this.ActivationBlock, player.GameId))
            {
                var playerRules = block.Get<IPlayerRulesLogic>();
                Ensure.That(playerRules != null);

                playerRules.DropPlayer(model.Id);
            }

            return this.SuccessJson();
        }
    }
}
