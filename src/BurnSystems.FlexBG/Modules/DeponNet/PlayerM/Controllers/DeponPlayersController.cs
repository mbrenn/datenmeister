using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.UserM.Logic;
using BurnSystems.FlexBG.Modules.UserM.Models;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Controllers
{
    /// <summary>
    /// Defines the player controller for the depon game
    /// </summary>
    public class DeponPlayersController : Controller
    {
        /// <summary>
        /// Session variable for current player
        /// </summary>
        public const string CurrentPlayerName = "FlexBG.CurrentPlayer";

        /// <summary>
        /// Static helper method which retrieves the current game out of the session variables
        /// which have been associated to the user
        /// </summary>
        /// <param name="activates">Activation container</param>
        /// <returns>Found game or null, if user has not continued a game</returns>
        public static Player GetPlayerOfWebRequest(IActivates activates)
        {
            var game = activates.GetByName<Game>(DeponGamesController.CurrentGameName);
            var user = activates.GetByName<User>(CurrentUserHelper.Name);
            var playerManagement = activates.Get<IPlayerManagement>();

            Ensure.That(game != null, "Binding to current Game has not been done");
            Ensure.That(user != null, "Binding to current User has not been done");

            return playerManagement.GetPlayersOfUser(user.Id).Where(x => x.GameId == game.Id).FirstOrDefault();
        }
    }
}
